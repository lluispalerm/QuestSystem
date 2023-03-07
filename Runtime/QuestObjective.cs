using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [System.Serializable]
    public class QuestObjective
    {
        public string keyName;
        public bool isCompleted;
        public int maxItems;
        public int actualItems;
        public string description;
        public bool hiddenObjective;
        public bool autoExitOnCompleted;

    }
}