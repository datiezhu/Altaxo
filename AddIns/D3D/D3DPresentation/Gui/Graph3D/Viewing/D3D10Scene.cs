﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2016 Dr. Dirk Lellinger
//
//    This program is free software; you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation; either version 2 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program; if not, write to the Free Software
//    Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
//
/////////////////////////////////////////////////////////////////////////////

#endregion Copyright

namespace Altaxo.Gui.Graph3D.Viewing
{
	using Altaxo.Drawing;
	using Altaxo.Drawing.D3D;
	using Altaxo.Geometry;
	using Altaxo.Graph.Graph3D;
	using Altaxo.Graph.Graph3D.Camera;
	using Altaxo.Graph.Graph3D.GraphicsContext.D3D;
	using Altaxo.Graph.Graph3D.Lighting;
	using Altaxo.Gui.Graph3D.Common;
	using SharpDX;
	using SharpDX.D3DCompiler;
	using SharpDX.Direct3D10;
	using SharpDX.DXGI;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Buffer = SharpDX.Direct3D10.Buffer;
	using Device = SharpDX.Direct3D10.Device;

	public partial class D3D10Scene : ID3D10Scene
	{
		#region Internal structs

		internal struct RenderLayout
		{
			public InputLayout VertexLayout;
			public EffectTechnique technique;
			public EffectPass pass;

			public void Dispose()
			{
				Disposer.RemoveAndDispose(ref VertexLayout);
				Disposer.RemoveAndDispose(ref technique);
				Disposer.RemoveAndDispose(ref pass);
			}
		}

		internal struct VertexAndIndexDeviceBuffer
		{
			public IMaterial Material;
			public Buffer VertexBuffer;
			public Buffer IndexBuffer;
			public int VertexCount;
			public int IndexCount;
			public Plane[] ClipPlanes;

			public void RemoveAndDispose()
			{
				Disposer.RemoveAndDispose(ref VertexBuffer);
				Disposer.RemoveAndDispose(ref IndexBuffer);
				VertexCount = 0;
				IndexCount = 0;
				Material = null;
			}
		}

		internal class VertexAndIndexDeviceBufferNoMaterial
		{
			public Buffer VertexBuffer;
			public Buffer IndexBuffer;
			public int VertexCount;
			public int IndexCount;

			public void RemoveAndDispose()
			{
				Disposer.RemoveAndDispose(ref VertexBuffer);
				Disposer.RemoveAndDispose(ref IndexBuffer);
				VertexCount = 0;
				IndexCount = 0;
			}
		}

		internal class VertexBufferNoMaterial
		{
			public Buffer VertexBuffer;
			public int VertexCount;

			public void RemoveAndDispose()
			{
				Disposer.RemoveAndDispose(ref VertexBuffer);
				VertexCount = 0;
			}
		}

		#endregion Internal structs

		private Device _hostDevice;

		private PointD2D _hostSize;

		private AxoColor? _sceneBackgroundColor;

		private D3D10GraphicsContext _drawing;

		private D3D10OverlayContext _markerGeometry;

		private D3D10OverlayContext _overlayGeometry;

		private CameraBase _camera;

		/// <summary>The light settings from AltaxoBase</summary>
		private LightSettings _lightSettings;

		protected Buffer _constantBuffer;

		protected Buffer _constantBufferForColor;

		protected Buffer _constantBufferForSixPlanes;

		private int _renderCounter;

		private string[] _layoutNames = new string[6] { "P", "PC", "PT", "PN", "PNC", "PNT" };

		private RenderLayout[] _renderLayouts = new RenderLayout[6];

		private Effect _lightingEffect;

		/// <summary>
		/// The _this triangle buffers. These buffers are used for current rendering
		/// 0: Position, 1: PositionColor, 2: PositionUV, 3: PositionNormal, 4: PositionNormalColor, 5: PositionNormalUV
		/// </summary>
		private List<VertexAndIndexDeviceBuffer>[] _thisTriangleDeviceBuffers = new List<VertexAndIndexDeviceBuffer>[6];

