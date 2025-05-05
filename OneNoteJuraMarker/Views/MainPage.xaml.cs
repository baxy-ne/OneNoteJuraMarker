using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using OneNoteJuraMarker.Utils;
using OneNoteJuraMarker.ViewModels;
using System.Diagnostics;


namespace OneNoteJuraMarker.Views;

public sealed partial class MainPage : Page
{
    private MainViewModel _mainViewModel;
    public MainPage(MainViewModel mainViewModel)
    {
        InitializeComponent();
        _mainViewModel = mainViewModel;
        DataContext = mainViewModel;
        PopulateTreeView(mainViewModel);
    }
    private void PopulateTreeView(MainViewModel mainViewModel)
    {
        NotebookTreeView.RootNodes.Clear();

        foreach (var notebook in mainViewModel.Notebooks)
        {
            var notebookNode = new TreeViewNode { Content = notebook.Name, IsExpanded = true };

            foreach (var section in notebook.Sections)
            {
                if (section.Name == "Gelöschte Seiten")
                {
                    Debug.WriteLine($"Gelöschte Seiten von: {section.Name}");
                    continue;
                }
                else
                {
                    var sectionNode = new TreeViewNode { Content = section.Name, IsExpanded = true };

                    foreach (var page in section.Pages)
                    {
                        var pageNode = new TreeViewNode { Content = page.Name };
                        sectionNode.Children.Add(pageNode);
                    }

                    notebookNode.Children.Add(sectionNode);
                }
            }

            NotebookTreeView.RootNodes.Add(notebookNode);
        }
    }

    private void NotebookTreeView_PointerPressed(object sender, PointerRoutedEventArgs e)
    {
        if (e.GetCurrentPoint(NotebookTreeView).Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
        {
            var originalSource = e.OriginalSource as FrameworkElement;
            if (originalSource != null)
            {
                DependencyObject obj = originalSource;
                while (obj != null && obj is not TreeViewItem)
                    obj = VisualTreeHelper.GetParent(obj);

                if (obj is TreeViewItem item)
                {
                    TreeViewNode node = NotebookTreeView.NodeFromContainer(item);

                    if (node != null && node.Children.Count == 0)
                    {
                        string pageName = node.Content as string;
                        OpenPreviewWindow(pageName);
                    }
                }
            }
        }
    }
    private void OpenPreviewWindow(string pageName)
    {
        YourOneNoteHelper yourOneNoteHelper = new YourOneNoteHelper(_mainViewModel);
        var previewWindow = new PreviewWindow(pageName, yourOneNoteHelper);
        previewWindow.Activate();
    }
}
