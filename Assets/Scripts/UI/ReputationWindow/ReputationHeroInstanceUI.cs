using Rondo.QuestSim.Heroes;
using Rondo.QuestSim.Reputation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rondo.QuestSim.UI.Reputation {

    public class ReputationHeroInstanceUI : MonoBehaviour {

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI classText;
        public RectTransform levelProgressFill;

        public HeroInstance Hero { get; private set; }

        void OnDestroy() {
            Hero.OnExperienceChange -= UpdateProgress;
        }

        public void ApplyHero(HeroInstance hero) {
            Hero = hero;
            Hero.OnExperienceChange += UpdateProgress;

            UpdateProgress();
        }

        private void UpdateProgress() {
            nameText.text = Hero.DisplayName;
            classText.text = Hero.ClassProgress;
            levelProgressFill.localScale = new Vector3(Hero.LevelProgress, levelProgressFill.localScale.y, levelProgressFill.localScale.z);
        }
    }

}