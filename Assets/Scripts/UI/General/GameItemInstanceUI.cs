using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rondo.QuestSim.UI.General {

    [RequireComponent(typeof(GameItemPopupCaller))]
    public class GameItemInstanceUI : MonoBehaviour {

        public TextMeshProUGUI titleText;

        private GameItemPopupCaller m_ItemPopupCaller;

        public void SetItem(GameItem item) {
            if(m_ItemPopupCaller == null) m_ItemPopupCaller = GetComponent<GameItemPopupCaller>();
            titleText.text = item.DisplayName;
            m_ItemPopupCaller.associatedItem = item;
        }

    }

}