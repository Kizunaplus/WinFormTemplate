using System;
using System.Collections.Generic;
using System.Reflection;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Views;

namespace Kizuna.Plus.WinMvcForm.Framework.Models
{
    /// <summary>
    /// Mvc連携データ
    /// </summary>
    public class MvcCooperationData : AbstractModel
    {
        #region 定数
        /// <summary>
        /// コントローラクラスのサフィックス
        /// </summary>
        public static String CONTROLLER = "Controller";

        /// <summary>
        /// モデルクラスのサフィックス
        /// </summary>
        public static String MODEL = "Model";

        /// <summary>
        /// ビュークラスのサフィックス
        /// </summary>
        public static String VIEW = "View";
        #endregion

        #region メンバー変数
        /// <summary>
        /// 現在定義されているコントローラのタイプ一覧
        /// </summary>
        private List<Type> currentDomainControllersType;

        /// <summary>
        /// 現在定義されているコマンドのタイプ一覧
        /// </summary>
        private List<Type> currentDomainCommandType;

        /// <summary>
        /// 現在定義されているビューのタイプ一覧
        /// </summary>
        private List<Type> currentDomainViewType;

        /// <summary>
        /// 現在定義されているモデルのタイプ一覧
        /// </summary>
        private List<Type> currentDomainModelType;
        #endregion

        #region プロパティ
        /// <summary>
        /// 現在の設定インスタンス
        /// </summary>
        public static MvcCooperationData Current
        {
            get;
            set;
        }

        /// <summary>
        /// 現在定義されているコントローラのタイプ一覧
        /// </summary>
        public List<Type> CurrentDomainControllersType
        {
            get
            {
                if (currentDomainControllersType == null)
                {
                    currentDomainControllersType = GetTypeList(typeof(IController));
                }
                return currentDomainControllersType;
            }
        }

        /// <summary>
        /// 現在定義されているコマンドのタイプ一覧
        /// </summary>
        public List<Type> CurrentDomainCommandType
        {
            get
            {
                if (currentDomainCommandType == null)
                {
                    currentDomainCommandType = GetTypeList(typeof(ICommand));
                }
                return currentDomainCommandType;
            }
        }

        /// <summary>
        /// 現在定義されているビューのタイプ一覧
        /// </summary>
        public List<Type> CurrentDomainViewType
        {
            get
            {
                if (currentDomainViewType == null)
                {
                    currentDomainViewType = GetTypeList(typeof(IView));
                }
                return currentDomainViewType;
            }
        }

        /// <summary>
        /// 現在定義されているモデルのタイプ一覧
        /// </summary>
        public List<Type> CurrentDomainModelType
        {
            get
            {
                if (currentDomainModelType == null)
                {
                    currentDomainModelType = GetTypeList(typeof(IModel));
                }
                return currentDomainModelType;
            }
        }
        #endregion

        #region 取得
        /// <summary>
        /// 指定したタイプを継承したクラス一覧を取得します。
        /// </summary>
        /// <param name="parentType">親のクラス</param>
        /// <returns>継承したクラスリスト</returns>
        public List<Type> GetTypeList(Type parentType)
        {
            var list = new List<Type>();

            var assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (parentType.IsAssignableFrom(type) == true)
                {
                    list.Add(type);
                }
            }

            return list;
        }
        #endregion

        #region 変換
        /// <summary>
        /// コントローラに対応するビュークラスを取得
        /// </summary>
        /// <param name="controllerType">コントローラのタイプ</param>
        /// <returns></returns>
        public static Type Controller2View(Type controllerType)
        {
            return GetMappedType(controllerType, CONTROLLER, VIEW);
        }

        /// <summary>
        /// ビューに対応するコントローラクラスを取得
        /// </summary>
        /// <param name="viewType">ビューのタイプ</param>
        /// <returns></returns>
        public static Type View2Controller(Type viewType)
        {
            return GetMappedType(viewType, VIEW, CONTROLLER);
        }

        /// <summary>
        /// コントローラに対応するモデルクラスを取得
        /// </summary>
        /// <param name="controllerType">コントローラのタイプ</param>
        /// <returns></returns>
        public static Type Controller2Model(Type controllerType)
        {
            return GetMappedType(controllerType, CONTROLLER, MODEL);
        }

        /// <summary>
        /// ビューに対応するモデルクラスを取得
        /// </summary>
        /// <param name="controllerType">コントローラのタイプ</param>
        /// <returns></returns>
        public static Type View2Model(Type viewType)
        {
            return GetMappedType(viewType, VIEW, MODEL);
        }

        /// <summary>
        /// 対応されているタイプを取得します。
        /// </summary>
        /// <param name="type">元のタイプ</param>
        /// <param name="srcType">元のタイプ（MVC）</param>
        /// <param name="destType">対応先のタイプ（MVC）</param>
        /// <returns></returns>
        private static Type GetMappedType(Type type, String srcType, String destType)
        {
            return GetMappedType(type.FullName, srcType, destType);
        }
        /// <summary>
        /// 対応されているタイプを取得します。
        /// </summary>
        /// <param name="typeName">元のタイプ名</param>
        /// <param name="srcType">元のタイプ（MVC）</param>
        /// <param name="destType">対応先のタイプ（MVC）</param>
        /// <returns></returns>
        private static Type GetMappedType(String typeName, String srcType, String destType)
        {
            string newTypeName = typeName.Replace(srcType, destType);
            Type newType = Type.GetType(newTypeName, false, true);
            if (newType == null)
            {
                // 見つからない
                return null;
            }

            Type newIfType = null;
            if (destType == CONTROLLER)
            {
                // Controller変換の場合
                newIfType = typeof(IController);
            }
            else if (destType == VIEW)
            {
                // View変換の場合
                newIfType = typeof(IView);
            }
            else if (destType == MODEL)
            {
                // Model変換の場合
                newIfType = typeof(IModel);
            }

            if (newIfType == null
                || newIfType.IsAssignableFrom(newType) == false)
            {
                // 対応する方ではない
                // 期待するdestType以外が指定
                return null;
            }


            return newType;
        }
        #endregion
    }
}
