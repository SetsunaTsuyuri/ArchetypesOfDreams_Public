using System.Linq;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// オーディオデータグループ
    /// </summary>
    public abstract class AudioDataGroup : DataGroup<AudioData>
    {
        public AudioData this[string name]
        {
            get => Data.FirstOrDefault(x => x.Name == name);
        }
    }
}