		private List<VertexAndIndexDeviceBuffer>[] _nextTriangleDeviceBuffers = new List<VertexAndIndexDeviceBuffer>[6];

		private VertexAndIndexDeviceBufferNoMaterial _markerGeometryTriangleDeviceBuffer;
		private VertexBufferNoMaterial _markerGeometryLineListBuffer;
		private VertexAndIndexDeviceBufferNoMaterial _overlayGeometryTriangleDeviceBuffer;
		private VertexBufferNoMaterial _overlayGeometryLineListBuffer;

		// Effect variables

		// Transformation variables
		private EffectConstantBuffer _cbViewTransformation;

		private EffectMatrixVariable _evWorldViewProj;
		private EffectVectorVariable _evEyePosition;

		// Materials
		private EffectConstantBuffer _cbMaterial;

		private EffectVectorVariable _evMaterialDiffuseColor;
		private EffectScalarVariable _evMaterialSpecularExponent;
		private EffectScalarVariable _evMaterialSpecularIntensity;
		private EffectScalarVariable _evMaterialSpecularMixingCoefficient;

		// Clip planes
		private EffectConstantBuffer _cbClipPlanes;

		private EffectVectorVariable[] _evClipPlanes = new EffectVectorVariable[6];

		// Lighting
		private Lighting _lighting;

		public void Attach(SharpDX.ComObject hostDevice, PointD2D hostSize)
		{
			Attach((Device)hostDevice, hostSize);
		}

		public void Attach(Device hostDevice, PointD2D hostSize)
		{
			if (hostDevice == null)
				throw new ArgumentNullException(nameof(hostDevice));

			_hostDevice = hostDevice;
			_hostSize = hostSize;

			Device device = _hostDevice;

			using (var stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Altaxo.CompiledShaders.Effects.Lighting.cso"))
			{
				if (null == stream)
					throw new InvalidOperationException(string.Format("Compiled shader resource not found: {0}", "Altaxo.CompiledShaders.Effects.Lighting.cso"));

				using (var shaderBytes = ShaderBytecode.FromStream(stream))
				{
					_lightingEffect = new Effect(device, shaderBytes);
				}
			}

			int i;

			for (i = 0; i < _layoutNames.Length; ++i)
			{
				string techniqueName = "Shade_" + _layoutNames[i];
				_renderLayouts[i].technique = this._lightingEffect.GetTechniqueByName(techniqueName);
				_renderLayouts[i].pass = _renderLayouts[i].technique.GetPassByIndex(0);

				if (null == _renderLayouts[i].technique || !_renderLayouts[i].technique.IsValid)
					throw new InvalidProgramException(string.Format("Technique {0} was not found or is invalid", techniqueName));
				if (null == _renderLayouts[i].pass || !_renderLayouts[i].pass.IsValid)
					throw new InvalidProgramException(string.Format("Pass[0] of technique {0} was not found or is invalid", techniqueName));
			}

			i = 0;
			_renderLayouts[i].VertexLayout = new InputLayout(device, _renderLayouts[i].pass.Description.Signature, new[] {
																																new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0)
																								});

			i = 1;
			_renderLayouts[i].VertexLayout = new InputLayout(device, _renderLayouts[i].pass.Description.Signature, new[] {
																																new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
																																new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 16, 0)
																								});

			i = 2;
			_renderLayouts[i].VertexLayout = new InputLayout(device, _renderLayouts[i].pass.Description.Signature, new[] {
																																new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
																																new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
																								});

			i = 3;
			_renderLayouts[i].VertexLayout = new InputLayout(device, _renderLayouts[i].pass.Description.Signature, new[] {
																																new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
																																new InputElement("NORMAL", 0, Format.R32G32B32A32_Float, 16, 0)
																								});

			i = 4;
			_renderLayouts[i].VertexLayout = new InputLayout(device, _renderLayouts[i].pass.Description.Signature, new[] {
																																new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
																																new InputElement("NORMAL", 0, Format.R32G32B32A32_Float, 16, 0),
																																new InputElement("COLOR", 0, Format.R32G32B32A32_Float, 32, 0)
																								});

