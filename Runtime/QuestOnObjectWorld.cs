using UnityEngine;

namespace QuestSystem
{
    public class QuestOnObjectWorld : MonoBehaviour
    {
        //Structs in order to show on editor
        [System.Serializable]
        public struct ObjectsForQuestTable
        {
            public Quest quest;
            public ActivationRowNode[] tableNodes;
        }

        [System.Serializable]
        public struct ActivationRowNode
        {
            public NodeQuest node;
            public bool activate;
        }

        //TODO revisar
        public ObjectsForQuestTable[] objectsForQuestTable;

        public GameObject[] objectsToControlList;

        private QuestManager questManagerRef;
        // Start is called before the first frame update
        private void Awake()
        {
            questManagerRef = QuestManager.GetInstance();
        }



        void Start()
        {
            //Default: every object is disabled
            foreach (GameObject g in objectsToControlList)
            {
                g.SetActive(false);
            }

            for (int i = 0; i < objectsForQuestTable.Length; i++)
            {
                bool isCurrent = questManagerRef.IsCurrent(objectsForQuestTable[i].quest);
                bool hasBeenActivated = false;


                for (int j = 0; j < objectsForQuestTable[i].tableNodes.Length; j++)
                {
                    if (objectsForQuestTable[i].tableNodes[j].activate)
                    {
                        foreach (GameObject g in objectsToControlList)
                        {
                            objectsForQuestTable[i].tableNodes[j].node.AddObject(g);
                        }
                    }

                    if (!hasBeenActivated && isCurrent && objectsForQuestTable[i].quest.nodeActual == objectsForQuestTable[i].tableNodes[j].node)
                    {
                        foreach (GameObject g in objectsToControlList)
                        {
                            g.SetActive(objectsForQuestTable[i].tableNodes[j].activate);
                        }
                        hasBeenActivated = true;
                    }
                }
            }

            


        }

        public void PopulateChildListDefault()
        {
            int numberOfChilds = transform.childCount;
            GameObject[] children = new GameObject[numberOfChilds];

            for (int i = 0; i < numberOfChilds; i++)
            {
                children[i] = transform.GetChild(i).gameObject;
            }

            objectsToControlList = children;
        }

        public void GetAllNodesFromQuest()
        {
            for (int i = 0; i < objectsForQuestTable.Length; i++)
            {
                string path = $"{QuestConstants.MISIONS_NAME}/{objectsForQuestTable[i].quest.misionName}/{QuestConstants.NODES_FOLDER_NAME}";
                NodeQuest[] nodesFromQuest = Resources.LoadAll<NodeQuest>(path);
                if (nodesFromQuest != null && nodesFromQuest.Length > 0)
                {
                    ActivationRowNode[] tableNodes = new ActivationRowNode[nodesFromQuest.Length]; 
                    for (int j = 0; j < tableNodes.Length; j++)
                    {
                        tableNodes[j].node = nodesFromQuest[j];
                    }

                    objectsForQuestTable[i].tableNodes = tableNodes;
                }
            }
        }

    }




}