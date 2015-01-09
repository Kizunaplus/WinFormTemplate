using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;

namespace Kizuna.Plus.WinMvcForm.Framework.Framework.Services.Interceptor
{
    class ExceptionInterceptorAttribute : ServiceInterceptorAttribute
    {
        /// <summary>
        /// データグリッドビューの入力チェック
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override object Invoker(object controller, MethodInfo invokerMethod, object[] parameters, IList<ServiceInterceptorAttribute> attributes)
        {
            object retValue = null;

            try
            {
                retValue = base.Invoker(controller, invokerMethod, parameters, attributes);
            }
            catch (Exception ex)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(LogType.Exception, "", ex);
            }

            return retValue;
        }
    }
}