			i = 5;
			_renderLayouts[i].VertexLayout = new InputLayout(device, _renderLayouts[i].pass.Description.Signature, new[] {
																																new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
																																new InputElement("NORMAL", 0, Format.R32G32B32A32_Float, 16, 0),
																																new InputElement("TEXCOORD", 0, Format.R32G32_Float, 32, 0)
																								});

			// Create Constant Buffers
			//_constantBuffer = new Buffer(device, Utilities.SizeOf<Matrix>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None);
			//_constantBufferForColor = new Buffer(device, Utilities.SizeOf<Vector4>(), ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None);
			//_constantBufferForSixPlanes = new Buffer(device, Utilities.SizeOf<Vector4>() * 6, ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None);

			// View transformation variables
			_cbViewTransformation = this._lightingEffect.GetConstantBufferByName("cbViewTransformation");
			_evWorldViewProj = _cbViewTransformation.GetMemberByName("WorldViewProj").AsMatrix();
			_evEyePosition = _cbViewTransformation.GetMemberByName("EyePosition").AsVector();

			_cbMaterial = this._lightingEffect.GetConstantBufferByName("cbMaterial");
			_evMaterialDiffuseColor = _cbMaterial.GetMemberByName("MaterialDiffuseColor").AsVector();
			_evMaterialSpecularExponent = _cbMaterial.GetMemberByName("MaterialSpecularExponent").AsScalar();
			_evMaterialSpecularExponent.Set(4.0f);
			_evMaterialSpecularIntensity = _cbMaterial.GetMemberByName("MaterialSpecularIntensity").AsScalar();
			_evMaterialSpecularIntensity.Set(1.0f);
			_evMaterialSpecularMixingCoefficient = _cbMaterial.GetMemberByName("MaterialSpecularMixingCoefficient").AsScalar();
			_evMaterialSpecularMixingCoefficient.Set(0.75f);

			// Clip plane variables
			_cbClipPlanes = this._lightingEffect.GetConstantBufferByName("cbClipPlanes");
			for (i = 0; i < 6; ++i)
			{
				_evClipPlanes[i] = _cbClipPlanes.GetMemberByName("ClipPlane" + i.ToString(System.Globalization.CultureInfo.InvariantCulture)).AsVector();
			}

			// Lighting variables

			_lighting.Initialize(_lightingEffect);
			_lighting.SetDefaultLighting();

			// --------------------
			if (_drawing != null)
			{
				BringDrawingIntoBuffers(_drawing);
			}

			if (null != _markerGeometry)
			{
				BringMarkerGeometryIntoDeviceBuffers(_markerGeometry);
			}

			if (null != _overlayGeometry)
			{
				BringOverlayGeometryIntoDeviceBuffers(_overlayGeometry);
			}
		}

		public void SetSceneBackColor(AxoColor? sceneBackColor)
		{
			_sceneBackgroundColor = sceneBackColor;
		}

		public AxoColor? SceneBackgroundColor
		{
			get
			{
				return _sceneBackgroundColor;
			}
		}

		internal void SetMarkerGeometry(D3D10OverlayContext markerGeometry)
		{
			_markerGeometry = markerGeometry;
			BringMarkerGeometryIntoDeviceBuffers(markerGeometry);
		}

		public void SetHostSize(PointD2D hostSize)
		{
			_hostSize = hostSize;
		}

		public void SetDrawing(D3D10GraphicsContext drawing)
		{
			_drawing = drawing;
			BringDrawingIntoBuffers(drawing);
		}

		public void SetOverlayGeometry(D3D10OverlayContext overlayGeometry)
		{
			_overlayGeometry = overlayGeometry;
			BringOverlayGeometryIntoDeviceBuffers(overlayGeometry);
		}

		public void SetCamera(CameraBase camera)
		{
			if (null == camera)
				throw new ArgumentNullException(nameof(camera));

			_camera = camera;
		}

		public void SetLighting(LightSettings lightSettings)
		{
			if (null == lightSettings)
				throw new ArgumentNullException(nameof(lightSettings));

			_lightSettings = lightSettings;
		}

