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

        private HeroInstance m_Hero;

        void OnDestroy() {
            m_Hero.OnExperienceChange -= UpdateProgress;
        }

        public void ApplyHero(HeroInstance hero) {
            m_Hero = hero;
            m_Hero.OnExperienceChange += UpdateProgress;

            UpdateProgress();
        }

        private void UpdateProgress() {
            nameText.text = m_Hero.DisplayName;
            classText.text = m_Hero.ClassProgress;
            levelProgressFill.localScale = new Vector3(m_Hero.LevelProgress, levelProgressFill.localScale.y, levelProgressFill.localScale.z);
        }
    }

}