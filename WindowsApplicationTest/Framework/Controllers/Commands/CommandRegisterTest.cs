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

namespace WindowsApplicationTest.Framework.Controllers.Commands
{
    [TestClass]
    public class CommandRegisterTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private CommandRegister Target
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
            this.Target = new CommandRegister();
            CommandRegister.Current = this.Target;
            this.Target.UnregistOfSource(typeof(Application));

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

        #region Registメソッド
        [TestMethod]
        public void TestRegist_ActionCommand()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args){});

            this.Target.Regist(eventData);

            Assert.AreEqual(1, this.Target.Events.Count);
            Assert.AreEqual(eventData, this.Target.Events[0]);
        }

        [TestMethod]
        public void TestRegist_ActionCommand_Count2()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });

            this.Target.Regist(eventData);
            this.Target.Regist(eventData);

            Assert.AreEqual(2, this.Target.Events.Count);
            Assert.AreEqual(eventData, this.Target.Events[0]);
        }

        [TestMethod]
        public void TestRegist_HelpContentsCommand()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(HelpContentsCommand), typeof(HelpContentsCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });

            this.Target.Regist(eventData);
            this.Target.Regist(eventData);

            Assert.AreEqual(2, this.Target.Events.Count);
            Assert.AreEqual(eventData, this.Target.Events[0]);
        }
        #endregion

        #region Unregistメソッド
        [TestMethod]
        public void TestUnregist_ActionCommand()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });

            this.Target.Regist(eventData);
            this.Target.Unregist(eventData);


            Assert.AreEqual(0, this.Target.Events.Count);
        }

        [TestMethod]
        public void TestUnregist_ActionCommand_Count2()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });

            this.Target.Regist(eventData);
            this.Target.Regist(eventData);
            this.Target.Unregist(eventData);

            Assert.AreEqual(0, this.Target.Events.Count);
        }

        [TestMethod]
        public void TestUnregist_HelpContentsCommand()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(HelpContentsCommand), typeof(HelpContentsCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });

            this.Target.Regist(eventData);
            this.Target.Regist(eventData);
            this.Target.Unregist(eventData);

            Assert.AreEqual(0, this.Target.Events.Count);
        }
        #endregion

        #region UnregistOfSourceメソッド
        [TestMethod]
        public void TestUnregistOfSource_ActionCommand()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });

            this.Target.Regist(eventData);
            this.Target.UnregistOfSource(typeof(Application));


            Assert.AreEqual(0, this.Target.Events.Count);
        }

        [TestMethod]
        public void TestUnregistOfSource_ActionCommand_Count2()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });

            this.Target.Regist(eventData);
            this.Target.Regist(eventData);
            this.Target.UnregistOfSource(typeof(Application));

            Assert.AreEqual(0, this.Target.Events.Count);
        }

        [TestMethod]
        public void TestUnregistOfSource_HelpContentsCommand()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(HelpContentsCommand), typeof(HelpContentsCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });
            this.Target.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(HelpContentsCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { });
            this.Target.Regist(eventData);

            this.Target.UnregistOfSource(typeof(Application));

            Assert.AreEqual(1, this.Target.Events.Count);
        }
        #endregion

        #region Executeメソッド
        [TestMethod]
        public void TestExecute_ActionCommand()
        {
            bool isCalled = false;
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { isCalled = true; });

            this.Target.Regist(eventData);
            
            this.Target.Execute(new ActionCommand(), new NonState(typeof(Application)), EventArgs.Empty);
            Assert.IsTrue(isCalled);
        }

        [TestMethod]
        public void TestExecute_ActionCommand_Count3()
        {
            int callCount = 0;
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { callCount++; });
            this.Target.Regist(eventData);

            eventData = new CommandEventData(typeof(Application), typeof(EditCopyCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { callCount++; });
            this.Target.Regist(eventData);

            eventData = new CommandEventData(eventData, typeof(ActionCommand), StateMode.Error
                , delegate(object sender, EventArgs args) { callCount++; });
            this.Target.Regist(eventData);

            this.Target.Execute(new ActionCommand(), new NonState(typeof(Application)), EventArgs.Empty);
            Assert.AreEqual(1, callCount);
        }

        [TestMethod]
        public void TestExecute_NonSetCommand()
        {
            bool isCalled = false;
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), null, StateMode.End
                , delegate(object sender, EventArgs args) { isCalled = true; });

            this.Target.Regist(eventData);

            this.Target.Execute(new ActionCommand(), new NonState(typeof(Application)), EventArgs.Empty);
            Assert.IsTrue(isCalled);
        }

        [TestMethod]
        public void TestExecute_NonSetHandler()
        {
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), null, StateMode.None
                , null);

            this.Target.Regist(eventData);

            this.Target.Execute(new ActionCommand(), new NonState(typeof(Application)), EventArgs.Empty);
        }

        [TestMethod]
        public void TestExecute_RunStateCommand()
        {
            bool isCalled = false;
            // ログ出力処理(エラー)
            var eventData = new CommandEventData(typeof(Application), null, StateMode.Process
                , delegate(object sender, EventArgs args) { isCalled = true; });
            this.Target.Regist(eventData);
            eventData = new CommandEventData(typeof(Application), null, StateMode.End
                , delegate(object sender, EventArgs args) { isCalled = true; });
            this.Target.Regist(eventData);

            this.Target.Execute(new ActionCommand(), new RunState(typeof(Application)), EventArgs.Empty);
            Assert.IsTrue(isCalled);
        }
        #endregion

    }
}
