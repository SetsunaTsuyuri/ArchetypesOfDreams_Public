namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 対象の状態
    /// </summary>
    public enum TargetCondition
    {
        Living = 0, // 生きている
        KnockedOut = 1, // 倒されている
        LivingAndKnockedOut = 2 // 生きているまたは倒されている
    }
}
