using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication.Views;

namespace WindowsFormsApplication.Models.EventArg
{
    /// <summary>
    /// 検索イベント引数
    /// </summary>
    class SearchEventArgs : EventArgs
    {
        #region プロパティ
        /// <summary>
        /// 検索文字列
        /// </summary>
        public string SearchWord
        {
            get;
            set;
        }

        /// <summary>
        /// 検索対象
        /// </summary>
        public IView Target
        {
            get;
            set;
        }

        /// <summary>
        /// 検索方向（次へ）
        /// </summary>
        public bool IsNext
        {
            get;
            set;
        }
        #endregion
    }
}
