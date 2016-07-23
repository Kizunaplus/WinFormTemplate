using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WindowsApplicationTest.Utility
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class UnitTestAssertAttribute : System.Attribute
    {
        public delegate void AssertDelegete(Object expected, Object actual);

        /// <summary>
        /// テスト結果期待値
        /// </summary>
        private Object expected;

        /// <summary>
        /// 検証用デリゲート
        /// </summary>
        private AssertDelegete assertDelegate;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="expected">期待値</param>
        public UnitTestAssertAttribute(object expected)
        {
            this.expected = expected;
            this.assertDelegate = null;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="expected">期待値</param>
        /// <param name="delegateType">期待値検証用クラス</param>
        /// <param name="delegateName">期待値検証用処理メソッド名</param>
        public UnitTestAssertAttribute(object expected, Type delegateType, string delegateName)
        {
            this.expected = expected;
            this.assertDelegate = (AssertDelegete)Delegate.CreateDelegate(typeof(AssertDelegete), delegateType.GetMethod(delegateName, BindingFlags.Public | BindingFlags.Static));
        }

        /// <summary>
        /// 期待値を取得
        /// </summary>
        public Object Expected
        {
            get
            {
                return expected;
            }
        }

        /// <summary>
        /// テスト結果検証用処理を取得
        /// </summary>
        public AssertDelegete AssertDelegate
        {
            get
            {
                return assertDelegate;
            }
        }


    }
}
