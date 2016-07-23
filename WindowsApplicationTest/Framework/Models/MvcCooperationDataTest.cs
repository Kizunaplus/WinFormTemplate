using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using WindowsApplicationTest.Dummy.Framework.Models;
using System.Reflection;
using System.IO;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using WindowsApplicationTest.Dummy.Framework.Controllers;
using WindowsApplicationTest.Dummy.Framework.Views;
using WindowsApplicationTest.Dummy.Framework.Controllers.Commands;

namespace WindowsApplicationTest.Framework.Models
{
    /// <summary>
    /// MvcCooperationDataクラスのユニットテスト
    /// </summary>
    [TestClass]
    public class MvcCooperationDataTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private MvcCooperationData Target
        {
            get;
            set;
        }
        #endregion

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        #region 初期化
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        [TestInitialize()]
        public void TestInitialize()
        {
            this.Target = new MvcCooperationData(new Assembly[]{ this.GetType().Assembly });

            // Mvc変換インスタンス取得
            MvcCooperationData.Current = new MvcCooperationData(new Assembly[] { Assembly.GetExecutingAssembly() });
        }
        #endregion

        #region 破棄
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        [TestCleanup()]
        public void TestCleanup()
        {
            this.Target.Dispose();
            this.Target = null;
        }
        #endregion

        #region CurrentDomainControllersTypeプロパティ
        /// <summary>
        /// CurrentDomainControllersTypeプロパティ
        /// </summary>
        [TestMethod]
        public void CurrentDomainControllersTypeTest()
        {
            var list = this.Target.CurrentDomainControllersType;

            Assert.IsTrue(list.Contains(typeof(DummyController)));
            Assert.IsTrue(list.Contains(typeof(Dummy2Controller)));
            Assert.IsTrue(list.Contains(typeof(DummyNonController)));

            Assert.IsFalse(list.Contains(typeof(DummyView)));
            Assert.IsFalse(list.Contains(typeof(DummyModel)));
        }
        #endregion

        #region CurrentDomainCommandTypeプロパティ
        /// <summary>
        /// CurrentDomainCommandTypeプロパティ
        /// </summary>
        [TestMethod]
        public void CurrentDomainCommandTypeTest()
        {
            var list = this.Target.CurrentDomainCommandType;

            Assert.IsTrue(list.Contains(typeof(DummyCommand)));

            Assert.IsFalse(list.Contains(typeof(DummyController)));
            Assert.IsFalse(list.Contains(typeof(DummyModel)));
            Assert.IsFalse(list.Contains(typeof(DummyView)));
        }
        #endregion

        #region CurrentDomainViewTypeプロパティ
        /// <summary>
        /// CurrentDomainViewTypeプロパティ
        /// </summary>
        [TestMethod]
        public void CurrentDomainViewTypeTest()
        {
            var list = this.Target.CurrentDomainViewType;

            Assert.IsTrue(list.Contains(typeof(DummyView)));
            Assert.IsTrue(list.Contains(typeof(Dummy2View)));

            Assert.IsFalse(list.Contains(typeof(DummyController)));
            Assert.IsFalse(list.Contains(typeof(DummyModel)));
        }
        #endregion

        #region CurrentDomainModelTypeプロパティ
        /// <summary>
        /// CurrentDomainModelTypeプロパティ
        /// </summary>
        [TestMethod]
        public void CurrentDomainModelTypeTest()
        {
            var list = this.Target.CurrentDomainModelType;

            Assert.IsTrue(list.Contains(typeof(DummyModel)));
            Assert.IsTrue(list.Contains(typeof(Dummy2Model)));
            Assert.IsTrue(list.Contains(typeof(Dummy3Model)));
            Assert.IsTrue(list.Contains(typeof(Dummy4Model)));
            Assert.IsTrue(list.Contains(typeof(Dummy5Model)));

            Assert.IsFalse(list.Contains(typeof(DummyController)));
            Assert.IsFalse(list.Contains(typeof(DummyView)));
        }
        #endregion

        #region Controller2Viewメソッド
        /// <summary>
        /// Controller2Viewメソッド
        /// </summary>
        [TestMethod]
        public void Controller2ViewTest()
        {
            var type = MvcCooperationData.Controller2View(typeof(DummyController));

            Assert.AreEqual(typeof(DummyView), type);
        }

        /// <summary>
        /// Controller2Viewメソッド
        /// </summary>
        [TestMethod]
        public void Controller2ViewNotFoundTest()
        {
            var type = MvcCooperationData.Controller2View(typeof(DummyNonController));

            Assert.IsNull(type);
        }
        #endregion

        #region View2Controllerメソッド
        /// <summary>
        /// View2Controllerメソッド
        /// </summary>
        [TestMethod]
        public void View2ControllerTest()
        {
            var type = MvcCooperationData.View2Controller(typeof(DummyView));

            Assert.AreEqual(typeof(DummyController), type);
        }

        /// <summary>
        /// View2Controllerメソッド
        /// </summary>
        [TestMethod]
        public void View2ControllerNotFoundTest()
        {
            var type = MvcCooperationData.View2Controller(typeof(Dummy3View));

            Assert.IsNull(type);
        }
        #endregion

        #region Controller2Modelメソッド
        /// <summary>
        /// Controller2Modelメソッド
        /// </summary>
        [TestMethod]
        public void Controller2ModelTest()
        {
            var type = MvcCooperationData.Controller2Model(typeof(DummyController));

            Assert.AreEqual(typeof(DummyModel), type);
        }

        /// <summary>
        /// Controller2Modelメソッド
        /// </summary>
        [TestMethod]
        public void Controller2ModelNotFoundTest()
        {
            var type = MvcCooperationData.Controller2Model(typeof(DummyNonController));

            Assert.IsNull(type);
        }
        #endregion

        #region View2Modelメソッド
        /// <summary>
        /// View2Modelメソッド
        /// </summary>
        [TestMethod]
        public void View2ModelTest()
        {
            var type = MvcCooperationData.View2Model(typeof(DummyView));

            Assert.AreEqual(typeof(DummyModel), type);
        }

        /// <summary>
        /// View2Modelメソッド
        /// </summary>
        [TestMethod]
        public void View2ModelNotFoundTest()
        {
            var type = MvcCooperationData.View2Model(typeof(DummyController));

            Assert.IsNull(type);
        }
        #endregion
    }
}
