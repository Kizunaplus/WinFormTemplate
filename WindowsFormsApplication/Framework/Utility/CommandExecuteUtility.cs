using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Kizuna.Plus.WinMvcForm.Framework.Utility
{
    class CommandExecuteUtility
    {
        /// <summary>
        /// 実行
        /// </summary>
        /// <param name="execName">実行ファイルパス</param>
        /// <param name="parameters">実行ファイルの引数</param>
        public static object Execute(String execName, object[] parameters)
        {
            String result = "";
            using (Process process = new Process())
            {
                // 実行ファイルの設定
                process.StartInfo.FileName = execName;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                // 引数を作成
                StringBuilder argument = new StringBuilder();
                foreach (object parameter in parameters)
                {
                    argument.AppendFormat("\"{0}\" ", parameter);
                }

                // 引数設定
                process.StartInfo.Arguments = argument.ToString();

                // コマンド実行
                process.Start();

                // 標準とエラー出力を取得
                result = process.StandardOutput.ReadToEnd();
                result += process.StandardError.ReadToEnd();

                process.WaitForExit();
            }

            return result;
        }
    }
}
