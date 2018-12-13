using System.Windows.Forms;

namespace KeyboardSwitcher
{
	partial class Form1
	{
		private System.ComponentModel.IContainer components = null;

		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private const int c_titleLabelPositionX = 9;
		private const int c_titleLabelPositionY = 8;
		private const int c_titleLabelSizeX = 83;
		private const int c_remoteLabelSizeX = 149;
		private const int c_titleLabelSizeY = 12;

		private const int c_titleTextBoxX = 10;
		private const int c_titleTextBoxY = 23;
		private const int c_titleTextBoxSizeX = 259;
		private const int c_titleTextBoxSizeY = 21;

		private const int c_titleCtrlOutHomeLabelX = 275;
		private const int c_titleCtrlOutHomeLabelY = 27;
		private const int c_titleCtrlOutHomeLabelSizeX = 108;
		private const int c_titleCtrlOutHomeLabelSizeY = 12;

		private const int c_titleKeyboardLabelX = 395;
		private const int c_titleKeyboardLabelY = 23;
		private const int c_titleKeyboardLabelSizeX = 94;
		private const int c_titleKeyboardLabelSizeY = 19;

		private const int c_ySpacing = 40;

		private const string c_localDesktopTitle = "Local Desktop:";
		private const string c_ctrlAltHomeText = @"Ctrl + Alt + Home, ";
		private const string c_homeString = "home";

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));

			InitializeControls();
			SetLocalDesktopControls();
			SetRemoteDesktopControls();
			SetButtons();
			SetToolTips(resources);

			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(500, 300);

			this.Controls.Add(m_localDesktopTitle);
			this.Controls.Add(m_localDesktopTextBox);
			this.Controls.Add(m_localDesktopCtrlAltHomeLabel);
			this.Controls.Add(m_localDesktopKeyboardBindingLabel);

			for (int i = 0; i < 5; i++)
			{
				this.Controls.Add(m_remoteDesktopTitles[i]);
				this.Controls.Add(m_textBoxes[i]);
				this.Controls.Add(m_ctrlAltHomeLabels[i]);
				this.Controls.Add(m_keyboardBindingLabels[i]);
			}

			this.Controls.Add(m_saveButton);
			this.Controls.Add(m_saveCloseButton);

			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Remote Desktop Switcher";
			this.TopMost = true;
			m_contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		private void InitializeControls()
		{
			m_localDesktopTextBox = new TextBox();
			m_localDesktopTitle = new Label();
			m_localDesktopCtrlAltHomeLabel = new Label();
			m_localDesktopKeyboardBindingLabel = new Label();

			m_textBoxes = new TextBox[5];
			m_remoteDesktopTitles = new Label[5];
			m_ctrlAltHomeLabels = new Label[5];
			m_keyboardBindingLabels = new Label[5];

			for (int i = 0; i < 5; i++)
			{
				m_textBoxes[i] = new TextBox();
				m_remoteDesktopTitles[i] = new Label();
				m_ctrlAltHomeLabels[i] = new Label();
				m_keyboardBindingLabels[i] = new Label();
			}

			m_notifyIcon = new NotifyIcon(components);
			m_contextMenuStrip = new ContextMenuStrip(components);
			m_openToolStripMenuItem = new ToolStripMenuItem();
			m_exitToolStripMenuItem = new ToolStripMenuItem();

			m_saveButton = new Button();
			m_saveCloseButton = new Button();

			m_contextMenuStrip.SuspendLayout();
			SuspendLayout();
		}

		private void SetLocalDesktopControls()
		{
			// Title label
			m_localDesktopTitle.AutoSize = true;
			m_localDesktopTitle.Location = new System.Drawing.Point(c_titleLabelPositionX, c_titleLabelPositionY);
			m_localDesktopTitle.Name = "LocalDesktopTitle";
			m_localDesktopTitle.Size = new System.Drawing.Size(c_titleLabelSizeX, c_titleLabelSizeY);
			m_localDesktopTitle.TabIndex = 0;
			m_localDesktopTitle.Text = c_localDesktopTitle;

			// Textbox
			m_localDesktopTextBox.BackColor = System.Drawing.SystemColors.Window;
			m_localDesktopTextBox.Enabled = false;
			m_localDesktopTextBox.Location = new System.Drawing.Point(c_titleTextBoxX, c_titleTextBoxY);
			m_localDesktopTextBox.Name = "LocalDesktopTextBox";
			m_localDesktopTextBox.Size = new System.Drawing.Size(c_titleTextBoxSizeX, c_titleTextBoxSizeY);
			m_localDesktopTextBox.TabIndex = 0;

			// CtrlAltHome
			m_localDesktopCtrlAltHomeLabel.AutoSize = true;
			m_localDesktopCtrlAltHomeLabel.Location = new System.Drawing.Point(c_titleCtrlOutHomeLabelX, c_titleCtrlOutHomeLabelY);
			m_localDesktopCtrlAltHomeLabel.Name = "LocalDesktopCtrlAltHomeLabel";
			m_localDesktopCtrlAltHomeLabel.Size = new System.Drawing.Size(c_titleCtrlOutHomeLabelSizeX, c_titleCtrlOutHomeLabelSizeY);
			m_localDesktopCtrlAltHomeLabel.TabIndex = 0;
			m_localDesktopCtrlAltHomeLabel.Text = c_ctrlAltHomeText;

			// Home label
			m_localDesktopKeyboardBindingLabel.BackColor = System.Drawing.SystemColors.Control;
			m_localDesktopKeyboardBindingLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			m_localDesktopKeyboardBindingLabel.Location = new System.Drawing.Point(c_titleKeyboardLabelX, c_titleKeyboardLabelY);
			m_localDesktopKeyboardBindingLabel.Name = "lblKB0";
			m_localDesktopKeyboardBindingLabel.Size = new System.Drawing.Size(c_titleKeyboardLabelSizeX, c_titleKeyboardLabelSizeY);
			m_localDesktopKeyboardBindingLabel.TabIndex = 0;
			m_localDesktopKeyboardBindingLabel.Text = c_homeString;
			m_localDesktopKeyboardBindingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		}

		private void SetRemoteDesktopControls()
		{
			for (int i = 0; i < 5; i++)
			{
				// Title label
				m_remoteDesktopTitles[i].AutoSize = true;
				m_remoteDesktopTitles[i].Location = new System.Drawing.Point(c_titleLabelPositionX, c_titleLabelPositionY + (i + 1) * c_ySpacing);
				m_remoteDesktopTitles[i].Size = new System.Drawing.Size(c_remoteLabelSizeX, c_titleLabelSizeY);
				m_remoteDesktopTitles[i].Name = string.Format("lblKB{0}", i + 1);
				m_remoteDesktopTitles[i].Text = string.Format("Title of Remote desktop {0}", i + 1);
				m_remoteDesktopTitles[i].TabIndex = 0;

				// TextBox
				m_textBoxes[i].Location = new System.Drawing.Point(c_titleTextBoxX, c_titleTextBoxY + (i + 1) * c_ySpacing);
				m_textBoxes[i].Size = new System.Drawing.Size(c_titleTextBoxSizeX, c_titleTextBoxSizeY);
				m_textBoxes[i].Name = string.Format("TextBox{0}", i + 1);
				m_textBoxes[i].TabIndex = i;

				// CtrlAltHome
				m_ctrlAltHomeLabels[i].AutoSize = true;
				m_ctrlAltHomeLabels[i].Location = new System.Drawing.Point(c_titleCtrlOutHomeLabelX, c_titleCtrlOutHomeLabelY + (i + 1) * c_ySpacing);
				m_ctrlAltHomeLabels[i].Size = new System.Drawing.Size(c_titleCtrlOutHomeLabelSizeX, c_titleCtrlOutHomeLabelSizeY);
				m_ctrlAltHomeLabels[i].TabIndex = 0;
				m_ctrlAltHomeLabels[i].Name = string.Format("CtrlAltHomeLabel{0}", i + 1);
				m_ctrlAltHomeLabels[i].Text = c_ctrlAltHomeText;

				// Keyboard
				m_keyboardBindingLabels[i].BackColor = System.Drawing.SystemColors.Control;
				m_keyboardBindingLabels[i].BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
				m_keyboardBindingLabels[i].Cursor = System.Windows.Forms.Cursors.Hand;
				m_keyboardBindingLabels[i].Location = new System.Drawing.Point(c_titleKeyboardLabelX, c_titleKeyboardLabelY + (i + 1) * c_ySpacing);
				m_keyboardBindingLabels[i].Size = new System.Drawing.Size(c_titleKeyboardLabelSizeX, c_titleKeyboardLabelSizeY);
				m_keyboardBindingLabels[i].TabIndex = 0;
				m_keyboardBindingLabels[i].Name = string.Format("KeyboardBindingLabel{0}", i + 1);
				m_keyboardBindingLabels[i].TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			}
		}

		private void SetButtons()
		{
			m_saveButton.Location = new System.Drawing.Point(296, 265);
			m_saveButton.Name = "btnSave";
			m_saveButton.Size = new System.Drawing.Size(94, 21);
			m_saveButton.TabIndex = 6;
			m_saveButton.Text = "Save";
			m_saveButton.UseVisualStyleBackColor = true;

			m_saveCloseButton.AccessibleName = "btnSaveClose";
			m_saveCloseButton.Location = new System.Drawing.Point(396, 265);
			m_saveCloseButton.Name = "btnSaveClose";
			m_saveCloseButton.Size = new System.Drawing.Size(94, 21);
			m_saveCloseButton.TabIndex = 6;
			m_saveCloseButton.Text = @"Save close";
			m_saveCloseButton.UseVisualStyleBackColor = true;
		}

		private void SetToolTips(System.ComponentModel.ComponentResourceManager resources)
		{
			m_notifyIcon.ContextMenuStrip = this.m_contextMenuStrip;
			m_notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
			m_notifyIcon.Text = "notifyIcon1";
			m_notifyIcon.Visible = true;

			m_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			m_openToolStripMenuItem,
			m_exitToolStripMenuItem});
			m_contextMenuStrip.Name = "contextMenuStrip1";
			m_contextMenuStrip.Size = new System.Drawing.Size(104, 48);

			m_openToolStripMenuItem.Name = "openToolStripMenuItem";
			m_openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			m_openToolStripMenuItem.Text = "Open";

			m_exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			m_exitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			m_exitToolStripMenuItem.Text = "Exit";
		}

		private TextBox m_localDesktopTextBox;
		private Label m_localDesktopTitle;
		private Label m_localDesktopCtrlAltHomeLabel;
		private Label m_localDesktopKeyboardBindingLabel;

		private TextBox[] m_textBoxes;
		private Label[] m_remoteDesktopTitles;
		private Label[] m_ctrlAltHomeLabels;
		private Label[] m_keyboardBindingLabels;

		private Button m_saveButton;
		private Button m_saveCloseButton;

		private NotifyIcon m_notifyIcon;
		private ContextMenuStrip m_contextMenuStrip;
		private ToolStripMenuItem m_openToolStripMenuItem;
		private ToolStripMenuItem m_exitToolStripMenuItem;
	}
}