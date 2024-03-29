﻿using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace SetsunaTsuyuri
{
    /// <summary>
    /// MessageBrokerの管理クラス
    /// </summary>
    public class MessageBrokersManager : Singleton<MessageBrokersManager>, IInitializable
    {
        /// <summary>
        /// 戦闘開始
        /// </summary>
        public static IMessageBroker BattleStart => Instance._battleStart;
        readonly IMessageBroker _battleStart = new MessageBroker();

        /// <summary>
        /// 戦闘終了
        /// </summary>
        public static IMessageBroker BattleEnd => Instance._battleEnd;
        readonly IMessageBroker _battleEnd = new MessageBroker();

        /// <summary>
        /// 味方逃走
        /// </summary>
        public static IMessageBroker AlliesEscape => Instance._allyEscape;
        readonly IMessageBroker _allyEscape = new MessageBroker();

        /// <summary>
        /// ダメージ
        /// </summary>
        public static IMessageBroker Damage => Instance._damage;
        readonly IMessageBroker _damage = new MessageBroker();

        /// <summary>
        /// 回復
        /// </summary>
        public static IMessageBroker Healing => Instance._healing;
        readonly IMessageBroker _healing = new MessageBroker();

        /// <summary>
        /// 失敗
        /// </summary>
        public static IMessageBroker Miss => Instance._miss;
        readonly IMessageBroker _miss = new MessageBroker();

        /// <summary>
        /// 戦闘不能
        /// </summary>
        public static IMessageBroker KnockedOut => Instance._knockedOut;
        readonly IMessageBroker _knockedOut = new MessageBroker();

        /// <summary>
        /// ステータス効果付与
        /// </summary>
        public static IMessageBroker StatusEffectAdded => Instance._statusEffectAdded;
        readonly IMessageBroker _statusEffectAdded = new MessageBroker();

        /// <summary>
        /// ステータス効果解除
        /// </summary>
        public static IMessageBroker StatusEffectsRemoved => Instance._statusEffectsRemoved;
        readonly IMessageBroker _statusEffectsRemoved = new MessageBroker();

        /// <summary>
        /// 対象フラグセット
        /// </summary>
        public static IMessageBroker TargetFlagSet => Instance._targetFlagSet;
        readonly IMessageBroker _targetFlagSet = new MessageBroker();

        /// <summary>
        /// シナリオ開始
        /// </summary>
        public static IMessageBroker ScenarioStart => Instance._scenarioStart;
        readonly IMessageBroker _scenarioStart = new MessageBroker();

        /// <summary>
        /// シナリオ終了
        /// </summary>
        public static IMessageBroker ScenarioEnd => Instance._scenarioEnd;
        readonly IMessageBroker _scenarioEnd = new MessageBroker();

        /// <summary>
        /// ステータスメニューの使用者が設定された
        /// </summary>
        public static IMessageBroker StatusMenuUserSet = Instance._statusMenuUserSet;
        readonly IMessageBroker _statusMenuUserSet = new MessageBroker();

        /// <summary>
        /// ステータスメニューが閉じられた
        /// </summary>
        public static IMessageBroker StatusMenuClosed = Instance._statusMenuClosed;
        readonly IMessageBroker _statusMenuClosed = new MessageBroker();

        /// <summary>
        /// 戦闘コマンド選択
        /// </summary>
        public static IMessageBroker BattleCommandsSelected => Instance._battleCommandsSelected;
        readonly IMessageBroker _battleCommandsSelected = new MessageBroker();

        /// <summary>
        /// スキルメニュー使用者が設定された
        /// </summary>
        public static IMessageBroker SkillMenuUserSet => Instance._skillMenuUserSet;
        readonly IMessageBroker _skillMenuUserSet = new MessageBroker();

        /// <summary>
        /// アイテムメニュー選択
        /// </summary>
        public static IMessageBroker ItemMenuSelected => Instance._itemMenuSelected;
        readonly IMessageBroker _itemMenuSelected = new MessageBroker();

        /// <summary>
        /// 対象選択開始
        /// </summary>
        public static IMessageBroker TargetSelectionStart => Instance._targetSelectionStart;
        readonly IMessageBroker _targetSelectionStart = new MessageBroker();

        /// <summary>
        /// 行動実行
        /// </summary>
        public static IMessageBroker ActionExecution => Instance._actionExecution;
        readonly IMessageBroker _actionExecution = new MessageBroker();

        /// <summary>
        /// 戦闘者がコンテナに設定された
        /// </summary>
        public static IMessageBroker CombatantSet => Instance._combatantSet;
        readonly IMessageBroker _combatantSet = new MessageBroker();

        /// <summary>
        /// 敵の位置が設定された
        /// </summary>
        public static IMessageBroker EnemyPositionSet => Instance._enemyPositionSet;
        readonly IMessageBroker _enemyPositionSet = new MessageBroker();
    }
}
