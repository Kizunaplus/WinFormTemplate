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
    /// StringLengthInputAttributeクラスのユニットテスト
    /// </summary>
    [TestClass]
    public class StringLengthInputAttributeTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private StringLengthInputAttribute Target
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
            this.Target = new StringLengthInputAttribute(10);
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
            String data = "";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Success1()
        {
            String data = "12345";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Success2()
        {
            String data = "1234567890";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Success3()
        {
            String data = null;
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Success4()
        {
            this.Target = new StringLengthInputAttribute(5, 10);
            String data = "12345";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Success5()
        {
            this.Target = new StringLengthInputAttribute(5, -1);
            String data = "123456789012345";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }


        [TestMethod]
        public void TestValid_Error()
        {
            String data = "12345678901";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Error1()
        {
            String data = "123456789012345";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Error2()
        {
            this.Target = new StringLengthInputAttribute(5, 10);
            String data = "1234";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Error3()
        {
            this.Target = new StringLengthInputAttribute(5, 10);
            String data = "12345678901";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValid_Error4()
        {
            this.Target = new StringLengthInputAttribute(5, -1);
            String data = "1234";
            String fieldName = "test_field";
            String message = "";
            bool actual = this.Target.Valid(data, ref message, fieldName);

            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }
        #endregion
    }
}
