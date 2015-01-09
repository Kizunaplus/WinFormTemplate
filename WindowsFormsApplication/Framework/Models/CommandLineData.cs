using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Models
{
    /// <summary>
    /// コマンドラインデータ
    /// </summary>
    class CommandLineData : AbstractModel
    {
        #region プロパティ
        /// <summary>
        /// Key名なしのコマンドラインデータ
        /// </summary>
        public string _Value
        {
            get;
            set;
        }
        #endregion
    }
}
