using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using System.Runtime.InteropServices;

namespace Kizuna.Plus.WinMvcForm.Framework.Views.WebJs
{
    /// <summary>
    /// Webファイルからアクセス用メソッド定義クラス
    /// </summary>
    [ComVisibleAttribute(true)]
    public class WebViewJsAccess
    {
        #region Html用処理
        /// <summary>
        /// アクションの実行
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        public void Action(String controller, String action)
        {
            ActionEventArgs eventArgs = new ActionEventArgs();
            eventArgs.Controller = controller;
            eventArgs.ActionName = action;

            ActionCommand command = new ActionCommand();
            command.Execute(new NonState(typeof(Application)), eventArgs);
        }
        #endregion

    }
}
