using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;

namespace Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands
{
    /// <summary>
    /// ログ出力
    /// </summary>
    class LogCommand : AbstractCommand
    {
        /// <summary>
        /// コマンドの実行
        /// </summary>
        /// <param name="type">ログタイプ</param>
        /// <param name="message">メッセージ</param>
        /// <param name="param">メッセージのparam String.Formatの引数と同様, 例外出力の場合は、Exceptionを指定</param>
        /// <returns>true: 成功 ,false: 失敗</returns>
        public bool Execute(LogType type, String message, params object[] param)
        {
            Type sourceType = typeof(Application);

            IState state;
            EventArgs args;
            switch (type)
            {
                case LogType.Exception:
                    // 例外
                    state = new ExceptionState(sourceType);
                    if (param.Length <= 0)
                    {
                        return false;
                    }
                    args = new ExceptionEventArgs() { Exception = param[0] as Exception };
                    break;
                case LogType.Error:
                    // エラー
                    state = new ErrorState(sourceType);
                    args = new LogMessageEventArgs() { Message = string.Format(message, param) };
                    break;
                case LogType.Warn:
                    // 警告
                    state = new WarningState(sourceType);
                    args = new LogMessageEventArgs() { Message = string.Format(message, param) };
                    break;
                case LogType.Info:
                    // 情報
                    state = new InformationState(sourceType);
                    args = new LogMessageEventArgs() { Message = string.Format(message, param) };
                    break;
                case LogType.Debug:
                    // デバッグ
                    state = new DebugState(sourceType);
                    args = new LogMessageEventArgs() { Message = string.Format(message, param) };
                    break;
                default:
                    return false;
            }


            return Execute(state, args);
        }

    }
}
