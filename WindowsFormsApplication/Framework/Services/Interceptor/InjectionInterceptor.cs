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
    class InjectionInterceptorAttribute : ServiceInterceptorAttribute
    {
        /// <summary>
        /// インジェクション設定インターセプター
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override object Invoker(object controller, MethodInfo invokerMethod, object[] parameters, IList<ServiceInterceptorAttribute> attributes)
        {
            FieldInfo[] fields = controller.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                Attribute[] attr = field.GetCustomAttributes(typeof(InjectAttribute), true) as Attribute[];
                if (attr != null && 0 < attr.Length)
                {
                    // Intect属性がついているフィールドに値を設定
                    InjectAttribute.InjectService<IService>(controller, field);
                }
            }
            return base.Invoker(controller, invokerMethod, parameters, attributes);
        }
    }
}
