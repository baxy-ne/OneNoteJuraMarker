using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Documents;
using OneNoteJuraMarker.Interfaces;
using System.Threading.Tasks;
using System.Xml;

namespace OneNoteJuraMarker.Views
{
    public sealed partial class PreviewWindow : Window
    {
        public PreviewWindow(string pageId, IYourOneNoteHelper yourOneNoteHelper)
        {
            InitializeComponent();
            LoadContentAsync(pageId, yourOneNoteHelper);
        }

        private async void LoadContentAsync(string pageId, IYourOneNoteHelper yourOneNoteHelper)
        {
            string xmlContent = await Task.Run(() => yourOneNoteHelper.GetPageContent(pageId));

            var doc = new XmlDocument();
            doc.LoadXml(xmlContent);
            var nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("one", "http://schemas.microsoft.com/office/onenote/2013/onenote");

            var nodes = doc.SelectNodes("//one:T", nsMgr);

            PreviewRichTextBlock.Blocks.Clear();

            foreach (XmlNode node in nodes)
            {
                string text = node.InnerText?.Trim();

                if (!string.IsNullOrEmpty(text))
                {
                    var paragraph = new Paragraph();
                    var run = new Run { Text = text };
                    if (text.StartsWith("§"))
                    {
                        run.FontWeight = Microsoft.UI.Text.FontWeights.Bold;
                        run.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
                    }

                    paragraph.Inlines.Add(run);
                    PreviewRichTextBlock.Blocks.Add(paragraph);
                }
            }
        }
    }
}
