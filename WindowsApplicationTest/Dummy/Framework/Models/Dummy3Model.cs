using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using System.Runtime.Serialization;

namespace WindowsApplicationTest.Dummy.Framework.Models
{
    class Dummy3Model : AbstractModel
    {
        #region プロパティ
        /// <summary>
        /// String値
        /// </summary>
        public String StringValue
        {
            get;
            set;
        }

        /// <summary>
        /// Int32値
        /// </summary>
        public Int32 Int32Value
        {
            get;
            set;
        }

        /// <summary>
        /// byte[]値
        /// </summary>
        public byte[] ByteArrayValue
        {
            get;
            set;
        }
        #endregion
    }
}
