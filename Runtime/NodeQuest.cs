using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "New NodeQuest", menuName = "QuestSystem/NodeQuest")]
    [System.Serializable]
    public class NodeQuest : ScriptableObject
    {
        public List<NodeQuest> nextNode = new List<NodeQuest>();
        public TextAsset extraText;
        public List<GameObject> objectsActivated;
        public bool isFinal;
        public QuestObjective[] nodeObjectives;

        [Header("Graph Part")]
        public string GUID;
        public Vector2 position;

        public void AddObject(GameObject g)
        {
            if (g == null) Debug.Log("Object is null");
            if (!objectsActivated.Contains(g))
            {
                objectsActivated.Add(g);
            }
        }

        public void ChangeTheStateOfObjects(bool b)
        {
            foreach (GameObject g in objectsActivated)
            {
                g.SetActive(b);
            }
        }

        private void OnEnable()
        {
            objectsActivated = new List<GameObject>();
        }



    }
}