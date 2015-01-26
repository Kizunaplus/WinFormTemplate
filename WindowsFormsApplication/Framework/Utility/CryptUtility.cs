using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsFormsApplication;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace Kizuna.Plus.WinMvcForm.Framework.Utility
{
    /// <summary>
    /// 暗号化/復号化ユーティリティ
    /// </summary>
    class CryptUtility
    {
        #region メンバークラス変数
        /// <summary>
        /// 暗号化パスワード
        /// </summary>
        private static string cryptPassword;
        #endregion

        #region 暗号化
        /// <summary>
        /// 暗号化
        /// </summary>
        /// <param name="value">暗号化対象文字</param>
        /// <returns></returns>
        public static String encrypt(String value)
        {
            byte[] encBytes = null;
            using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(AppEnviroment.CrypotAlgorithm))
            {
                //パスワードから共有キーと初期化ベクタを作成
                byte[] key, iv;
                GenerateKeyFromPassword(
                    symmetricAlgorithm.KeySize, out key, symmetricAlgorithm.BlockSize, out iv);
                symmetricAlgorithm.Key = key;
                symmetricAlgorithm.IV = iv;

                //文字列をバイト型配列に変換する
                byte[] strBytes = System.Text.Encoding.UTF8.GetBytes(value);

                //対称暗号化オブジェクトの作成
                using (var encryptor = symmetricAlgorithm.CreateEncryptor())
                {
                    //バイト型配列を暗号化する
                    encBytes = encryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
                }
            }
            //バイト型配列を文字列に変換して返す
            return System.Convert.ToBase64String(encBytes);
        }
        #endregion

        #region 復号化
        /// <summary>
        /// 復号化
        /// </summary>
        /// <param name="value"復号化文字></param>
        /// <returns></returns>
        public static string decrypt(string value)
        {
            byte[] decBytes = null;
            using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(AppEnviroment.CrypotAlgorithm))
            {
                //パスワードから共有キーと初期化ベクタを作成
                byte[] key, iv;
                GenerateKeyFromPassword(symmetricAlgorithm.KeySize, out key, symmetricAlgorithm.BlockSize, out iv);
                symmetricAlgorithm.Key = key;
                symmetricAlgorithm.IV = iv;

                //文字列をバイト型配列に戻す
                byte[] strBytes = System.Convert.FromBase64String(value);

                //対称暗号化オブジェクトの作成
                using (var decryptor = symmetricAlgorithm.CreateDecryptor())
                {
                    //バイト型配列を復号化する
                    //復号化に失敗すると例外CryptographicExceptionが発生
                    decBytes = decryptor.TransformFinalBlock(strBytes, 0, strBytes.Length);
                }
            }

            //バイト型配列を文字列に戻して返す
            return System.Text.Encoding.UTF8.GetString(decBytes);
        }
        #endregion

        #region パスワード取得
        /// <summary>
        /// パスワードから共有キーと初期化ベクタを生成する
        /// </summary>
        /// <param name="keySize">共有キーのサイズ（ビット）</param>
        /// <param name="key">作成された共有キー</param>
        /// <param name="blockSize">初期化ベクタのサイズ（ビット）</param>
        /// <param name="iv">作成された初期化ベクタ</param>
        private static void GenerateKeyFromPassword(int keySize, out byte[] key, int blockSize, out byte[] iv)
        {
            if (cryptPassword == null)
            {
                cryptPassword = AppEnviroment.PasswordPrefix + Application.ProductName + "e@!2ZA=$あ#み%";
            }
            //パスワードから共有キーと初期化ベクタを作成する
            //saltを決める
            string saltKey = cryptPassword + cryptPassword.Length + typeof(CryptUtility).Name;
            byte[] salt = System.Text.Encoding.UTF8.GetBytes(saltKey);
            //Rfc2898DeriveBytesオブジェクトを作成する
            var deriveBytes = new System.Security.Cryptography.Rfc2898DeriveBytes(cryptPassword, salt);

            //反復処理回数を指定する デフォルトで1000回
            deriveBytes.IterationCount = 1000;

            //共有キーと初期化ベクタを生成する
            key = deriveBytes.GetBytes(keySize / 8);
            iv = deriveBytes.GetBytes(blockSize / 8);
        }
        #endregion
    }
}
