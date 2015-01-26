using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kizuna.Plus.WinMvcForm.Framework.Logger
{
    /// <summary>
    /// ログ出力生成クラス
    /// </summary>
    class LogFactory
    {
        #region メンバー変数
        /// <summary>
        /// 現在のインスタンス
        /// </summary>
        private static LogFactory current;

        /// <summary>
        /// ログ出力クラス
        /// </summary>
        public Log4NetLogger logService;
        #endregion

        #region プロパティ
        /// <summary>
        /// 現在のインスタンス
        /// </summary>
        public static LogFactory Current
        {
            get
            {
                if (current == null)
                {
                    current = new LogFactory();
                    current.LogService = new Log4NetLogger();
                }

                return current;
            }
            set
            {
                current = value;
            }
        }
        /// <summary>
        /// 現在のログ出力クラス
        /// </summary>
        public Log4NetLogger LogService
        {
            get
            {
                if (logService == null)
                {
                    current = new LogFactory();
                }

                return logService;
            }
            set
            {
                logService = value;
            }
        }
        #endregion

        #region 出力処理
        /// <summary>
        /// デバッグメッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Debug(string message)
        {
            Current.LogService.Debug(message);
        }

        /// <summary>
        /// 情報メッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Info(string message)
        {
            Current.LogService.Info(message);
        }

        /// <summary>
        /// 警告メッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Warn(string message)
        {
            Current.LogService.Warn(message);
        }

        /// <summary>
        /// エラーメッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public static void Error(string message)
        {
            Current.LogService.Error(message);
        }

        /// <summary>
        /// 致命的な障害メッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外</param>
        public static void Fatal(string message, Exception ex = null)
        {
            Current.LogService.Fatal(message, ex);
        }

        /// <summary>
        /// 致命的な障害メッセージの出力
        /// </summary>
        /// <param name="ex">例外</param>
        public static void Fatal(Exception ex)
        {
            Current.LogService.Fatal(ex);
        }
        #endregion
    }
}