		private void UseNextTriangleDeviceBuffers()
		{
			for (int i = 0; i < _nextTriangleDeviceBuffers.Length; ++i)
			{
				if (null != _nextTriangleDeviceBuffers[i])
				{
					var oldBuffers = _thisTriangleDeviceBuffers[i];
					_thisTriangleDeviceBuffers[i] = System.Threading.Interlocked.Exchange(ref _nextTriangleDeviceBuffers[i], null);

					if (null != oldBuffers)
					{
						foreach (var entry in oldBuffers)
							entry.RemoveAndDispose();
					}
				}
			}
		}

		private void BringDrawingIntoBuffers(D3D10GraphicsContext drawing)
		{
			Device device = _hostDevice;
			if (device == null)
				return;

			var buffersOfType =
							new IEnumerable<KeyValuePair<IMaterial, IndexedTriangleBuffer>>[]
							{
																drawing.PositionIndexedTriangleBuffersAsIndexedTriangleBuffers,
																drawing.PositionColorIndexedTriangleBuffersAsIndexedTriangleBuffers,
																drawing.PositionUVIndexedTriangleBuffersAsIndexedTriangleBuffers,
																drawing.PositionNormalIndexedTriangleBuffersAsIndexedTriangleBuffers,
																drawing.PositionNormalColorIndexedTriangleBuffersAsIndexedTriangleBuffers,
																drawing.PositionNormalUVIndexedTriangleBuffersAsIndexedTriangleBuffers
							};

			for (int i = 0; i < buffersOfType.Length; ++i)
			{
				var newDeviceBuffers = new List<VertexAndIndexDeviceBuffer>();
				foreach (var entry in buffersOfType[i])
				{
					var buf = entry.Value;
					if (buf.TriangleCount == 0)
						continue;

					var vertexBuffer = Buffer.Create<float>(device, buf.VertexStream, new BufferDescription()
					{
						BindFlags = BindFlags.VertexBuffer,
						CpuAccessFlags = CpuAccessFlags.None,
						OptionFlags = ResourceOptionFlags.None,
						SizeInBytes = buf.VertexStreamLength,
						Usage = ResourceUsage.Default
					});

					var indexBuffer = Buffer.Create<int>(device, buf.IndexStream, new BufferDescription()
					{
						BindFlags = BindFlags.IndexBuffer,
						CpuAccessFlags = CpuAccessFlags.None,
						OptionFlags = ResourceOptionFlags.None,
						SizeInBytes = buf.IndexStreamLength,
						Usage = ResourceUsage.Default
					});
					var indexCount = buf.TriangleCount * 3;

					Plane[] clipPlanes = null;
					if (buf is PositionNormalColorIndexedTriangleBufferWithClipping)
					{
						var axoClipPlanes = (buf as PositionNormalColorIndexedTriangleBufferWithClipping).ClipPlanes;
						if (null != axoClipPlanes)
							clipPlanes = axoClipPlanes.Select(axoPlane => new Plane((float)axoPlane.X, (float)axoPlane.Y, (float)axoPlane.Z, (float)-axoPlane.W)).ToArray();
					}

					newDeviceBuffers.Add(new VertexAndIndexDeviceBuffer { Material = entry.Key, VertexBuffer = vertexBuffer, VertexCount = buf.VertexCount, IndexBuffer = indexBuffer, IndexCount = indexCount, ClipPlanes = clipPlanes });
				}

				System.Threading.Interlocked.Exchange(ref _nextTriangleDeviceBuffers[i], newDeviceBuffers);
			}
		}

