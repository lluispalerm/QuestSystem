using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace QuestSystem.QuestEditor
{
    [CustomEditor(typeof(QuestOnObjectWorld))]
    public class QuestOnObjectWorldEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            QuestOnObjectWorld qOW = (QuestOnObjectWorld)target;

            if (GUILayout.Button("Get all Nodes from Quest"))
            {
                qOW.GetAllNodesFromQuest();
            }

            DrawDefaultInspector();

            if (GUILayout.Button("Populate with children"))
            {
                qOW.PopulateChildListDefault();
            }

        }
    }
}
