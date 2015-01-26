using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    /// <summary>
    /// サービス属性
    /// サービス名称を宣言します。
    /// インジェクションするメソッド名と一致している場合設定します。
    /// </summary>
    class ServiceAttribute : Attribute
    {
        #region プロパティ
        /// <summary>
        /// サービス名称
        /// </summary>
        public String Name
        {
            get;
            private set;
        }
        #endregion

        #region 初期化
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">サービス名称</param>
        public ServiceAttribute(String name)
        {
            this.Name = name;
        }
        #endregion
    }
}
