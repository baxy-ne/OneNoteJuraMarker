using Microsoft.UI.Xaml;
using OneNoteJuraMarker.Interfaces;
using OneNoteJuraMarker.ViewModels;
using OneNoteJuraMarker.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OneNoteJuraMarker
{
    public sealed partial class MainWindow
    {
        public MainWindow(IWindowSizeUtility windowSizeUtility, MainPage mainPage)
        {
            InitializeComponent();
            windowSizeUtility.SetWindowSize(550, 700, this);
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            SetBody(mainPage);
        }
        public void SetBody(UIElement uiElement) => Body.Content = uiElement;
    }
}
