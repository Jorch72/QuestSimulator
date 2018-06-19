using Rondo.Generic.Utility;
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

        private Coroutine m_UpdateRoutine = null;
        private float m_CurrentExperience = 0;

        void OnDestroy() {
            Hero.OnExperienceChange -= UpdateProgressSmooth;
        }

        public void ApplyHero(HeroInstance hero) {
            Hero = hero;
            nameText.text = Hero.DisplayName;
            Hero.OnExperienceChange += UpdateProgressSmooth;

            UpdateProgressInstant();
        }

        private void UpdateProgressInstant() {
            nameText.text = Hero.DisplayName;
            SetExperience(Hero.Experience);
        }

        private void UpdateProgressSmooth() {
            if (!gameObject.activeInHierarchy) return;
            if (m_UpdateRoutine != null) StopCoroutine(m_UpdateRoutine);
            m_UpdateRoutine = StartCoroutine(SmoothUpdate(Hero.Experience));
        }

        private void SetExperience(float experience) {
            m_CurrentExperience = experience;

            int level;
            int expForNextLevel;
            float levelProgress;
            HeroUtility.CalculateHeroLevel(m_CurrentExperience, out level, out expForNextLevel, out levelProgress);

            classText.text = Hero.GetClassProgress(level);
            levelProgressFill.localScale = new Vector3(levelProgress, levelProgressFill.localScale.y, levelProgressFill.localScale.z);
        }

        private IEnumerator SmoothUpdate(float targetExp) {
            while(Mathf.Abs(targetExp - m_CurrentExperience) >= 0.01f) {
                m_CurrentExperience = Mathf.Lerp(m_CurrentExperience, targetExp, 0.1f);
                SetExperience(m_CurrentExperience);
                yield return null;
            }
            m_CurrentExperience = targetExp;
            SetExperience(Mathf.RoundToInt(m_CurrentExperience));
            StopCoroutine(m_UpdateRoutine);
            m_UpdateRoutine = null;
            yield return null;
        }
    }

}