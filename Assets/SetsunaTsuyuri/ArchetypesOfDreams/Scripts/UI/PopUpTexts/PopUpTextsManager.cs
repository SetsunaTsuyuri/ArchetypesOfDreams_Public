using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;

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
        readonly List<PopUpText> _popUpTexts = new();

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
                instance.SetEnabled(false);
                _popUpTexts.Add(instance);
            }

            // 失敗
            MessageBrokersManager.Miss
                .Receive<CombatantContainer>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(PopUpMiss);

            // ダメージ
            MessageBrokersManager.Damage
                .Receive<CombatantContainer>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(PopUpDamage);

            // 回復
            MessageBrokersManager.Healing
                .Receive<CombatantContainer>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(PopUpHealing);

            // ステータス効果付与
            MessageBrokersManager.StatusEffectAdded
                .Receive<AddedStatusEffectResult>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(PopUpAddedStatusEffect);

            // ステータス効果解除
            MessageBrokersManager.StatusEffectsRemoved
                .Receive<StatusEffectsResult>()
                .TakeUntilDestroy(gameObject)
                .Subscribe(PopUpRemovedStatusEffects);
        }

        /// <summary>
        /// セットアップする
        /// </summary>
        /// <param name="targetSelection">対象選択UI</param>
        public void SetUp(TargetSelectionUI targetSelection)
        {
            targetSelection.AllyTargetSelectionStart += OnAllyTargeSelectionEnter;
            targetSelection.AllyTargetSelectionEnd += OnAllyargetSelectionExit;
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
        /// 味方による対象選択開始時の処理
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="targetables"></param>
        private void OnAllyTargeSelectionEnter(EffectData effect, List<CombatantContainer> targetables)
        {
            foreach (var targetable in targetables)
            {
                if (effect.IsOffensive)
                {
                    BlinkEffectiveness(targetable);
                }
                else if (effect.IsPurification)
                {
                    BlinkPurificationSuccessRate(targetable);
                }
            }
        }

        /// <summary>
        /// 対象選択終了時の処理
        /// </summary>
        public void OnAllyargetSelectionExit()
        {
            // ループ表示を止める
            var loops = _popUpTexts.Where(x => x.IsLoop);
            foreach (var loop in loops)
            {
                loop.SetEnabled(false);
            }
        }

        /// <summary>
        /// 浄化成功率を表示する
        /// </summary>
        /// <param name="container"></param>
        private void BlinkPurificationSuccessRate(CombatantContainer container)
        {
            string rate = GameSettings.PopUpTexts.PurificationSuccessRateText;
            rate += container.Combatant.Results.PurificationSuccessRate.ToString();
            rate += GameSettings.PopUpTexts.PurificatinSuccessRateSuffix;

            Blink(container, rate, GameSettings.PopUpTexts.PurificationSuccessRateColor);
        }

        /// <summary>
        /// 属性の有効性を表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        private void BlinkEffectiveness(CombatantContainer container)
        {
            GameAttribute.Effectiveness effectiveness = container.Combatant.Results.Effectiveness;
            (string, Color) result = GameSettings.PopUpTexts.GetEffectivenessTextAndColor(effectiveness);
            Blink(container, result.Item1, result.Item2);
        }

        /// <summary>
        /// ポップアップテキストをループ表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        /// <param name="text">文字列</param>
        /// <param name="color">色</param>
        private void Blink(CombatantContainer container, string text, Color color)
        {
            PopUpText popUpText = GetPopUpText();
            Vector3 position = DecidePosition(container);
            popUpText.BlinkRepeatedly(position, text, color);
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
        /// ダメージを表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void PopUpDamage(CombatantContainer container)
        {
            string text = string.Empty;
            DamageResult damage = container.Combatant.Results.Damage;

            if (damage.IsCritical)
            {
                AddNewLineText(ref text, GameSettings.PopUpTexts.CriticalText);
            }

            if (damage.IsCrush)
            {
                AddNewLineText(ref text, GameSettings.PopUpTexts.CrushText);
            }

            AddDamageOrHealingText(ref text, damage.HP, damage.DP, damage.GP);
            PopUp(container, text, GameSettings.PopUpTexts.DamageColor);
        }

        /// <summary>
        /// 回復を表示する
        /// </summary>
        /// <param name="container">戦闘者コンテナ</param>
        public void PopUpHealing(CombatantContainer container)
        {
            string text = string.Empty;
            HealingResult healing = container.Combatant.Results.Healing;
            AddDamageOrHealingText(ref text, healing.HP, healing.DP, healing.GP);
            PopUp(container, text, GameSettings.PopUpTexts.HealingColor);
        }

        /// <summary>
        /// 付与ステータス効果を表示する
        /// </summary>
        /// <param name="result"></param>
        public void PopUpAddedStatusEffect(AddedStatusEffectResult result)
        {
            string text = result.Effect.Name;
            Color color = result.Effect.Category switch
            {
                StatusEffectCategory.Abnormality => GameSettings.PopUpTexts.AbnormalityColor,
                StatusEffectCategory.Buff => GameSettings.PopUpTexts.BuffColor,
                StatusEffectCategory.Debuff => GameSettings.PopUpTexts.DebuffColor,
                StatusEffectCategory.Stance => GameSettings.PopUpTexts.StanceColor,
                _ => Color.white
            };

            PopUp(result.Container, text, color);
        }

        /// <summary>
        /// 解除ステータス効果をポップアップする
        /// </summary>
        /// <param name="result"></param>
        public void PopUpRemovedStatusEffects(StatusEffectsResult result)
        {
            CancellationToken token = this.GetCancellationTokenOnDestroy();
            PopUpStatusEffects(result, token, true).Forget();
        }

        /// <summary>
        /// ステータス効果をポップアップする
        /// </summary>
        /// <param name="result"></param>
        /// <param name="token"></param>
        /// <param name="isRemoval"></param>
        /// <returns></returns>
        private async UniTask PopUpStatusEffects(StatusEffectsResult result, CancellationToken token, bool isRemoval)
        {
            var effects = result.Effects.Where(x => x.CanPopUp);
            foreach (var effect in effects)
            {
                string text = effect.Name;
                Color color = effect.Category switch
                {
                    StatusEffectCategory.Abnormality => GameSettings.PopUpTexts.AbnormalityColor,
                    StatusEffectCategory.Buff => GameSettings.PopUpTexts.BuffColor,
                    StatusEffectCategory.Debuff => GameSettings.PopUpTexts.DebuffColor,
                    StatusEffectCategory.Stance => GameSettings.PopUpTexts.StanceColor,
                    _ => Color.white
                };

                if (isRemoval)
                {
                    color.a *= GameSettings.PopUpTexts.RemovedEffectAlpha;
                }

                PopUp(result.Container, text, color);
                await TimeUtility.Wait(GameSettings.WaitTime.StatusEffectAdded, token);
            }
        }

        /// <summary>
        /// ダメージまたは回復の表示文字列を作る
        /// </summary>
        /// <param name="text">文字列</param>
        /// <param name="hp">HP</param>
        /// <param name="dp">DP</param>
        /// <param name="gp">GP</param>
        private void AddDamageOrHealingText(ref string text, int? hp, int? dp, int? gp)
        {
            AddNewLineAndText(ref text, hp);
            AddNewLineAndText(ref text, dp, GameSettings.PopUpTexts.DPPrefix);
            AddNewLineAndText(ref text, gp, GameSettings.PopUpTexts.GPPrefix);
        }

        /// <summary>
        /// テキストを改行して追加する
        /// </summary>
        /// <param name="text"></param>
        /// <param name="newLineText"></param>
        private void AddNewLineText(ref string text, string newLineText)
        {
            if (!string.IsNullOrEmpty(text))
            {
                text += "\n";
            }

            text += newLineText;
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
            if (!value.HasValue)
            {
                return;
            }

            if (!string.IsNullOrEmpty(text))
            {
                text += "\n";
            }

            text += prefix;
            text += value;
            text += suffix;
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
            popUpText.PopUp(position, text, color);
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
