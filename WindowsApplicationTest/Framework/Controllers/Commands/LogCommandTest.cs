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
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using WindowsApplicationTest.Utility;

namespace WindowsApplicationTest.Framework.Controllers.Commands
{
    [TestClass]
    public class LogCommandTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private LogCommand Target
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
            this.Target = new LogCommand();

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
        [UnitTestParameter(LogType.Exception)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { "" })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Exception()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Exception)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(false)]
        public void TestExecute_LogType_Exception_NonParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Exception)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { "1" })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Exception_OneParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Exception)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { "1", "2"})]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Exception_TwoParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Exception)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { "1", "2", "3" })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Exception_ThreeParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Error)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Error_NonParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Warn)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Warn_NonParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Info)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Info_NonParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }

        [TestMethod]
        [UnitTestParameter(LogType.Debug)]
        [UnitTestParameter("message")]
        [UnitTestParameter(new object[] { })]
        [UnitTestTarget(typeof(LogCommand), "Execute", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, typeof(LogType), typeof(string), typeof(object[][]))]
        [UnitTestAssert(true)]
        public void TestExecute_LogType_Debug_NonParam()
        {
            UnitTestUtility.ExecuteUnitTest(Target);
        }
        #endregion

    }
}
