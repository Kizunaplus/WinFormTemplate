using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kizuna.Plus.WinMvcForm.Framework
{
    public partial class SprashForm : Form
    {
        #region プロパティ
        /// <summary>
        /// 現在のインスタンス
        /// </summary>
        private static SprashForm Current
        {
            get;
            set;
        }
        #endregion

        #region 初期化処理
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SprashForm()
        {
            InitializeComponent();
        }
        #endregion

        #region フォームの表示
        /// <summary>
        /// Splashフォームを表示する
        /// </summary>
        /// <param name="image">表示するイメージ</param>
        public static void ShowSplash(Image image)
        {

            if (SprashForm.Current == null)
            {
                // フォームの生成
                SprashForm.Current = new SprashForm();
                if (image == null)
                {
                    return;
                }
                // 画像のイメージサイズに合わせる
                SprashForm.Current.Size = image.Size;
                SprashForm.Current.BackgroundImage = image;

                //Splashフォームを表示する
                SprashForm.Current.Show();

                //Application.IdleイベントハンドラでSplashフォームを閉じる
                Application.Idle += new EventHandler(Application_Idle);
            }
        }
        #endregion

        #region イベント
        //アプリケーションがアイドル状態になった時
        private static void Application_Idle(object sender, EventArgs e)
        {
            //Application.Idleイベントハンドラの削除
            Application.Idle -= new EventHandler(Application_Idle);

            //Splashフォームがあるか調べる
            if (SprashForm.Current != null && SprashForm.Current.IsDisposed == false)
            {
                //Splashフォームを閉じる
                SprashForm.Current.Close();
            }
            SprashForm.Current = null;
        }
        #endregion
    }
}
