using System;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			bool result;
			var mutex = new System.Threading.Mutex(true, "Remote Desktop Switcher", out result);

			if (!result)
			{
				MessageBox.Show("Another instance is already running.");
				return;
			}
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
			GC.KeepAlive(mutex);
		}
	}
}