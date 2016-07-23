using System;
using System.Diagnostics;
using System.Windows.Forms;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using Kizuna.Plus.WinMvcForm.Framework.Utility;
using Microsoft.Win32;
using WindowsFormsApplication.Models;
using Kizuna.Plus.WinMvcForm.Framework.Services;
using WindowsFormsApplication.Framework.Message;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using Kizuna.Plus.WinMvcForm.Framework;
using Kizuna.Plus.WinMvcForm.Framework.Logger;
using System.Reflection;

namespace WindowsFormsApplication
{
    /// <summary>
    /// エントリーポイントクラス
    /// </summary>
    internal static class Program
    {
        #region 定数
        /// <summary>
        /// 64bit IEバージョン設定
        /// </summary>
        private const string IE_VERSION_EMULATION_32BIT = @"SOFTWARE\\Wow6432Node\\Microsoft\\Internet Explorer\\MAIN\\FeatureControl\\FEATURE_BROWSER_EMULATION";

        /// <summary>
        /// 32bit IEバージョン設定
        /// </summary>
        private const string IE_VERSION_EMULATION_64BIT = @"SOFTWARE\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION";
        #endregion

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
            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ProessStartMessage, processId);

            // IE バージョン設定
            var targetApplication = Process.GetCurrentProcess().ProcessName + ".exe";
            int ie_emulation = int.Parse(AppEnviroment.IEVersion);
            SetIE8KeyforWebBrowserControl(targetApplication, ie_emulation);

#if ENABLE_CONFIGURATION
            // 設定ファイルの読み込み
            // 条件付きコンパイルにて指定した場合実行を行います。
            ReadConfiguration();
#endif

            // ServicePoolの初期化
            ServicePool.Current = new ServicePool();
            ServicePool.Current.Initialize();


#if DUPLICATE_EXEC
            // 多重起動抑止
            // 条件付きコンパイルにて指定した場合実行を行います。
            if (IsDuplicateExec() == true)
            {
                // ログに出力
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ExistSameProcess, processId);
                return;
            }
#endif

#if ENABLE_COMMANDLINE
            // コマンドラインデータの解析
            // 条件付きコンパイルにて指定した場合実行を行います。
            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ProessStartCommandLineMessage, Environment.GetCommandLineArgs());
            ParseCommandLine();
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
#if ENABLE_SPLASH
            // スプラッシュ表示
            // 条件付きコンパイルにて指定した場合実行を行います。
            SprashForm.ShowSplash(Properties.Resources.splash);
#endif

            // 例外処理
            Application.ThreadException += Application_ThreadException;
            // 終了処理
            Application.ApplicationExit += Application_ApplicationExit;

            // Mvc変換インスタンス取得
            MvcCooperationData.Current = new MvcCooperationData();
            
            // 起動
            var mainForm = new MainForm();

            // StartControllerを実行
            ActionEventArgs eventArgs = new ActionEventArgs();
            eventArgs.Controller = AppEnviroment.StartController;
            ActionCommand command = new ActionCommand();
            command.Execute(new NonState(typeof(Application)), eventArgs);

            Application.Run(mainForm);
            mainForm.Dispose();

            // ログに出力
            LogFactory.Debug(String.Format(FrameworkDebugMessage.ProessEndMessage, processId));

            // IEバージョン設定削除
            RemoveIE8KeyforWebBrowserControl(targetApplication);
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
            var commandLineParser = new CommandLineUtility<CommandLineData>();
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
                    logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name);
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

            var service = new BackupFileUtility();
            service.BackupWriteFile(ConfigurationData.GetConfigurationFilePath(), data);
        }
        #endregion

        #region レジストリ
        /// <summary>
        /// WebBrowserのIEバージョン設定
        /// </summary>
        /// <param name="appName"></param>
        private static void SetIE8KeyforWebBrowserControl(string appName, int value)
        {
            RegistryKey Regkey = null;
            try
            {
                //For 64 bit Machine 
                if (Environment.Is64BitOperatingSystem)
                    Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(IE_VERSION_EMULATION_64BIT, true);
                else  //For 32 bit Machine 
                    Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(IE_VERSION_EMULATION_32BIT, true);

                //If the path is not correct or 
                //If user't have priviledges to access registry 
                if (Regkey == null)
                {
                    return;
                }

                string FindAppkey = Convert.ToString(Regkey.GetValue(appName));

                // すでに設定済みの場合
                if (FindAppkey == "" + value)
                {
                    return;
                }

                //If key is not present add the key , Kev value 8000-Decimal 
                if (string.IsNullOrEmpty(FindAppkey))
                    Regkey.SetValue(appName, value, RegistryValueKind.DWord);

                //check for the key after adding 
                FindAppkey = Convert.ToString(Regkey.GetValue(appName));
            }
            catch
            {
            }
            finally
            {
                //Close the Registry 
                if (Regkey != null)
                {
                    Regkey.Close();
                }
            }
        }
        /// <summary>
        /// WebBrowserのIEバージョン設定の削除
        /// </summary>
        /// <param name="appName"></param>
        private static void RemoveIE8KeyforWebBrowserControl(string appName)
        {
            RegistryKey Regkey = null;
            try
            {
                //For 64 bit Machine 
                if (Environment.Is64BitOperatingSystem)
                    Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(IE_VERSION_EMULATION_64BIT, true);
                else  //For 32 bit Machine 
                    Regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(IE_VERSION_EMULATION_32BIT, true);

                //If the path is not correct or 
                //If user't have priviledges to access registry 
                if (Regkey == null)
                {
                    return;
                }

                string FindAppkey = Convert.ToString(Regkey.GetValue(appName));
                // キーが存在するかチェック
                if (FindAppkey == null)
                {
                    return;
                }

                // キーが存在する場合は削除
                Regkey.DeleteValue(appName, false);
            }
            catch
            {
            }
            finally
            {
                //Close the Registry 
                if (Regkey != null)
                {
                    Regkey.Close();
                }
            }
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
            LogFactory.Fatal(FrameworkMessage.UncatchExceptionMessage, e.Exception);

#if DEBUG
            // 再Throw
            throw e.Exception;
#endif
        }
        #endregion
    }
}
