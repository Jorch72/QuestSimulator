
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.Generic.Utility {

    [RequireComponent(typeof(Image))]
    public class ImageColorPulsator : MonoBehaviour {

        [SerializeField] private Color m_Color1 = Color.white;
        [SerializeField] private Color m_Color2 = Color.white;
        public float pulseSpeed = 2;

        private Image m_Image;

        private void Awake() {
            m_Image = GetComponent<Image>();
        }

        private void Update() {
            Color trueTargetColor = GetCurrentColor();
            m_Image.color = Color.Lerp(m_Image.color, trueTargetColor, 0.05f);
        }

        public void SetColor1(Color color, bool force = false) {
            m_Color1 = color;
            if (force) {
                m_Image.color = GetCurrentColor();
            }
        }

        public void SetColor2(Color color, bool force= false) {
            m_Color2 = color;
            if (force) {
                m_Image.color = GetCurrentColor();
            }
        }

        private Color GetCurrentColor() {
            return Color.Lerp(m_Color1, m_Color2, Mathf.Sin(Time.time * pulseSpeed).Map(-1, 1, 0, 1));
        }
    }

}