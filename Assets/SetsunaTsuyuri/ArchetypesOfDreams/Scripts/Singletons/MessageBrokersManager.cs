using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// MessageBrokerの管理クラス
    /// </summary>
    public class MessageBrokersManager : Singleton<MessageBrokersManager>, IInitializable
    {
        /// <summary>
        /// ダメージ
        /// </summary>
        readonly IMessageBroker _damage = new MessageBroker();

        /// <summary>
        /// ダメージ
        /// </summary>
        public static IMessageBroker Damage => Instance._damage;

        /// <summary>
        /// 回復
        /// </summary>
        readonly IMessageBroker _healing = new MessageBroker();

        /// <summary>
        /// 回復
        /// </summary>
        public static IMessageBroker Healing => Instance._healing;

        /// <summary>
        /// 失敗
        /// </summary>
        readonly IMessageBroker _miss = new MessageBroker();

        /// <summary>
        /// 失敗
        /// </summary>
        public static IMessageBroker Miss => Instance._miss;

        /// <summary>
        /// 戦闘中の対象選択開始
        /// </summary>
        readonly IMessageBroker _targetSelectionStartInBattle = new MessageBroker();

        /// <summary>
        /// 戦闘中の対象選択開始
        /// </summary>
        public static IMessageBroker TargetSelectionStartInBattle => Instance._targetSelectionStartInBattle;

        /// <summary>
        /// コマンド選択開始
        /// </summary>
        readonly IMessageBroker _commandSelectionStart = new MessageBroker();

        /// <summary>
        /// コマンド選択開始
        /// </summary>
        public static IMessageBroker CommandSelectionStart => Instance._commandSelectionStart;


        //TODO: MessageBrokerを使うやり方に修正する

        /// <summary>
        /// ステータス効果付与
        /// </summary>
        readonly Subject<AddedStatusEffectResult> _statusEffectAdded = new();
        public static IObservable<AddedStatusEffectResult> OnStatusEffectAdded
        {
            get => Instance._statusEffectAdded;
        }
        public static void FireStatusEffectAdded(AddedStatusEffectResult result)
        {
            Instance._statusEffectAdded.OnNext(result);
        }

        /// <summary>
        /// ステータス効果解除
        /// </summary>
        readonly Subject<StatusEffectsResult> _statusEffectsRemoved = new();
        public static IObservable<StatusEffectsResult> OnStatusEffectsRemoved
        {
            get => Instance._statusEffectsRemoved;
        }
        public static void FireStatusEffectsRemoved(StatusEffectsResult result)
        {
            Instance._statusEffectsRemoved.OnNext(result);
        }
    }
}
