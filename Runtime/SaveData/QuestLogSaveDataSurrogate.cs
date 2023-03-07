using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace QuestSystem.SaveSystem
{
    public class QuestLogSaveDataSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            QuestLog ql = (QuestLog)obj;

            info.AddValue("curentQuest", ql.curentQuests);
            info.AddValue("doneQuest", ql.doneQuest);
            info.AddValue("failedQuest", ql.failedQuest);
            info.AddValue("businessDay", ql.businessDay);

        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            QuestLog ql = (QuestLog)obj;

            ql.curentQuests = (List<Quest>)info.GetValue("curentQuest", typeof(List<Quest>));
            ql.doneQuest = (List<Quest>)info.GetValue("doneQuest", typeof(List<Quest>));
            ql.failedQuest = (List<Quest>)info.GetValue("failedQuest", typeof(List<Quest>));

            ql.businessDay = (int)info.GetValue("businessDay", typeof(int));

            obj = ql;
            return obj;
        }
    }

    [System.Serializable]
    public class QuestLogSaveData
    {
        public List<QuestSaveData> currentQuestSave;
        public List<QuestSaveData> doneQuestSave;
        public List<QuestSaveData> failedQuestSave;

        public int dia;

        public QuestLogSaveData(QuestLog ql)
        {
            //Manage current quest
            currentQuestSave = new List<QuestSaveData>();
            doneQuestSave = new List<QuestSaveData>();
            failedQuestSave = new List<QuestSaveData>();

            foreach (Quest q in ql.curentQuests)
            {
                QuestSaveData aux = new QuestSaveData();
                aux.name = q.misionName;
                aux.states = q.state;

                aux.actualNodeData = new NodeQuestSaveData(q.nodeActual.nodeObjectives.Length);

                for (int i = 0; i < q.nodeActual.nodeObjectives.Length; i++)
                    aux.actualNodeData.objectives[i] = q.nodeActual.nodeObjectives[i];

                currentQuestSave.Add(aux);
            }

            foreach (Quest q in ql.doneQuest)
            {
                QuestSaveData aux = new QuestSaveData();
                aux.name = q.misionName;

                currentQuestSave.Add(aux);
            }

            foreach (Quest q in ql.failedQuest)
            {
                QuestSaveData aux = new QuestSaveData();
                aux.name = q.misionName;

                currentQuestSave.Add(aux);
            }

            dia = ql.businessDay;

        }
    }
}