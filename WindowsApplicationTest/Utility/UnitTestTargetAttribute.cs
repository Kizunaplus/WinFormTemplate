using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WindowsApplicationTest.Utility
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class UnitTestTargetAttribute : System.Attribute
    {
        /// <summary>
        /// テストメソッド名
        /// </summary>
        private Type classType;

        /// <summary>
        /// テストメソッド名
        /// </summary>
        private String methodName;

        /// <summary>
        /// バインドフラグ
        /// </summary>
        private BindingFlags flags;

        /// <summary>
        /// メソッド引数
        /// </summary>
        private Type[] types;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="classType">クラス</param>
        /// <param name="methodName">メソッド名</param>
        public UnitTestTargetAttribute(Type classType, String methodName)
        {
            this.classType = classType;
            this.methodName = methodName;
            this.flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod;
            this.types = null;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="classType">クラス</param>
        /// <param name="methodName">メソッド名</param>
        public UnitTestTargetAttribute(Type classType, String methodName, BindingFlags flags,  params Type[] types)
        {
            this.classType = classType;
            this.methodName = methodName;
            this.flags = flags;
            this.types = types;
        }

        /// <summary>
        /// テストメソッド名を取得
        /// </summary>
        public Type ClassType
        {
            get
            {
                return classType;
            }
        }

        /// <summary>
        /// テストメソッド名を取得
        /// </summary>
        public String MethodName
        {
            get
            {
                return methodName;
            }
        }

        public MethodInfo Method
        {
            get
            {
                if (types == null)
                {
                    return classType.GetMethod(methodName, flags);
                }

                return classType.GetMethod(methodName, flags, null, types, null);
            }
        }


    }
}
