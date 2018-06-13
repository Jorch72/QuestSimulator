using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.UI.Requests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.ActiveQuests {

    public class ActiveQuestInstance : MonoBehaviour {

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI difficultyText;

        private Button m_Button;
        private QuestInstance m_QuestInstance;
        
        void Awake() {
            m_Button = GetComponent<Button>();
        }

        void Start() {
            m_Button.onClick.AddListener(StartQuestPost);
        }

        private void StartQuestPost() {
            RequestsWindow.Instance.postWindow.OpenWindow(m_QuestInstance);
        }

        public void ApplyQuestChain(QuestInstance chain) {
            m_QuestInstance = chain;
            UpdateText();
        }

        private void UpdateText() {
            nameText.text = "<b>"+m_QuestInstance.QuestSource.RequestTitle + "</b>\n<i>" + m_QuestInstance.GoldReward.GoldCount + " GP</i>";
            difficultyText.text = ""+m_QuestInstance.DifficultyLevel;
        }
    }

}