		private void BringMarkerGeometryIntoDeviceBuffers(D3D10OverlayContext overlayGeometry)
		{
			Device device = _hostDevice;
			if (device == null)
				return;

			// ------------------  Triangle buffer ------------------------------------
			{
				var buf = (PositionColorIndexedTriangleBuffer)overlayGeometry.PositionColorIndexedTriangleBuffers;

				var vertexBuffer = Buffer.Create<float>(device, buf.VertexStream, new BufferDescription()
				{
					BindFlags = BindFlags.VertexBuffer,
					CpuAccessFlags = CpuAccessFlags.None,
					OptionFlags = ResourceOptionFlags.None,
					SizeInBytes = buf.VertexStreamLength,
					Usage = ResourceUsage.Default
				});

				var indexBuffer = Buffer.Create<int>(device, buf.IndexStream, new BufferDescription()
				{
					BindFlags = BindFlags.IndexBuffer,
					CpuAccessFlags = CpuAccessFlags.None,
					OptionFlags = ResourceOptionFlags.None,
					SizeInBytes = buf.IndexStreamLength,
					Usage = ResourceUsage.Default
				});
				var indexCount = buf.TriangleCount * 3;

				var devBuffer = new VertexAndIndexDeviceBufferNoMaterial { VertexBuffer = vertexBuffer, VertexCount = buf.VertexCount, IndexBuffer = indexBuffer, IndexCount = indexCount };

				var oldBuffer = System.Threading.Interlocked.Exchange(ref _markerGeometryTriangleDeviceBuffer, devBuffer);

				oldBuffer?.RemoveAndDispose();
			}

			// ------------------  Line list buffer ------------------------------------
			{
				var buf = (PositionColorLineListBuffer)overlayGeometry.PositionColorLineListBuffer;

				var vertexBuffer = Buffer.Create<float>(device, buf.VertexStream, new BufferDescription()
				{
					BindFlags = BindFlags.VertexBuffer,
					CpuAccessFlags = CpuAccessFlags.None,
					OptionFlags = ResourceOptionFlags.None,
					SizeInBytes = buf.VertexStreamLength,
					Usage = ResourceUsage.Default
				});

				var devBuffer = new VertexBufferNoMaterial { VertexBuffer = vertexBuffer, VertexCount = buf.VertexCount };

				var oldBuffer = System.Threading.Interlocked.Exchange(ref _markerGeometryLineListBuffer, devBuffer);

				oldBuffer?.RemoveAndDispose();
			}
		}

		private void BringOverlayGeometryIntoDeviceBuffers(D3D10OverlayContext overlayGeometry)
		{
			Device device = _hostDevice;
			if (device == null)
				return;

			// ------------------  Triangle buffer ------------------------------------
			{
				var buf = (PositionColorIndexedTriangleBuffer)overlayGeometry.PositionColorIndexedTriangleBuffers;

				var vertexBuffer = Buffer.Create<float>(device, buf.VertexStream, new BufferDescription()
				{
					BindFlags = BindFlags.VertexBuffer,
					CpuAccessFlags = CpuAccessFlags.None,
					OptionFlags = ResourceOptionFlags.None,
					SizeInBytes = buf.VertexStreamLength,
					Usage = ResourceUsage.Default
				});

				var indexBuffer = Buffer.Create<int>(device, buf.IndexStream, new BufferDescription()
				{
					BindFlags = BindFlags.IndexBuffer,
					CpuAccessFlags = CpuAccessFlags.None,
					OptionFlags = ResourceOptionFlags.None,
					SizeInBytes = buf.IndexStreamLength,
					Usage = ResourceUsage.Default
				});
				var indexCount = buf.TriangleCount * 3;

				var devBuffer = new VertexAndIndexDeviceBufferNoMaterial { VertexBuffer = vertexBuffer, VertexCount = buf.VertexCount, IndexBuffer = indexBuffer, IndexCount = indexCount };

				var oldBuffer = System.Threading.Interlocked.Exchange(ref _overlayGeometryTriangleDeviceBuffer, devBuffer);

				oldBuffer?.RemoveAndDispose();
			}

			// ------------------  Line list buffer ------------------------------------
			{
				var buf = (PositionColorLineListBuffer)overlayGeometry.PositionColorLineListBuffer;

				var vertexBuffer = Buffer.Create<float>(device, buf.VertexStream, new BufferDescription()
				{
					BindFlags = BindFlags.VertexBuffer,
					CpuAccessFlags = CpuAccessFlags.None,
					OptionFlags = ResourceOptionFlags.None,
					SizeInBytes = buf.VertexStreamLength,
					Usage = ResourceUsage.Default
				});

				var devBuffer = new VertexBufferNoMaterial { VertexBuffer = vertexBuffer, VertexCount = buf.VertexCount };

				var oldBuffer = System.Threading.Interlocked.Exchange(ref _overlayGeometryLineListBuffer, devBuffer);

				oldBuffer?.RemoveAndDispose();
			}
		}

