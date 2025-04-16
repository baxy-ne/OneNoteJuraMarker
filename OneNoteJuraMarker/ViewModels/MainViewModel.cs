using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OneNoteJuraMarker.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] private bool _isSelectAllChecked;

    [ObservableProperty] private bool _viewCheckboxIsChecked;

    [ObservableProperty] private bool _tableCheckboxIsChecked;


    public MainViewModel()
    {
        // _userProfiles = new ObservableCollection<Models.UserProfile>(userLoader.LoadProfiles());
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
