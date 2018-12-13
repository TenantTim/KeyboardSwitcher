using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KeyboardSwitcher
{
	public class ExternalAppController
	{
		[DllImport("user32.dll")]
		private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

		public static void SetWindowState(IntPtr hWnd, AppState appState)
		{
			if (!hWnd.Equals(IntPtr.Zero))
			{
				ShowWindowAsync(hWnd, (int)appState);
			}
		}

		public static List<IntPtr> GetWindowHandleByTitle(string title)
		{
			List<IntPtr> result = new List<IntPtr>();

			foreach (Process proc in Process.GetProcesses())
			{
				if (proc.MainWindowTitle.Contains(title))
				{
					IntPtr handle = proc.MainWindowHandle;
					result.Add(handle);
				}
			}

			return result;
		}
	}

	public enum AppState
	{
		SW_HIDE = 0,
		SW_SHOWNORMAL = 1,
		SW_SHOWMINIMIZED = 2,
		SW_SHOWMAXIMIZED = 3,
		SW_SHOWNOACTIVATE = 4,
		SW_RESTORE = 9,
		SW_SHOWDEFAULT = 10
	}
}