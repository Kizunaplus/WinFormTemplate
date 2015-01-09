using System;
using System.Collections.Generic;
using Kizuna.Plus.WinMvcForm.Framework.Framework.Services.Interceptor;

namespace Kizuna.Plus.WinMvcForm.Framework.Services.Interceotor
{
    class ServiceInterceptor
    {
        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="delegateMethod">計測するメソッド</param>
        /// <param name="parameters">メソッド引数</param>
        public static object asInvoker(object service, String methodName, object[] parameters)
        {
            var invokerMethod = service.GetType().GetMethod(methodName);
            if (invokerMethod == null)
            {
                // メソッドを取得失敗
                return null;
            }

            // 属性を取得
            IList<ServiceInterceptorAttribute> attributes = invokerMethod.GetCustomAttributes(typeof(ServiceInterceptorAttribute), true) as IList<ServiceInterceptorAttribute>;

            object retValue = null;
            if (attributes == null || attributes.Count <= 0)
            {
                // 属性が指定されていない場合
                retValue = invokerMethod.Invoke(service, parameters);
            }
            else
            {
                var attr = attributes[0];

                var newAttributes = new List<ServiceInterceptorAttribute>(attributes);
                newAttributes.RemoveAt(0);
                return attr.Invoker(service, invokerMethod, parameters, newAttributes);
            }

            return retValue;
        }
    }
}
