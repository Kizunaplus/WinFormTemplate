using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication.Controllers.State;

namespace WindowsFormsApplication.Models
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

        #region コピー
        /// <summary>
        /// コピー処理
        /// </summary>
        /// <returns>コピーしたインスタンス</returns>
        public override object Clone()
        {
            CommandEventData data = new CommandEventData(this.Source, this.CommandType, this.Mode, this.Handler);

            return data;
        }
        #endregion

        #region 比較
        /// <summary>
        /// 引数のオブジェクトが等しいか判断します。
        /// </summary>
        /// <param name="obj">比較先</param>
        /// <returns>true: 等しい, false: 等しくない</returns>
        public override bool Equals(object obj)
        {
            if (base.Equals(obj) == true)
            {
                // 同一インスタンス
                return true;
            }

            var data = obj as CommandEventData;
            if (data == null)
            {
                return false;
            }

            if (this.Source.Equals(data.Source) == false)
            {
                return false;
            }
            if (this.Mode.Equals(data.Mode) == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ハッシュ関数として機能します。
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            int hashcode = 17;
            hashcode *= this.Source.GetHashCode();
            hashcode *= this.Mode.GetHashCode();

            return hashcode;
        }
        #endregion
    }
}
