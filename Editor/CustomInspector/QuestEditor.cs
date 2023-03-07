using UnityEditor;
using UnityEngine;

namespace QuestSystem.QuestEditor
{
    [CustomEditor(typeof(Quest))]
    public class QuestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Quest q = (Quest)target;

            DrawDefaultInspector();

            if (GUILayout.Button("Reset"))
            {
                q.Reset();
            }

        }
    }
}