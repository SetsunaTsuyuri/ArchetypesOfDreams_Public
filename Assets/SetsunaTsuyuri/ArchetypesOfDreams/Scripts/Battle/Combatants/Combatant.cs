using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者
    /// </summary>
    [System.Serializable]
    public abstract partial class Combatant : IInitializable
    {
        /// <summary>
        /// 戦闘者コンテナ
        /// </summary>
        public CombatantContainer Container { get; set; } = null;

        /// <summary>
        /// データID
        /// </summary>
        [field: SerializeField]
        public int DataId { get; set; } = 0;

        /// <summary>
        /// データ
        /// </summary>
        /// <returns></returns>
        public abstract CombatantData Data { get; }

        /// <summary>
        /// リーダーである
        /// </summary>
        public bool IsLeader { get; set; } = false;

        /// <summary>
        /// ボス耐性を持っている
        /// </summary>
        public bool HasBossResistance { get; set; } = false;

        /// <summary>
        /// 行動の結果
        /// </summary>
        public ActionResultSet Results { get; set; } = new ActionResultSet();

        /// <summary>
        /// 最後に選択した戦闘コマンドタイプ
        /// </summary>
        public int LastBattleCommandTypeSelected { get; set; } = 0;

        /// <summary>
        /// 最後に選択したスキルID
        /// </summary>
        public int LastSkillIdSelected { get; set; } = 0;

        /// <summary>
        /// 最後に選択したアイテムID
        /// </summary>
        public int LastItemIdSelected { get; set; } = 0;

        /// <summary>
        /// スプライト
        /// </summary>
        public Sprite Sprite { get; private set; } = null;

        /// <summary>
        /// 顔スプライト
        /// </summary>
        public Sprite FaceSprite { get; private set; } = null;

        AddressableLoader<Sprite[]> _loader = new();

        public void Initialize()
        {
            Results.Initialize();
            Experience = ToMinExperience(Level);
            InitializeStatus();

            WaitTime = 0;

            LoadSprites();
        }

        /// <summary>
        /// スプライトをロードする
        /// </summary>
        public void LoadSprites()
        {
            Sprite[] sprites = _loader.Load(Data.SpriteName);
            if (sprites is not null)
            {
                // 名前順でソートする
                var sortedSprites = sprites
                    .OrderBy(x => x.name)
                    .ToArray();

                Sprite = sortedSprites.Length >= 1 ? sortedSprites[0] : null;
                FaceSprite = sortedSprites.Length >= 2 ? sortedSprites[1] : Sprite;
            }
        }

        /// <summary>
        /// スプライトを解放する
        /// </summary>
        public void ReleaseSprites()
        {
            _loader.Release();
        }

        /// <summary>
        /// 顔スプライトまたはスプライトを取得する
        /// </summary>
        /// <returns></returns>
        public Sprite GetFaceSpriteOrSprite()
        {
            Sprite sprite = FaceSprite ? FaceSprite : Sprite;
            return sprite;
        }

        /// <summary>
        /// 行動可能である
        /// </summary>
        /// <returns></returns>
        public bool CanAct()
        {
            return !StatusEffects.Any(x => x.Data.Action == EffectOnAction.InabiltyToAct)
                && !HasActed;
        }

        /// <summary>
        /// 戦闘不能である
        /// </summary>
        /// <returns></returns>
        public bool IsKnockedOut => IsAffected(StatusEffectId.KnockedOut);

        /// <summary>
        /// ブレイクしている
        /// </summary>
        /// <returns></returns>
        public bool IsBroken => IsAffected(StatusEffectId.Break);

        /// <summary>
        /// スキルを有している
        /// </summary>
        /// <returns></returns>
        public bool HasAnySkill => Data.Skills.Any(x => x.AcquisitionLevel <= Level);

        /// <summary>
        /// 防御コマンドを選択できる
        /// </summary>
        /// <returns></returns>
        public bool CanSelectDefending()
        {
            return true;
        }

        /// <summary>
        /// 浄化コマンドを選択できる
        /// </summary>
        /// <returns></returns>
        public abstract bool CanSelectPurification();

        /// <summary>
        /// コンテナから解放できる
        /// </summary>
        /// <returns></returns>
        public abstract bool CanBeReleased();

        /// <summary>
        /// 歩いたときの処理
        /// </summary>
        public void OnWalk()
        {
            // HPを1回復する
            CurrentHP++;

            // DPを1回復する
            CurrentDP++;
        }

        /// <summary>
        /// クローン（ディープコピー）を作る
        /// </summary>
        /// <returns></returns>
        public Combatant Clone()
        {
            string json = JsonUtility.ToJson(this);
            Combatant clone = CreateClone(json);
            clone.Initialize();
            return clone;
        }

        /// <summary>
        /// クローン（ディープコピー）を作る
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        protected abstract Combatant CreateClone(string json);
    }
}