		void IScene.Detach()
		{
			if (null != _nextTriangleDeviceBuffers)
				foreach (var bufType in _nextTriangleDeviceBuffers)
					if (null != bufType)
						foreach (var ele in bufType)
							ele.RemoveAndDispose();

			if (null != _thisTriangleDeviceBuffers)
				foreach (var bufType in _thisTriangleDeviceBuffers)
					if (null != bufType)
						foreach (var ele in bufType)
							ele.RemoveAndDispose();

			foreach (var entry in _renderLayouts)
				entry.Dispose();
		}

		void IScene.Update(TimeSpan sceneTime)
		{
			// use sceneTime.TotalSeconds to update the scene here in dependence on the scene time
		}

		void IScene.Render()
		{
			Device device = _hostDevice;
			if (device == null)
				return;
			if (_camera == null)
				return;
			if (_drawing == null)
				return;

			UseNextTriangleDeviceBuffers();

			float time = _renderCounter / 100f;
			++_renderCounter;

			Matrix worldViewProjTr; // world-view matrix, transposed

			if (null != _camera)
			{
				var cam = _camera;
				var eye = cam.EyePosition;
				var target = cam.TargetPosition;
				var up = cam.UpVector;
				//view = Matrix.LookAtRH(new Vector3((float)eye.X, (float)eye.Y, (float)eye.Z), new Vector3((float)target.X, (float)target.Y, (float)target.Z), new Vector3((float)up.X, (float)up.Y, (float)up.Z));

				//var viewProjD3D = (cam as PerspectiveCamera).GetLookAtRHTimesPerspectiveRHMatrix(_hostSize.Y / _hostSize.X);
				var viewProjD3D = cam.GetViewProjectionMatrix(_hostSize.Y / _hostSize.X);
				worldViewProjTr = new Matrix(
								(float)viewProjD3D.M11, (float)viewProjD3D.M21, (float)viewProjD3D.M31, (float)viewProjD3D.M41,
								(float)viewProjD3D.M12, (float)viewProjD3D.M22, (float)viewProjD3D.M32, (float)viewProjD3D.M42,
								(float)viewProjD3D.M13, (float)viewProjD3D.M23, (float)viewProjD3D.M33, (float)viewProjD3D.M43,
								(float)viewProjD3D.M14, (float)viewProjD3D.M24, (float)viewProjD3D.M34, (float)viewProjD3D.M44
								);
			}
			else
			{
				var view = Matrix.LookAtRH(new Vector3(0, 0, -1500), new Vector3(0, 0, 0), Vector3.UnitY);
				var proj = Matrix.PerspectiveFovRH((float)Math.PI / 4.0f, (float)(_hostSize.X / _hostSize.Y), 0.1f, float.MaxValue);
				var viewProj = Matrix.Multiply(view, proj);

				// Update WorldViewProj Matrix
				worldViewProjTr = viewProj;
				worldViewProjTr.Transpose();
			}

			// World projection and camera
			_evWorldViewProj.SetMatrixTranspose(ref worldViewProjTr);
			_evEyePosition.Set(ToVector3(_camera.EyePosition));

			// lighting
			_lighting.SetLighting(_lightSettings, _camera);

			// Material is separate for each buffer, therefore it is set there

			foreach (var entry in _thisTriangleDeviceBuffers[1]) // Position-Color
			{
				DrawPositionColorIndexedTriangleBuffer(device, entry, worldViewProjTr);
			}

			foreach (var entry in _thisTriangleDeviceBuffers[3]) // Position-Normal
			{
				DrawPositionNormalIndexedTriangleBuffer(device, entry, worldViewProjTr);
			}

			foreach (var entry in _thisTriangleDeviceBuffers[4]) // Position-Normal-Color
			{
				DrawPositionNormalColorIndexedTriangleBuffer(device, entry, worldViewProjTr);
			}

			// ------------------ end of document geometry drawing ----------------------------------

			// ------------------ start of marker geometry drawing ----------------------------------

			var markerTriangles = _markerGeometryTriangleDeviceBuffer;
			if (null != markerTriangles)
				DrawPositionColorIndexedTriangleBufferNoMaterial(device, markerTriangles, worldViewProjTr);

			var markerLines = _markerGeometryLineListBuffer;
			if (null != markerLines && markerLines.VertexCount > 0)
				DrawPositionColorLineListBufferNoMaterial(device, markerLines, worldViewProjTr);

			// ------------------ end of marker geometry drawing ----------------------------------

			// ------------------ start of overlay geometry drawing ----------------------------------

			var overlayTriangles = _overlayGeometryTriangleDeviceBuffer;
			if (null != overlayTriangles)
				DrawPositionColorIndexedTriangleBufferNoMaterial(device, overlayTriangles, worldViewProjTr);

			var overlayLines = _overlayGeometryLineListBuffer;
			if (null != overlayLines && overlayLines.VertexCount > 0)
				DrawPositionColorLineListBufferNoMaterial(device, overlayLines, worldViewProjTr);

			// ------------------ end of overlay geometry drawing ----------------------------------
		}

