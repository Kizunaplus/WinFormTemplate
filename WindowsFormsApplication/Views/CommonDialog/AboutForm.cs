using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication.Views.CommonDialog
{
    public partial class AboutForm : Form
    {
        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();
        }
        #endregion

        #region イベント
        /// <summary>
        /// フォームロードイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AbountForm_Load(object sender, EventArgs e)
        {
            FontFamily fontfamily = this.applicationNameLabel.Font.FontFamily;

            this.applicationNameLabel.Text = Application.ProductName;
            this.applicationNameLabel.Font = new Font(fontfamily, this.Font.Size + 5, FontStyle.Bold);

            // C#
            Assembly mainAssembly = Assembly.GetEntryAssembly();

            // コピーライト情報を取得
            string appCopyright = "-";
            object[] CopyrightArray =
              mainAssembly.GetCustomAttributes(
                typeof(AssemblyCopyrightAttribute), false);
            if ((CopyrightArray != null) && (CopyrightArray.Length > 0))
            {
                appCopyright =
                  ((AssemblyCopyrightAttribute)CopyrightArray[0]).Copyright;
            }

            // 詳細情報を取得
            string appDescription = "-";
            object[] DescriptionArray =
              mainAssembly.GetCustomAttributes(
                typeof(AssemblyDescriptionAttribute), false);
            if ((DescriptionArray != null) && (DescriptionArray.Length > 0))
            {
                appDescription =
                  ((AssemblyDescriptionAttribute)DescriptionArray[0]).Description;
            }
            this.applicationDescriptionLabel.Text = string.Format("Version: {0}\n\n{1}\n\n{2}", Application.ProductVersion, appCopyright, appDescription);

            // アイコンの取得
            var applicationIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            this.applicationImagePictureBox.Image = applicationIcon.ToBitmap();
        }

        /// <summary>
        /// クローズボタンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
