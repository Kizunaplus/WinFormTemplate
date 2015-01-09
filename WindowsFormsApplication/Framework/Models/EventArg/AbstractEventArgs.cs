using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Kizuna.Plus.WinMvcForm.Framework.Models.EventArg
{
    class AbstractEventArgs : EventArgs
    {
        #region toString
        /// <summary>
        /// クラスの状態を文字列化します。
        /// 
        /// Reflectionを用いるため
        /// 頻度が高い場合は、独自で実装する
        /// </summary>
        /// <returns>クラス状態</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            Type type = this.GetType();
            sb.Append(type.Name);
            sb.Append(":");

            // 各フィールドの値を比較
            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                object src = field.GetValue(this);
                sb.AppendFormat("{0}-[{1}],", field.Name, src);
            }

            return sb.ToString();
        }
        #endregion
    }
}
