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
using Kizuna.Plus.WinMvcForm.Framework.Controllers.Commands;
using Kizuna.Plus.WinMvcForm.Framework.Controllers.State;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;
using Kizuna.Plus.WinMvcForm.Framework.Models.EventArg;
using System.Reflection;
using WindowsFormsApplication;
using WindowsFormsApplication.Framework.Message;
using Kizuna.Plus.WinMvcForm.Framework.Models.Validation;

namespace Kizuna.Plus.WinMvcForm.Framework.Models
{
    [Serializable]
    /// <summary>
    /// 抽象モデルクラス
    /// </summary>
    public abstract class AbstractModel : IModel
    {
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

        /// <summary>
        /// 同一のメンバーが存在する場合は、値をコピーします。
        /// </summary>
        /// <param name="obj"></param>
        public void Copy(object obj)
        {
            if (obj == null)
            {
                return;
            }

            FieldInfo[] srcFields = obj.GetType().GetFields();
            FieldInfo[] destFields = this.GetType().GetFields();

            foreach (FieldInfo srcField in srcFields)
            {
                foreach (FieldInfo destField in destFields)
                {
                    if (srcField.Name.Equals(destField.Name) == true
                        && srcField.FieldType == destField.FieldType)
                    {
                        // 同一名、同一型の値を設定
                        destField.SetValue(this, srcField.GetValue(obj));
                    }
                }
            }
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
                var logCommand = new LogCommand();
                logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ModelSerialize_FileNotFound, this.GetType().FullName, "Load", filePath, type);

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

            var logCommand = new LogCommand();
            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ModelSerialize_UnsupportedSerializeType, this.GetType().FullName, "Load", type);
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
            using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(AppEnviroment.CrypotAlgorithm))
            {
                symmetricAlgorithm.GenerateIV();

                //パスワードを適切な書式に処理する
                byte[] bytesPassword = System.Text.Encoding.UTF8.GetBytes(AppEnviroment.PasswordPrefix + password);
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

            var logCommand = new LogCommand();
            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ModelSerialize_UnsupportedSerializeType, this.GetType().FullName, "LoadDecrypt", type);
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
                logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, stream);
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
                logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, stream);
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
                logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, stream);
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

            var logCommand = new LogCommand();
            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ModelSerialize_UnsupportedSerializeType, this.GetType().FullName, "Save", type);

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
            using (SymmetricAlgorithm symmetricAlgorithm = SymmetricAlgorithm.Create(AppEnviroment.CrypotAlgorithm))
            {
                //パスワードを適切な書式に処理する
                byte[] bytesPassword = System.Text.Encoding.UTF8.GetBytes(AppEnviroment.PasswordPrefix + password);
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

            var logCommand = new LogCommand();
            logCommand.Execute(LogType.Debug, FrameworkDebugMessage.ModelSerialize_UnsupportedSerializeType, this.GetType().FullName, "SaveCrypt", type);
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
                logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, stream);

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
                logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, stream);

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
                logCommand.Execute(LogType.Exception, FrameworkMessage.ExceptionMessage, ex, MethodBase.GetCurrentMethod().Name, stream);

                return false;
            }

            return true;
        }
        #endregion

        #region 入力値検証
        /// <summary>
        /// 入力値検証
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <returns>true: 入力値に問題がない場合, false : 入力値エラー</returns>
        public bool Valid(out String message)
        {
            bool isValid = true;
            message = String.Empty;

            PropertyInfo[] properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (typeof(AbstractModel).IsAssignableFrom(property.PropertyType) == false)
                {
                    foreach (Attribute attr in property.GetCustomAttributes(typeof(ModelValidationAttribute), true))
                    {
                        ModelValidationAttribute ivAttr = attr as ModelValidationAttribute;
                        if (ivAttr == null)
                        {
                            continue;
                        }

                        isValid |= ivAttr.Valid(this, property, ref message);
                        if (isValid == false)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    // Modelクラスが指定されている場合
                    isValid = ((AbstractModel)property.GetValue(this, null)).Valid(out message);
                }

                if (isValid == false)
                {
                    break;
                }
            }

            return isValid;
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

        #region 比較
        /// <summary>
        /// 引数のオブジェクトが等しいか判断します。
        /// 
        /// Reflectionを用いるため
        /// 頻度が高い場合は、独自で実装する
        /// </summary>
        /// <param name="obj">比較先</param>
        /// <returns>true: 等しい, false: 等しくない</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj, this.GetType());
        }
        
        /// <summary>
        /// 引数のオブジェクトが等しいか判断します。
        /// 
        /// Reflectionを用いるため
        /// 頻度が高い場合は、独自で実装する
        /// </summary>
        /// <param name="obj">比較先</param>
        /// <param name="type">比較元の型</param>
        /// <returns>true: 等しい, false: 等しくない</returns>
        public bool Equals(object obj, Type type)
        {
            if (base.Equals(obj) == true)
            {
                // 同一インスタンス
                return true;
            }

            var data = obj as AbstractModel;
            if (type.IsAssignableFrom(data.GetType()) == false)
            {
                // 指定した型ではない場合
                return false;
            }

            // 各フィールドの値を比較
            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                object src = field.GetValue(this);
                object dest = field.GetValue(data);

                if ((src == null && dest != null) 
                    || (src != null && src.Equals(dest) == false))
                {
                    // 値が異なる場合
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ハッシュ関数として機能します。
        /// 
        /// Reflectionを用いるため
        /// 頻度が高い場合は、独自で実装する
        /// </summary>
        /// <returns>ハッシュ値</returns>
        public override int GetHashCode()
        {
            int hashcode = 17;

            Type type = this.GetType();
            // 各フィールドの値を比較
            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                object src = field.GetValue(this);

                if (src != null)
                {
                    hashcode *= src.GetHashCode() * 31;
                }
                else
                {
                    hashcode *= 31;
                }
            }

            return hashcode;
        }

        #endregion

        #region toString
        /// <summary>
        /// クラスの状態を文字列化します。
        /// 
        /// Reflectionを用いるため
        /// 頻度が高い場合は、独自で実装する
        /// </summary>
        /// <returns>クラス状態</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            Type type = this.GetType();
            sb.Append(type.Name);
            sb.Append(":");

            // 各フィールドの値を比較
            FieldInfo[] fields = type.GetFields(BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo field in fields)
            {
                object src = field.GetValue(this);
                sb.AppendFormat("{0}-[{1}],", field.Name, src);
            }

            return sb.ToString();
        }
        #endregion
    }
}
