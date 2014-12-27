using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication.Models;

namespace WindowsFormsApplication.Views
{
    /// <summary>
    /// 表示クラスインターフェース
    /// </summary>
    public interface IView : IDisposable
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        void Initialize();

        /// <summary>
        /// 画面の再描画
        /// 　データの再設定など
        /// </summary>
        void Refresh();

        #region イベント操作
        /// <summary>
        /// イベント一覧の取得
        /// </summary>
        /// <returns></returns>
        List<CommandEventData> GetCommandEventDataList();
        #endregion

        #region 検索
        /// <summary>
        /// 検索可能かを取得します。
        /// </summary>
        bool CanSearch { get; }

        /// <summary>
        /// 検索
        /// </summary>
        /// <param name="searchWord">検索キーワード</param>
        /// <param name="isNext">true: 次を検索, false: 前を検索</param>
        /// <returns>true: 見つかった場合, false: 見つからなかった場合</returns>
        bool Search(string searchWord, bool isNext);
        #endregion
    }
}
