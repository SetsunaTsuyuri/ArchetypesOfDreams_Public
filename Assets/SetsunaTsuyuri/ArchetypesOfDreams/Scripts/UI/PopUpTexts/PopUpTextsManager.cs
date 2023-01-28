using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ポップアップテキストの管理者
    /// </summary>
    public class PopUpTextsManager : MonoBehaviour
    {
        /// <summary>
        /// プレファブ
        /// </summary>
        [SerializeField]
        PopUpText _prefab = null;

        /// <summary>
        /// インスタンスを作る数
        /// </summary>
        [SerializeField]
        int _numberToInstantiate = 20;

        /// <summary>
        /// 味方UIトランスフォーム配列
        /// </summary>
        [SerializeField]
        RectTransform[] _allyUIs = null;

        /// <summary>
        /// メインカメラ
        /// </summary>
        Camera _mainCamera = null;

        /// <summary>
        /// ポップアップテキストのリスト
        /// </summary>
        List<PopUpText> _popUpTexts = new();

        /// <summary>
        /// レクトトランスフォーム
        /// </summary>
        RectTransform _rectTransform = null;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Start()
        {
            // プレファブをインスタンス化する
            for (int i = 0; i < _numberToInstantiate; i++)
            {
                PopUpText instance = Instantiate(_prefab, transform);
                instance.Hide();
                _popUpTexts.Add(instance);
            }
        }

        /// <summary>
        /// ポップアップテキストを取得する
        /// </summary>
        /// <returns></returns>
        public PopUpText GetPopUpText()
        {
            PopUpText popUpText = _popUpTexts.FirstOrDefault(x => !x.enabled);
            if (!popUpText)
            {
                popUpText = Instantiate(_prefab, transform);
                _popUpTexts.Add(popUpText);
            }
            return popUpText;
        }

        /// <summary>
        /// 対象選択開始時の処理
        /// </summary>
        /// <param name="battle">戦闘の管理者</param>
        public void OnTargetSelectionEnter(Battle battle)
        {
            // プレイヤー操作が不可能、または交代、逃走する場合は中止する
            if (!battle.Actor.ContainsPlayerControlled())
            {
                return;
            }

            // 行動者の行動効果データを取得する
            EffectData effect = battle.ActorAction.Effect;

            // 効果に応じて処理を決める
            System.Action<CombatantContainer> action = null;
            if (effect.IsOffensive)
            {
                // 有効性を表示する
                action = (x) => PopUpEffectiveness(x);
            }
            else if (effect.IsPurification)
            {
                // 浄化成功率を表示する
                action = (x) => PopUpPurificationSuccessRate(x);
            }

            if (action != null)
            {
                foreach (var container in battle.Targetables)
                {
                    action.Invoke(container);
                }
            }
        }

        /// <summary>
        /// 行動開始時の処理
        /// </summary>
        public void OnTargetSelectionExit()
        {
            // ループ表示しているテキストを全て非表示にする
            PopUpText[] loops = _popUpTexts
               .Where(x => x.LoopSequence != null && x.LoopSequence.active)
               .ToArray();

            foreach (var loop in loops)
            {
                loop.Hide();
            }
        }

        /// <summary>
        /// 浄化成功率を表示する
        /// </summary>
        /// <param name="container"></param>
        private void PopUpPurificationSuccessRate(CombatantContainer container)
        {
            string rate = GameSettings.PopUpTexts.PurificationSuccessRateText;
            rate += container.Combatant.Result.PurificationSuccessRate.ToString();
            rate += GameSettings.PopUpTexts.PurificatinSuccessRateSuffix;

            PopUpLoop(container, rate, GameSettings.PopUpTexts.PurificationSuccessRateColor);
        }

        /// <summary>
        /// 感情属性の有効性を表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        private void PopUpEffectiveness(CombatantContainer container)
        {
            Attribute.Effectiveness effectiveness = container.Combatant.Result.Effectiveness;
            (string, Color) result = GameSettings.PopUpTexts.GetEffectivenessTextAndColor(effectiveness);
            PopUpLoop(container, result.Item1, result.Item2);
        }

        /// <summary>
        /// ポップアップテキストをループ表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        private void PopUpLoop(CombatantContainer container, string text, Color color)
        {
            PopUpText popUpText = GetPopUpText();
            Vector3 position = DecidePosition(container);
            popUpText.DoBlinkingSequence(position, text, color);
        }

        /// <summary>
        /// 失敗を表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void PopUpMiss(CombatantContainer container)
        {
            PopUp(container, GameSettings.PopUpTexts.MissText, GameSettings.PopUpTexts.MissColor);
        }

        /// <summary>
        /// ダメージ量を表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void PopUpDamage(CombatantContainer container)
        {
            string damage = string.Empty;
            ActionResult result = container.Combatant.Result;
            MakeDamageOrRecoveryText(ref damage, result.HPDamage, result.DPDamage, result.GPDamage);

            PopUp(container, damage, GameSettings.PopUpTexts.DamageColor);
        }

        /// <summary>
        /// 回復量を表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void PopUpRecovery(CombatantContainer container)
        {
            string recovery = string.Empty;
            ActionResult result = container.Combatant.Result;
            MakeDamageOrRecoveryText(ref recovery, result.HPHealing, result.DPHealing, result.GPHealing);

            PopUp(container, recovery, GameSettings.PopUpTexts.HealingColor);
        }

        /// <summary>
        /// ダメージまたは回復の表示文字列を作る
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="hp">生命力</param>
        /// <param name="dp">夢想力</param>
        /// <param name="sp">精神力</param>
        private void MakeDamageOrRecoveryText(ref string text, int? hp, int? dp, int? sp)
        {
            AddNewLineAndText(ref text, hp);
            AddNewLineAndText(ref text, dp, GameSettings.PopUpTexts.DPPrefix);
            AddNewLineAndText(ref text, sp, GameSettings.PopUpTexts.GPPrefix);
        }

        /// <summary>
        /// 改行しつつ数値の変化を文字列に加える
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="value">値</param>
        /// <param name="prefix">接頭辞</param>
        /// <param name="suffix">接尾辞</param>
        private void AddNewLineAndText(ref string text, int? value, string prefix = "", string suffix = "")
        {
            if (value.HasValue)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    text += "\n";
                }

                text += prefix;
                text += value;
                text += suffix;
            }
        }

        /// <summary>
        /// ポップアップテキストを表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        private void PopUp(CombatantContainer container, string text, Color color)
        {
            PopUpText popUpText = GetPopUpText();
            Vector3 position = DecidePosition(container);
            popUpText.DoPopUpSequence(position, text, color);
        }

        /// <summary>
        /// 戦闘者コンテナの情報を基に文字の表示位置を決定する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        /// <returns></returns>
        private Vector3 DecidePosition(CombatantContainer container)
        {
            Vector3 result = Vector3.zero;

            // 味方の場合
            AllyContainer allyContainer = container as AllyContainer;
            if (allyContainer)
            {
                // UIのある位置
                result = _allyUIs[allyContainer.Id].position;
            }
            else
            {
                Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(
                    _mainCamera,
                    container.transform.position + GameSettings.PopUpTexts.Offset);

                if(RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPoint, _mainCamera, out Vector2 localPoint))
                {
                    result = transform.TransformPoint(localPoint);
                }
            }
            return result;
        }
    }
}
