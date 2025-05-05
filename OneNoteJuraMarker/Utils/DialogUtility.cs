using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using OneNoteJuraMarker.Interfaces;

namespace OneNoteJuraMarker.Utils;

public class DialogUtility : IDialogUtility
{
    public required Page ActivePage { get; set; }

    public async Task ShowMessage(string title, string message)
    {
        var dialog = new ContentDialog
        {
            Title = title,
            Content = message,
            CloseButtonText = "OK",
            XamlRoot = ActivePage.XamlRoot
        };

        await dialog.ShowAsync();
    }
}