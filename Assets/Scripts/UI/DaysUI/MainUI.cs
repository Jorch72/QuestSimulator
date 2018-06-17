using Rondo.QuestSim.Gameplay;
using Rondo.QuestSim.Inventory;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rondo.QuestSim.UI.Main {

    public class MainUI : MonoBehaviour {

        [Header("Main resources")]
        public TextMeshProUGUI goldText;

        [Header("Days")]
        public TextMeshProUGUI daysText;
        public Button endDayButton;

        private void Start() {
            endDayButton.onClick.AddListener(()=> {
                DayManager.Instance.EndDay();
            });

            DayManager.Instance.OnNextDay += () => { daysText.text = "Day " + DayManager.Instance.CurrentDay; };
            InventoryManager.OnGoldChange += (gold) => { goldText.text = gold + " Gold"; };
        }
    }

}