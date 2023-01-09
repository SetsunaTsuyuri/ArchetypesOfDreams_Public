using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.Threading;
using UnityEngine.Networking;
using UnityEditor;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
#endif

namespace SetsunaTsuyuri.ArchetypesOfDreams
{
    /// <summary>
    /// ナイトメアのデータ集
    /// </summary>
    [CreateAssetMenu(menuName = "Data/Combatants/Nightmares")]
    public class NightmareDataGroup : DataGroup<NightmareData>
    {
#if UNITY_EDITOR
        [ContextMenu("Update")]
        public void StartUpdate()
        {
            CancellationTokenSource source = new();
            CancellationToken token = source.Token;
            UpdateAsync(token).Forget();
        }

        private async UniTask UpdateAsync(CancellationToken token)
        {
            UnityWebRequest request = UnityWebRequest.Get("https://script.google.com/macros/s/AKfycbzv0dXNgXQ-w-gqdWvbA2RRrpysb8xlGuheuKHDCPObBCe1BwLRNtNPOEsOqEY9ueoBHA/exec?sheet=Nightmares");

            token.ThrowIfCancellationRequested();
            await request.SendWebRequest();
            token.ThrowIfCancellationRequested();
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                UpdateData(request);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }

        private void UpdateData(UnityWebRequest request)
        {
            string text = request.downloadHandler.text;
            Debug.Log(text);
            JArray jArray = JArray.Parse(text);
            for (int i = 0; i < jArray.Count; i++)
            {
                string json = jArray[i].ToString();
                JsonUtility.FromJsonOverwrite(json, Data[i]);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
#endif
    }
}
