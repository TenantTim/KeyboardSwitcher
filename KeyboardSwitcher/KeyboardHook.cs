using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
	public class KeyboardHook
	{
		#region Fields
		private const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;
		private const int WM_SYSKEYDOWN = 0x104;
		private const int WM_SYSKEYUP = 0x105;

		// Global events.
		public event KeyEventHandler OnKeyDownEvent;
		public event KeyEventHandler OnKeyUpEvent;
		public event KeyPressEventHandler OnKeyPressEvent;

		// The handle of keyboard hook.
		static int hKeyboardHook = 0;

		//keyboard hook constant.
		public const int WH_KEYBOARD_LL = 13;

		public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);

		// Declare the keyboard hook event type. 
		HookProc KeyboardHookProcedure;
		#endregion

		#region DLL Imports
		// Functions to hook.
		[DllImport("user32.dll ", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

		// Function to unhook.
		[DllImport("user32.dll ", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern bool UnhookWindowsHookEx(int idHook);

		// Function for next hook.
		[DllImport("user32.dll ", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

		[DllImport("user32 ")]
		public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

		[DllImport("user32 ")]
		public static extern int GetKeyboardState(byte[] pbKeyState);

		[DllImport("kernel32.dll")]
		public static extern int GetCurrentThreadId();

		[DllImport("kernel32.dll")]
		public static extern IntPtr GetModuleHandle(string name);
		#endregion

		public KeyboardHook()
		{
			Start();
		}

		~KeyboardHook()
		{
			Stop();
		}

		public void Start()
		{
			// Set the keyboard hook.
			if (hKeyboardHook == 0)
			{
				KeyboardHookProcedure = new HookProc(KeyboardHookProc);

				hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL,
				   KeyboardHookProcedure,
				   GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName),
				   0);

				if (hKeyboardHook == 0)
				{
					Stop();
					throw new Exception("SetWindowsHookEx is failed. ");
				}

			}
		}

		public void Stop()
		{
			bool retKeyboard = true;

			if (hKeyboardHook != 0)
			{
				retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
				hKeyboardHook = 0;
			}

			// If unhook failed.
			if (!retKeyboard)
			{
				throw new Exception("UnhookWindowsHookEx failed. ");
			}
		}

		private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
		{
			if ((nCode >= 0) && (OnKeyDownEvent != null || OnKeyUpEvent != null || OnKeyPressEvent != null))
			{
				KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

				// Trigger OnKeyDownEvent 
				if (OnKeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
				{
					Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
					KeyEventArgs e = new KeyEventArgs(keyData);
					OnKeyDownEvent(this, e);
				}

				// Trigger OnKeyPressEvent 
				if (OnKeyPressEvent != null && wParam == WM_KEYDOWN)
				{
					byte[] keyState = new byte[256];
					GetKeyboardState(keyState);

					byte[] inBuffer = new byte[2];
					if (ToAscii(MyKeyboardHookStruct.vkCode,
					  MyKeyboardHookStruct.scanCode,
					  keyState,
					  inBuffer,
					  MyKeyboardHookStruct.flags) == 1)
					{
						KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
						OnKeyPressEvent(this, e);
					}
				}

				// Trigger OnKeyUpEvent 
				if (OnKeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
				{
					Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
					KeyEventArgs e = new KeyEventArgs(keyData);
					OnKeyUpEvent(this, e);
				}
			}

			return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
		}
	}

	// Declare the keyboard hook struct type.
	[StructLayout(LayoutKind.Sequential)]
	public class KeyboardHookStruct
	{
		// Virtual keyboard code, from 1 ~ 254.
		public int vkCode;

		// Hardware scan code.
		public int scanCode;

		public int flags;

		public int time;

		public int dwExtraInfo;
	}
}