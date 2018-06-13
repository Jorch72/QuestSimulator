using Rondo.QuestSim.Quests;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Requests {

    public class RequestPostWindow : MonoBehaviour {

        public TextMeshProUGUI questChainTitle;
        public TextMeshProUGUI difficultyText;

        public TMP_InputField goldInputField;

        public Button cancelButton;
        public Button postButton;

        private QuestInstance m_CurrentRequest;

        void Start() {
            cancelButton.onClick.AddListener(() => {
                Reset();
                gameObject.SetActive(false);
            });

            postButton.onClick.AddListener(() => {
                Reset();
                gameObject.SetActive(false);

                QuestManager.Requests.Remove(m_CurrentRequest);
                QuestManager.PostedQuests.Add(m_CurrentRequest);

                RequestsWindow.Instance.ReloadInstances();
            });

            goldInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            goldInputField.onEndEdit.AddListener((value) => {
                if (string.IsNullOrEmpty(value)) {
                    value = "0";
                    goldInputField.text = value;
                }
                int iValue = int.Parse(value);
                m_CurrentRequest.GoldReward.GoldCount = iValue;
            });
        }

        public void OpenWindow(QuestInstance request) {
            if(request == m_CurrentRequest) {
                gameObject.SetActive(!gameObject.activeSelf);
            } else {
                Reset();
                gameObject.SetActive(true);
            }
            m_CurrentRequest = request;

            questChainTitle.text = "<b><u>" + request.QuestSource.RequestTitle + "</u></b>\n<size=18><i>" + request.ObjectiveCount + " Objective(s)</i></size>";
            difficultyText.text = ""+request.DifficultyLevel;
        }

        public void Reset() {
            goldInputField.text = "0";
        }
    }

}