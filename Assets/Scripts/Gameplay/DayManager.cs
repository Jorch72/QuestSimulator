using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Inventory;
using Rondo.QuestSim.Quests;
using Rondo.QuestSim.UI.General;
using Rondo.QuestSim.UI.PostedQuests;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    public class DayManager : MonoBehaviourSingleton<DayManager> {

        public int CurrentDay { get; set; }
        public Action OnNextDay = delegate { };

        private List<QuestInstance> m_ActiveQuestsToUpdate = new List<QuestInstance>();
        private List<QuestInstance> m_QuestsToAssign = new List<QuestInstance>();

        private void Awake() {
            CurrentDay = 1;

            Instance = this;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                EndDay();
            }
        }

        public void EndDay() {
            PostedQuestWindow.Instance.OnWindowClose += NextDayStep;

            m_QuestsToAssign = new List<QuestInstance>(QuestManager.PostedQuests);
            m_ActiveQuestsToUpdate = new List<QuestInstance>(QuestManager.ActiveQuests.Keys);

            NextDayStep();
        }

        private void NextDayStep() {
            Debug.Log("Quests left to assign = " + m_QuestsToAssign.Count);
            if(m_QuestsToAssign.Count != 0) {
                QuestInstance nextQuest = m_QuestsToAssign[m_QuestsToAssign.Count - 1];
                m_QuestsToAssign.Remove(nextQuest);

                PostedQuestWindow.Instance.OpenWindow(nextQuest, PostedQuestWindow.PostedQuestMode.HERO_SELECT);
                return;
            }

            Debug.Log("Quests to finish = " + m_ActiveQuestsToUpdate.Count);
            while (m_ActiveQuestsToUpdate.Count != 0) {
                //Show reward thing, for now just give everything
                QuestInstance activeQuest = m_ActiveQuestsToUpdate[m_ActiveQuestsToUpdate.Count -1];
                activeQuest.DaysLeftOnQuest--;

                m_ActiveQuestsToUpdate.Remove(activeQuest);

                if (activeQuest.DaysLeftOnQuest <= 0) {
                    PostedQuestWindow.Instance.OpenWindow(activeQuest, PostedQuestWindow.PostedQuestMode.COMPLETED);
                    activeQuest.CompleteQuest(QuestManager.ActiveQuests[activeQuest]);
                    return;
                }
            }

            CurrentDay++;

            OnNextDay();

            QuestManager.PostedQuests = UpdateQuestTimeLimits(QuestManager.PostedQuests, 0);
            QuestManager.Requests = UpdateQuestTimeLimits(QuestManager.Requests, 1);
            InventoryManager.Gold -= 10;

            NightFadeUI.Instance.Disable(()=> { });
            PostedQuestWindow.Instance.OnWindowClose -= NextDayStep;
        }

        private List<QuestInstance> UpdateQuestTimeLimits(List<QuestInstance> list, int dayLimit) {
            for (int i = list.Count - 1; i >= 0; i--) {
                QuestInstance quest = list[i];
                quest.DaysLeftOnPost--;
                if (quest.DaysLeftOnPost <= dayLimit) {
                    list.Remove(quest);
                }
            }
            return list;
        }

    }

}