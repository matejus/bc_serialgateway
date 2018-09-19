namespace Adastra.BigClownGateway.GUI
{
    partial class ConsoleWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cbxOccupied = new System.Windows.Forms.CheckBox();
            this.cbxOpen = new System.Windows.Forms.CheckBox();
            this.cbxEnabled = new System.Windows.Forms.CheckBox();
            this.panMsg = new System.Windows.Forms.Panel();
            this.tbMessage = new System.Windows.Forms.ComboBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cbCom = new System.Windows.Forms.ComboBox();
            this.tbResponse = new System.Windows.Forms.TextBox();
            this.lblBcId = new System.Windows.Forms.Label();
            this.lblDeviceID = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnWatcher = new System.Windows.Forms.Button();
            this.tbWatcherLog = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panMsg.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cbxOccupied);
            this.splitContainer1.Panel1.Controls.Add(this.cbxOpen);
            this.splitContainer1.Panel1.Controls.Add(this.cbxEnabled);
            this.splitContainer1.Panel1.Controls.Add(this.btnClear);
            this.splitContainer1.Panel1.Controls.Add(this.panMsg);
            this.splitContainer1.Panel1.Controls.Add(this.cbCom);
            this.splitContainer1.Panel1.Controls.Add(this.tbResponse);
            this.splitContainer1.Panel1.Controls.Add(this.lblBcId);
            this.splitContainer1.Panel1.Controls.Add(this.lblDeviceID);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.btnOpen);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.btnWatcher);
            this.splitContainer1.Panel2.Controls.Add(this.tbWatcherLog);
            this.splitContainer1.Size = new System.Drawing.Size(1604, 728);
            this.splitContainer1.SplitterDistance = 1054;
            this.splitContainer1.TabIndex = 0;
            // 
            // cbxOccupied
            // 
            this.cbxOccupied.AutoSize = true;
            this.cbxOccupied.Enabled = false;
            this.cbxOccupied.Location = new System.Drawing.Point(331, 14);
            this.cbxOccupied.Name = "cbxOccupied";
            this.cbxOccupied.Size = new System.Drawing.Size(72, 17);
            this.cbxOccupied.TabIndex = 8;
            this.cbxOccupied.Text = "Occupied";
            this.cbxOccupied.UseVisualStyleBackColor = true;
            // 
            // cbxOpen
            // 
            this.cbxOpen.AutoSize = true;
            this.cbxOpen.Enabled = false;
            this.cbxOpen.Location = new System.Drawing.Point(252, 14);
            this.cbxOpen.Name = "cbxOpen";
            this.cbxOpen.Size = new System.Drawing.Size(60, 17);
            this.cbxOpen.TabIndex = 8;
            this.cbxOpen.Text = "IsOpen";
            this.cbxOpen.UseVisualStyleBackColor = true;
            // 
            // cbxEnabled
            // 
            this.cbxEnabled.AutoSize = true;
            this.cbxEnabled.Enabled = false;
            this.cbxEnabled.Location = new System.Drawing.Point(173, 14);
            this.cbxEnabled.Name = "cbxEnabled";
            this.cbxEnabled.Size = new System.Drawing.Size(73, 17);
            this.cbxEnabled.TabIndex = 8;
            this.cbxEnabled.Text = "IsEnabled";
            this.cbxEnabled.UseVisualStyleBackColor = true;
            // 
            // panMsg
            // 
            this.panMsg.Controls.Add(this.tbMessage);
            this.panMsg.Controls.Add(this.btnSend);
            this.panMsg.Controls.Add(this.button1);
            this.panMsg.Enabled = false;
            this.panMsg.Location = new System.Drawing.Point(47, 38);
            this.panMsg.Name = "panMsg";
            this.panMsg.Size = new System.Drawing.Size(1004, 27);
            this.panMsg.TabIndex = 6;
            // 
            // tbMessage
            // 
            this.tbMessage.FormattingEnabled = true;
            this.tbMessage.Location = new System.Drawing.Point(3, 3);
            this.tbMessage.Name = "tbMessage";
            this.tbMessage.Size = new System.Drawing.Size(608, 21);
            this.tbMessage.TabIndex = 5;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(617, 3);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(121, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(774, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "GW Detail";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbCom
            // 
            this.cbCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCom.FormattingEnabled = true;
            this.cbCom.Location = new System.Drawing.Point(47, 11);
            this.cbCom.Name = "cbCom";
            this.cbCom.Size = new System.Drawing.Size(120, 21);
            this.cbCom.TabIndex = 4;
            this.cbCom.SelectedValueChanged += new System.EventHandler(this.cbCom_SelectedValueChanged);
            // 
            // tbResponse
            // 
            this.tbResponse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResponse.Location = new System.Drawing.Point(3, 65);
            this.tbResponse.Multiline = true;
            this.tbResponse.Name = "tbResponse";
            this.tbResponse.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbResponse.Size = new System.Drawing.Size(1048, 660);
            this.tbResponse.TabIndex = 0;
            this.tbResponse.WordWrap = false;
            // 
            // lblBcId
            // 
            this.lblBcId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBcId.Location = new System.Drawing.Point(409, 15);
            this.lblBcId.Name = "lblBcId";
            this.lblBcId.Size = new System.Drawing.Size(151, 13);
            this.lblBcId.TabIndex = 2;
            this.lblBcId.Text = "BcID";
            // 
            // lblDeviceID
            // 
            this.lblDeviceID.Location = new System.Drawing.Point(566, 15);
            this.lblDeviceID.Name = "lblDeviceID";
            this.lblDeviceID.Size = new System.Drawing.Size(249, 13);
            this.lblDeviceID.TabIndex = 2;
            this.lblDeviceID.Text = "--";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port:";
            // 
            // btnOpen
            // 
            this.btnOpen.Enabled = false;
            this.btnOpen.Location = new System.Drawing.Point(821, 9);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(121, 23);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.Text = "Open";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnWatcher
            // 
            this.btnWatcher.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWatcher.Location = new System.Drawing.Point(3, 3);
            this.btnWatcher.Name = "btnWatcher";
            this.btnWatcher.Size = new System.Drawing.Size(540, 23);
            this.btnWatcher.TabIndex = 1;
            this.btnWatcher.Text = "Start / Stop watching";
            this.btnWatcher.UseVisualStyleBackColor = true;
            this.btnWatcher.Click += new System.EventHandler(this.btnWatcher_Click);
            // 
            // tbWatcherLog
            // 
            this.tbWatcherLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWatcherLog.Location = new System.Drawing.Point(3, 29);
            this.tbWatcherLog.Multiline = true;
            this.tbWatcherLog.Name = "tbWatcherLog";
            this.tbWatcherLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbWatcherLog.Size = new System.Drawing.Size(540, 696);
            this.tbWatcherLog.TabIndex = 0;
            this.tbWatcherLog.WordWrap = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(948, 10);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(91, 23);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ConsoleWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1604, 728);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ConsoleWindow";
            this.Text = "ConsoleWindow";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panMsg.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnWatcher;
        private System.Windows.Forms.TextBox tbWatcherLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.TextBox tbResponse;
        private System.Windows.Forms.ComboBox cbCom;
        private System.Windows.Forms.ComboBox tbMessage;
        private System.Windows.Forms.Panel panMsg;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox cbxEnabled;
        private System.Windows.Forms.CheckBox cbxOccupied;
        private System.Windows.Forms.CheckBox cbxOpen;
        private System.Windows.Forms.Label lblDeviceID;
        private System.Windows.Forms.Label lblBcId;
        private System.Windows.Forms.Button btnClear;
    }
}