using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// UniRxによるイベント管理クラス
    /// </summary>
    public class UniRxEventsManager : Singleton<UniRxEventsManager>, IInitializable
    {
        /// <summary>
        /// ステータス効果付与
        /// </summary>
        readonly Subject<AddedStatusEffectResult> _statusEffectAdded = new();

        /// <summary>
        /// ステータス効果付与
        /// </summary>
        public static IObservable<AddedStatusEffectResult> OnStatusEffectAdded
        {
            get => Instance._statusEffectAdded;
        }

        /// <summary>
        /// ステータス効果付与
        /// </summary>
        /// <param name="result"></param>
        public static void FireStatusEffectAdded(AddedStatusEffectResult result)
        {
            Instance._statusEffectAdded.OnNext(result);
        }

        /// <summary>
        /// ステータス効果解除
        /// </summary>
        readonly Subject<StatusEffectsResult> _statusEffectsRemoved = new();

        /// <summary>
        /// ステータス効果解除
        /// </summary>
        public static IObservable<StatusEffectsResult> OnStatusEffectsRemoved
        {
            get => Instance._statusEffectsRemoved;
        }

        /// <summary>
        /// ステータス効果解除
        /// </summary>
        /// <param name="result"></param>
        public static void FireStatusEffectsRemoved(StatusEffectsResult result)
        {
            Instance._statusEffectsRemoved.OnNext(result);
        }
    }
}
