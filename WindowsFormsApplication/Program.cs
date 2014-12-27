using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication.Controllers.Commands;
using WindowsFormsApplication.Controllers.State;
using WindowsFormsApplication.Models;
using WindowsFormsApplication.Models.EventArg;
using WindowsFormsApplication.Services.CommandLine;
using WindowsFormsApplication.Services.File;

namespace WindowsFormsApplication
{
    /// <summary>
    /// エントリーポイントクラス
    /// </summary>
    static class Program
    {
        #region メンバー変数
        /// <summary>
        /// 多重起動抑止チェック用Mutex
        /// </summary>
        private static System.Threading.Mutex mutex;
        #endregion

        #region エントリーポイント
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // プロセスIDの取得
            int processId = Process.GetCurrentProcess().Id;

            // ログに出力
            var logCommand = new LogCommand();
            logCommand.Execute(new DebugState(typeof(Application)), new LogMessageEventArgs() { Message = string.Format("Start Process : {0}", processId) });

#if DUPLICATE_EXEC
            // 多重起動抑止
            if (IsDuplicateExec() == true)
            {
                // ログに出力
                logCommand.Execute(new DebugState(typeof(Application)), new LogMessageEventArgs() { Message = string.Format("Duplicate Process : {0}", processId) });

                return;
            }
#endif

#if ENABLE_COMMANDLINE
            // コマンドラインデータの解析
            ParseCommandLine();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if ENABLE_SPLASH
            // スプラッシュ表示
            SprashForm.ShowSplash(Properties.Resources.splash);
#endif

#if ENABLE_CONFIGURATION
            // 設定ファイルの読み込み
            ReadConfiguration();
#endif

            // 例外処理
            Application.ThreadException += Application_ThreadException;
            // 終了処理
            Application.ApplicationExit += Application_ApplicationExit;

            // 起動
            var mainForm = new MainForm();
            mainForm.ChangeController(AppEnviroment.Default.StartController);
            Application.Run(mainForm);

            // ログに出力
            logCommand.Execute(new DebugState(typeof(Application)), new LogMessageEventArgs() { Message = string.Format("End Process : {0}", processId) });
        }
        #endregion

        #region 多重起動抑止
        /// <summary>
        /// すでに実行中のプロセスが存在するか
        /// </summary>
        /// <returns>true: 存在する, false: 存在しない</returns>
        private static bool IsDuplicateExec()
        {
            // 多重起動抑止
            bool createdNew;
            string mutexName = Application.CompanyName + Application.ProductName;
            mutex = new System.Threading.Mutex(true, mutexName, out createdNew);
            if (createdNew == false)
            {
                //ミューテックスの初期所有権が付与されなかったときは
                //すでに起動していると判断して終了

                //ミューテックスを解放する
                mutex.ReleaseMutex();
                return true;
            }

            return false;
        }
        #endregion

        #region コマンドライン解析
        /// <summary>
        /// コマンドラインデータの解析
        /// </summary>
        private static void ParseCommandLine()
        {
            // コマンドラインデータの解析
            CommandLineData commandData;
            var commandLineParser = new CommandLineService<CommandLineData>();
            if (commandLineParser.TryParse(Environment.GetCommandLineArgs(), out commandData) == false)
            {
#if DEBUG
                try
                {
                    commandLineParser.Parse(Environment.GetCommandLineArgs());
                }
                catch (Exception ex)
                {
                    // ログに出力
                    var logCommand = new LogCommand();
                    logCommand.Execute(new DebugState(typeof(Application)), new ExceptionEventArgs() { Exception = ex });
                }
#endif
            }
        }
        #endregion

        #region 設定
        /// <summary>
        /// 設定情報の読み込み
        /// </summary>
        private static void ReadConfiguration()
        {
            ConfigurationData.Current = new ConfigurationData().Load(ConfigurationData.GetConfigurationFilePath()) as ConfigurationData;

            // 設定情報を作成
            if (ConfigurationData.Current == null)
            {
                ConfigurationData.Current = new ConfigurationData();
            }
        }

        /// <summary>
        /// 設定情報の保存
        /// </summary>
        private static void SaveConfiguration()
        {
            var data = ConfigurationData.Current;
            if (data == null)
            {
                return;
            }

            var service = new BackupFileService();
            service.BackupWriteFile(ConfigurationData.GetConfigurationFilePath(), data);
        }
        #endregion

        #region イベント
        /// <summary>
        /// アプリケーション終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ApplicationExit(object sender, EventArgs e)
        {
#if ENABLE_CONFIGURATION
            SaveConfiguration();
#endif

#if DUPLICATE_EXEC
            //ミューテックスを解放する
            mutex.ReleaseMutex();
#endif
        }

        /// <summary>
        /// 未Catch例外処理イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // ログに出力
            var logCommand = new LogCommand();
            logCommand.Execute(new ExceptionState(typeof(Application)), e);

#if DEBUG
            // 再Throw
            throw e.Exception;
#endif
        }
        #endregion
    }
}
