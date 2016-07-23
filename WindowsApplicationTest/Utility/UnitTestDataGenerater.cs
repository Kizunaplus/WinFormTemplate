using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsApplicationTest.Utility
{
    class UnitTestDataGenerater
    {
        /// <summary>
        /// ランダム発生器
        /// </summary>
        private Random random = new Random();

        public T generate<T>(Type type)
        {
            object obj = null;
            if (type == typeof(int))
            {
                obj = random.Next();
            } else if(type == typeof(long))
            {
                byte[] buf = new byte[8];
                random.NextBytes(buf);
                obj = BitConverter.ToInt64(buf, 0);
            }
            else if (type == typeof(float))
            {
                obj = (float)random.NextDouble();
            }
            else if (type == typeof(double))
            {
                obj = random.NextDouble();
            }
            else if (type == typeof(Byte[]))
            {
                byte[] bytes = new byte[random.Next(15) + 1];
                random.NextBytes(bytes);
                obj = bytes;
            } else if (type == typeof(String))
            {
                int length = random.Next(7) + 1;
                obj = System.Guid.NewGuid().ToString("N").Substring(length);
            }
            else if (type == typeof(DateTime))
            {

                obj = new DateTime(generate<long>(typeof(long)));
            }

            return (T)obj;
        }
    }
}
