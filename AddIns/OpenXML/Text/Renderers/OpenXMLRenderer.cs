﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Altaxo.Text.Renderers.OpenXML;
using Altaxo.Text.Renderers.OpenXML.Inlines;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Markdig.Renderers;
using Markdig.Syntax;

namespace Altaxo.Text.Renderers
{
  /// <summary>
  /// Renderer for a Markdown <see cref="MarkdownDocument"/> object that renders into one or multiple MAML files (MAML = Microsoft Assisted Markup Language).
  /// </summary>
  /// <seealso cref="RendererBase" />
  public partial class OpenXMLRenderer : RendererBase, IDisposable
  {
    private const bool EnableHtmlEscape = false;
    /// <summary>
    /// Gets the name of the word document file.
    /// </summary>
    /// <value>
    /// The name of the word document file.
    /// </value>
    public string WordDocumentFileName { get; private set; }

    /// <summary>
    /// The word document
    /// </summary>
    public WordprocessingDocument _wordDocument { get; private set; }
    private MainDocumentPart _mainDocumentPart;
    public Body Body { get; private set; }
    public Paragraph Paragraph { get; set; }
    public Run Run { get; set; }

    public IReadOnlyDictionary<string, Altaxo.Graph.MemoryStreamImageProxy> Images { get; private set; }


    public OpenXMLRenderer(string wordDocumentFileName, IReadOnlyDictionary<string, Altaxo.Graph.MemoryStreamImageProxy> images)
    {
      WordDocumentFileName = wordDocumentFileName;
      Images = images;


      // Extension renderers that must be registered before the default renders
      //ObjectRenderers.Add(new MathBlockRenderer()); // since MathBlock derives from CodeBlock, it must be registered before CodeBlockRenderer

      // Default block renderers
      ObjectRenderers.Add(new CodeBlockRenderer());
      ObjectRenderers.Add(new ListRenderer());
      ObjectRenderers.Add(new HeadingRenderer());
      //ObjectRenderers.Add(new HtmlBlockRenderer());
      ObjectRenderers.Add(new ParagraphRenderer());
      ObjectRenderers.Add(new QuoteBlockRenderer());
      ObjectRenderers.Add(new ThematicBreakRenderer());

      // Default inline renderers
      ObjectRenderers.Add(new AutolinkInlineRenderer());
      ObjectRenderers.Add(new CodeInlineRenderer());
      ObjectRenderers.Add(new DelimiterInlineRenderer());
      ObjectRenderers.Add(new EmphasisInlineRenderer());
      ObjectRenderers.Add(new LineBreakInlineRenderer());
      //ObjectRenderers.Add(new HtmlInlineRenderer());
      ObjectRenderers.Add(new HtmlEntityInlineRenderer());
      ObjectRenderers.Add(new LinkInlineRenderer());
      ObjectRenderers.Add(new LiteralInlineRenderer());

      // Extension renderers
      //ObjectRenderers.Add(new TableRenderer());
      //ObjectRenderers.Add(new MathInlineRenderer());

    }

    public void Dispose()
    {
      _wordDocument?.Dispose();
      _wordDocument = null;
    }

    public override object Render(MarkdownObject markdownObject)
    {
      object result = null;

      if (null == markdownObject)
        throw new ArgumentNullException(nameof(markdownObject));

      if (markdownObject is MarkdownDocument markdownDocument)
      {

        using (_wordDocument = WordprocessingDocument.Create(WordDocumentFileName, WordprocessingDocumentType.Document))
        {

          // Add a main document part. 
          _mainDocumentPart = _wordDocument.AddMainDocumentPart();

          // Create the document structure and add some text.
          _mainDocumentPart.Document = new Document();
          Body = _mainDocumentPart.Document.AppendChild(new Body());

          // Ensure that a style part exists in this document

          // Get the Styles part for this document.
          StyleDefinitionsPart part = _mainDocumentPart.StyleDefinitionsPart;

          // If the Styles part does not exist, add it and then add the style.
          if (part == null)
          {
            part = AddStylesPartToPackage(_wordDocument);
          }

          // now write the document
          Write(markdownObject);
        }
      }
      else
      {
        Write(markdownObject);
        return Body;
      }
      return Body;
    }




    /// <summary>
    /// Writes the inlines of a leaf inline.
    /// </summary>
    /// <param name="leafBlock">The leaf block.</param>
    /// <returns>This instance</returns>
    public OpenXMLRenderer WriteLeafInline(LeafBlock leafBlock)
    {
      if (leafBlock == null) throw new ArgumentNullException(nameof(leafBlock));
      var inline = (Markdig.Syntax.Inlines.Inline)leafBlock.Inline;
      if (inline != null)
      {
        while (inline != null)
        {
          Write(inline);
          inline = inline.NextSibling;
        }
      }
      return this;
    }




  }
}
