using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;

namespace Kizuna.Plus.WinMvcForm.Framework.Models
{
    /// <summary>
    /// コマンドイベントデータ
    /// </summary>
    public class CommandEventData : AbstractModel
    {
        #region プロパティ
        /// <summary>
        /// コマンドタイプ
        /// </summary>
        public Type CommandType
        {
            get;
            private set;
        }

        /// <summary>
        /// イベント発行元
        /// </summary>
        public object Source
        {
            get;
            private set;
        }

        /// <summary>
        /// イベント発行元状態
        /// </summary>
        public StateMode Mode
        {
            get;
            private set;
        }

        /// <summary>
        /// イベント処理デリゲート
        /// </summary>
        public EventHandler Handler
        {
            get;
            private set;
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">イベント発行元</param>
        /// <param name="commandType">コマンドタイプ</param>
        /// <param name="mode">イベント発行元状態</param>
        /// <param name="handler">イベント処理デリゲート</param>
        public CommandEventData(object source, Type commandType, StateMode mode, EventHandler handler)
        {
            this.Source = source;
            this.CommandType = commandType;
            this.Mode = mode;
            this.Handler = handler;

            if (this.Source == null)
            {
                throw new NullReferenceException();
            }
        }
        #endregion
    }
}