		private struct SixPlanes
		{
			public Plane v0, v1, v2, v3, v4, v5;

			public Plane this[int i]
			{
				get
				{
					switch (i)
					{
						case 0:
							return v0;

						case 1:
							return v1;

						case 2:
							return v2;

						case 3:
							return v3;

						case 4:
							return v4;

						case 5:
							return v5;

						default:
							throw new IndexOutOfRangeException();
					}
				}
				set
				{
					switch (i)
					{
						case 0:
							v0 = value;
							break;

						case 1:
							v1 = value;
							break;

						case 2:
							v2 = value;
							break;

						case 3:
							v3 = value;
							break;

						case 4:
							v4 = value;
							break;

						case 5:
							v5 = value;
							break;

						default:
							throw new IndexOutOfRangeException();
					}
				}
			}
		}

		private void SetShaderMaterialVariables(IMaterial material)
		{
			_evMaterialSpecularIntensity.Set((float)material.SpecularIntensityNormalizedForPhongModel);
			_evMaterialSpecularExponent.Set((float)material.SpecularExponent);
			_evMaterialSpecularMixingCoefficient.Set((float)material.SpecularMixingCoefficient);
			if (material.HasColor)
			{
				Vector4 colorVec = ToVector4(material.Color.Color);
				_evMaterialDiffuseColor.Set(ref colorVec);
			}
		}

		private void DrawPositionColorIndexedTriangleBuffer(Device device, VertexAndIndexDeviceBuffer deviceBuffers, Matrix worldViewProj)
		{
			int layoutNumber = 1;
			device.InputAssembler.InputLayout = _renderLayouts[layoutNumber].VertexLayout;
			device.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

			SetShaderMaterialVariables(deviceBuffers.Material);

			var planes = new SixPlanes();
			if (null != deviceBuffers.ClipPlanes)
			{
				for (int i = 0; i < Math.Min(6, deviceBuffers.ClipPlanes.Length); ++i)
				{
					planes[i] = deviceBuffers.ClipPlanes[i];
				}
			}

			device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(deviceBuffers.VertexBuffer, 32, 0));
			device.InputAssembler.SetIndexBuffer(deviceBuffers.IndexBuffer, Format.R32_UInt, 0);

