using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using WindowsFormsApplication.Controllers.Commands;
using WindowsFormsApplication.Controllers.State;
using WindowsFormsApplication.Models.Enums;
using WindowsFormsApplication.Models.EventArg;

namespace WindowsFormsApplication.Models
{
    [Serializable]
    /// <summary>
    /// 抽象モデルクラス
    /// </summary>
    public abstract class AbstractModel : IModel
    {
        #region 定数
        /// <summary>
        /// 暗号化アルゴリズム名
        /// </summary>
        private const string CRYPOT_ALGORITHM_NAME = "aes";

        /// <summary>
        /// パスワードプレフィックス
        /// </summary>
        private const string PASSWORD_PREFIX = "E#p 5M9q_";
        #endregion

        #region 初期化処理
        /// <summary>
        /// 初期化処理
        /// </summary>
        public void Initialize()
        {

        }
        #endregion

        #region コピー
        /// <summary>
        /// コピーしたインスタンスを取得します。
        /// </summary>
        /// <returns>コピーしたインスタンス</returns>
        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region 読み込み
        /// <summary>
        /// ファイルから読み込みます。
        /// </summary>
        /// <param name="filePath">読み込むファイルパス</param>
        public virtual IModel Load(string filePath)
        {
            return Load(filePath, SerializeType.Xml);
        }

        /// <summary>
        /// ファイルから読み込みます。
        /// </summary>
        /// <param name="filePath">読み込むファイルパス</param>
        /// <param name="type">シリアライズタイプ</param>
        public virtual IModel Load(string filePath, SerializeType type)
        {
            if (File.Exists(filePath) == false)
            {
                // ファイルが存在しない
                return this;
            }

            IModel model = null;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                model = Load(stream, type);
            }

            return model;
        }

        /// <summary>
        /// ストリームから読み込みます。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="type">シリアライズタイプ</param>
        public virtual IModel Load(Stream stream, SerializeType type)
        {
            if (SerializeType.Xml == type)
            {
                return LoadXml(stream);
            }
            else if (SerializeType.Binary == type)
            {
                return LoadBinary(stream);
            }
            else if (SerializeType.Json == type)
            {
                return LoadJson(stream);
            }

            return null;
        }

        /// <summary>
        /// ストリームから読み込みます。
        /// 復号化
        /// </summary>
        /// <param name="stream">ストリーム</param>
        /// <param name="type">シリアライズタイプ</param>
        public virtual IModel LoadDecrypt(Stream stream, string password, SerializeType type)
        {
            using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(CRYPOT_ALGORITHM_NAME))
            {
                symmetricAlgorithm.GenerateIV();

                //パスワードを適切な書式に処理する
                byte[] bytesPassword = System.Text.Encoding.UTF8.GetBytes(PASSWORD_PREFIX + password);
                byte[] bytesKey = new byte[symmetricAlgorithm.KeySize / 8];

                //有効なキーサイズになっていない場合は調整する
                for (int keyIndex = 0; keyIndex < bytesKey.Length; keyIndex++)
                {
                    if (keyIndex < bytesPassword.Length)
                    {
                        bytesKey[keyIndex] = bytesPassword[keyIndex];
                    }
                    else
                    {
                        bytesKey[keyIndex] = 0; //余白はゼロで埋める
                    }
                }

                // IVの取得
                byte[] IV = new byte[symmetricAlgorithm.IV.Length];
                stream.Read(IV, 0, IV.Length);

                ICryptoTransform dencryptor = symmetricAlgorithm.CreateDecryptor(bytesKey, IV);
                using (CryptoStream csEncrypt = new CryptoStream(stream, dencryptor, CryptoStreamMode.Read))
                {
                    if (SerializeType.Xml == type)
                    {
                        return LoadXml(csEncrypt);
                    }
                    else if (SerializeType.Binary == type)
                    {
                        return LoadBinary(csEncrypt);
                    }
                    else if (SerializeType.Json == type)
                    {
                        return LoadJson(csEncrypt);
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// ストリームから読み込みます。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        protected IModel LoadXml(Stream stream)
        {
            IModel result = this;

            try
            {
                //DataContractSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                var serializer = new DataContractSerializer(this.GetType());
                //デシリアル化し、XMLファイルからデータを生成
                result = serializer.ReadObject(stream) as IModel;
            }
            catch (Exception ex)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(new ExceptionState(typeof(Application)), new ExceptionEventArgs() { Exception = ex });
            }

            return result;
        }

        /// <summary>
        /// ストリームから読み込みます。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        protected IModel LoadBinary(Stream stream)
        {
            IModel result = this;

            try
            {
                //DataContractSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                var serializer = new BinaryFormatter();
                //デシリアル化し、XMLファイルからデータを生成
                result = serializer.Deserialize(stream) as IModel;
            }
            catch (Exception ex)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(new ExceptionState(typeof(Application)), new ExceptionEventArgs() { Exception = ex });
            }

            return result;
        }

        /// <summary>
        /// ストリームから読み込みます。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        protected IModel LoadJson(Stream stream)
        {
            IModel result = this;

            try
            {
                //DataContractSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                var serializer = new JavaScriptSerializer();
                //読み込むファイルを開く
                byte[] jsonDataBytes = new byte[stream.Length - stream.Position];
                stream.Read(jsonDataBytes, 0, jsonDataBytes.Length);

                string jsonData = System.Text.Encoding.UTF8.GetString(jsonDataBytes);
                //デシリアル化し、XMLファイルからデータを生成
                result = serializer.Deserialize(jsonData, typeof(IModel)) as IModel;
            }
            catch (Exception ex)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(new ExceptionState(typeof(Application)), new ExceptionEventArgs() { Exception = ex });
            }

            return result;
        }
        #endregion

