using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace WindowsFormsApplication.Framework.Models
{
    [Serializable]
    class DependencyInjectionSetting : AbstractModel
    {
        #region 定数
        /// <summary>
        /// 設定ファイル名
        /// </summary>
        public const string CONFIG_FILE_NAME = "service_config.xml";
        #endregion

        #region プロパティ
        [DataMemberAttribute]
        /// <summary>
        /// DI対象のタイプ
        /// </summary>
        public String Type
        {
            get;
            set;
        }

        /// <summary>
        /// DI対象の名
        /// </summary>
        public String Name
        {
            get;
            set;
        }

        [DataMemberAttribute]
        /// <summary>
        /// DIする設定
        /// </summary>        
        public Dictionary<String, String> Setting
        {
            get;
            set;
        }
        #endregion
    }
}
