using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kizuna.Plus.WinMvcForm.Framework.Models.Enums;

namespace Kizuna.Plus.WinMvcForm.Framework.Models
{
    /// <summary>
    /// モデルクラスインターフェース
    /// </summary>
    public interface IModel : ICloneable, IDisposable
    {
        /// <summary>
        /// 初期化処理
        /// </summary>
        void Initialize();

        #region 読み込み
        /// <summary>
        /// ファイルから読み込み
        /// </summary>
        /// <param name="filePath">読み込むファイルパス</param>
        /// <param name="type">シリアライズタイプ</param>
        IModel Load(string filePath, SerializeType type = SerializeType.Xml);
        #endregion

        #region 保存
        /// <summary>
        /// ファイルへ保存
        /// </summary>
        /// <param name="filePath">保存するファイルパス</param>
        /// <param name="type">シリアライズタイプ</param>
        bool Save(string filePath, SerializeType type = SerializeType.Xml);
        #endregion
    }
}
