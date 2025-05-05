using Microsoft.UI.Xaml;
using OneNoteJuraMarker.Interfaces;


namespace OneNoteJuraMarker.Views;

public sealed partial class PreviewWindow : Window
{
    public PreviewWindow(string pageId, IYourOneNoteHelper yourOneNoteHelper)
    {
        InitializeComponent();
        PreviewTextBlock.Text = (yourOneNoteHelper.GetPageContent(pageId));
    }
}
