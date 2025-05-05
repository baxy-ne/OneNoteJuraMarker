using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

namespace OneNoteJuraMarker.Interfaces;

public interface IDialogUtility
{
    Page ActivePage { get; set; }
    Task ShowMessage(string title, string message);
}