namespace Kizuna.Plus.WinMvcForm.Framework.Views.CommonDialog
{
    partial class SettingForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.applyButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.oKButton = new System.Windows.Forms.Button();
            this.settingPanel = new System.Windows.Forms.Panel();
            this.buttonPanel = new System.Windows.Forms.Panel();
            this.menuImageList = new System.Windows.Forms.ImageList(this.components);
            this.menuPanel = new Kizuna.Plus.WinMvcForm.Framework.Views.CommonControl.ImageMenuControl();
            this.homeButton = new System.Windows.Forms.Button();
            this.rootButton = new System.Windows.Forms.Button();
            this.buttonPanel.SuspendLayout();
            this.menuPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(467, 9);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 0;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(386, 9);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // oKButton
            // 
            this.oKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.oKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.oKButton.Location = new System.Drawing.Point(305, 9);
            this.oKButton.Name = "oKButton";
            this.oKButton.Size = new System.Drawing.Size(75, 23);
            this.oKButton.TabIndex = 2;
            this.oKButton.Text = "OK";
            this.oKButton.UseVisualStyleBackColor = true;
            // 
            // settingPanel
            // 
            this.settingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingPanel.Location = new System.Drawing.Point(0, 54);
            this.settingPanel.Name = "settingPanel";
            this.settingPanel.Size = new System.Drawing.Size(554, 376);
            this.settingPanel.TabIndex = 3;
            // 
            // buttonPanel
            // 
            this.buttonPanel.Controls.Add(this.applyButton);
            this.buttonPanel.Controls.Add(this.cancelButton);
            this.buttonPanel.Controls.Add(this.oKButton);
            this.buttonPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonPanel.Location = new System.Drawing.Point(0, 430);
            this.buttonPanel.Name = "buttonPanel";
            this.buttonPanel.Size = new System.Drawing.Size(554, 44);
            this.buttonPanel.TabIndex = 5;
            // 
            // menuImageList
            // 
            this.menuImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("menuImageList.ImageStream")));
            this.menuImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.menuImageList.Images.SetKeyName(0, "homeButton");
            this.menuImageList.Images.SetKeyName(1, "rootButton");
            // 
            // menuPanel
            // 
            this.menuPanel.BackColor = System.Drawing.SystemColors.Window;
            this.menuPanel.Controls.Add(this.homeButton);
            this.menuPanel.Controls.Add(this.rootButton);
            this.menuPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuPanel.ImageList = this.menuImageList;
            this.menuPanel.Location = new System.Drawing.Point(0, 0);
            this.menuPanel.Name = "menuPanel";
            this.menuPanel.Size = new System.Drawing.Size(554, 54);
            this.menuPanel.TabIndex = 4;
            // 
            // homeButton
            // 
            this.homeButton.FlatAppearance.BorderSize = 0;
            this.homeButton.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.homeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.homeButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.homeButton.ImageKey = "homeButton";
            this.homeButton.ImageList = this.menuImageList;
            this.homeButton.Location = new System.Drawing.Point(3, 3);
            this.homeButton.Name = "homeButton";
            this.homeButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.homeButton.Size = new System.Drawing.Size(48, 48);
            this.homeButton.TabIndex = 6;
            this.homeButton.Text = "全般";
            this.homeButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.homeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.homeButton.UseVisualStyleBackColor = true;
            // 
            // rootButton
            // 
            this.rootButton.FlatAppearance.BorderSize = 0;
            this.rootButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rootButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.rootButton.ImageKey = "rootButton";
            this.rootButton.ImageList = this.menuImageList;
            this.rootButton.Location = new System.Drawing.Point(57, 3);
            this.rootButton.Name = "rootButton";
            this.rootButton.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.rootButton.Size = new System.Drawing.Size(48, 48);
            this.rootButton.TabIndex = 7;
            this.rootButton.Text = "演算";
            this.rootButton.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.rootButton.UseVisualStyleBackColor = true;
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 474);
            this.Controls.Add(this.settingPanel);
            this.Controls.Add(this.menuPanel);
            this.Controls.Add(this.buttonPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "設定";
            this.buttonPanel.ResumeLayout(false);
            this.menuPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button oKButton;
        private System.Windows.Forms.Panel settingPanel;
        private Kizuna.Plus.WinMvcForm.Framework.Views.CommonControl.ImageMenuControl menuPanel;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.ImageList menuImageList;
        private System.Windows.Forms.Button rootButton;
    }
}