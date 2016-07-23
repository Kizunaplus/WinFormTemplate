using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kizuna.Plus.WinMvcForm.Framework.Controllers;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using System.Reflection;
using WindowsFormsApplication.Views;
using WindowsFormsApplication.Controllers;
using WindowsApplicationTest.Dummy.Framework.Controllers;
using WindowsApplicationTest.Dummy.Framework.Views;
using WindowsApplicationTest.Utility;

namespace WindowsApplicationTest
{
    /// <summary>
    /// AbstractControllerクラスのユニットテスト
    /// </summary>
    [TestClass]
    public class AbstractControllerTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private AbstractController Target
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
        public void TestInitialize() {
            this.Target = new DummyController();

            // Mvc変換インスタンス取得
            MvcCooperationData.Current = new MvcCooperationData(new Assembly[]{Assembly.GetExecutingAssembly()});
        }
        #endregion

        #region 破棄
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        [TestCleanup()]
        public void TestCleanup() {
            this.Target.Dispose();
            this.Target = null;
        }
        #endregion

        #region ServiceIdプロパティ
        [TestMethod]
        public void TestServiceId()
        {
            Guid guid = this.Target.ServiceId;

            Assert.AreNotEqual(Guid.Empty, guid);
        }
        #endregion

        #region Initializeメソッド
        [TestMethod]
        public void TestInitializeMethod()
        {
            this.Target.Initialize();
        }
        #endregion

        #region Indexメソッド
        [TestMethod]
        [UnitTestTarget(typeof(AbstractController), "Index")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestIndex()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestTarget(typeof(AbstractController), "Index")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestIndex_MultipleCall()
        {
            UnitTestUtility.ExecuteMultipleUnitTest(Target);
        }
        #endregion

        #region GetDefaultViewメソッド
        [TestMethod]
        [UnitTestTarget(typeof(AbstractController), "GetDefaultView")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestGetDefaultView()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestTarget(typeof(AbstractController), "GetDefaultView")]
        [UnitTestAssert(null)]
        public void TestGetDefaultView_NotFound()
        {
            var controller = new DummyNonController();
            UnitTestUtility.ExecuteUnitTest(controller);
        }

        [TestMethod]
        [UnitTestTarget(typeof(AbstractController), "GetDefaultView")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestGetDefaultView_Cache()
        {
            UnitTestUtility.ExecuteMultipleUnitTest(Target);
        }
        #endregion

        #region GetViewメソッド
        [TestMethod]
        [UnitTestParameter("Dummy")]
        [UnitTestTarget(typeof(AbstractController), "GetView")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestGetView_Dummy()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter("DummyView")]
        [UnitTestTarget(typeof(AbstractController), "GetView")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestGetView_DummyController()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter("Dummy2")]
        [UnitTestTarget(typeof(AbstractController), "GetView")]
        [UnitTestAssert(typeof(Dummy2View), typeof(UnitTestUtility), "TypeAssert")]
        public void TestGetView_Dummy2()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter("")]
        [UnitTestTarget(typeof(AbstractController), "GetView")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestGetView_NullDefault()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter("NonDummy")]
        [UnitTestTarget(typeof(AbstractController), "GetView")]
        [UnitTestAssert(null)]
        public void TestGetView_NotFoundClass()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter("NonDummy")]
        [UnitTestTarget(typeof(AbstractController), "GetView")]
        [UnitTestAssert(null)]
        public void TestGetView_NonViewClass()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }


        [TestMethod]
        [UnitTestParameter("Dummy")]
        [UnitTestTarget(typeof(AbstractController), "GetView")]
        [UnitTestAssert(typeof(DummyView), typeof(UnitTestUtility), "TypeAssert")]
        public void TestGetView_Cache()
        {
            UnitTestUtility.ExecuteMultipleUnitTest(Target);
        }
        #endregion
    }
}
