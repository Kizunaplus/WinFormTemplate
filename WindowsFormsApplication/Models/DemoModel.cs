using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using Kizuna.Plus.WinMvcForm.Framework.Views;
using System.Threading;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Models.Validation;
using System.Windows.Forms;

namespace WindowsFormsApplication.Models
{
    /// <summary>
    /// デモデータ
    /// </summary>
    public class DemoModel : AbstractModel
    {

        #region プロパティ
        /// <summary>
        /// DemoViewに表示されるメッセージ
        /// </summary>
        [RequiresInput]
        public String InfoMessage
        {
            get;
            set;
        }
        #endregion
    }
}
