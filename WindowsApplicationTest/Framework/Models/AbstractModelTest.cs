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

namespace WindowsApplicationTest.Framework.Models
{
    /// <summary>
    /// AbstractModelクラスのユニットテスト
    /// </summary>
    [TestClass]
    public class AbstractModelTest
    {
        #region プロパティ
        /// <summary>
        /// テスト対象
        /// </summary>
        private AbstractModel Target
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
            this.Target = new DummyModel();

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

        #region Cloneメソッド
        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestInitClone()
        {
            var clone = (DummyModel)this.Target.Clone();

            Assert.IsNull(clone.StringValue);
            Assert.AreEqual(0, clone.Int32Value);
            Assert.IsNull(clone.ByteArrayValue);
        }

        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestSetValueClone()
        {
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            var clone = (DummyModel)this.Target.Clone();

            Assert.AreEqual(stringValue, clone.StringValue);
            Assert.AreEqual(int32Value, clone.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, clone.ByteArrayValue.Length);
        }

        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestSetValueCloneClone()
        {
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            var clone = (DummyModel)this.Target.Clone();

            Assert.AreEqual(stringValue, clone.StringValue);
            Assert.AreEqual(int32Value, clone.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, clone.ByteArrayValue.Length);

            clone = (DummyModel)clone.Clone();

            Assert.AreEqual(stringValue, clone.StringValue);
            Assert.AreEqual(int32Value, clone.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, clone.ByteArrayValue.Length);
        }

        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestSetValueCloneChange()
        {
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            var clone = (DummyModel)this.Target.Clone();
            ((DummyModel)this.Target).StringValue = "";
            ((DummyModel)this.Target).Int32Value = 0;
            ((DummyModel)this.Target).ByteArrayValue = new byte[0];

            Assert.AreEqual(stringValue, clone.StringValue);
            Assert.AreEqual(int32Value, clone.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, clone.ByteArrayValue.Length);
        }

        /// <summary>
        /// 初期状態のコピー - 中にオブジェクト
        /// </summary>
        [TestMethod]
        public void TestSetValue2Clone()
        {
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            this.Target = new Dummy2Model();
            ((Dummy2Model)this.Target).StringValue = stringValue;
            ((Dummy2Model)this.Target).Int32Value = int32Value;
            ((Dummy2Model)this.Target).ByteArrayValue = byteArrayValue;
            ((Dummy2Model)this.Target).DummyModelValue = new DummyModel();
            ((Dummy2Model)this.Target).DummyModelValue.StringValue = stringValue;

            var clone = (Dummy2Model)this.Target.Clone();

            Assert.AreEqual(stringValue, clone.StringValue);
            Assert.AreEqual(int32Value, clone.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, clone.ByteArrayValue.Length);
            Assert.AreEqual(stringValue, clone.DummyModelValue.StringValue);
        }

        /// <summary>
        /// 初期状態のコピー - 中にオブジェクト
        /// 内部のオブジェクトの値は置き換わる
        /// </summary>
        [TestMethod]
        public void TestSetValue2CloneChange()
        {
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            this.Target = new Dummy2Model();
            ((Dummy2Model)this.Target).StringValue = stringValue;
            ((Dummy2Model)this.Target).Int32Value = int32Value;
            ((Dummy2Model)this.Target).ByteArrayValue = byteArrayValue;
            ((Dummy2Model)this.Target).DummyModelValue = new DummyModel();
            ((Dummy2Model)this.Target).DummyModelValue.StringValue = stringValue;

            var clone = (Dummy2Model)this.Target.Clone();
            ((Dummy2Model)this.Target).StringValue = "";
            ((Dummy2Model)this.Target).Int32Value = 0;
            ((Dummy2Model)this.Target).ByteArrayValue = new byte[0];
            ((Dummy2Model)this.Target).DummyModelValue.StringValue = "";


            Assert.AreEqual(stringValue, clone.StringValue);
            Assert.AreEqual(int32Value, clone.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, clone.ByteArrayValue.Length);
            Assert.AreEqual(stringValue, clone.DummyModelValue.StringValue);
        }
        #endregion

