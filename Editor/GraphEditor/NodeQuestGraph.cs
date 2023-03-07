using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace QuestSystem.QuestEditor
{
    [System.Serializable]
    public class NodeQuestGraph : UnityEditor.Experimental.GraphView.Node
    {
        public string GUID;

        public TextAsset extraText;

        public VisualElement objectivesRef;

        public List<QuestObjectiveGraph> questObjectives;

        public bool isFinal;

        public bool entryPoint = false;

        public int limitDay;

        public int startDay;

        public string misionName;

        public bool isMain;


    }
}