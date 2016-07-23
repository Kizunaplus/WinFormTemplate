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
using System.Threading;

namespace WindowsApplicationTest.Framework.Models
{
    /// <summary>
    /// ViewStateDataクラスのユニットテスト
    /// </summary>
    [TestClass]
    public class ViewStateDataTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private ViewStateData Target
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
            this.Target = ViewStateData.CurrentThread;
        }
        #endregion

        #region 破棄
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        [TestCleanup()]
        public void TestCleanup()
        {
            this.Target.Dispose();
            this.Target = null;
            
            var checkSlot = Thread.GetNamedDataSlot("TH_VIEW_STATE");
            if (Thread.GetData(checkSlot) != null)
            {
                // 現在のスレッドに新たなインスタンスを設定
                Thread.SetData(checkSlot, null);
            }
        }
        #endregion

        #region CurrentThreadプロパティ
        /// <summary>
        /// CurrentThreadプロパティ
        /// </summary>
        [TestMethod]
        public void CurrentThreadTest()
        {
            var viewData = this.Target;

            Assert.IsNotNull(viewData);
            Assert.IsNotNull(viewData.Items);
            Assert.AreEqual(0, viewData.Items.Count);
        }
        #endregion

        #region Itemsプロパティ
        /// <summary>
        /// Itemsプロパティ
        /// </summary>
        [TestMethod]
        public void ItemsTest()
        {
            var viewData = this.Target;

            Assert.IsNotNull(viewData);
            Assert.IsNotNull(viewData.Items);
            Assert.AreEqual(0, viewData.Items.Count);
        }

        /// <summary>
        /// Itemsプロパティ
        /// </summary>
        [TestMethod]
        public void ItemsAddTest()
        {
            var viewData = this.Target;

            Assert.IsNotNull(viewData);
            Assert.IsNotNull(viewData.Items);
            Assert.AreEqual(0, viewData.Items.Count);

            viewData.Items.Add("aaaa", "bbbb");
            Assert.AreEqual(1, viewData.Items.Count);
        }

        /// <summary>
        /// Itemsプロパティ
        /// </summary>
        [TestMethod]
        public void ItemsAdd2Test()
        {
            var viewData = this.Target;

            Assert.IsNotNull(viewData);
            Assert.IsNotNull(viewData.Items);
            Assert.AreEqual(0, viewData.Items.Count);

            viewData.Items.Add("aaaa1", "bbbb");
            viewData.Items.Add("aaaa2", "bbbb");
            Assert.AreEqual(2, viewData.Items.Count);
        }

        /// <summary>
        /// Itemsプロパティ
        /// </summary>
        [TestMethod]
        public void ItemsAdd2OtherThreadTest()
        {
            var viewData = this.Target;

            Assert.IsNotNull(viewData);
            Assert.IsNotNull(viewData.Items);
            Assert.AreEqual(0, viewData.Items.Count);

            viewData.Items.Add("aaaa1", "bbbb");
            viewData.Items.Add("aaaa2", "bbbb");
            Assert.AreEqual(2, viewData.Items.Count);

            var thread = new Thread(delegate() {
                var items = ViewStateData.CurrentThread.Items;
                Assert.AreEqual(0, items.Count);
            });
            thread.Start();
            thread.Join();
            
        }
        #endregion
    }
}