        #region 保存
        /// <summary>
        /// ファイルへ保存します。
        /// </summary>
        /// <param name="filePath">保存するファイルパス</param>
        public virtual bool Save(string filePath)
        {
            return Save(filePath, SerializeType.Xml);
        }

        /// <summary>
        /// ファイルへ保存します。
        /// </summary>
        /// <param name="filePath">保存するファイルパス</param>
        /// <param name="type">シリアライズタイプ</param>
        public virtual bool Save(string filePath, SerializeType type)
        {
            bool bRet = false;
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                bRet = Save(stream, type);
            }

            return bRet;
        }

        /// <summary>
        /// ファイルへ保存します。
        /// </summary>
        /// <param name="filePath">保存するファイルパス</param>
        /// <param name="type">シリアライズタイプ</param>
        public virtual bool Save(Stream stream, SerializeType type)
        {
            if (SerializeType.Xml == type)
            {
                return SaveXml(stream);
            }
            else if (SerializeType.Binary == type)
            {
                return SaveBinary(stream);
            }
            else if (SerializeType.Json == type)
            {
                return SaveJson(stream);
            }

            return false;
        }

        /// <summary>
        /// ファイルへ保存します。
        /// 暗号化
        /// </summary>
        /// <param name="filePath">保存するファイルパス</param>
        /// <param name="type">シリアライズタイプ</param>
        public virtual bool SaveCrypt(Stream stream, string password, SerializeType type)
        {
            using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(CRYPOT_ALGORITHM_NAME))
            {
                //パスワードを適切な書式に処理する
                byte[] bytesPassword = System.Text.Encoding.UTF8.GetBytes(PASSWORD_PREFIX + password);
                byte[] bytesKey = new byte[symmetricAlgorithm.KeySize / 8];

                //有効なキーサイズになっていない場合は調整する
                for (int keyIndex = 0; keyIndex < bytesKey.Length; keyIndex++)
                {
                    if (keyIndex < bytesPassword.Length)
                    {
                        bytesKey[keyIndex] = bytesPassword[keyIndex];
                    }
                    else
                    {
                        bytesKey[keyIndex] = 0; //余白はゼロで埋める
                    }
                }

                // IVの保存
                symmetricAlgorithm.GenerateIV();
                byte[] IV = symmetricAlgorithm.IV;
                stream.Write(IV, 0, IV.Length);

                ICryptoTransform encryptor = symmetricAlgorithm.CreateEncryptor(bytesKey, IV);
                using (CryptoStream csEncrypt = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
                {
                    if (SerializeType.Xml == type)
                    {
                        return SaveXml(csEncrypt);
                    }
                    else if (SerializeType.Binary == type)
                    {
                        return SaveBinary(csEncrypt);
                    }
                    else if (SerializeType.Json == type)
                    {
                        return SaveJson(csEncrypt);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// ストリームへ保存します。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        protected virtual bool SaveXml(Stream stream)
        {
            try
            {
                //DataContractSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                var serializer = new DataContractSerializer(this.GetType());

                //シリアル化し、XMLファイルに保存する
                serializer.WriteObject(stream, this);
            }
            catch (Exception ex)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(new DebugState(typeof(Application)), new ExceptionEventArgs() { Exception = ex });

                return false;
            }

            return true;
        }

        /// <summary>
        /// ストリームへ保存します。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        protected virtual bool SaveBinary(Stream stream)
        {
            try
            {
                //DataContractSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                var serializer = new BinaryFormatter();

                //シリアル化し、バイナリーファイルに保存する
                serializer.Serialize(stream, this);
            }
            catch (Exception ex)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(new DebugState(typeof(Application)), new ExceptionEventArgs() { Exception = ex });

                return false;
            }

            return true;
        }

        /// <summary>
        /// ストリームへ保存します。
        /// </summary>
        /// <param name="stream">ストリーム</param>
        protected virtual bool SaveJson(Stream stream)
        {
            try
            {
                //DataContractSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                var serializer = new JavaScriptSerializer();

                StringBuilder stringBuilder = new StringBuilder();
                //シリアル化し、
                serializer.Serialize(this, stringBuilder);

                // Jsonファイルに保存する
                byte[] writeData = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
                stream.Write(writeData, 0, writeData.Length);
            }
            catch (Exception ex)
            {
                var logCommand = new LogCommand();
                logCommand.Execute(new DebugState(typeof(Application)), new ExceptionEventArgs() { Exception = ex });

                return false;
            }

            return true;
        }
        #endregion

        #region 破棄処理
        /// <summary>
        /// 破棄処理
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// 破棄処理
        /// </summary>
        /// <param name="isDispose"></param>
        protected virtual void Dispose(bool isDispose)
        {

        }
        #endregion
    }
}
