using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System.Runtime.InteropServices;
using Windows.Graphics;

namespace OneNoteJuraMarker.Utils;

public class WindowSizeUtility() : IWindowSizeUtility
{
    [DllImport("user32.dll", SetLastError = true)]
    private static  extern int GetWindowLong(nint hWnd, int nIndex);

    [DllImport("user32.dll", SetLastError = true)]
    static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);
    private const int GwlStyle = -16;
    private const int WsSizebox = 0x00040000;
    private const int WsMaximizebox = 0x00010000;

    public void SetWindowSize(int width, int height,Window window)
    {
        nint hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window); // Get window handle
        var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

        // Desired window size
        int windowWidth = width;
        int windowHeight = height;

        // Get the display size
        DisplayArea displayArea = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary);
        int screenWidth = displayArea.WorkArea.Width;
        int screenHeight = displayArea.WorkArea.Height;

        // Calculate centered position
        int centerX = (screenWidth - windowWidth) / 2;
        int centerY = (screenHeight - windowHeight) / 2;

        // Resize and move to center
        appWindow.Resize(new SizeInt32(windowWidth, windowHeight));
        appWindow.Move(new PointInt32(centerX, centerY));

        // Disable resizing (Remove WS_SIZEBOX and WS_MAXIMIZEBOX)
        int style = GetWindowLong(hwnd, GwlStyle);
        style &= ~WsSizebox;     // Disable resizing
        style &= ~WsMaximizebox; // Disable maximize button
        SetWindowLong(hwnd, GwlStyle, style);

        // Apply presenter mode (optional)
        appWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
    }
}
