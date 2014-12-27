using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication.Services.CommandLine
{
    /// <summary>
    /// コマンドラインデータのサービス提供クラス
    /// </summary>
    class CommandLineService<T> : ICommandLineService<T> where T : new()
    {
        #region 変換
        /// <summary>
        /// コマンドラインデータをデータクラスに変換します。
        /// 
        /// 変換に失敗した内容は、例外のMessageに設定されます。
        /// </summary>
        /// <param name="commandLine">コマンドラインデータ</param>
        /// <returns>データクラス</returns>
        public T Parse(string[] commandLine)
        {
            Exception ex;
            // 変換処理
            T data = Parse(commandLine, out ex);
            if (ex != null)
            {
                throw ex;
            }

            return data;
        }

        /// <summary>
        /// コマンドラインデータをデータクラスに変換します。
        /// 
        /// 変換に失敗した内容は、例外のMessageに設定されます。
        /// </summary>
        /// <param name="commandLine">コマンドラインデータ</param>
        /// <returns>データクラス</returns>
        private T Parse(string[] commandLine, out Exception ex)
        {
            T data = new T();
            StringBuilder message = new StringBuilder();

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            for (int argIndex = 1; argIndex < commandLine.Length; argIndex++)
            {
                PropertyInfo foundProperty = null;
                foreach (PropertyInfo property in properties)
                {
                    if (commandLine[argIndex].Equals("/" + property.Name, StringComparison.CurrentCultureIgnoreCase) == true)
                    {
                        // プロパティ名と一致する引数を発見
                        foundProperty = property;
                        break;
                    }
                }
                if (foundProperty == null)
                {
                    if (commandLine[argIndex].StartsWith("/") == false)
                    {
                        // key名が存在していないがプロパティ[_value]が存在する場合はそこへ設定する
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.Name.Equals("_value", StringComparison.CurrentCultureIgnoreCase) == true)
                            {
                                foundProperty = property;
                                break;
                            }
                        }
                    }

                    if (foundProperty == null)
                    {
                        message.AppendFormat("引数{0}は、指定可能な引数ではありません。\r\n", commandLine[argIndex]);
                        continue;
                    }
                }
                // 論理型
                if (foundProperty.PropertyType == typeof(bool))
                {
                    // boolの場合は定義が存在するだけでtrueを設定
                    foundProperty.SetValue(data, true, null);
                    continue;
                }

                // その他は、ConvertChangeTypeによって変換する。
                if (foundProperty.Name.Equals("_value", StringComparison.CurrentCultureIgnoreCase) == false)
                {
                    argIndex++;
                }
                if (commandLine.Length <= argIndex)
                {
                    continue;
                }

                try
                {
                    foundProperty.SetValue(data, Convert.ChangeType(commandLine[argIndex], foundProperty.PropertyType, CultureInfo.CurrentCulture), null);
                }
                catch
                {
                    message.AppendFormat("引数{0}のデータは、指定可能なデータ型ではありません: {1}\r\n", commandLine[argIndex - 1], commandLine[argIndex]);
                    argIndex--;
                }
            }

            ex = null;
            if (0 < message.Length)
            {
                // エラーメッセージが設定されている場合は例外を設定
                ex = new ArgumentException(message.ToString());
            }

            return data;
        }

        /// <summary>
        /// コマンドラインデータをデータクラスに変換します。
        /// </summary>
        /// <param name="commandLine">コマンドラインデータ</param>
        /// <param name="data">データクラス</param>
        /// <returns>true: 成功, false: 失敗</returns>
        public bool TryParse(string[] commandLine, out T data)
        {
            Exception ex;
            // 変換処理
            data = Parse(commandLine, out ex);

            return ex == null;
        }
        #endregion
    }
}
