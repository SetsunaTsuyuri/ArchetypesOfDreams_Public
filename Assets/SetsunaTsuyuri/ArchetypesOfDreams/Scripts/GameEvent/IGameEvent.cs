using System.Threading;
using Cysharp.Threading.Tasks;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ゲームイベントのインターフェース
    /// </summary>
    public interface IGameEvent
    {
        /// <summary>
        /// ゲームイベントのUniTaskを取得する
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UniTask Resolve(CancellationToken token);
    }
}
