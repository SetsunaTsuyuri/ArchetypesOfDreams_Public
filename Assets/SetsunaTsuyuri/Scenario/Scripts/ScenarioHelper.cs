using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SetsunaTsuyuri.Scenario
{
    public class ScenarioHelper : MonoBehaviour
    {
        [SerializeField]
        TextAsset text = null;

        private void Awake()
        {
            ScenarioManager scenario = FindObjectOfType<ScenarioManager>();
            GetComponent<Button>().onClick.AddListener(() =>
            {
                scenario.StartPlaying(text);
                gameObject.SetActive(false);
            });
        }
    }
}
