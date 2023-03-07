using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using QuestSystem.SaveSystem;

namespace QuestSystem
{
    public class QuestManager
    {
        public QuestLog misionLog;
        public QuestLogSaveData data;

        private static QuestManager instance;

        public static QuestManager GetInstance()
        {
            if (instance == null) instance = new QuestManager();
            return instance;
        }

        private QuestManager()
        {
            misionLog = Resources.Load<QuestLog>(QuestConstants.QUEST_LOG_NAME);

            if (misionLog == null)
            {
                // crear
                misionLog = ScriptableObject.CreateInstance<QuestLog>();
#if UNITY_EDITOR
                AssetDatabase.CreateAsset(misionLog, QuestConstants.RESOURCES_PATH + "/" + QuestConstants.QUEST_LOG_NAME + ".asset");
#endif

            }

            QuestLogSaveData aux = QuestSaveSystem.Load(QuestConstants.SAVE_FILE_PATH) as QuestLogSaveData;
            if (aux == null) Debug.Log("No file to load in " + aux);
            else
            {
                data = aux;
                misionLog.LoadUpdate(data);
            }
        }

        public void AddMisionToCurrent(Quest q)
        {
            q.nodeActual = q.firtsNode;
            q.nodeActual.ChangeTheStateOfObjects(true);
            misionLog.curentQuests.Add(q);
        }

        public bool IsMisionInLog(Quest q)
        {
            return misionLog.IsCurrent(q) || misionLog.IsDoned(q) || misionLog.IsFailed(q);
        }

        public bool IsCurrent(Quest q) => misionLog.IsCurrent(q);

        public bool IsDoned(Quest q) => misionLog.IsDoned(q);

        public bool IsFailed(Quest q) => misionLog.IsFailed(q);

        public void DonearQuest(Quest q)
        {
            misionLog.curentQuests.Remove(q);
            misionLog.doneQuest.Add(q);

            Save();
        }

        public void Save()
        {
            //Comprovar que se guarde compilado
#if !UNITY_EDITOR
            data = new QuestLogSaveData(misionLog);
            QuestSaveSystem.Save(data);
#endif
        }



        /// <summary>
        /// Formats the current mision information like this:
        /// Mision Name /n
        ///    /tab Objective description 0/1
        /// </summary>
        /// <returns> Returns the string formated </returns>
        public string GetCurrentQuestsInformation()
        {
            string textFormated = "";

            foreach (Quest quest in misionLog.curentQuests)
            {
                textFormated += quest.misionName + "\n";

                for (int i = 0; i < quest.nodeActual.nodeObjectives.Length; i++)
                {
                    QuestObjective currentObjective = quest.nodeActual.nodeObjectives[i];
                    textFormated += "   " + currentObjective.description  + " "
                                    + currentObjective.actualItems + "/" + currentObjective.maxItems
                                    + "\n";
                }
            }

            return textFormated;
        }



    }
}