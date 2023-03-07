using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace QuestSystem.QuestEditor
{
    [CustomEditor(typeof(QuestObjectiveUpdater))]
    public class QuestObjectiveUpdaterEditor : Editor
    {           
        private int selectedValue = 0;
        private string previousKey = "";
        private readonly float marginX = 25;
        private readonly int marginBottomSpaces = 8;
        //private int 

        

        public override void OnInspectorGUI()
        {
            QuestObjectiveUpdater qU = (QuestObjectiveUpdater)target;
            GUIContent objectFieldLabel = new GUIContent("Hola");
            Rect commonRect = new Rect(new Vector2(20,344), new Vector2(EditorGUIUtility.currentViewWidth - marginX, 20));
            DrawDefaultInspector();



            //Get all Keys
            if (qU.nodeToUpdate != null) {
                string[] keyList = qU.GetListOfObjectives();
                int[] numberList = new int[keyList.Length];

                for (int i = 0; i < keyList.Length; i++)
                {
                    numberList[i] = i;
                }

                if (!string.IsNullOrEmpty(qU.keyObjectiveSelected)) selectedValue = Array.IndexOf(keyList, qU.keyObjectiveSelected);

                selectedValue = EditorGUILayout.IntPopup("Keys: ", selectedValue, keyList, numberList);
                if (keyList.Length > 0 && selectedValue >= 0)
                {
                    if(previousKey != keyList[selectedValue])
                    {
                        qU.keyObjectiveSelected = keyList[selectedValue];
                        previousKey = keyList[selectedValue];
                        EditorUtility.SetDirty(qU);
                    }
                }
            }

            for (int i = 0; i < marginBottomSpaces; i++)
            {
                EditorGUILayout.Space();
            }

            if (GUI.Button(commonRect, "~"))
            {
                EditorGUIUtility.ShowObjectPicker<NodeQuest>(qU.nodeToUpdate, false, qU.GetMisionName(), GetInstanceID());
            }

            if (Event.current.commandName == "ObjectSelectorUpdated")
            {
                if (EditorGUIUtility.GetObjectPickerControlID() == GetInstanceID())
                {
                    qU.nodeToUpdate = EditorGUIUtility.GetObjectPickerObject() as NodeQuest;
                    EditorUtility.SetDirty(qU);
                }

            }

            EditorGUI.ObjectField(commonRect, "   Node to Update", qU.nodeToUpdate, typeof(NodeQuest), false);


            for (int i = 0; i < marginBottomSpaces; i++)
            {
                EditorGUILayout.Space();
            }
        }




    }
}
