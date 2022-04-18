namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者のステータスを表示する
    /// </summary>
    public interface IStatusDisplayer
    {
        /// <summary>
        /// 表示対象を設定する
        /// </summary>
        /// <param name="target">対象</param>
        void SetTarget(CombatantContainer target);
    }
}