        #region Copyメソッド
        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestInitCopy()
        {
            var src = new DummyModel();

            this.Target.Copy(src);

            var obj = (DummyModel)this.Target;
            Assert.IsNull(obj.StringValue);
            Assert.AreEqual(0, obj.Int32Value);
            Assert.IsNull(obj.ByteArrayValue);
        }

        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestSetValueCopy()
        {
            var src = new DummyModel();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            src.StringValue = stringValue;
            src.Int32Value = int32Value;
            src.ByteArrayValue = byteArrayValue;

            this.Target.Copy(src);

            var obj = (DummyModel)this.Target;

            Assert.AreEqual(stringValue, obj.StringValue);
            Assert.AreEqual(int32Value, obj.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, obj.ByteArrayValue.Length);
        }

        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestSetValue2Copy()
        {
            var src = new Dummy2Model();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            src.StringValue = stringValue;
            src.Int32Value = int32Value;
            src.ByteArrayValue = byteArrayValue;
            src.DummyModelValue = new DummyModel();
            src.DummyModelValue.StringValue = stringValue;

            this.Target.Copy(src);

            var obj = (DummyModel)this.Target;

            Assert.AreEqual(stringValue, obj.StringValue);
            Assert.AreEqual(int32Value, obj.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, obj.ByteArrayValue.Length);
        }

        /// <summary>
        /// 初期状態のコピー
        /// </summary>
        [TestMethod]
        public void TestSetValue1_2Copy()
        {
            var src = new DummyModel();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            src.StringValue = stringValue;
            src.Int32Value = int32Value;
            src.ByteArrayValue = byteArrayValue;

            this.Target.Copy(src);

            var obj = (DummyModel)this.Target;

            Assert.AreEqual(stringValue, obj.StringValue);
            Assert.AreEqual(int32Value, obj.Int32Value);
            Assert.AreEqual(byteArrayValue.Length, obj.ByteArrayValue.Length);
        }
        #endregion

        #region Loadメソッド
        [TestMethod]
        public void TestLoadXmlFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.Save(filePath);

