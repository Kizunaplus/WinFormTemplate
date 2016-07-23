using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using WindowsFormsApplication.Framework.Message;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using WindowsFormsApplication.Models;
using System.IO;
using WindowsFormsApplication.Framework.Models;

namespace Kizuna.Plus.WinMvcForm.Framework.Services
{
    /// <summary>
    /// サービスインスタンスの管理クラス
    /// </summary>
    class ServicePool
    {
        #region メンバー変数
        /// <summary>
        /// サービスリスト
        /// </summary>
        private Dictionary<String, IList<ServiceProxy>> serviceMap;

        /// <summary>
        /// サービス型リスト
        /// </summary>
        private Dictionary<String, Tuple<Type,Type>> serviceTypeMap;

        /// <summary>
        /// 使用中サービスリスト
        /// </summary>
        private Dictionary<Guid, IList<Tuple<String, ServiceProxy>>> serviceUseInstanceMap;
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
            serviceMap = new Dictionary<string, IList<ServiceProxy>>();
            serviceTypeMap = new Dictionary<string, Tuple<Type, Type>>();
            serviceUseInstanceMap = new Dictionary<Guid, IList<Tuple<String, ServiceProxy>>>();

            var logCommand = new LogCommand();
            if (ConfigurationData.Current != null)
            {
                String xmlFolderPath = ConfigurationData.Current.DiConfigPath;
                if (String.IsNullOrEmpty(xmlFolderPath) == false && Directory.Exists(xmlFolderPath) == true)
                {

                    foreach (String filePath in Directory.GetFiles(xmlFolderPath, "*" + DependencyInjectionSetting.CONFIG_FILE_NAME, SearchOption.AllDirectories))
                    {
                        DependencyInjectionSetting setting = new DependencyInjectionSetting();
                        setting = (DependencyInjectionSetting)setting.Load(filePath);
                        Type type = Type.GetType(setting.Type);
                        if (LoadServiceInstance(type, setting.Name, "Configuration", logCommand) == false)
                        {
                            continue;
                        }

                    }
                }
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {

                ServiceAttribute[] attr = type.GetCustomAttributes(typeof(ServiceAttribute), true) as ServiceAttribute[];
                if (attr != null && 0 < attr.Length)
                {
                    if (LoadServiceInstance(type, attr[0].Name, attr[0].GetType().FullName, logCommand) == false)
                    {

                        continue;
                    }
                }
            }

        }

        /// <summary>
        /// タイプの解釈
        /// </summary>
        /// <param name="type">ロードするタイプ</param>
        /// <param name="logCommand">ロガー</param>
        /// <returns></returns>
        private bool LoadServiceInstance(Type type, String name, String attrType, LogCommand logCommand)
        {
            if (serviceMap.ContainsKey(name) == true)
            {
                // サービス名が設定されているサービスが含まれない
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.NotFoundInjectionService, type.FullName, attrType, name);

                return false;
            }

            object instance = Activator.CreateInstance(type);

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
                // Interfaceタイプが見つからないまたは、IServiceを継承していない
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.InjectionServiceIsNullOrNotImplements, type.FullName, attrType, name);
                return false;
            }

            if (serviceTypeMap.ContainsKey(name) == true)
            {
                // すでに登録済み
                return false;
            }

            IList<ServiceProxy> proxyList = new List<ServiceProxy>();
            proxyList.Add(new ServiceProxy(instance, interfaceType));
            serviceMap.Add(name, proxyList);
            serviceTypeMap.Add(name, new Tuple<Type, Type>(type, interfaceType));

            return true;
        }
        #endregion

        #region 取得
        /// <summary>
        /// サービスの取得
        /// 
        /// id がEmptyの場合は、インスタンスを専有しない
        /// （ステートを持たないサービスの場合のみ使用）
        /// </summary>
        /// <param name="name">サービス名</param>
        /// <returns></returns>
        public object GetService(String name, Guid id)
        {
            if (String.IsNullOrEmpty(name) == true)
            {
                // 名称が空の場合
                return null;
            }

            if (serviceMap.ContainsKey(name) == false 
                && ViewStateData.CurrentThread.Items.ContainsKey(name) == true)
            {
                // サービスプールに存在していなく、スレッド変数に定義されている場合                
                return ViewStateData.CurrentThread.Items[name];
            } else if (serviceMap.ContainsKey(name) == false)
            {
                // サービスプールに存在していない
                return null;
            }

            if (id == Guid.Empty) {
                // Guidが未指定の場合は、専有しない
                return serviceMap[name][0].GetTransparentProxy();
            }

            ServiceProxy proxy;
            lock (serviceMap)
            {
                if (serviceMap[name].Count <= 0)
                {
                    object instance = Activator.CreateInstance(serviceTypeMap[name].Item1);
                    proxy = new ServiceProxy(instance, serviceTypeMap[name].Item2);
                }
                else
                {
                    proxy = serviceMap[name][0];
                    serviceMap[name].RemoveAt(0);
                }

                // 使用済みリストに登録
                if (serviceUseInstanceMap.ContainsKey(id) == false)
                {
                    serviceUseInstanceMap.Add(id, new List<Tuple<String, ServiceProxy>>());
                }
                serviceUseInstanceMap[id].Add(new Tuple<String, ServiceProxy>(name, proxy));

                return proxy.GetTransparentProxy();
            }
        }
        #endregion

        #region 解放
        /// <summary>
        /// 設定したサービスをプーリングします。
        /// </summary>
        /// <param name="name"></param>
        public void ReleaseService(Guid id) {

            if (serviceUseInstanceMap.ContainsKey(id) == false)
            {
                // 存在しないID
                return;
            }

            lock (serviceMap)
            {
                // 使用済みサービスをプールに戻す。
                var nonUseServiceList = serviceUseInstanceMap[id];
                serviceUseInstanceMap.Remove(id);
                foreach (Tuple<String, ServiceProxy> item in nonUseServiceList)
                {
                    serviceMap[item.Item1].Add(item.Item2);
                }
            }
        }
        #endregion
    }
}
