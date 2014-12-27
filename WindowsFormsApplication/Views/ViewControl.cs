using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication.Models;
using WindowsFormsApplication.Controllers.Commands;
using WindowsFormsApplication.Controllers.State;

namespace WindowsFormsApplication.Views
{
    public class ViewControl : UserControl, IView
    {
        #region メンバー変数
        /// <summary>
        /// イベント割付データ一覧
        /// </summary>
        List<CommandEventData> commandEventDataList;

        /// <summary>
        /// 
        /// </summary>
        private CommonControl.HelperControl helperControl;
        #endregion

        #region プロパティ
        /// <summary>
        /// ヘルプ情報表示設定
        /// </summary>
        protected List<Tuple<Control, string>> HelpGuide
        {
            get
            {
                return this.helperControl.HelpGuide;
            }
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewControl()
        {
            // イベント割付データ一覧の取得
            commandEventDataList = GetCommandEventDataList();

            var eventData = new CommandEventData(typeof(Control), typeof(HelpCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                this.helperControl.Show(sender as Control);
            });
            commandEventDataList.Add(eventData);

            // ヘルプ表示
            this.helperControl = new WindowsFormsApplication.Views.CommonControl.HelperControl();
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            RegistViewEvent();
        }

        /// <summary>
        /// バインドデータの設定
        /// </summary>
        public virtual void InitBindData()
        {
        }
        #endregion

        #region 更新
        public void Refresh()
        {
            InitBindData();
        }
        #endregion

        #region イベント発行
        /// <summary>
        /// ロードイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (DesignMode == false)
            {
                this.helperControl.AssignControl = this;
                this.Controls.Add(this.helperControl);
                this.helperControl.BringToFront();
            }
        }
        #endregion

        #region イベント操作
        /// <summary>
        /// 表示イベントの登録
        /// </summary>
        private void RegistViewEvent()
        {
            if (commandEventDataList == null || commandEventDataList.Count <= 0)
            {
                // 未登録
                return;
            }

            var register = CommandRegister.Current;
            foreach (CommandEventData data in commandEventDataList)
            {
                register.Regist(data);
            }
        }

        /// <summary>
        /// 表示イベントの解除
        /// </summary>
        private void UnregistViewEvent()
        {
            if (commandEventDataList == null || commandEventDataList.Count <= 0)
            {
                // 未登録
                return;
            }

            var register = CommandRegister.Current;
            foreach (CommandEventData data in commandEventDataList)
            {
                register.Unregist(data);
            }
        }

        /// <summary>
        /// イベント一覧の取得
        /// </summary>
        /// <returns></returns>
        public virtual List<CommandEventData> GetCommandEventDataList() { return new List<CommandEventData>(); }
        #endregion

        #region 検索
        /// <summary>
        /// 検索可能かを取得します。
        /// </summary>
        public virtual bool CanSearch { get { return false; } }

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="searchWord">検索キーワード</param>
        /// <param name="isNext">true: 次を検索, false: 前を検索</param>
        public virtual bool Search(string searchWord, bool isNext)
        {
            return false;
        }
        #endregion

        #region 破棄処理
        /// <summary>
        /// 破棄処理
        /// </summary>
        /// <param name="isDispose"></param>
        protected override void Dispose(bool isDispose)
        {
            base.Dispose(isDispose);

            if (isDispose == true)
            {
                UnregistViewEvent();
            }
        }
        #endregion
    }
}
