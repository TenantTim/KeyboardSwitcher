using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
	public partial class Form1 : Form
	{
		const string WAITING_KEYBOARD_MESSAGE = "press a key...";
		const string EMPTY_KEYBOARD_SHORTCUT = "N/A";

		KeyboardHook kbHook;
		UserSetting userSetting;
		bool userClosing = false;

		private String SettingPath
		{
			get
			{
				return Path.Combine(Environment.CurrentDirectory, "settings.xml");
			}
		}

		public Form1()
		{
			InitializeComponent();

			Load += Form1_Load;
			VisibleChanged += Form1_VisibleChanged;
			FormClosing += Form1_FormClosing;
			Disposed += Form1_Disposed;

			m_saveButton.Click += btnSave_Click;
			m_saveCloseButton.Click += btnSaveClose_Click;

			for (int i = 0; i < 5; i++)
			{
				m_keyboardBindingLabels[i].Click += lblKeyboard_Click;
				m_keyboardBindingLabels[i].KeyDown += lblKeyboard_KeyDown;
				m_keyboardBindingLabels[i].LostFocus += lblKeyboard_LostFocus;
			}

			m_notifyIcon.DoubleClick += notifyIcon1_DoubleClick;

			m_openToolStripMenuItem.Click += openToolStripMenuItem_Click;
			m_exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
		}

		void Form1_VisibleChanged(object sender, EventArgs e)
		{
			if (kbHook != null)
			{
				if (this.Visible)
				{
					kbHook.Stop();
				}
				else
				{
					kbHook.Start();
				}
			}
		}

		#region Events
		void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (e.CloseReason == CloseReason.UserClosing && !userClosing)
			{
				e.Cancel = true;
				MinimizeForm();
			}
			else
			{
				// User wants to exit the application.
			}

			userClosing = false;
		}

		void Form1_Disposed(object sender, EventArgs e)
		{
			if (kbHook != null)
			{
				kbHook.Stop();
			}
		}

		void Form1_Load(object sender, EventArgs e)
		{
			kbHook = new KeyboardHook();
			kbHook.Start();
			kbHook.OnKeyDownEvent += kbHook_OnKeyDownEvent;

			FileInfo fileInfo = new FileInfo(SettingPath);
			userSetting = UserSetting.Default;

			if (fileInfo.Exists)
			{
				try
				{
					userSetting.AllSettings = XmlPersister.DeSerializeObject<RemoteDesktopShortCutSetting[]>(SettingPath);
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception);
				}
			}

			if (userSetting != null)
			{
				for (int i = 0; i < 5; i++)
				{
					m_textBoxes[i].Text = userSetting[i].Title;
					m_keyboardBindingLabels[i].Text = userSetting[i].ShortcutKey.HasValue ? userSetting[i].ShortcutKey.ToString() : EMPTY_KEYBOARD_SHORTCUT;
					m_keyboardBindingLabels[i].Tag = userSetting[i].ShortcutKey;
				}
			}
		}

		void btnSaveClose_Click(object sender, EventArgs e)
		{
			SaveUserSetting();
			MinimizeForm();
		}

		void btnSave_Click(object sender, EventArgs e)
		{
			SaveUserSetting();
		}

		void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			if (this.Visible)
			{
				MinimizeForm();
			}
			else
			{
				ShowForm();
			}
		}

		void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ShowForm();
		}

		void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			userClosing = true;

			m_notifyIcon.Visible = false;
			m_notifyIcon.Dispose();

			this.Close();
			this.Dispose();

			System.Environment.Exit(0);
		}

		void lblKeyboard_Click(object sender, EventArgs e)
		{
			var label = sender as Label;
			if (label != null)
			{
				label.Focus();
				label.Text = WAITING_KEYBOARD_MESSAGE;
			}
		}

		void lblKeyboard_LostFocus(object sender, EventArgs e)
		{
			var label = sender as Label;
			if (label != null && label.Text == WAITING_KEYBOARD_MESSAGE)
			{
				label.Tag = null;
				label.Text = EMPTY_KEYBOARD_SHORTCUT;
			}
		}

		void lblKeyboard_KeyDown(object sender, KeyEventArgs e)
		{
			var label = sender as Label;
			if (label != null && label.Text == WAITING_KEYBOARD_MESSAGE)
			{
				label.Text = e.KeyCode.ToString();
				label.Tag = e.KeyCode;
			}
		}

		void kbHook_OnKeyDownEvent(object sender, KeyEventArgs e)
		{
			for (int i = 0; i < 5; i++)
			{
				if (userSetting[i].ShortcutKey.HasValue && e.KeyData == userSetting[i].ShortcutKey.Value && ModifierKeys == (Keys.Control | Keys.Alt))
				{
					MinimizeAllRemoteDesktopConnections();
					ShowRemoteDesktopByTitle(userSetting[i].Title);
				}
			}

			if (e.KeyData == Keys.Home
			   && Control.ModifierKeys == (Keys.Control | Keys.Alt))
			{
				MinimizeAllRemoteDesktopConnections();
			}
		}
		#endregion

		#region Private Methods
		private void MinimizeForm()
		{
			WindowState = FormWindowState.Minimized;
			m_notifyIcon.Visible = true;
			Hide();
		}

		private void ShowForm()
		{
			Visible = true;
			WindowState = FormWindowState.Normal;
			Activate();
		}

		private static void ShowRemoteDesktopByTitle(string title)
		{
			var rdHandles = ExternalAppController.GetWindowHandleByTitle(title);
			if (rdHandles != null && rdHandles.Count > 0)
			{
				var handle = rdHandles[0];
				ExternalAppController.SetWindowState(handle, AppState.SW_RESTORE);
			}
		}

		private void MinimizeAllRemoteDesktopConnections()
		{
			var rdHandles = ExternalAppController.GetWindowHandleByTitle("- Remote Desktop Connection");
			foreach (var handle in rdHandles)
			{
				ExternalAppController.SetWindowState(handle, AppState.SW_SHOWMINIMIZED);
			}
		}

		private void SaveUserSetting()
		{
			for (int i = 0; i < 5; i++)
			{
				userSetting[i] = new RemoteDesktopShortCutSetting()
				{
					Title = m_textBoxes[i].Text,
					ShortcutKey = m_keyboardBindingLabels[i].Tag == null ? (Keys?)null : (Keys)m_keyboardBindingLabels[i].Tag
				};
			}

			XmlPersister.SerializeObject<RemoteDesktopShortCutSetting[]>(userSetting.AllSettings, SettingPath);
		}
		#endregion
	}
}