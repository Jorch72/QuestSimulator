using Rondo.Generic.Utility;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rondo.QuestSim.UI.General {

    public class GameItemPopupCaller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler {

        public GameItem associatedItem;

        public void OnPointerEnter(PointerEventData eventData) {
            GameItemPopup.Instance.SwitchItemTarget(associatedItem);
        }

        public void OnPointerExit(PointerEventData eventData) {
            GameItemPopup.Instance.SwitchItemTarget(null);
        }

        public void OnPointerUp(PointerEventData eventData) {
            GameItemPopup.Instance.SwitchItemTarget(null);
        }
    }

}