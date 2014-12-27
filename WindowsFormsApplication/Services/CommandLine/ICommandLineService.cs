using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication.Services.CommandLine
{
    /// <summary>
    /// コマンドラインデータのサービス提供インターフェース
    /// </summary>
    interface ICommandLineService<T>
    {
        /// <summary>
        /// コマンドラインデータをデータクラスに変換します。
        /// 
        /// 変換に失敗した内容は、例外のMessageに設定されます。
        /// </summary>
        /// <param name="commandLine">コマンドラインデータ</param>
        /// <returns>データクラス</returns>
        T Parse(string[] commandLine);

        /// <summary>
        /// コマンドラインデータをデータクラスに変換します。
        /// </summary>
        /// <param name="commandLine">コマンドラインデータ</param>
        /// <param name="data">データクラス</param>
        /// <returns>true: 成功, false: 失敗</returns>
        bool TryParse(string[] commandLine, out T data);
    }
}
