using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace QuestSystem.SaveSystem
{
    public class NodeQuestSaveDataSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            NodeQuest nq = (NodeQuest)obj;
            info.AddValue("extraText", nq.extraText);
            info.AddValue("isFinal", nq.isFinal);
            info.AddValue("nodeObjectives", nq.nodeObjectives);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            NodeQuest nq = (NodeQuest)obj;
            nq.isFinal = (bool)info.GetValue("isFinal", typeof(bool));
            nq.nodeObjectives = (QuestObjective[])info.GetValue("nodeObjectives", typeof(QuestObjective[]));
            obj = nq;
            return obj;
        }

    }

    [System.Serializable]
    public class NodeQuestSaveData
    {
        public QuestObjective[] objectives;

        public NodeQuestSaveData()
        {
            objectives = new QuestObjective[1];
        }

        public NodeQuestSaveData(int i)
        {
            objectives = new QuestObjective[i];
        }


    }
}