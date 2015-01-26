using System.Collections.Generic;
using System.Reflection;
using System.Transactions;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using WindowsFormsApplication.Framework.Services;

namespace Kizuna.Plus.WinMvcForm.Framework.Framework.Services.Interceptor
{
    class TransactionInterceptorAttribute : ServiceInterceptorAttribute
    {
        /// <summary>
        /// トランザクションオプション設定
        /// </summary>
        private TransactionScopeOption option;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="option">トランザクションオプション設定</param>
        public TransactionInterceptorAttribute(TransactionScopeOption option = TransactionScopeOption.Required)
        {
            this.option = option;
        }

        /// <summary>
        /// Intercept処理　トランザクション処理を行う
        /// </summary>
        /// <param name="control"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public override object Invoker(object controller, MethodInfo invokerMethod, object[] parameters, IList<ServiceInterceptorAttribute> attributes)
        {
            object retValue = null;

            using (TransactionScope scope = new TransactionScope(option))
            {
                ITransactionDataService service = ServicePool.Current.GetService("transaction") as ITransactionDataService;
                service.RollbackReserve = false;

                retValue = base.Invoker(controller, invokerMethod, parameters, attributes);
                // null以外の文字場合は、コミットを行う
                if (service.RollbackReserve == false)
                {
                    scope.Complete();
                }
            }

            return retValue;
        }

    }
}
