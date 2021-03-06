﻿#region Copyright

/////////////////////////////////////////////////////////////////////////////
//    Altaxo:  a data processing and data plotting program
//    Copyright (C) 2002-2019 Dr. Dirk Lellinger
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

namespace Altaxo.Gui.Graph.Graph3D.Viewing
{
  using System;
  using Altaxo.Drawing.D3D;
  using Altaxo.Gui.Graph.Graph3D.Common;
  using SharpDX;
  using Buffer = SharpDX.Direct3D10.Buffer;

  public partial class D3D10Scene
  {
    internal class VertexAndIndexDeviceBuffer
    {
      private bool _isDisposed;

      IMaterial _material;
      Buffer _vertexBuffer;
      int _vertexCount;
      Buffer _indexBuffer;
      int _indexCount;
      Plane[] _clipPlanes;
      byte[] _uColors;


      public IMaterial Material => _material;
      public Buffer VertexBuffer => _vertexBuffer;
      public int VertexCount => _vertexCount;
      public Buffer IndexBuffer => _indexBuffer;
      public int IndexCount => _indexCount;
      public Plane[] ClipPlanes => _clipPlanes;
      public byte[] UColors => _uColors;

      public VertexAndIndexDeviceBuffer(
        IMaterial material,
        Buffer vertexBuffer,
        int vertexCount,
        Buffer indexBuffer,
        int indexCount,
        Plane[] clipPlanes,
        byte[] uColors)
      {
        _isDisposed = false;
        _material = material;
        _vertexBuffer = vertexBuffer;
        _vertexCount = vertexCount;
        _indexBuffer = indexBuffer;
        _indexCount = indexCount;
        _clipPlanes = clipPlanes;
        _uColors = uColors;
      }

      #region IDisposable Support



      ~VertexAndIndexDeviceBuffer()
      {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(false);
      }

      protected virtual void Dispose(bool disposing)
      {
        if (!_isDisposed)
        {
          Disposer.RemoveAndDispose(ref _material);
          Disposer.RemoveAndDispose(ref _vertexBuffer);
          Disposer.RemoveAndDispose(ref _indexBuffer);
          _vertexCount = 0;
          _indexCount = 0;
          _clipPlanes = null;
          _uColors = null;

          _isDisposed = true;
        }
      }

      public void Dispose()
      {
        Dispose(true);
        GC.SuppressFinalize(this);
      }
      #endregion
    }
  }
}
