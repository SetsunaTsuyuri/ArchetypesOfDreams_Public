using System.Linq;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// オーディオデータグループ
    /// </summary>
    public abstract class AudioDataGroup : DataGroup<AudioData>
    {
        /// <summary>
        /// インデクサー
        /// </summary>
        /// <param name="name">名前</param>
        /// <returns></returns>
        public AudioData this[string name]
        {
            get => Data.FirstOrDefault(x => x.Name == name);
        }
    }
}
