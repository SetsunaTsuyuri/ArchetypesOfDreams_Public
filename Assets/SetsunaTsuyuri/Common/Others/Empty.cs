namespace SetsunaTsuyuri
{
    /// <summary>
    /// 空の構造体
    /// </summary>
    public struct Empty
    {
        public static Empty Instance { get; private set; } = new();
    }
}
