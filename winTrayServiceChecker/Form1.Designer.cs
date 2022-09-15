namespace winTrayServiceChecker
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lvwLog = new System.Windows.Forms.ListView();
            this.colResult = new System.Windows.Forms.ColumnHeader();
            this.colHttpStatus = new System.Windows.Forms.ColumnHeader();
            this.colTimestamp = new System.Windows.Forms.ColumnHeader();
            this.colService = new System.Windows.Forms.ColumnHeader();
            this.imageListStatuses = new System.Windows.Forms.ImageList(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.timerCheckServices = new System.Windows.Forms.Timer(this.components);
            this.serviceStatusIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SuspendLayout();
            // 
            // lvwLog
            // 
            this.lvwLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colResult,
            this.colHttpStatus,
            this.colTimestamp,
            this.colService});
            this.lvwLog.Location = new System.Drawing.Point(12, 12);
            this.lvwLog.Name = "lvwLog";
            this.lvwLog.Size = new System.Drawing.Size(586, 469);
            this.lvwLog.SmallImageList = this.imageListStatuses;
            this.lvwLog.TabIndex = 0;
            this.lvwLog.UseCompatibleStateImageBehavior = false;
            this.lvwLog.UseWaitCursor = true;
            this.lvwLog.View = System.Windows.Forms.View.Details;
            // 
            // colResult
            // 
            this.colResult.Text = "";
            this.colResult.Width = 30;
            // 
            // colHttpStatus
            // 
            this.colHttpStatus.Text = "Status";
            this.colHttpStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colHttpStatus.Width = 130;
            // 
            // colTimestamp
            // 
            this.colTimestamp.Text = "Timestamp";
            this.colTimestamp.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colTimestamp.Width = 150;
            // 
            // colService
            // 
            this.colService.Text = "Service Name";
            this.colService.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colService.Width = 240;
            // 
            // imageListStatuses
            // 
            this.imageListStatuses.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListStatuses.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListStatuses.ImageStream")));
            this.imageListStatuses.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListStatuses.Images.SetKeyName(0, "StatusOK.png");
            this.imageListStatuses.Images.SetKeyName(1, "StatusInvalid.png");
            this.imageListStatuses.Images.SetKeyName(2, "StatusWarning.png");
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(473, 500);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(125, 29);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.UseWaitCursor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // serviceStatusIcon
            // 
            this.serviceStatusIcon.ContextMenuStrip = this.contextMenu;
            this.serviceStatusIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("serviceStatusIcon.Icon")));
            this.serviceStatusIcon.Visible = true;
            // 
            // contextMenu
            // 
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(608, 541);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.lvwLog);
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Service Checker Log";
            this.UseWaitCursor = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private ListView lvwLog;
        private ColumnHeader colResult;
        private ColumnHeader colHttpStatus;
        private ColumnHeader colService;
        private Button btnClear;
        private ColumnHeader colTimestamp;
        private System.Windows.Forms.Timer timerCheckServices;
        private ImageList imageListStatuses;
        private NotifyIcon serviceStatusIcon;
        private ContextMenuStrip contextMenu;
    }
}