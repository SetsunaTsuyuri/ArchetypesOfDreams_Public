using System.Collections.Generic;
using UnityEngine;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ポップアップテキストの設定
    /// </summary>
    [CreateAssetMenu(fileName = "PopUpTexts", menuName = "Settings/PopUpTexts")]
    public class PopUpTextsSettings : ScriptableObject
    {
        /// <summary>
        /// 表示オフセット
        /// </summary>
        [field: SerializeField]
        public Vector3 Offset { get; private set; } = new Vector2(0.0f, 0.5f);

        /// <summary>
        /// 移動ベクトル
        /// </summary>
        [field: SerializeField]
        public Vector2 MoveVector { get; private set; } = Vector2.zero;

        /// <summary>
        /// 移動時間
        /// </summary>
        [field: SerializeField]
        public float MoveDuration { get; private set; } = 0.0f;

        /// <summary>
        /// フェードアウト時間
        /// </summary>
        [field: SerializeField]
        public float FadeOutDuration { get; private set; } = 0.0f;

        /// <summary>
        /// ダメージの色
        /// </summary>
        [field: SerializeField]
        public Color DamageColor { get; private set; } = Color.white;

        /// <summary>
        /// 回復の色
        /// </summary>
        [field: SerializeField]
        public Color HealingColor { get; private set; } = Color.green;

        /// <summary>
        /// 失敗の色
        /// </summary>
        [field: SerializeField]
        public Color MissColor { get; private set; } = Color.white;

        /// <summary>
        /// 失敗の文字列
        /// </summary>
        [field: SerializeField]
        public string MissText { get; private set; } = "MISS";

        /// <summary>
        /// DPダメージ・回復表示の接頭辞
        /// </summary>
        [field: SerializeField]
        public string DPPrefix { get; private set; } = "DP";

        /// <summary>
        /// GPダメージ・回復表示の接頭辞
        /// </summary>
        [field: SerializeField]
        public string GPPrefix { get; private set; } = "SP";

        /// <summary>
        /// クリティカルの文字列
        /// </summary>
        [field: SerializeField]
        public string CriticalText { get; private set; } = "Critical!";

        /// <summary>
        /// クラッシュの文字列
        /// </summary>
        [field: SerializeField]
        public string CrushText { get; private set; } = "Crsuh!";

        /// <summary>
        /// 異常ステータス効果の色
        /// </summary>
        [field: SerializeField]
        public Color AbnormalityColor { get; private set; } = Color.white;

        /// <summary>
        /// 強化ステータス効果の色
        /// </summary>
        [field: SerializeField]
        public Color BuffColor { get; private set; } = Color.white;

        /// <summary>
        /// 弱体ステータス効果の色
        /// </summary>
        [field: SerializeField]
        public Color DebuffColor { get; private set; } = Color.white;

        /// <summary>
        /// 構えステータス効果の色
        /// </summary>
        [field: SerializeField]
        public Color StanceColor { get; private set; } = Color.white;

        /// <summary>
        /// 解除ステータス効果のアルファ値
        /// </summary>
        [field: SerializeField]
        public float RemovedEffectAlpha { get; private set; } = 0.5f;

        /// <summary>
        /// 点滅表示のアニメーションカーブ
        /// </summary>
        [field: SerializeField]
        public AnimationCurve BlinkingCurve { get; private set; } = new();

        /// <summary>
        /// 点滅表示の時間
        /// </summary>
        [field: SerializeField]
        public float BlinkingDutation { get; private set; } = 3.0f;

        /// <summary>
        /// 浄化成功率の文字列
        /// </summary>
        [field: SerializeField, TextArea]
        public string PurificationSuccessRateText { get; private set; } = "成功率";

        /// <summary>
        /// 浄化成功率の接尾辞
        /// </summary>
        [field: SerializeField]
        public string PurificatinSuccessRateSuffix { get; private set; } = "%";

        /// <summary>
        /// 浄化成功率の色
        /// </summary>
        [field: SerializeField]
        public Color PurificationSuccessRateColor { get; private set; } = Color.white;

        /// <summary>
        /// 弱点の文字列
        /// </summary>
        [field: SerializeField]
        public string WeaknessText { get; private set; } = "Weak";

        /// <summary>
        /// 弱点の色
        /// </summary>
        [field: SerializeField]
        public Color WeaknessColor { get; private set; } = Color.red;

        /// <summary>
        /// 耐性の文字列
        /// </summary>
        [field: SerializeField]
        public string RegistanceText { get; private set; } = "Regist";

        /// <summary>
        /// 耐性の色
        /// </summary>
        [field: SerializeField]
        public Color RegistanceColor { get; private set; } = Color.blue;

        /// <summary>
        /// 有効性の文字列と色を取得する
        /// </summary>
        /// <param name="effectiveness">有効性</param>
        /// <returns></returns>
        public (string, Color) GetEffectivenessTextAndColor(GameAttribute.Effectiveness effectiveness)
        {
            string text = string.Empty;
            Color color = Color.white;
            switch (effectiveness)
            {
                case GameAttribute.Effectiveness.Weakness:
                    text = WeaknessText;
                    color = WeaknessColor;
                    break;

                case GameAttribute.Effectiveness.Resistance:
                    text = RegistanceText;
                    color = RegistanceColor;
                    break;
            }

            return (text, color);
        }
    }
}