			_renderLayouts[layoutNumber].pass.Apply();
			device.DrawIndexed(deviceBuffers.IndexCount, 0, 0);
		}

		private void DrawPositionNormalColorIndexedTriangleBuffer(Device device, VertexAndIndexDeviceBuffer deviceBuffers, Matrix worldViewProj)
		{
			int layoutNumber = 4;
			device.InputAssembler.InputLayout = _renderLayouts[layoutNumber].VertexLayout;
			device.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

			if (null != deviceBuffers.ClipPlanes)
			{
				for (int i = 0; i < Math.Min(6, deviceBuffers.ClipPlanes.Length); ++i)
				{
					_evClipPlanes[i].Set(deviceBuffers.ClipPlanes[i]);
				}
			}

			SetShaderMaterialVariables(deviceBuffers.Material);

			device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(deviceBuffers.VertexBuffer, 48, 0));
			device.InputAssembler.SetIndexBuffer(deviceBuffers.IndexBuffer, Format.R32_UInt, 0);

			_renderLayouts[layoutNumber].pass.Apply();
			device.DrawIndexed(deviceBuffers.IndexCount, 0, 0);

			if (null != deviceBuffers.ClipPlanes)
			{
				var emptyPlane = new Plane();
				for (int i = 0; i < Math.Min(6, deviceBuffers.ClipPlanes.Length); ++i)
				{
					_evClipPlanes[i].Set(emptyPlane);
				}
			}
		}

		private void DrawPositionNormalIndexedTriangleBuffer(Device device, VertexAndIndexDeviceBuffer deviceBuffers, Matrix worldViewProj)
		{
			int layoutNumber = 3;
			device.InputAssembler.InputLayout = _renderLayouts[layoutNumber].VertexLayout;
			device.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

			SetShaderMaterialVariables(deviceBuffers.Material);

			device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(deviceBuffers.VertexBuffer, 32, 0));
			device.InputAssembler.SetIndexBuffer(deviceBuffers.IndexBuffer, Format.R32_UInt, 0);

			_renderLayouts[layoutNumber].pass.Apply();
			device.DrawIndexed(deviceBuffers.IndexCount, 0, 0);
		}

		private void DrawPositionColorIndexedTriangleBufferNoMaterial(Device device, VertexAndIndexDeviceBufferNoMaterial deviceBuffers, Matrix worldViewProj)
		{
			int layoutNumber = 1;
			device.InputAssembler.InputLayout = _renderLayouts[layoutNumber].VertexLayout;
			device.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;

			device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(deviceBuffers.VertexBuffer, 32, 0));
			device.InputAssembler.SetIndexBuffer(deviceBuffers.IndexBuffer, Format.R32_UInt, 0);

			_renderLayouts[layoutNumber].pass.Apply();
			device.DrawIndexed(deviceBuffers.IndexCount, 0, 0);
		}

		private void DrawPositionColorLineListBufferNoMaterial(Device device, VertexBufferNoMaterial deviceBuffers, Matrix worldViewProj)
		{
			int layoutNumber = 1;
			device.InputAssembler.InputLayout = _renderLayouts[layoutNumber].VertexLayout;
			device.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.LineList;

			device.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(deviceBuffers.VertexBuffer, (8 * 4), 0));

			_renderLayouts[layoutNumber].pass.Apply();
			device.Draw(deviceBuffers.VertexCount, 0);
		}

		// helper

		private static Vector3 ToVector3(PointD3D a)
		{
			return new Vector3((float)a.X, (float)a.Y, (float)a.Z);
		}

		private static Vector3 ToVector3(VectorD3D a)
		{
			return new Vector3((float)a.X, (float)a.Y, (float)a.Z);
		}

		private static Vector4 ToVector4(PointD3D a)
		{
			return new Vector4((float)a.X, (float)a.Y, (float)a.Z, 1.0f);
		}

		private static Vector3 ToVector3(Altaxo.Drawing.AxoColor color)
		{
			return new Vector3(color.ScR, color.ScG, color.ScB);
		}

		private static Vector3 ToVector3(Altaxo.Drawing.AxoColor color, double amplitude)
		{
			float amp = (float)amplitude;
			return new Vector3(color.ScR * amp, color.ScG * amp, color.ScB * amp);
		}

		private static Vector4 ToVector4(Altaxo.Drawing.AxoColor color)
		{
			return new Vector4(color.ScR, color.ScG, color.ScB, color.ScA);
		}
	}
}