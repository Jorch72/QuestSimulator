using Rondo.Generic.Utility;
using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Quests;
using System;
using System.Collections.Generic;

namespace Rondo.QuestSim.Gameplay {

    public static class DayManager {

        public static int CurrentDay { get; set; }
        public static Action OnNextDay = delegate { };

        public static void Initialize() {
            CurrentDay = 1;
        }

        public static void EndDay() {
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

            foreach (QuestInstance postedQuest in QuestManager.PostedQuests) {
                QuestManager.ActiveQuests.Add(postedQuest, HeroManager.GetRandomHero());
            }

            QuestManager.PostedQuests.Clear();
        }

    }

}