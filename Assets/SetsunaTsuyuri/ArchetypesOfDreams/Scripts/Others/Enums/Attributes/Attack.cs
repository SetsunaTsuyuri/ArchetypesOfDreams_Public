namespace SetsunaTsuyuri.ArchetypesOfDreams.Attribute
{
    /// <summary>
    /// 攻撃の属性
    /// </summary>
    public enum Attack
    {
        User = -1, // 使い手依存
        None = 0, // なし
        Strength = 1, // STR依存
        Technique = 2, // TEC依存
        Mix = 3 // 混合
    }
}
