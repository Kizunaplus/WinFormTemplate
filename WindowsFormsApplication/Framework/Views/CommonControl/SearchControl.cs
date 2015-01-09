using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;

namespace Kizuna.Plus.WinMvcForm.Framework.Views.CommonControl
{
    public partial class SearchControl : UserControl
    {
        #region プロパティ
        /// <summary>
        /// 対象ビュー
        /// </summary>
        public IView View
        {
            get;
            set;
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchControl()
        {
            InitializeComponent();
            RegistViewEvent();
        }

        /// <summary>
        /// ビューイベントの登録
        /// </summary>
        private void RegistViewEvent()
        {
            var register = CommandRegister.Current;

            // 検索イベントの登録
            var eventData = new CommandEventData(typeof(Application), typeof(SearchCommand), StateMode.None, delegate(object sender, EventArgs args)
            {
                var searchEventArgs = args as SearchEventArgs;
                if (searchEventArgs != null && searchEventArgs.Target != null)
                {
                    // 引数で設定されている場合は、引数の検索文字を使用する。
                    // 設定されていない場合は、設定値を使用する
                    string searchWord = this.searchTextBox.Text;
                    if (string.IsNullOrEmpty(searchEventArgs.SearchWord) == false)
                    {
                        this.searchTextBox.Text = searchEventArgs.SearchWord;
                        searchWord = searchEventArgs.SearchWord;
                    }

                    if (string.IsNullOrEmpty(searchWord) == true)
                    {
                        // 検索文字が無い
                        return;
                    }

                    if (this.View.CanSearch == true)
                    {
                        this.View = searchEventArgs.Target;
                        bool isFound = this.View.Search(searchWord, searchEventArgs.IsNext);
                        if (isFound == true)
                        {
                            this.searchTextBox.BackColor = SystemColors.Window;
                        }
                        else
                        {
                            this.searchTextBox.BackColor = Color.LightPink;
                        }
                    }
                }
            });
            register.Regist(eventData);
        }
        #endregion

        #region イベント
        /// <summary>
        /// 前を検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevSearchButton_Click(object sender, EventArgs e)
        {
            if (this.View != null)
            {
                bool isFound = this.View.Search(this.searchTextBox.Text, false);
                if (isFound == true)
                {
                    this.searchTextBox.BackColor = SystemColors.Window;
                }
                else
                {
                    this.searchTextBox.BackColor = Color.LightPink;
                }
            }
        }

        /// <summary>
        /// 次を検索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextSearchButton_Click(object sender, EventArgs e)
        {
            if (this.View != null)
            {
                bool isFound = this.View.Search(this.searchTextBox.Text, false);
                if (isFound == true)
                {
                    this.searchTextBox.BackColor = SystemColors.Window;
                }
                else
                {
                    this.searchTextBox.BackColor = Color.LightPink;
                }
            }
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeButton_Click(object sender, EventArgs e)
        {
            this.searchTextBox.Text = string.Empty;
            this.Visible = false;
        }

        /// <summary>
        /// アクティブになった際のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchControl_Enter(object sender, EventArgs e)
        {
            this.searchTextBox.Select();
            this.searchTextBox.SelectAll();
        }

        /// <summary>
        /// テキスト内容変更イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            bool isEnabled = !string.IsNullOrEmpty(searchTextBox.Text);

            this.nextSearchButton.Enabled = isEnabled;
            this.prevSearchButton.Enabled = isEnabled;
            this.searchTextBox.BackColor = SystemColors.Window;
        }
        #endregion
    }
}
