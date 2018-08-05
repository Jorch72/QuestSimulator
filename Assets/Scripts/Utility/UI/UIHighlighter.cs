using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.Generic.Utility.UI {

    public class UIHighlighter : MonoBehaviourSingleton<UIHighlighter> {

        public float pulseSpeed = 3;
        public float lerpSpeed = 5;

        [Header("Default colors")]
        public Color greenHightlightColor;
        public Color redHighlightColor;

        private Dictionary<Graphic, HighlightSettings> m_HighlightObjects = new Dictionary<Graphic, HighlightSettings>();

        private void Update() {
            float value = Mathf.Sin(Time.time * pulseSpeed).Map(-1, 1, 0, 1);
            foreach(HighlightSettings hs in m_HighlightObjects.Values) {
                hs.Update(value, lerpSpeed);
            }
        }

        public void AddObjects(Graphic[] graphics, Color hightlightColor) {
            foreach (Graphic i in graphics) AddObject(i, hightlightColor, i.color);
        }


        public void AddObjects(Graphic[] graphics, Color hightlightColor, Color startColor) {
            foreach (Graphic i in graphics) AddObject(i, hightlightColor, startColor);
        }

        public void AddObject(Graphic image, Color hightlightColor) {
            AddObject(image, hightlightColor, image.color);
        }

        public void AddObject(Graphic graphic, Color hightlightColor, Color startColor) {
            if (m_HighlightObjects.ContainsKey(graphic)) {
                m_HighlightObjects[graphic].SetHighlightColor(hightlightColor);
            }else {
                m_HighlightObjects.Add(graphic, new HighlightSettings(graphic, startColor, hightlightColor));
            }
        }

        public void RemoveObjects(Graphic[] graphic) {
            foreach (Graphic g in graphic) RemoveObject(g);
        }

        public void RemoveObject(Graphic graphic) {
            if (!m_HighlightObjects.ContainsKey(graphic)) return;
            m_HighlightObjects[graphic].Reset();
            m_HighlightObjects.Remove(graphic);
        }

        public void RemoveAll() {
            List<Graphic> keys = new List<Graphic>(m_HighlightObjects.Keys);
            for(int i = keys.Count - 1; i >= 0; i--) {
                RemoveObject(keys[i]);
            }
        }

        private class HighlightSettings {

            private Graphic m_Image;
            private Color m_StartColor;
            private Color m_EndColor;

            private Color m_EndColorTarget;

            public HighlightSettings(Graphic image, Color startColor, Color endColor) {
                m_Image = image;
                m_StartColor = startColor;
                m_EndColor = endColor;
                m_EndColorTarget = endColor;
            }

            public void Update(float value, float lerpSpeed) {
                m_EndColor = Color.Lerp(m_EndColor, m_EndColorTarget, lerpSpeed * Time.time);
                Color highlightColor = Color.Lerp(m_StartColor, m_EndColor, value);
                m_Image.color = Color.Lerp(m_Image.color, highlightColor, 1 * Time.time);
            }

            public void SetHighlightColor(Color c) {
                m_EndColorTarget = c;
            }

            public void Reset() {
                m_Image.color = m_StartColor;
            }
        }
    }

}