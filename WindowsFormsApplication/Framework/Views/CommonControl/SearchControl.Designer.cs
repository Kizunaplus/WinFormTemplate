namespace Kizuna.Plus.WinMvcForm.Framework.Views.CommonControl
{
    partial class SearchControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.prevSearchButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.nextSearchButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // searchTextBox
            // 
            this.searchTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.searchTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.searchTextBox.Location = new System.Drawing.Point(3, 5);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(190, 19);
            this.searchTextBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.searchTextBox, "検索語句");
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // prevSearchButton
            // 
            this.prevSearchButton.Enabled = false;
            this.prevSearchButton.FlatAppearance.BorderSize = 0;
            this.prevSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.prevSearchButton.Image = global::WindowsFormsApplication.Properties.Resources.MovePrevious_7195;
            this.prevSearchButton.Location = new System.Drawing.Point(197, 2);
            this.prevSearchButton.Name = "prevSearchButton";
            this.prevSearchButton.Size = new System.Drawing.Size(23, 23);
            this.prevSearchButton.TabIndex = 3;
            this.toolTip.SetToolTip(this.prevSearchButton, "前を検索");
            this.prevSearchButton.UseVisualStyleBackColor = true;
            this.prevSearchButton.Click += new System.EventHandler(this.prevSearchButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.FlatAppearance.BorderSize = 0;
            this.closeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeButton.Image = global::WindowsFormsApplication.Properties.Resources.Error_grey_677_16x16;
            this.closeButton.Location = new System.Drawing.Point(249, 2);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(21, 23);
            this.closeButton.TabIndex = 2;
            this.closeButton.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.toolTip.SetToolTip(this.closeButton, "閉じる");
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // nextSearchButton
            // 
            this.nextSearchButton.Enabled = false;
            this.nextSearchButton.FlatAppearance.BorderSize = 0;
            this.nextSearchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nextSearchButton.Image = global::WindowsFormsApplication.Properties.Resources.GotoNextRow_289;
            this.nextSearchButton.Location = new System.Drawing.Point(223, 2);
            this.nextSearchButton.Name = "nextSearchButton";
            this.nextSearchButton.Size = new System.Drawing.Size(23, 23);
            this.nextSearchButton.TabIndex = 1;
            this.toolTip.SetToolTip(this.nextSearchButton, "次を検索");
            this.nextSearchButton.UseVisualStyleBackColor = true;
            this.nextSearchButton.Click += new System.EventHandler(this.nextSearchButton_Click);
            // 
            // SearchControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.prevSearchButton);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.nextSearchButton);
            this.Controls.Add(this.searchTextBox);
            this.Name = "SearchControl";
            this.Size = new System.Drawing.Size(273, 27);
            this.Enter += new System.EventHandler(this.SearchControl_Enter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Button nextSearchButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button prevSearchButton;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
