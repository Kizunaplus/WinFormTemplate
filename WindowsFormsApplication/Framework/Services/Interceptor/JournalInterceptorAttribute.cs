using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using WindowsFormsApplication.Framework.Message;
using System.Diagnostics;

namespace Kizuna.Plus.WinMvcForm.Framework.Framework.Services.Interceptor
{
    class JournalInterceptorAttribute : ServiceInterceptorAttribute
    {
        /// <summary>
        /// Intercept処理　ジャーナルログ出力処理を行う
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override object Invoker(object controller, MethodInfo invokerMethod, object[] parameters, IList<ServiceInterceptorAttribute> attributes)
        {
            object retValue = null;
            var logCommand = new LogCommand();

            string typeName = controller.GetType().FullName;

            // 開始ジャーナル
            logCommand.Execute(LogType.Info, FrameworkMessage.JournalMessageServiceStart, typeName, invokerMethod.Name, parameters);

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            retValue = base.Invoker(controller, invokerMethod, parameters, attributes);
            stopWatch.Stop();

            // 終了ジャーナル
            logCommand.Execute(LogType.Info, FrameworkMessage.JournalMessageServiceEnd, typeName, invokerMethod.Name, stopWatch.ElapsedMilliseconds);

            return retValue;
        }
    }
}
