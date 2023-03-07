using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestGiver : MonoBehaviour , IQuestInteraction
    {
        public Quest questToGive;
        public TextAsset extraText;

        private bool ableToGive = false;
        private bool questAlreadyGiven;
        private QuestManager questManagerRef;

        // Start is called before the first frame update
        void Start()
        {
            questManagerRef = QuestManager.GetInstance();
            questAlreadyGiven = questManagerRef.IsMisionInLog(questToGive);
        }

        public void giveQuest()
        {
            showDialogue();
            questManagerRef.AddMisionToCurrent(questToGive);

            questAlreadyGiven = true;
            QuestManager.GetInstance().Save();
        }

        public void showDialogue()
        {
            //if (conversation != null) FindObjectOfType<DialogueWindow>().StartDialogue(conversation, null);
            //else return;
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

        private void resultOfEnter(bool ableToGiveResult, string tag)
        {
            if (tag == "Player") ableToGive = ableToGiveResult;
        }

        public void Interact()
        {
            if(ableToGive && !questAlreadyGiven)
            {
                giveQuest();
            }
        }


    }
}