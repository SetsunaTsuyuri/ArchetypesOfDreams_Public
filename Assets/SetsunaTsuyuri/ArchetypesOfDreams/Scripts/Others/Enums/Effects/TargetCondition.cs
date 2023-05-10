namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 対象の状態
    /// </summary>
    public enum TargetCondition
    {
        None = 0,

        /// <summary>
        /// 生存
        /// </summary>
        Living = 1,

        /// <summary>
        /// 戦闘不能
        /// </summary>
        KnockedOut = 2,

        /// <summary>
        /// 生存・戦闘不能
        /// </summary>
        LivingOrKnockedOut = 3,
    }
}
