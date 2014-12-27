using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication.Views;

namespace WindowsFormsApplication.Controllers
{
    /// <summary>
    /// コントローラクラス
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
            return GetDefaultView();
        }
        #endregion

        #region ビュー
        /// <summary>
        /// 標準のビューを取得
        /// </summary>
        /// <returns></returns>
        protected IView GetDefaultView()
        {
            string viewName = this.GetType().FullName.Replace("Controller", "View");
            Type viewType = Type.GetType(viewName, false, true);
            if (viewType == null)
            {
                // 見つからない
                return null;
            }

            if (cacheViewList.ContainsKey(viewType) == true)
            {
                // キャッシュを発見
                return cacheViewList[viewType];
            }

            Type viewIfType = typeof(IView);
            if (viewIfType.IsAssignableFrom(viewType) == false)
            {
                // 見つからない
                return null;
            }

            ConstructorInfo constructor = viewType.GetConstructor(new Type[0]);
            var newController = constructor.Invoke(null) as IController;
            var newView = constructor.Invoke(null) as IView;
            this.cacheViewList.Add(viewType, newView);

            return constructor.Invoke(null) as IView;
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

        }
        #endregion
    }
}
