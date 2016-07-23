using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;

namespace Kizuna.Plus.WinMvcForm.Framework.Logger
{
    /// <summary>
    /// Log4Netで出力クラス
    /// </summary>
    class Log4NetLogger
    {
        #region 呼び出し元を取得
        /// <summary>
        /// 実行メソッドを呼び出したメソッドを取得します。
        /// </summary>
        /// <returns></returns>
        private static MethodBase GetBeforeCallMethodBase()
        {
            MethodBase methodBase = null;
            StackTrace stackTrace = new StackTrace();
            for (int frame = 0; frame < stackTrace.FrameCount; frame++)
            {
                StackFrame stackFrame = stackTrace.GetFrame(frame);
                methodBase = stackFrame.GetMethod();
                if (0 <= methodBase.DeclaringType.FullName.IndexOf("Kizuna.Plus.WinMvcForm.Framework")
                    || 0 == methodBase.DeclaringType.FullName.IndexOf("System")
                    )
                {
                    // Frameworkのログクラスのため
                    continue;
                }

                if (MethodBase.GetCurrentMethod().DeclaringType.FullName
                != methodBase.DeclaringType.FullName)
                {
                    break;
                }
            }
            stackTrace = null;
            return methodBase;
        }
        #endregion

        #region 出力処理
        /// <summary>
        /// デバッグメッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Debug(string message)
        {
            Type type = GetBeforeCallMethodBase().DeclaringType;
            log4net.ILog log = log4net.LogManager.GetLogger(type); 
            log.Debug(message);
        }

        /// <summary>
        /// 情報メッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Info(string message)
        {
            Type type = GetBeforeCallMethodBase().DeclaringType;
            log4net.ILog log = log4net.LogManager.GetLogger(type); 
            log.Info(message);
        }

        /// <summary>
        /// 警告メッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Warn(string message)
        {
            Type type = GetBeforeCallMethodBase().DeclaringType;
            log4net.ILog log = log4net.LogManager.GetLogger(type);
            log.Warn(message);
        }

        /// <summary>
        /// エラーメッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        public void Error(string message)
        {
            Type type = GetBeforeCallMethodBase().DeclaringType;
            log4net.ILog log = log4net.LogManager.GetLogger(type); 
            log.Error(message);
        }

        /// <summary>
        /// 致命的な障害メッセージの出力
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="ex">例外</param>
        public void Fatal(string message, Exception ex)
        {
            Type type = GetBeforeCallMethodBase().DeclaringType;
            log4net.ILog log = log4net.LogManager.GetLogger(type); 
            if (ex == null)
            {
                log.Fatal(message);
            }
            else
            {
                log.Fatal(message, ex);
            }
        }

        /// <summary>
        /// 致命的な障害メッセージの出力
        /// </summary>
        /// <param name="ex">例外</param>
        public void Fatal(Exception ex)
        {
            Type type = GetBeforeCallMethodBase().DeclaringType;
            log4net.ILog log = log4net.LogManager.GetLogger(type); 
            log.Fatal("", ex);
        }
        #endregion
    }
}
