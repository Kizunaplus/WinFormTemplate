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
            IList<ServiceInterceptorAttribute> attributes = new List<ServiceInterceptorAttribute>(invokerMethod.GetCustomAttributes(typeof(ServiceInterceptorAttribute), true) as IList<ServiceInterceptorAttribute>);

            // InjectionInterceptorが宣言されているかチェック
            bool isFound = false;
            foreach (var checkAttr in attributes) {
                if (checkAttr.GetType() == typeof(InjectionInterceptorAttribute))
                {
                    isFound = true;
                    break;
                }
            }

            if (isFound == false) {
                // 設定されていない場合は設定
                attributes.Insert(0, new InjectionInterceptorAttribute());
            }

            var attr = attributes[0];

            attributes.RemoveAt(0);
            return attr.Invoker(service, invokerMethod, parameters, attributes);
        }
    }
}
