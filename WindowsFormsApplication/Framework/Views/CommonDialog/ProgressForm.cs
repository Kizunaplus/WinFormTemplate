using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework.Views.CommonDialog
{
    /// <summary>
    /// 進捗表示ダイアログ
    /// </summary>
    public partial class ProgressForm : Form
    {
        #region イベント定義
        /// <summary>
        /// プログレスのキャンセル要求イベント
        /// </summary>
        public event CancelEventHandler ProgressChancelEvent;
        #endregion

        #region プロパティ
        /// <summary>
        /// プログレスバーの現在値を取得、設定します
        /// </summary>
        [DefaultValue(0)]
        public int Value
        {
            get { return this.progressControl.Value; }
            set { this.progressControl.Value = value; }
        }

        /// <summary>
        /// プログレスバーの最大値を取得、設定します
        /// </summary>
        [DefaultValue(100)]
        public int Maximum
        {
            get { return this.progressControl.Maximum; }
            set { this.progressControl.Maximum = value; }
        }

        /// <summary>
        /// プログレスバーの最小値を取得、設定します
        /// </summary>
        [DefaultValue(0)]
        public int Minimum
        {
            get { return this.progressControl.Minimum; }
            set { this.progressControl.Minimum = value; }
        }

        /// <summary>
        /// アニメーションスピード(ms)を取得、設定します
        /// </summary>
        [DefaultValue(100)]
        public int MarqueeAnimationSpeed
        {
            get { return this.progressControl.MarqueeAnimationSpeed; }
            set { this.progressControl.MarqueeAnimationSpeed = value; }
        }

        /// <summary>
        /// アニメーションスピード(ms)を取得、設定します
        /// </summary>
        [DefaultValue(ProgressBarStyle.Blocks)]
        public ProgressBarStyle Style
        {
            get { return this.progressControl.Style; }
            set { this.progressControl.Style = value; }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProgressForm()
        {
            InitializeComponent();
        }
        #endregion

        #region プログレス
        /// <summary>
        /// プログレス更新処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e)
        {
            if (this.progressControl == null)
            {
                return;
            }

            // 状態
            if (e.UserState != null)
            {
                this.descriptionLabel.Text = e.UserState.ToString();
            }

            // 移譲
            this.progressControl.ProgressChangedEventHandler(sender, e);
        }
        #endregion

        #region イベント
        /// <summary>
        /// プログレス完了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void progressControl_ProgressCompliteEvent(object sender, EventArgs e)
        {
            Thread.Sleep(200);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// フォームクローズイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != System.Windows.Forms.DialogResult.OK)
            {
                CancelEventArgs cancelArgs = new CancelEventArgs();
                OnProgressCancelEvent(ref cancelArgs);

                e.Cancel = cancelArgs.Cancel;
            }
        }
        #endregion

        #region イベント発行
        /// <summary>
        /// キャンセル要求イベント発行
        /// </summary>
        /// <param name="e"></param>
        protected void OnProgressCancelEvent(ref CancelEventArgs e)
        {
            if (ProgressChancelEvent != null)
            {
                ProgressChancelEvent(this, e);
            }
        }
        #endregion
    }
}
