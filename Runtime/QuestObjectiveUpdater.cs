using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public class QuestObjectiveUpdater : MonoBehaviour, IQuestInteraction
    {
        public Quest questToUpdate;
        [HideInInspector] public NodeQuest nodeToUpdate;
        [HideInInspector] public string keyObjectiveSelected;
        public int adder = 1;
        public int exit = 0;
        public TextAsset extraText;

        public UnityEvent eventsOnUpdate;
        public UnityEvent eventsOnFinish;

        private bool canUpdate;
        private bool updating;
        private bool isDone;
        private QuestManager questManagerRef;


        // Start is called before the first frame update
        void Start()
        {
            canUpdate = false;
            updating = false;
            questManagerRef = QuestManager.GetInstance();
            isDone = questManagerRef.IsDoned(questToUpdate) || questManagerRef.IsFailed(questToUpdate); 
        }

        public void updateQuest()
        {
            //Afegir instancies al objectiu
            if (!string.IsNullOrEmpty(keyObjectiveSelected)) {
                QuestObjective questObjective = new QuestObjective();

                int i = 0;
                //Find
                while (questObjective.keyName == null && i < nodeToUpdate.nodeObjectives.Length)
                {
                    if (nodeToUpdate.nodeObjectives[i].keyName == keyObjectiveSelected)
                    {
                        questObjective = nodeToUpdate.nodeObjectives[i];
                    }
                    i++;
                }

                questObjective.actualItems += adder;

                if (IsNodeCompleted(questObjective))
                {
                    if (nodeToUpdate.isFinal)
                    {
                        questToUpdate.nodeActual.ChangeTheStateOfObjects(false);
                        questManagerRef.DonearQuest(questToUpdate);
                        isDone = true;
                    }
                    else
                    {
                        questToUpdate.nodeActual.ChangeTheStateOfObjects(false);
                        questToUpdate.state.Add(exit);
                        Debug.Log("Exit :" + exit + ", Next node: " + nodeToUpdate.nextNode.Count);
                        questToUpdate.nodeActual = nodeToUpdate.nextNode[exit];
                        questToUpdate.nodeActual.ChangeTheStateOfObjects(true);
                        
                    }

                    eventsOnFinish.Invoke();
                    QuestManager.GetInstance().Save();
                }

                eventsOnUpdate.Invoke();
            }
        }

        IEnumerator ShowDialogue()
        {
            updating = true;
            
            if (extraText == null)
            {
                yield return null;
                updateQuest();
                updating = false;
            }
            else
            {

            }
        }

        private bool isCurrent() => questToUpdate.nodeActual == nodeToUpdate;

        private bool isAbleToUpdateQuest() => canUpdate && !isDone && isCurrent() && !updating;

        public void ChangeExit(int n)
        {
            exit = n;
        }

        //Delete the ones you don't want to use

        private void OnTriggerEnter(Collider other)
        {
            resultOfEnter(true, other.tag);
        }

        private void OnTriggerExit(Collider other)
        {
            resultOfEnter(false, other.tag);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            resultOfEnter(true, other.tag);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            resultOfEnter(false, other.tag);
        }

        private void resultOfEnter(bool canUpdateResult, string tag)
        {
            if (tag == "Player") canUpdate = canUpdateResult;
        }

        public string GetMisionName()
        {
            string returnValue = "";

            if (questToUpdate != null) returnValue += questToUpdate.misionName + "_";

            return returnValue;
        }

        public string[] GetListOfObjectives()
        {
            List<string> returnList = new List<string>() ;

            if(nodeToUpdate != null)
            {
                if (nodeToUpdate.nodeObjectives != null) {
                    for (int i = 0; i < nodeToUpdate.nodeObjectives.Length; i++)
                    {
                        returnList.Add(nodeToUpdate.nodeObjectives[i].keyName);
                    }
                }
            }

            return returnList.ToArray();
        }

        public void Interact()
        {
            if (isAbleToUpdateQuest())
            {
                StartCoroutine(ShowDialogue());
            }
        }

        private bool IsNodeCompleted(QuestObjective questObjective)
        {
            if (questObjective.actualItems >= questObjective.maxItems)
            {
                questObjective.isCompleted = true;
                if(questObjective.autoExitOnCompleted) return true;
            }

            for (int i = 0; i < nodeToUpdate.nodeObjectives.Length; i++)
            {
                if (!nodeToUpdate.nodeObjectives[i].isCompleted && !nodeToUpdate.nodeObjectives[i].autoExitOnCompleted)
                {
                    return false;
                }
            } 

            return true;
        }
    }
}