using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyboardSwitcher
{
	public class UserSetting
	{
		private static Dictionary<int, Keys> c_DefaultKeys = new Dictionary<int, Keys>
		{
			{0, Keys.Insert},
			{1, Keys.OemPipe},
			{2, Keys.End},
			{3, Keys.PageDown},
			{4, Keys.PageUp},
		};

		internal UserSetting()
		{
			AllSettings = new RemoteDesktopShortCutSetting[5];
			for (int i = 0; i < 5; i++)
			{
				this[i] = new RemoteDesktopShortCutSetting()
				{
					Title = string.Empty,
					ShortcutKey = c_DefaultKeys[i]
				};
			}
		}

		public static UserSetting Default
		{
			get
			{
				return new UserSetting();
			}
		}

		private RemoteDesktopShortCutSetting[] m_remoteDesktopShortCutSettings;

		public RemoteDesktopShortCutSetting this[int index]
		{
			get { return m_remoteDesktopShortCutSettings[index]; }
			set { m_remoteDesktopShortCutSettings[index] = value; }
		}

		public RemoteDesktopShortCutSetting[] AllSettings
		{
			get { return m_remoteDesktopShortCutSettings; }
			set { m_remoteDesktopShortCutSettings = value; }
		}
	}

	public class RemoteDesktopShortCutSetting
	{
		public string Title { get; set; }

		public Keys? ShortcutKey { get; set; }
	}
}