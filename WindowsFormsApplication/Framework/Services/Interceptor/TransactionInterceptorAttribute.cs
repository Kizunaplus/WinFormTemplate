using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using Kizuna.Plus.WinMvcForm.Framework.Services;

namespace Kizuna.Plus.WinMvcForm.Framework.Framework.Services.Interceptor
{
    class TransactionInterceptorAttribute : ServiceInterceptorAttribute
    {
        /// <summary>
        /// Intercept処理　トランザクション処理を行う
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override object Invoker(object controller, MethodInfo invokerMethod, object[] parameters, IList<ServiceInterceptorAttribute> attributes)
        {
            object retValue = null;

            using (TransactionScope scope = new TransactionScope())
            {
                retValue = base.Invoker(controller, invokerMethod, parameters, attributes);

                if (retValue != null)
                {
                    // null以外の文字場合は、コミットを行う
                    scope.Complete();
                }
            }

            return retValue;
        }

    }
}
