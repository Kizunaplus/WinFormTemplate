using System.Collections.Generic;
using System.Reflection;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using System;

namespace Kizuna.Plus.WinMvcForm.Framework.Services.Interceptor
{
    class ServiceInterceptorAttribute : Attribute
    {
        /// <summary>
        /// サービス処理のInterceptor処理
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual object Invoker(object controller, MethodInfo invokerMethod, object[] parameters, IList<ServiceInterceptorAttribute> attributes)
        {
            if (controller == null || invokerMethod == null)
            {
                // 引数エラー
                return null;
            }

            if (attributes != null & 0 < attributes.Count)
            {
                // 属性のよびだし
                var attr = attributes[0];
                attributes.RemoveAt(0);
                return attr.Invoker(controller, invokerMethod, parameters, attributes);
            }
            else
            {
                return invokerMethod.Invoke(controller, parameters);
            }
        }

    }
}
