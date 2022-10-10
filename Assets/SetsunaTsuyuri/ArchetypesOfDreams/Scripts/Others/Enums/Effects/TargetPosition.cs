namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 対象の立場
    /// </summary>
    public enum TargetPosition
    {
        /// <summary>
        /// 対象なし
        /// </summary>
        None = -1,

        /// <summary>
        /// 敵
        /// </summary>
        Enemies = 0,

        /// <summary>
        /// 味方
        /// </summary>
        Allies = 1,

        /// <summary>
        /// 敵味方両方
        /// </summary>
        Both = 2,

        /// <summary>
        /// 自分自身
        /// </summary>
        Oneself = 3,

        /// <summary>
        /// 自身以外の味方
        /// </summary>
        AlliesOtherThanOneself = 4,
        
        /// <summary>
        /// 自身以外の敵味方両方
        /// </summary>
        OtherThanOneself = 5,

        /// <summary>
        /// 控え
        /// </summary>
        Reserves = 6
    }
}
