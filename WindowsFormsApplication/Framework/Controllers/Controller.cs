using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Views;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using WindowsFormsApplication.Framework.Message;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers
{
    /// <summary>
    /// 基本コントローラクラス
    /// コントローラを実装する場合は、このクラスを継承してください。
    /// </summary>
    public class Controller : IController
    {
        #region メンバー変数
        /// <summary>
        /// ビューのCache
        /// </summary>
        protected Dictionary<Type, IView> cacheViewList;
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Controller()
        {
            cacheViewList = new Dictionary<Type, IView>();
        }

        /// <summary>
        /// 初期化処理
        /// 
        /// コントローラが変更された場合に呼び出される。
        /// </summary>
        public virtual void Initialize()
        {
        }
        #endregion

        #region アクション
        /// <summary>
        /// 初期表示
        /// </summary>
        /// <returns>表示画面</returns>
        public virtual IView Index()
        {
            // DefaultView(コントローラに対応するビューを表示します。
            return GetDefaultView();
        }
        #endregion

        #region ビュー
        /// <summary>
        /// 対応するビューを取得
        /// ***Controller => ***View
        /// </summary>
        /// <returns></returns>
        protected IView GetDefaultView()
        {
            var logCommand = new LogCommand();
            var viewType = MvcCooperationData.Controller2View(this.GetType());
            if (viewType == null)
            {
                // 見つからない
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.NotFoundTargetView, "GetDefaultView", this.GetType().FullName);
                return null;
            }

            if (cacheViewList.ContainsKey(viewType) == true)
            {
                // キャッシュされている場合は、そのインスタンスを返却
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.CacheFoundTargetView, "GetDefaultView", this.GetType().FullName);
                return cacheViewList[viewType];
            }

            ConstructorInfo constructor = viewType.GetConstructor(new Type[0]);
            var newView = constructor.Invoke(null) as IView;
            this.cacheViewList.Add(viewType, newView);

            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.CreateInstanceTargetView, "GetView", this.GetType().FullName);
            return newView;
        }

        /// <summary>
        /// 対応するビューを取得
        /// ***Controller => ***View
        /// </summary>
        /// <returns></returns>
        protected IView GetView(String viewName)
        {
            if (string.IsNullOrEmpty(viewName) == true)
            {
                // 標準のビューを取得
                return GetDefaultView();
            }
            var logCommand = new LogCommand();

            Type viewType = null;
            foreach (Type type in MvcCooperationData.Current.CurrentDomainViewType)
            {
                if (type.Name.EndsWith(viewName) == true
                    || type.Name.EndsWith(viewName + MvcCooperationData.VIEW) == true)
                {
                    viewType = type;
                }
            }
            if (viewType == null)
            {
                // 見つからない
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.NotFoundTargetView, "GetView", viewName);
                return null;
            }

            if (cacheViewList.ContainsKey(viewType) == true)
            {
                // キャッシュされている場合は、そのインスタンスを返却
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.CacheFoundTargetView, "GetView", this.GetType().FullName);
                return cacheViewList[viewType];
            }

            ConstructorInfo constructor = viewType.GetConstructor(new Type[0]);
            var newView = constructor.Invoke(null) as IView;
            this.cacheViewList.Add(viewType, newView);

            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.CreateInstanceTargetView, "GetView", this.GetType().FullName);
            return newView;
        }
        #endregion

        #region 破棄処理
        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        /// <param name="isDispose"></param>
        protected virtual void Dispose(bool isDispose)
        {
            // キャッシュのクリア
            foreach (IView view in this.cacheViewList.Values)
            {
                if (view == null)
                {
                    continue;
                }
                view.Dispose();
            }

            this.cacheViewList.Clear();
        }
        #endregion
    }
}
