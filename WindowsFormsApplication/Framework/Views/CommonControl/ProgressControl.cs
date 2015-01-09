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
    public partial class ProgressControl : ProgressBar
    {
        #region イベント定義
        /// <summary>
        /// 進捗が完了した場合に発生するイベント
        /// </summary>
        public event EventHandler ProgressCompliteEvent;
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ProgressControl()
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
            this.Value = e.ProgressPercentage;

            if (this.Maximum <= this.Value)
            {
                // 完了イベントの発行
                OnProgressCompliteEvent(EventArgs.Empty);
            }
        }
        #endregion

        #region イベント発行
        /// <summary>
        /// プログレス完了イベントの発行
        /// </summary>
        /// <param name="e"></param>
        protected void OnProgressCompliteEvent(EventArgs e)
        {
            if (ProgressCompliteEvent != null)
            {
                ProgressCompliteEvent(this, e);
            }
        }
        #endregion
    }
}
