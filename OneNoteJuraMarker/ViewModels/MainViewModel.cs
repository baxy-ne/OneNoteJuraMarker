using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using OneNoteJuraMarker.Interfaces;
using OneNoteJuraMarker.Models;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OneNoteJuraMarker.ViewModels;

public partial class MainViewModel(IConfiguration configuration, IOneNoteProgram oneNoteProgram, IOneNoteParser oneNoteParser) : ObservableObject
{
    [ObservableProperty] private ObservableCollection<NotebookModel> _notebooks = new(oneNoteParser.LoadNotebooksFromXml());
    [ObservableProperty] private NotebookModel _selectedNotebook;

    [ObservableProperty] private bool _viewCheckboxIsChecked;

    [ObservableProperty] private bool _tableCheckboxIsChecked;

    [RelayCommand]
    private void LoadNoteBooks()
    {
        foreach (var notebook in _notebooks)
        {
            foreach (var sec in notebook.Sections)
            {
                foreach (var page in sec.Pages)
                {
                Debug.WriteLine($"{notebook.Sections}");

                }

            }
        }
    }

    [RelayCommand]
    private void SelectAll()
    {
    }
}
