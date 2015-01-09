using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using WindowsFormsApplication.Framework.Services;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    class ServicePool
    {
        #region メンバー変数
        /// <summary>
        /// サービスリスト
        /// </summary>
        private Dictionary<String, ServiceProxy> serviceMap;
        #endregion

        #region プロパティ
        /// <summary>
        /// 現在の設定インスタンス
        /// </summary>
        public static ServicePool Current
        {
            get;
            set;
        }
        #endregion

        #region 初期化
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {
            var list = new List<Type>();
            serviceMap = new Dictionary<string, ServiceProxy>();

            var assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                ServiceAttribute[] attr = type.GetCustomAttributes(typeof(ServiceAttribute), true) as ServiceAttribute[];
                if (attr != null && 0 < attr.Length)
                {
                    if (serviceMap.ContainsKey(attr[0].Name) == true)
                    {
                        continue;
                    }

                    object instance = type.InvokeMember(null,
                        System.Reflection.BindingFlags.CreateInstance,
                        null, null,
                        null);

                    Type interfaceType = null;
                    foreach (Type checkType in type.GetInterfaces())
                    {
                        if (typeof(IService).IsAssignableFrom(checkType) == true)
                        {
                            interfaceType = checkType;
                            break;
                        }
                    }

                    if (interfaceType == null)
                    {
                        return;
                    }

                    serviceMap.Add(attr[0].Name, new ServiceProxy(instance, interfaceType));
                }
            }

        }
        #endregion

        #region 取得
        /// <summary>
        /// サービスの取得
        /// </summary>
        /// <param name="name">サービス名</param>
        /// <returns></returns>
        public object GetService(String name)
        {
            if (String.IsNullOrEmpty(name) == true
                || serviceMap.ContainsKey(name) == false)
            {
                // 名称が空の場合
                return null;
            }

            return serviceMap[name].GetTransparentProxy();
        }
        #endregion
    }
}
