namespace WindowsFormsApplication.Views.CommonControl
{
    partial class HelperControl
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
            if (backGroundImage != null)
            {
                backGroundImage.Dispose();
                backGroundImage = null;
            }

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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // HelperControl
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.HelpBalloon;
            this.Cursor = System.Windows.Forms.Cursors.Help;
            this.Name = "HelperControl";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HelperControl_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.HelperControl_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
    }
}
