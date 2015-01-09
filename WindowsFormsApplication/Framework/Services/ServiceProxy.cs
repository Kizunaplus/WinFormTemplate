using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Services;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using System.Runtime.Remoting.Channels;
using Kizuna.Plus.WinMvcForm.Framework.Framework.Services.Interceptor;
using Kizuna.Plus.WinMvcForm.Framework.Services.Interceotor;
using System.Reflection;

namespace WindowsFormsApplication.Framework.Services
{
    class ServiceProxy : RealProxy
    {
        #region メンバー変数
        /// <summary>
        /// 実インスタンス
        /// </summary>
        private object _target;
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ServiceProxy(object target, Type interfaceType)
            : base(interfaceType)
        {
            this._target = target;
        }
        #endregion

        #region 処理
        /// <summary>
        /// 処理の実行
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override IMessage Invoke(IMessage msg)
        {
            IMessage response = null;
            IMethodCallMessage call = (IMethodCallMessage)msg;
            IConstructionCallMessage ctor = call as IConstructionCallMessage;

            if (ctor != null)
            {
                RealProxy sp = RemotingServices.GetRealProxy(this._target);
                sp.InitializeServerObject(ctor);
                MarshalByRefObject tp = this.GetTransparentProxy() as MarshalByRefObject;
                response = EnterpriseServicesHelper.CreateConstructionReturnMessage(ctor, tp);
            }
            else
            {
                MethodInfo method = this._target.GetType().GetMethod(call.MethodName);

                var attrs = method.GetCustomAttributes(typeof(ServiceInterceptorAttribute), true) as ServiceInterceptorAttribute[];

                object returnValue = null;
                if (attrs == null || attrs.Length <= 0)
                {
                    returnValue = call.MethodBase.Invoke(this._target, call.Args);
                }
                else
                {
                    returnValue = ServiceInterceptor.asInvoker(this._target, call.MethodName, call.Args);
                }

                response = new ReturnMessage(returnValue, null, 0,
                                     ((IMethodCallMessage)msg).LogicalCallContext,
                                     (IMethodCallMessage)msg);
            }

            return response;
        }
        #endregion
    }
}
