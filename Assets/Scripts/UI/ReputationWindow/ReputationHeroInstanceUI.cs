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

        public void ApplyHero(HeroInstance hero) {
            nameText.text = hero.DisplayName;
            m_Hero = hero;
            UpdateProgress();
        }

        private void UpdateProgress() {
            classText.text = m_Hero.ClassProgress;
            int remainder = m_Hero.Experience % 20;
            levelProgressFill.localScale = new Vector3(remainder / 20f, levelProgressFill.localScale.y, levelProgressFill.localScale.z);
        }
    }

}