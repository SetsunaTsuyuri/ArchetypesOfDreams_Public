namespace SetsunaTsuyuri
{
    /// <summary>
    /// オブジェクトプーリングされるオブジェクト
    /// </summary>
    public interface IPooled
    {
        /// <summary>
        /// 利用可能である
        /// </summary>
        /// <returns></returns>
        bool IsAvailable();

        /// <summary>
        /// オブジェクトプールから取り出されたときの処理
        /// </summary>
        void OnGot();

        /// <summary>
        /// オブジェクトプールに戻す処理
        /// </summary>
        void Release();
    }
}
