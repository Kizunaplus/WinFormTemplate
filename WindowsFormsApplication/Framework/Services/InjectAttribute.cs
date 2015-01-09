using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using System.Reflection;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    /// <summary>
    /// サービスのインジェクション属性
    /// </summary>
    class InjectAttribute : Attribute
    {
        public static void InjectService<T>(IController controller, FieldInfo field) where T : class
        {
            T service = (T)ServicePool.Current.GetService(field.Name);
            if (service == null)
            {
                return;
            }

            field.SetValue(controller, service);
        }
    }
}
