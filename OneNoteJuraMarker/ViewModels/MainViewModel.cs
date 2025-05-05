using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OneNoteJuraMarker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private bool _isSelectAllChecked;

    [ObservableProperty]
    private bool _viewCheckboxIsChecked;

    [ObservableProperty]
    private bool _tableCheckboxIsChecked;

    private readonly IConfiguration _config;

    public string DefaultNotebook { get; set; }

    public MainViewModel(IConfiguration configuration)
    {
        _config = configuration;
        DefaultNotebook = _config["OneNoteJuraMarker:DefaultNotebook"];
    }

    [RelayCommand]
    private void StartCompare()
    {
    }

    [RelayCommand]
    private void SelectAll()
    {
    }
}
