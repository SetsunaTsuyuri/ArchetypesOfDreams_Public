using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    public partial class Combatant
    {
        /// <summary>
        /// ステータス効果を付与する
        /// </summary>
        /// <param name="data">ステータス効果付与データ</param>
        /// <returns>付与された</returns>
        public bool AddStatusEffect(EffectData.StatusEffect data)
        {
            bool result = false;

            StatusEffect statusEffect = StatusEffects.FirstOrDefault(x => x.Data.Id == data.StatusEffectId);
            if (statusEffect is not null)
            {
                result = statusEffect.Data.Stack switch
                {
                    StatusEffectSatck.Prolong => ProlongStatusEffect(statusEffect, data),
                    StatusEffectSatck.Overwrite => OverwriteStatusEffect(statusEffect, data),
                    StatusEffectSatck.Stack => AddNewStatusEffect(data),
                    _ => false
                };
            }
            else
            {
                result = AddNewStatusEffect(data);
            }

            return result;
        }

        /// <summary>
        /// ステータス効果を付与する
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool AddNewStatusEffect(EffectData.StatusEffect data)
        {
            StatusEffect statusEffect = new(data.StatusEffectId, data.Turns, data.IsPermanent);
            StatusEffects.Add(statusEffect);
            Delay(statusEffect.Data.DelayTime);
            RefreshStatus();

            return true;
        }

        /// <summary>
        /// ステータス効果を延長する
        /// </summary>
        /// <param name="statusEffect"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool ProlongStatusEffect(StatusEffect statusEffect, EffectData.StatusEffect data)
        {
            statusEffect.RemainingTurns += data.Turns;
            Delay(statusEffect.Data.DelayTime);

            return true;
        }

        /// <summary>
        /// ステータス効果を上書きする
        /// </summary>
        /// <param name="statusEffect"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool OverwriteStatusEffect(StatusEffect statusEffect, EffectData.StatusEffect data)
        {
            statusEffect.RemainingTurns = data.Turns;
            Delay(statusEffect.Data.DelayTime);

            return true;
        }

        /// <summary>
        /// ステータス効果を解除する
        /// </summary>
        /// <param name="id"></param>
        public void RemoveStatusEffect(StatusEffectId id)
        {
            RemoveStatusEffect((int)id);
        }

        /// <summary>
        /// ステータス効果を解除する
        /// </summary>
        /// <param name="id"></param>
        public void RemoveStatusEffect(int id)
        {
            StatusEffects.RemoveAll(x => x.Data.Id == id);
        }

        /// <summary>
        /// ステータス効果を受けている
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAffected(StatusEffectId id)
        {
            return IsAffected((int)id);
        }

        /// <summary>
        /// ステータス効果を受けている
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsAffected(int id)
        {
            return StatusEffects.Any(x => x.Data.Id == id);
        }

        /// <summary>
        /// ステータス効果を受けられる
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CanBeAffected(StatusEffectId id)
        {
            return CanBeAffected((int)id);
        }

        /// <summary>
        /// ステータス効果を受けられる
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CanBeAffected(int id)
        {
            bool result = true;

            StatusEffect effect = StatusEffects.FirstOrDefault(x => x.Data.Id == id);
            if (effect is not null)
            {
                result = effect.Data.Stack != StatusEffectSatck.Stack;
            }

            return result;
        }
    }
}
