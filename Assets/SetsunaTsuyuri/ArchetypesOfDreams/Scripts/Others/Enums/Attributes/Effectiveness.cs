namespace SetsunaTsuyuri.ArchetypesOfDreams.GameAttribute
{
    /// <summary>
    /// 有効性
    /// </summary>
    public enum Effectiveness
    {
        Normal = 0, // 通常
        Weakness = 1, // 弱点
        SuperWeakness = 2, // 超弱点
        Resistance = -1, // 耐性
        Invalid = -2 // 無効
    }
}
