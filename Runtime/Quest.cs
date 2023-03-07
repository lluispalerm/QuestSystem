using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace QuestSystem
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "QuestSystem/Quest")]
    [System.Serializable]
    public class Quest : ScriptableObject
    {
        [Header("Warning!!!! This ScriptaleObject has to be in a resources folder  under Missions/[MisionName]")]
        public NodeQuest firtsNode;
        public NodeQuest nodeActual;
        public List<int> state;
        public int limitDay;
        public int startDay;
        public string misionName;
        public bool isMain;

        [Header("Graph Part")]
        public List<NodeLinksGraph> nodeLinkData;

        [System.Serializable]
        public class NodeLinksGraph
        {
            public string baseNodeGUID;
            public string portName;
            public string targetNodeGUID;
        }

        public void Reset()
        {
            state = new List<int>();
            nodeActual = null;

            NodeQuest[] getNodes = Resources.LoadAll<NodeQuest>($"{QuestConstants.MISIONS_NAME}/{ this.misionName}/Nodes");

            foreach (NodeQuest n in getNodes)
            {
                for (int i = 0; i < n.nodeObjectives.Length; i++)
                {
                    n.nodeObjectives[i].isCompleted = false;
                    n.nodeObjectives[i].actualItems = 0;
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(n);
#endif
            }

            QuestManager.GetInstance().misionLog.RemoveQuest(this);
        }

        public void AdvanceToCurrentNode()
        {
            nodeActual = firtsNode;
            foreach (int i in state)
            {
                nodeActual = nodeActual.nextNode[i];
            }


        
        }

        public void ResetNodeLinksGraph()
        {
            nodeLinkData = new List<NodeLinksGraph>();
        }
    }
}