            try
            {
                var obj = this.Target.Load(filePath);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadXmlFileException()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            Dummy3Model model = new Dummy3Model();
            model.Copy(this.Target);
            this.Target.Save(filePath);

            try
            {
                var obj = model.Load(filePath);

                Assert.IsNull(obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadJsonFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.Save(filePath, SerializeType.Json);

            try
            {
                var obj = this.Target.Load(filePath, SerializeType.Json);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadJsonFileException()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            Dummy3Model model = new Dummy3Model();
            model.Copy(this.Target);
            //this.Target.Save(filePath, SerializeType.Json);

            try
            {
                var obj = model.Load(filePath, SerializeType.Json);

                Assert.IsNull(obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadBinaryFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.Save(filePath, SerializeType.Binary);

            try
            {
                var obj = this.Target.Load(filePath, SerializeType.Binary);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadBinaryFileException()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            Dummy3Model model = new Dummy3Model();
            model.Copy(this.Target);
            //this.Target.Save(filePath, SerializeType.Binary);

            try
            {
                var obj = model.Load(filePath, SerializeType.Binary);

                Assert.IsNull(obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadNonFile()
        {
            String filePath = null;
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            var obj = this.Target.Load(filePath, SerializeType.Json);

            Assert.IsNull(obj);
        }
        #endregion

        #region LoadDecryptメソッド
        [TestMethod]
        public void TestLoadDecryptXmlFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.SaveCrypt(filePath, stringValue);

            try
            {
                var obj = this.Target.LoadDecrypt(filePath, stringValue);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadDecryptJsonFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.SaveCrypt(filePath, stringValue, SerializeType.Json);

            try
            {
                var obj = this.Target.LoadDecrypt(filePath, stringValue, SerializeType.Json);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadDecryptBinaryFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.SaveCrypt(filePath, stringValue, SerializeType.Binary);

            try
            {
                var obj = this.Target.LoadDecrypt(filePath, stringValue, SerializeType.Binary);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestLoadDecryptNonFile()
        {
            String filePath = null;
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            var obj = this.Target.LoadDecrypt(filePath, stringValue, SerializeType.Json);

            Assert.IsNull(obj);
        }
        #endregion

        #region Saveメソッド
        [TestMethod]
        public void TestSaveXmlFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.Save(filePath);

            try
            {
                bool actual = this.Target.Save(filePath);
                Assert.IsTrue(actual);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveXmlFileException()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            Dummy3Model model = new Dummy3Model();
            model.Copy(this.Target);

            try
            {
                bool actual = model.Save(filePath);
                Assert.IsFalse(actual);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveJsonFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            try
            {
                bool actual = this.Target.Save(filePath, SerializeType.Json);
                Assert.IsTrue(actual);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveJsonFileException()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            Dummy3Model model = new Dummy3Model();
            model.Copy(this.Target);

            try
            {
                bool actual = model.Save(filePath, SerializeType.Json);
                Assert.IsTrue(actual);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveBinaryFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            try
            {
                bool actual = this.Target.Save(filePath, SerializeType.Binary);
                Assert.IsTrue(actual);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveBinaryFileException()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            Dummy3Model model = new Dummy3Model();
            model.Copy(this.Target);

            try
            {
                bool actual = model.Save(filePath, SerializeType.Binary);
                Assert.IsFalse(actual);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveNonFile()
        {
            String filePath = null;
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            var obj = this.Target.Save(filePath, SerializeType.Json);

            Assert.IsFalse(obj);
        }
        #endregion

        #region SaveDecryptメソッド
        [TestMethod]
        public void TestSaveDecryptXmlFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.SaveCrypt(filePath, stringValue);

            try
            {
                var obj = this.Target.LoadDecrypt(filePath, stringValue);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveDecryptJsonFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.SaveCrypt(filePath, stringValue, SerializeType.Json);

            try
            {
                var obj = this.Target.LoadDecrypt(filePath, stringValue, SerializeType.Json);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveDecryptBinaryFile()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            this.Target.SaveCrypt(filePath, stringValue, SerializeType.Binary);

            try
            {
                var obj = this.Target.LoadDecrypt(filePath, stringValue, SerializeType.Binary);

                Assert.AreEqual(this.Target, obj);
            }
            finally
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void TestSaveDecryptNonFile()
        {
            String filePath = null;
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            var obj = this.Target.LoadDecrypt(filePath, stringValue, SerializeType.Json);

            Assert.IsNull(obj);
        }
        #endregion

        #region Validメソッド
        [TestMethod]
        public void TestValidRequiredInput()
        {
            var model = new Dummy4Model();
            model.StringRequireValue = "aaaa";
            model.Int32RequireValue = 0;
            model.DummyModelValue = new DummyModel();

            String message;
            bool actual = model.Valid(out message);
            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValidRequiredInputError_Empty()
        {
            var model = new Dummy4Model();
            model.StringRequireValue = "";
            model.Int32RequireValue = 0;
            model.DummyModelValue = new DummyModel();

            String message;
            bool actual = model.Valid(out message);
            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValidRequiredInputError_Null()
        {
            var model = new Dummy4Model();
            model.StringRequireValue = null;
            model.Int32RequireValue = 0;
            model.DummyModelValue = new DummyModel();

            String message;
            bool actual = model.Valid(out message);
            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValidRequiredInputError_Int32Null()
        {
            var model = new Dummy4Model();
            model.StringRequireValue = "aaaa";
            model.Int32RequireValue = null;
            model.DummyModelValue = new DummyModel();

            String message;
            bool actual = model.Valid(out message);
            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValidRequiredInputError_DummyModelNull()
        {
            var model = new Dummy4Model();
            model.StringRequireValue = "aaaa";
            model.Int32RequireValue = 0;
            model.DummyModelValue = null;

            String message;
            bool actual = model.Valid(out message);
            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValidAlphabetInput()
        {
            var model = new Dummy4Model();
            model.StringRequireValue = "aaaa";
            model.Int32RequireValue = 0;
            model.StringAlphabetValue = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            model.DummyModelValue = new DummyModel();

            String message;
            bool actual = model.Valid(out message);
            Assert.IsTrue(actual);
            Assert.IsTrue(string.IsNullOrEmpty(message));
        }

        [TestMethod]
        public void TestValidAlphabetInput_Error()
        {
            var model = new Dummy4Model();
            model.StringRequireValue = "aaaa";
            model.Int32RequireValue = 0;
            model.StringAlphabetValue = "abcdefghijklmnopqrstuvwxyz1";
            model.DummyModelValue = new DummyModel();

            String message;
            bool actual = model.Valid(out message);
            Assert.IsFalse(actual);
            Assert.IsFalse(string.IsNullOrEmpty(message));
        }
        #endregion

        #region GetHashCodeメソッド
        [TestMethod]
        public void TestGetHashCode()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            int hashCode1 = this.Target.GetHashCode();
            int hashCode2 = this.Target.GetHashCode();

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void TestGetHashCodeConst1()
        {
            String stringValue = null;
            int int32Value = 0;
            byte[] byteArrayValue = null;

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            int hashCode1 = this.Target.GetHashCode();
            int hashCode2 = 16337;

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void TestGetHashCodeConst2()
        {
            String stringValue = "";
            int int32Value = 0;
            byte[] byteArrayValue = null;

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            int hashCode1 = this.Target.GetHashCode();
            int hashCode2 = -2088873587;

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void TestGetHashCodeConst3()
        {
            String stringValue = null;
            int int32Value = 1;
            byte[] byteArrayValue = null;

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            int hashCode1 = this.Target.GetHashCode();
            int hashCode2 = 17298;

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void TestGetHashCodeConst4()
        {
            String stringValue = "";
            int int32Value = 0;
            byte[] byteArrayValue = new byte[2];

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            int hashCode1 = this.Target.GetHashCode();
            int hashCode2 = 2010826963;

            Assert.AreEqual(hashCode1, hashCode2);
        }

        [TestMethod]
        public void TestGetHashCodeConst5()
        {
            String stringValue = "";
            int int32Value = 0;
            byte[] byteArrayValue = new byte[]{ 12, 34 };

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            int hashCode1 = this.Target.GetHashCode();
            int hashCode2 = 2010828389;

            Assert.AreEqual(hashCode1, hashCode2);
        }


        [TestMethod]
        public void TestGetHashCodeConst6()
        {
            String stringValue = "";
            int int32Value = 0;
            byte?[] byteArrayValue = new byte?[] { 12, 34, null };

            this.Target = new Dummy5Model();
            ((Dummy5Model)this.Target).StringValue = stringValue;
            ((Dummy5Model)this.Target).Int32Value = int32Value;
            ((Dummy5Model)this.Target).ByteArrayValue = byteArrayValue;

            int hashCode1 = this.Target.GetHashCode();
            int hashCode2 = -2088829381;

            Assert.AreEqual(hashCode1, hashCode2);
        }
        #endregion

        #region ToStringメソッド
        [TestMethod]
        public void TestToString()
        {
            String filePath = Path.GetTempFileName();
            String stringValue = DateTime.Now.ToString();
            int int32Value = (int)DateTime.Now.Ticks;
            byte[] byteArrayValue = UTF8Encoding.UTF8.GetBytes(stringValue);

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            string string1 = this.Target.ToString();
            string string2 = this.Target.ToString();

            Assert.AreEqual(string1, string2);
        }

        [TestMethod]
        public void TestToString1()
        {
            String stringValue = null;
            int int32Value = 0;
            byte[] byteArrayValue = null;

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            String string1 = this.Target.ToString();
            String string2 = "DummyModel:<StringValue>k__BackingField-[],<Int32Value>k__BackingField-[0],<ByteArrayValue>k__BackingField-[],";

            Assert.AreEqual(string1, string2);
        }

        [TestMethod]
        public void TestToString2()
        {
            String stringValue = "";
            int int32Value = 0;
            byte[] byteArrayValue = null;

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            String string1 = this.Target.ToString();
            String string2 = "DummyModel:<StringValue>k__BackingField-[],<Int32Value>k__BackingField-[0],<ByteArrayValue>k__BackingField-[],";

            Assert.AreEqual(string1, string2);
        }

        [TestMethod]
        public void TestToString3()
        {
            String stringValue = null;
            int int32Value = 1;
            byte[] byteArrayValue = null;

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            string string1 = this.Target.ToString();
            string string2 = "DummyModel:<StringValue>k__BackingField-[],<Int32Value>k__BackingField-[1],<ByteArrayValue>k__BackingField-[],";

            Assert.AreEqual(string1, string2);
        }

        [TestMethod]
        public void TestToString4()
        {
            String stringValue = "";
            int int32Value = 0;
            byte[] byteArrayValue = new byte[2];

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            string string1 = this.Target.ToString();
            string string2 = "DummyModel:<StringValue>k__BackingField-[],<Int32Value>k__BackingField-[0],<ByteArrayValue>k__BackingField-[System.Byte[]],";

            Assert.AreEqual(string1, string2);
        }

        [TestMethod]
        public void TestToString5()
        {
            String stringValue = "";
            int int32Value = 0;
            byte[] byteArrayValue = new byte[] { 12, 34 };

            ((DummyModel)this.Target).StringValue = stringValue;
            ((DummyModel)this.Target).Int32Value = int32Value;
            ((DummyModel)this.Target).ByteArrayValue = byteArrayValue;

            string string1 = this.Target.ToString();
            string string2 = "DummyModel:<StringValue>k__BackingField-[],<Int32Value>k__BackingField-[0],<ByteArrayValue>k__BackingField-[System.Byte[]],";

            Assert.AreEqual(string1, string2);
        }


        [TestMethod]
        public void TestToString6()
        {
            String stringValue = "";
            int int32Value = 0;
            byte?[] byteArrayValue = new byte?[] { 12, 34, null };

            this.Target = new Dummy5Model();
            ((Dummy5Model)this.Target).StringValue = stringValue;
            ((Dummy5Model)this.Target).Int32Value = int32Value;
            ((Dummy5Model)this.Target).ByteArrayValue = byteArrayValue;

            string string1 = this.Target.ToString();
            string string2 = "Dummy5Model:<StringValue>k__BackingField-[],<Int32Value>k__BackingField-[0],<ByteArrayValue>k__BackingField-[System.Nullable`1[System.Byte][]],";

            Assert.AreEqual(string1, string2);
        }

        #endregion
    }
}
