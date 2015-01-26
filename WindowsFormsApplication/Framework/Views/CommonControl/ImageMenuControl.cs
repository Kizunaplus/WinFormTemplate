using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework.Views.CommonControl
{
    /// <summary>
    /// 画像メニュー表示コントロール
    /// </summary>
    public partial class ImageMenuControl : FlowLayoutPanel
    {
        #region メンバー変数
        /// <summary>
        /// ボタンサイズ
        /// </summary>
        private Size buttonSize;

        /// <summary>
        /// イメージリスト
        /// </summary>
        private ImageList imageList;

        /// <summary>
        /// 内在するコントロールの感覚
        /// </summary>
        private Padding internalPadding;
        #endregion

        #region プロパティ
        /// <summary>
        /// ボタンサイズ
        /// </summary>
        [DefaultValue(typeof(Size), "48,48")]
        public Size ButtonSize
        {
            get
            {
                return buttonSize;
            }
            set
            {
                buttonSize = value;
                ChangeButtonSize();
            }
        }

        /// <summary>
        /// イメージリスト
        /// </summary>
        [DefaultValue(typeof(ImageList), null)]
        public ImageList ImageList
        {
            get
            {
                return imageList;
            }
            set
            {
                imageList = value;
                ChangeButtonImage();
            }
        }

        /// <summary>
        /// 内在するコントロールの感覚
        /// </summary>
        [DefaultValue(typeof(Padding), "5,0,5,0")]
        public Padding InternalPadding
        {
            get
            {
                return internalPadding;
            }
            set
            {
                internalPadding = value;
                ChangeButtonPadding();
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ImageMenuControl()
        {
            InitializeComponent();

            // ボタンサイズ 48 * 48
            this.ButtonSize = new System.Drawing.Size(48, 48);
            // 内在するコントロールの間隔
            this.internalPadding = new Padding(5, 0, 5, 0);
        }
        #endregion

        #region イベント
        /// <summary>
        /// コントロールを追加した際のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageMenuControl_ControlAdded(object sender, ControlEventArgs e)
        {
            ChangeButtonSize();

            Button button = e.Control as Button;
            if (button != null)
            {
                button.ImageList = null;
                button.ImageKey = null;
                if (this.ImageList != null
                    && this.ImageList.Images.ContainsKey(button.Name) == true)
                {
                    button.ImageList = this.ImageList;
                    button.ImageKey = button.Name;

                    button.TextAlign = ContentAlignment.BottomCenter;
                }
                toolTip.SetToolTip(button, button.Text);
            }
        }
        #endregion

        #region ボタンスタイル変更
        /// <summary>
        /// ボタンサイズを登録されているボタンへ反映する。
        /// </summary>
        private void ChangeButtonSize()
        {
            MethodInvoker methodDelegate = delegate()
            {
                // サイズを統一する。
                foreach (Control control in Controls)
                {
                    control.Size = this.ButtonSize;
                }
            };

            if (this.InvokeRequired == true)
            {
                this.Invoke(methodDelegate);
            }
            else
            {
                methodDelegate();
            }
        }

        /// <summary>
        /// ボタンのイメージ設定を変更
        /// </summary>
        private void ChangeButtonImage()
        {
            foreach (Control control in this.Controls)
            {
                Button button = control as Button;
                if (button != null)
                {
                    button.ImageList = null;
                    button.ImageKey = null;
                    if (this.ImageList != null
                        && this.ImageList.Images.ContainsKey(button.Name) == true)
                    {
                        button.ImageList = this.ImageList;
                        button.ImageKey = button.Name;

                        button.ImageAlign = ContentAlignment.TopCenter;
                        button.TextAlign = ContentAlignment.BottomCenter;
                    }
                    toolTip.SetToolTip(button, button.Text);
                }
            }
        }

        /// <summary>
        /// ボタンの間隔変更
        /// </summary>
        private void ChangeButtonPadding()
        {
            foreach (Control control in this.Controls)
            {
                control.Padding = internalPadding;
            }
        }
        #endregion
    }
}
