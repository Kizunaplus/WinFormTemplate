using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using Kizuna.Plus.WinMvcForm.Framework.Views;
using System.Threading;

namespace Kizuna.Plus.WinMvcForm.Framework.Models
{
    /// <summary>
    /// Controller => View 連携データ
    /// 
    /// CurrentThreadプロパティにて取得したインスタンスが
    /// 連携データとして有効です。
    /// 
    /// スレッドセーフな実装は行っておりません。
    /// </summary>
    public class ViewStateData : AbstractModel
    {
        #region 定数
        /// <summary>
        /// スレッドローカル変数名
        /// </summary>
        private const String THREAD_DATA_NAME = "TH_VIEW_STATE";
        #endregion

        #region メンバー変数
        /// <summary>
        /// 連携データ
        /// </summary>
        private Dictionary<String, Object> data = new Dictionary<String, Object>();
        #endregion

        #region プロパティ
        /// <summary>
        /// スレッドに割り当てられたViewStateデータを取得します。
        /// </summary>
        public static ViewStateData CurrentThread
        {
            get
            {
                ViewStateData value = null;
                var checkSlot = Thread.GetNamedDataSlot(THREAD_DATA_NAME);
                if (Thread.GetData(checkSlot) == null)
                {
                    // 現在のスレッドに新たなインスタンスを設定
                    Thread.SetData(checkSlot, new ViewStateData());
                }
                value = Thread.GetData(checkSlot) as ViewStateData;

                return value;
            }
        }

        /// <summary>
        /// ViewStateデータを取得します。
        /// </summary>
        public IDictionary<String, Object> Items
        {
            get
            {
                return this.data;
            }
        }
        #endregion
    }
}
