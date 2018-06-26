using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using Rondo.QuestSim.UI.PostedQuests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.ActiveQuests {

    public class QuestInstanceUI : MonoBehaviour {

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI difficultyText;

        private Button m_Button;
        private QuestInstance m_QuestInstance;
        private QuestDetailsWindow.QuestMode m_QuestMode;

        void Awake() {
            m_Button = GetComponent<Button>();
        }

        void Start() {
            m_Button.onClick.AddListener(OpenQuestWindow);
        }

        private void OnDestroy() {
            m_QuestInstance.OnDaysLeftUpdate -= UpdateText;
        }

        private void OpenQuestWindow() {
            QuestDetailsWindow.Instance.OpenWindow(m_QuestInstance, m_QuestMode);
        }

        public void ApplyQuestChain(QuestInstance chain) {
            m_QuestInstance = chain;

            if (QuestManager.PostedQuests.Contains(m_QuestInstance)) {
                m_QuestMode = QuestDetailsWindow.QuestMode.POSTED_REVIEW;
            } else if (QuestManager.Requests.Contains(m_QuestInstance)) {
                m_QuestMode = QuestDetailsWindow.QuestMode.SETUP;
            } else {
                m_QuestMode = QuestDetailsWindow.QuestMode.ACTIVE_REVIEW;
            }

            m_QuestInstance.OnDaysLeftUpdate += UpdateText;

            UpdateText();
        }

        private void UpdateText() {
            string titleText = "<b>" + m_QuestInstance.QuestSource.RequestTitle + "</b>\n<i>";

            switch (m_QuestMode) {
                case QuestDetailsWindow.QuestMode.SETUP:
                case QuestDetailsWindow.QuestMode.POSTED_REVIEW:
                    int expiresInDays = m_QuestInstance.DaysLeftOnPost;
                    titleText += "Expires in " + expiresInDays + " day" + (expiresInDays > 1 ? "s" : "");
                    break;
                case QuestDetailsWindow.QuestMode.ACTIVE_REVIEW:
                    int daysLeft = m_QuestInstance.DaysLeftOnQuest;
                    titleText += daysLeft + " day" + (daysLeft > 1 ? "s" : "") + " left until completed";
                    break;
                default:
                    break;
            }

            nameText.text = titleText;
            difficultyText.text = "" + m_QuestInstance.DifficultyLevel;
        }
    }

}