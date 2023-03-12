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
        /// Skills
        /// </summary>
        static readonly string s_skills = "Skills";

        /// <summary>
        /// Effect
        /// </summary>
        static readonly string s_effect = "Effect";

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

            EditorUtility.SetDirty(this);
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
            {
                JToken dreamWalkers = masterData["DreamWalkers"];
                JToken skills = masterData["DreamWalkerSkills"];
                FormatCombatants(dreamWalkers, skills, "DreamWalkerId");
            }

            // ナイトメア
            {
                JToken nightmares = masterData["Nightmares"];
                JToken skills = masterData["NightmareSkills"];
                FormatCombatants(nightmares, skills, "NightmareId");
            }

            //// スキル
            //{
            //    JToken skills = masterData["Skills"];
            //    JToken effects = masterData["SkillEffects"];
            //    JToken damageEffects = masterData["SkillDamageEffects"];
            //    JToken statusEffects = masterData["SkillStatusEffects"];
            //    FormatSkillsOrItems(skills, effects, damageEffects, statusEffects, "SkillId");
            //}

            //// アイテム
            //{
            //    JToken items = masterData["Items"];
            //    JToken effects = masterData["ItemEffects"];
            //    JToken damageEffects = masterData["ItemDamageEffects"];
            //    JToken statusEffects = masterData["ItemStatusEffects"];
            //    FormatSkillsOrItems(items, effects, damageEffects, statusEffects, "ItemId");
            //}

            string result = masterData.ToString();
            return result;
        }

        /// <summary>
        /// 戦闘者データを整形する
        /// </summary>
        /// <param name="combatants"></param>
        /// <param name="skills"></param>
        /// <param name="id"></param>
        private void FormatCombatants(JToken combatants, JToken skills, string id)
        {
            foreach (var combatant in combatants)
            {
                var acquisitionSkills = skills
                    .Where(x => x[id].ToString() == combatant[s_id].ToString());

                if (acquisitionSkills.Any())
                {
                    JArray skillArray = JArray.FromObject(acquisitionSkills);
                    JObject combatantObject = (JObject)combatant;
                    combatantObject.Add(s_skills, skillArray);
                }
            }
        }

        /// <summary>
        /// スキルまたはアイテムのデータを整形する
        /// </summary>
        /// <param name="skillsOrItems"></param>
        /// <param name="effects"></param>
        /// <param name="damageEffects"></param>
        /// <param name="statusEffects"></param>
        /// <param name="id"></param>
        private void FormatSkillsOrItems(JToken skillsOrItems, JToken effects, JToken damageEffects, JToken statusEffects, string id)
        {
            foreach (var skillOrItem in skillsOrItems)
            {
                JObject effect = (JObject)effects.FirstOrDefault(x => x[s_id].ToString() == skillOrItem[s_id].ToString());
                if (effect is not null)
                {
                    var damage = damageEffects
                        .Where(x => x[id].ToString() == skillOrItem[s_id].ToString());

                    effect.Add(s_damageEffects, JArray.FromObject(damage));

                    var status = statusEffects
                        .Where(x => x[id].ToString() == skillOrItem[s_id].ToString());

                    effect.Add(s_statusEffects, JArray.FromObject(status));

                    JObject skillOrItemObject = (JObject)skillOrItem;
                    skillOrItemObject.Add(s_effect, effect);
                }
            }
        }
    }
}
