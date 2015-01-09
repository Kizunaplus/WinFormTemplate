namespace Kizuna.Plus.WinMvcForm.Framework.Views.CommonDialog
{
    partial class AboutForm
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
            this.applicationImagePictureBox = new System.Windows.Forms.PictureBox();
            this.applicationNameLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.applicationDescriptionLabel = new System.Windows.Forms.Label();
            this.line1label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.applicationImagePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // applicationImagePictureBox
            // 
            this.applicationImagePictureBox.Location = new System.Drawing.Point(12, 12);
            this.applicationImagePictureBox.Name = "applicationImagePictureBox";
            this.applicationImagePictureBox.Size = new System.Drawing.Size(130, 130);
            this.applicationImagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.applicationImagePictureBox.TabIndex = 0;
            this.applicationImagePictureBox.TabStop = false;
            // 
            // applicationNameLabel
            // 
            this.applicationNameLabel.AutoSize = true;
            this.applicationNameLabel.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.applicationNameLabel.Location = new System.Drawing.Point(148, 12);
            this.applicationNameLabel.Name = "applicationNameLabel";
            this.applicationNameLabel.Size = new System.Drawing.Size(106, 18);
            this.applicationNameLabel.TabIndex = 1;
            this.applicationNameLabel.Text = "ApplicationName";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.closeButton.Location = new System.Drawing.Point(363, 160);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            // 
            // applicationDescriptionLabel
            // 
            this.applicationDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applicationDescriptionLabel.Location = new System.Drawing.Point(162, 49);
            this.applicationDescriptionLabel.Name = "applicationDescriptionLabel";
            this.applicationDescriptionLabel.Size = new System.Drawing.Size(276, 93);
            this.applicationDescriptionLabel.TabIndex = 3;
            this.applicationDescriptionLabel.Text = "Application Description";
            // 
            // line1label
            // 
            this.line1label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.line1label.Location = new System.Drawing.Point(12, 148);
            this.line1label.Name = "line1label";
            this.line1label.Size = new System.Drawing.Size(426, 1);
            this.line1label.TabIndex = 4;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 190);
            this.Controls.Add(this.line1label);
            this.Controls.Add(this.applicationDescriptionLabel);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.applicationNameLabel);
            this.Controls.Add(this.applicationImagePictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "バージョン情報";
            this.Load += new System.EventHandler(this.AbountForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.applicationImagePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox applicationImagePictureBox;
        private System.Windows.Forms.Label applicationNameLabel;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label applicationDescriptionLabel;
        private System.Windows.Forms.Label line1label;
    }
}