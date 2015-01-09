using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// ファイルから読み込み
        /// </summary>
        /// <param name="filePath">読み込むファイルパス</param>
        IModel Load(string filePath);

        /// <summary>
        /// ファイルへ保存
        /// </summary>
        /// <param name="filePath">保存するファイルパス</param>
        bool Save(string filePath);
    }
}
