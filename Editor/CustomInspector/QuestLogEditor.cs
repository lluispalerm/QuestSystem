using UnityEngine;
using UnityEditor;


namespace QuestSystem.QuestEditor
{
    [CustomEditor(typeof(QuestLog))]
    public class QuestLogEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            QuestLog questLog = (QuestLog)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Reset all Quest"))
            {
                questLog.ResetAllQuest();
            }

        }
    }
}
