using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.UI.PostedQuests;
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
            m_Button.onClick.AddListener(OpenQuestWindow);
        }

        private void OpenQuestWindow() {
            PostedQuestWindow.PostedQuestMode postMode;
            if (QuestManager.PostedQuests.Contains(m_QuestInstance)) {
                postMode = PostedQuestWindow.PostedQuestMode.POSTED_REVIEW;
            } else {
                postMode = PostedQuestWindow.PostedQuestMode.ACTIVE_REVIEW;
            }
            PostedQuestWindow.Instance.OpenWindow(m_QuestInstance, postMode);
        }

        public void ApplyQuestChain(QuestInstance chain) {
            m_QuestInstance = chain;
            UpdateText();
        }

        private void UpdateText() {
            string titleText = "<b>" + m_QuestInstance.QuestSource.RequestTitle + "</b>\n<i>" + m_QuestInstance.GoldReward.GoldCount + " GP</i>";
            if (m_QuestInstance.ItemRewards.Count != 0) titleText += "<i>" + " & " + m_QuestInstance.ItemRewards.Count + " Item(s)</i>";
            nameText.text = titleText;
            difficultyText.text = "" + m_QuestInstance.DifficultyLevel;
        }
    }

}