using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// マスターデータのエディタ拡張
    /// </summary>
    [CustomEditor(typeof(MasterData))]
    public class MasterDataEditor : Editor
    {
        /// <summary>
        /// ID
        /// </summary>
        static readonly string s_id = "Id";

        /// <summary>
        /// SkillID
        /// </summary>
        static readonly string s_skillId = "SkillId";

        /// <summary>
        /// ItemID
        /// </summary>
        static readonly string s_itemId = "ItemId";

        /// <summary>
        /// Skills
        /// </summary>
        static readonly string s_skills = "Skills";

        /// <summary>
        /// Abilities
        /// </summary>
        static readonly string s_abilities = "Abilities";

        /// <summary>
        /// DamageEffects
        /// </summary>
        static readonly string s_damageEffects = "DamageEffects";

        /// <summary>
        /// StatusEffects
        /// </summary>
        static readonly string s_statusEffects = "StatusEffects";

        /// <summary>
        /// マスターデータ
        /// </summary>
        MasterData _masterData = null;

        /// <summary>
        /// 更新中である
        /// </summary>
        bool _isUpdating = false;

        private void Awake()
        {
            _masterData = target as MasterData;
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(_isUpdating);

            if (GUILayout.Button("Update"))
            {
                UpdateAsync().Forget();
            }

            EditorGUI.EndDisabledGroup();

            base.OnInspectorGUI();
        }

        /// <summary>
        /// マスターデータを更新する非同期処理
        /// </summary>
        /// <returns></returns>
        private async UniTask UpdateAsync()
        {
            try
            {
                _isUpdating = true;

                UnityWebRequest request = UnityWebRequest.Get(_masterData.GasUrl);

                await request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.Success)
                {
                    UpdateMasterData(request.downloadHandler.text);
                }
                else
                {
                    Debug.LogError(request.error);
                }
            }
            finally
            {
                _isUpdating = false;
            }
        }

        /// <summary>
        /// マスターデータを更新する
        /// </summary>
        /// <param name="text"></param>
        private void UpdateMasterData(string text)
        {
            string masterData = Format(text);
            Debug.Log(masterData);

            EditorUtility.SetDirty(_masterData);
            JsonUtility.FromJsonOverwrite(masterData, _masterData);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("MasterData update is complete.");
        }

        /// <summary>
        /// jsonを整形する
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string Format(string json)
        {
            JToken masterData = JToken.Parse(json);

            // 夢渡り
            JToken dreamWalkers = masterData["DreamWalkers"];
            JToken dreamWalkerSkills = masterData["DreamWalkerSkills"];
            AddArray(dreamWalkers, dreamWalkerSkills, "DreamWalkerId", s_skills);

            // ナイトメア
            JToken nightmares = masterData["Nightmares"];
            JToken nightmareSkills = masterData["NightmareSkills"];
            AddArray(nightmares, nightmareSkills, "NightmareId", s_skills);

            // スキル
            JToken skills = masterData["Skills"];
            JToken skillDamageEffects = masterData["SkillDamageEffects"];
            JToken skillStatusEffects = masterData["SkillStatusEffects"];
            AddArray(skills, skillDamageEffects, s_skillId, s_damageEffects);
            AddArray(skills, skillStatusEffects, s_skillId, s_statusEffects);

            // アイテム
            JToken items = masterData["Items"];
            JToken itemDamageEffects = masterData["ItemDamageEffects"];
            JToken itemStatusEffects = masterData["ItemStatusEffects"];
            AddArray(items, itemDamageEffects, s_itemId, s_damageEffects);
            AddArray(items, itemStatusEffects, s_itemId, s_statusEffects);

            // エフェクトアニメーション
            JToken effectAnimations = masterData["EffectAnimations"];
            JToken effectAnimationElements = masterData["EffectAnimationElements"];
            AddArray(effectAnimations, effectAnimationElements, "EffectAnimationId", "Elements");

            // ステータス効果
            JToken statusEffects = masterData["StatusEffects"];
            JToken statusEffectAbilities = masterData["StatusEffectAbilities"];
            AddArray(statusEffects, statusEffectAbilities, "StatusEffectId", s_abilities);

            string result = masterData.ToString();
            return result;
        }

        /// <summary>
        /// 整形する
        /// </summary>
        /// <param name="mainTokens"></param>
        /// <param name="arrayTokens"></param>
        /// <param name="id"></param>
        /// <param name="arrayName"></param>
        private void AddArray(JToken mainTokens, JToken arrayTokens, string id, string arrayName)
        {
            foreach (var mainToken in mainTokens)
            {
                var filtered = arrayTokens
                    .Where(x => x[id].ToString() == mainToken[s_id].ToString());

                if (filtered.Any())
                {
                    JArray array = JArray.FromObject(filtered);
                    JObject mainObject = (JObject)mainToken;
                    mainObject.Add(arrayName, array);
                }
            }
        }
    }
}
