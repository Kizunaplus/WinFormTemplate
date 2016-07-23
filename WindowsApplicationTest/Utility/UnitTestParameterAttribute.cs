using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WindowsApplicationTest.Utility
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class UnitTestParameterAttribute : System.Attribute
    {
        /// <summary>
        /// テストメソッドのパラメータ
        /// </summary>
        private Object parameter;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="parameters"></param>
        public UnitTestParameterAttribute(Object parameter)
        {
            this.parameter = parameter;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">取得対象の型</param>
        /// <param name="name">取得対象のプロパティ名</param>
        public UnitTestParameterAttribute(Type type, String name, BindingFlags flags, bool isField = false)
        {
            if (isField == false) { 
                PropertyInfo property = type.GetProperty(name, flags | BindingFlags.GetProperty);

                this.parameter = property.GetValue(null, null);
            } else {
                FieldInfo field = type.GetField(name, flags | BindingFlags.GetField);

                this.parameter = field.GetValue(null);
   
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="type">取得対象の型</param>
        /// <param name="name">取得対象の実行メソッド名</param>
        public UnitTestParameterAttribute(Type type, BindingFlags flags, params object[] parameter)
        {
            List<Type> types = new List<Type>();
            foreach (object obj in parameter)
            {
                types.Add(obj.GetType());
            }

            ConstructorInfo method = type.GetConstructor(flags | BindingFlags.CreateInstance, null, types.ToArray(), null);

            this.parameter = method.Invoke(null, parameter);
        }

        /// <summary>
        /// テストメソッドのパラメータを取得
        /// </summary>
        public Object Parameter
        {
            get
            {
                return parameter;
            }
        }

    }
}
