using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    /// <summary>
    /// サービス属性
    /// </summary>
    class ServiceAttribute : Attribute
    {
        #region プロパティ
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
