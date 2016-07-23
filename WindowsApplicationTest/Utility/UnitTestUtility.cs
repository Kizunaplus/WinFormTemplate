using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace WindowsApplicationTest.Utility
{
    /// <summary>
    /// ユニットテストユーティリティ
    /// </summary>
    class UnitTestUtility
    {
        public delegate void PreExecuteDelegate<T>(T instance, List<object> parameter, UnitTestAssertAttribute assertAttribute);

        /// <summary>
        /// ユニットテスト実行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">テスト対象インスタンス</param>
        /// <returns>テストメソッド返却値</returns>
        public static object ExecuteUnitTest<T>(T instance)
        {
            return ExecuteUnitTest(instance, null);
        }

        /// <summary>
        /// ユニットテスト実行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">テスト対象インスタンス</param>
        /// <returns>テストメソッド返却値</returns>
        public static object ExecuteUnitTest<T>(T instance, PreExecuteDelegate<T> preDelegate)
        {
            int stackIndex = 1;
            while (new StackFrame(stackIndex).GetMethod().DeclaringType.IsAssignableFrom(typeof(UnitTestUtility)))
            {
                stackIndex++;
            }
            MethodInfo callMethod = new StackFrame(stackIndex).GetMethod() as MethodInfo;

            UnitTestTargetAttribute targetAttr = Attribute.GetCustomAttribute(callMethod, typeof(UnitTestTargetAttribute)) as UnitTestTargetAttribute;
            if (targetAttr == null)
            {
                return null;
            }
            MethodInfo testMethod = targetAttr.Method;
            UnitTestParameterAttribute[] parameterAttrs = Attribute.GetCustomAttributes(callMethod, typeof(UnitTestParameterAttribute)) as UnitTestParameterAttribute[];
            List<object> parameters = new List<object>();
            if (parameterAttrs != null && 0 < parameterAttrs.Length)
            {
                foreach (UnitTestParameterAttribute parameterAttr in parameterAttrs)
                {
                    object obj = parameterAttr.Parameter;
                    if (obj is UnitTestDataGenerater)
                    {
                        obj = ((UnitTestDataGenerater)obj);
                    }
                    parameters.Add(obj);
                }   
            }

            UnitTestAssertAttribute assertData = Attribute.GetCustomAttribute(callMethod, typeof(UnitTestAssertAttribute)) as UnitTestAssertAttribute;
            if (preDelegate != null)
            {
                preDelegate(instance, parameters, assertData);
            }

 
            object returnValue = null;
            try
            {
                returnValue = testMethod.Invoke(instance, parameters.ToArray());

                if (assertData == null)
                {
                    return returnValue;
                }


                if (assertData.AssertDelegate != null)
                {
                    // 検証用のデリゲートを実行
                    assertData.AssertDelegate.DynamicInvoke(assertData.Expected, returnValue);
                }
                else
                {
                    InstanceAssert(assertData.Expected, returnValue);
                }
            } catch (Exception ex)
            {
                returnValue = ex;
                if (assertData == null)
                {
                    return returnValue;
                }

                if (assertData != null && assertData.AssertDelegate != null)
                {
                    assertData.AssertDelegate.DynamicInvoke(assertData.Expected, ex);
                }
                else
                {
                    InstanceAssert(assertData.Expected, ex);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// ユニットテスト実行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">テスト対象インスタンス</param>
        /// <returns>テストメソッド返却値</returns>
        public static object ExecuteMultipleUnitTest<T>(T instance, int count = 2)
        {
            object retObj = ExecuteUnitTest(instance);
            for (int index = 1; index < count; index++)
            {
                object retObjSec = ExecuteUnitTest(instance);

                Assert.AreEqual(retObj, retObjSec);
            }

            return retObj;
        }

        /// <summary>
        /// ユニットテスト実行
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">テスト対象インスタンス</param>
        /// <returns>テストメソッド返却値</returns>
        public static object ExecuteMultipleUnitTest<T>(T instance, PreExecuteDelegate<T> preDelegate, int count = 2)
        {
            object retObj = ExecuteUnitTest(instance, preDelegate);
            for (int index = 1; index < count; index++)
            {
                object retObjSec = ExecuteUnitTest(instance, preDelegate);

                Assert.AreEqual(retObj, retObjSec);
            }

            return retObj;
        }

        /// <summary>
        /// インスタンスの比較を行います。
        /// </summary>
        /// <param name="expected">期待値</param>
        /// <param name="actual">実際の結果</param>
        public static void InstanceAssert(Object expected, Object actual)
        {
            // 返却値と期待値を比較
            if (actual != null && actual.Equals(expected) == false)
            {
                Console.WriteLine(expected);
                if (actual is Exception)
                {
                    Exception ex = actual as Exception;
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                } else
                {
                    Console.WriteLine(actual);
                }
            }

            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// 型の比較を行います。
        /// </summary>
        /// <param name="expected">期待値</param>
        /// <param name="actual">実際の結果</param>
        public static void TypeAssert(Object expected, Object actual)
        {
            // 返却値と期待値を比較
            if (actual != null && !actual.GetType().Equals(expected))
            {
                Console.WriteLine(expected);
                Console.WriteLine(actual);
            }

            Assert.AreEqual(expected, actual.GetType());
        }

        /// <summary>
        /// 数の比較を行います。
        /// </summary>
        /// <param name="expected">期待値</param>
        /// <param name="actual">実際の結果</param>
        public static void LengthAssert(Object expected, Object actual)
        {
            // 返却値と期待値を比較
            if (actual == null || !(actual is ICollection) || ((ICollection)actual).Count != (int)expected)
            {
                Console.WriteLine(expected);
                Console.WriteLine(actual);
            }

            Assert.AreEqual(expected, ((ICollection)actual).Count);
        }

        /// <summary>
        /// 比較を行いません。
        /// </summary>
        /// <param name="expected">期待値</param>
        /// <param name="actual">実際の結果</param>
        public static void NonCheckAssert(Object expected, Object actual)
        {
            Console.WriteLine(expected);
            Console.WriteLine(actual);
        }
    }
}
