using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OneNoteJuraMarker.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;


namespace OneNoteJuraMarker.Views
{
    public sealed partial class MainPage : Page
    {

        public MainPage(MainViewModel mainViewModel)
        {
            InitializeComponent();
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
                    var sectionNode = new TreeViewNode { Content = section.Name, IsExpanded = true };

                    foreach (var page in section.Pages)
                    {
                        var pageNode = new TreeViewNode { Content = page.Name };
                        sectionNode.Children.Add(pageNode);
                    }

                    notebookNode.Children.Add(sectionNode);
                }

                NotebookTreeView.RootNodes.Add(notebookNode);
            }
        }
    }
}
