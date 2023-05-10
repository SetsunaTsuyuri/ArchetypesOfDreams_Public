namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 対象の立場
    /// </summary>
    public enum TargetPosition
    {
        None = 0,

        /// <summary>
        /// 敵
        /// </summary>
        Enemies = 1,

        /// <summary>
        /// 味方
        /// </summary>
        Allies = 2,

        /// <summary>
        /// 敵味方両方
        /// </summary>
        Both = 3,

        /// <summary>
        /// 自分自身
        /// </summary>
        Oneself = 4,

        /// <summary>
        /// 自分以外の味方
        /// </summary>
        AlliesOtherThanOneself = 5,

        /// <summary>
        /// 控え
        /// </summary>
        Reserves = 6
    }
}
