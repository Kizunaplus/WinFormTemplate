using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using System.Runtime.Serialization;
using Kizuna.Plus.WinMvcForm.Framework.Models.Validation;

namespace WindowsApplicationTest.Dummy.Framework.Models
{
    class Dummy4Model : AbstractModel
    {
        #region プロパティ
        [RequiresInput]
        /// <summary>
        /// String値
        /// </summary>
        public String StringRequireValue
        {
            get;
            set;
        }

        [RequiresInput]
        /// <summary>
        /// Int32値
        /// </summary>
        public Int32? Int32RequireValue
        {
            get;
            set;
        }

        [AlphabetValueCheck]
        /// <summary>
        /// String値
        /// </summary>
        public String StringAlphabetValue
        {
            get;
            set;
        }

        [RequiresInput]
        /// <summary>
        /// DummyModel値
        /// </summary>
        public DummyModel DummyModelValue
        {
            get;
            set;
        }

        [DataMember]
        /// <summary>
        /// DateTime値
        /// </summary>
        public DateTime DateTimelValue
        {
            get;
            set;
        }
        #endregion
    }
}
