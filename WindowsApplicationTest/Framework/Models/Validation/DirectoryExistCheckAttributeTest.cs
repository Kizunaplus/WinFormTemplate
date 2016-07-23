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
using Kizuna.Plus.WinMvcForm.Framework.Models.Validation;

namespace WindowsApplicationTest.Framework.Models
{
    /// <summary>
    /// DirectoryExistCheckAttributeクラスのユニットテスト
    /// </summary>
    [TestClass]
    public class DirectoryExistCheckAttributeTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private DirectoryExistCheckAttribute Target
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
            this.Target = new DirectoryExistCheckAttribute(true);
        }
        #endregion

        #region 破棄
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        [TestCleanup()]
        public void TestCleanup()
        {
            this.Target = null;
        }
        #endregion

        #region Validメソッド
        [TestMethod]
        public void TestValid_Success()
        {
            String data = "C:\\";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Success1()
        {
            String data = "C:\\Windows";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Success2()
        {
            this.Target = new DirectoryExistCheckAttribute(false);
            String data = "C:\\Windows\\win.ini";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Error()
        {
            String data = "C:\\test";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Error1()
        {
            String data = "C:\\Windows\\win.ini";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Error2()
        {
            this.Target = new DirectoryExistCheckAttribute(false);
            String data = "C:\\Windows";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }
        #endregion
    }
}
