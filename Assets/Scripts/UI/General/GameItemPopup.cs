using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.General {

    public class GameItemPopup : MonoBehaviourSingleton<GameItemPopup> {

        public GameObject content;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI attackText;
        public TextMeshProUGUI defenceText;
        public TextMeshProUGUI overallPowerText;

        public GraphicRaycaster canvasRaycaster;

        private RectTransform m_RectTransform;
        private GameItem m_CurrentItem = null;

        private void Awake() {
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void Update() {
            if (m_CurrentItem == null) return;
            m_RectTransform.localPosition = Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }

        public void SwitchItemTarget(GameItem item) {
            m_CurrentItem = item;

            if (m_CurrentItem == null) {
                content.SetActive(false);
                return;
            }

            content.SetActive(true);

            titleText.text = "<b>" + item.DisplayName + "</b>\n<size=18><i>" + item.Rarity.ToString() + "</size></i>";
            attackText.text = ""+item.AttackPower;
            defenceText.text = "" + item.DefencePower;
            overallPowerText.text = "" + item.OverallPower;
        }
    }

}