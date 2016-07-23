using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using System.Reflection;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using WindowsApplicationTest.Utility;

namespace WindowsApplicationTest.Framework.Controllers.Commands
{
    [TestClass]
    public class AbstractCommandTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private AbstractCommand Target
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
            this.Target = new ActionCommand();

            // Mvc変換インスタンス取得
            MvcCooperationData.Current = new MvcCooperationData(new Assembly[] { Assembly.GetExecutingAssembly() });
        }
        #endregion

        #region 破棄
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        [TestCleanup()]
        public void TestCleanup()
        {
            CommandRegister.Current = null;
            this.Target = null;
        }
        #endregion

        #region Executeメソッド
        [TestMethod]
        [UnitTestParameter(null)]
        [UnitTestParameter(typeof(EventArgs), "Empty", BindingFlags.Static | BindingFlags.Public, true)]
        [UnitTestTarget(typeof(AbstractCommand), "Execute", BindingFlags.Instance | BindingFlags.Public, typeof(object), typeof(EventArgs))]
        [UnitTestAssert(true, typeof(UnitTestUtility), "InstanceAssert")]
        public void TestExecute()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }
        #endregion

    }
}
