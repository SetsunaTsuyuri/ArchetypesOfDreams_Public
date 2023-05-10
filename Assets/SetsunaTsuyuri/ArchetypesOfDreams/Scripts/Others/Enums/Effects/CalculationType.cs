namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 計算の種類
    /// </summary>
    public enum CalculationType
    {
        None = 0,

        /// <summary>
        /// 使用者の力
        /// </summary>
        UserPower = 1,

        /// <summary>
        /// 使用者の技
        /// </summary>
        UserTechnique = 2,

        /// <summary>
        /// 対象の現在値
        /// </summary>
        TargetCurrent = 3,

        /// <summary>
        /// 対象の最大値
        /// </summary>
        TargetMax = 4,
    }
}
