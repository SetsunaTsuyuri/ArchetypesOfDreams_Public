﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// 戦闘者
    /// </summary>
    [Serializable]
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
        /// 健康状態
        /// </summary>
        Attribute.Condition condition = Attribute.Condition.Normal;

        /// <summary>
        /// 健康状態
        /// </summary>
        public Attribute.Condition Condition
        {
            get => condition;
            set
            {
                condition = value;
                if (Container)
                {
                    Container.OnConditionSet();
                }
            }
        }

        /// <summary>
        /// 行動の結果
        /// </summary>
        public ActionResult Result { get; set; } = new ActionResult();

        public void Initialize()
        {
            Result.Initialize();
            Experience = ToMinExperience(Level);
            InitializeStatus();

            WaitTime = 0;
        }

        /// <summary>
        /// 行動可能である
        /// </summary>
        /// <returns></returns>
        public bool CanAct()
        {
            bool result = false;

            if (Condition == Attribute.Condition.Normal ||
                (Condition == Attribute.Condition.Crush && HasBossResistance))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// クラッシュしている
        /// </summary>
        /// <returns></returns>
        public bool IsCrushed()
        {
            return Condition == Attribute.Condition.Crush;
        }

        /// <summary>
        /// 戦闘不能である
        /// </summary>
        /// <returns></returns>
        public bool IsKnockedOut()
        {
            return Condition == Attribute.Condition.KnockedOut;
        }

        /// <summary>
        /// スキルを有している
        /// </summary>
        /// <returns></returns>
        public bool HasAnySkill()
        {
            return Data.Skills.Any(x => x.AcquisitionLevel >= Level);
        }

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
        /// HPの割合を取得する
        /// </summary>
        /// <returns></returns>
        public float GetHPRate()
        {
            return (float)CurrentHP / MaxHP;
        }

        /// <summary>
        /// HPの減少率を取得する
        /// </summary>
        /// <returns></returns>
        public float GetHPReductionRate()
        {
            return 1.0f - GetHPRate();
        }

        /// <summary>
        /// DPの割合を取得する
        /// </summary>
        /// <returns></returns>
        public float GetDPRate()
        {
            return (float)CurrentDP / GameSettings.Combatants.MaxDP;
        }

        /// <summary>
        /// DPの減少率を取得する
        /// </summary>
        /// <returns></returns>
        public float GetDPReductionRate()
        {
            return 1.0f - GetDPRate();
        }

        /// <summary>
        /// SPの割合を取得する
        /// </summary>
        /// <returns></returns>
        public float GetSPRate()
        {
            return (float)CurrentGP / MaxGP;
        }

        /// <summary>
        /// SPの減少率を取得する
        /// </summary>
        /// <returns></returns>
        public float GetSPReductionRate()
        {
            return 1.0f - GetSPRate();
        }

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
