using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Quests;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rondo.QuestSim.Gameplay {

    public class DayManager : MonoBehaviourSingleton<DayManager> {

        public int CurrentDay { get; set; }
        public Action OnNextDay = delegate { };

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
            //Return heroes
            List<QuestInstance> activeQuestList = new List<QuestInstance>(QuestManager.ActiveQuests.Keys);
            for (int i = activeQuestList.Count - 1; i >= 0; i--) {
                QuestInstance activeQuest = activeQuestList[i];
                activeQuest.DaysLeftOnQuest--;

                if (activeQuest.DaysLeftOnQuest <= 0) {
                    activeQuest.CompleteQuest(QuestManager.ActiveQuests[activeQuest]);
                    QuestManager.ActiveQuests.Remove(activeQuest);
                }
            }

            CurrentDay++;

            OnNextDay();

            QuestManager.PostedQuests = UpdateQuestTimeLimits(QuestManager.PostedQuests, 0);
            QuestManager.Requests = UpdateQuestTimeLimits(QuestManager.Requests, 1);
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