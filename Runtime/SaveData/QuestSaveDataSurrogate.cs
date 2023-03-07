using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace QuestSystem.SaveSystem
{
    public class QuestSaveDataSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            Quest q = (Quest)obj;

            info.AddValue("firtsNode", q.firtsNode);
            info.AddValue("nodeActual", q.nodeActual);
            info.AddValue("state", q.state);
            info.AddValue("limitDay", q.limitDay);
            info.AddValue("startDay", q.startDay);
            info.AddValue("misionName", q.misionName);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            Quest q = (Quest)obj;

            q.firtsNode = (NodeQuest)info.GetValue("firtsNode", typeof(NodeQuest));
            q.firtsNode = (NodeQuest)info.GetValue("nodeActual", typeof(NodeQuest));
            q.state = (List<int>)info.GetValue("state", typeof(List<int>));
            q.limitDay = (int)info.GetValue("limitDay", typeof(int));
            q.startDay = (int)info.GetValue("startDay", typeof(int));
            q.misionName = (string)info.GetValue("misionName", typeof(string));

            obj = q;
            return obj;
        }

    }

    [System.Serializable]
    public class QuestSaveData
    {
        public List<int> states;
        public string name;
        public NodeQuestSaveData actualNodeData;
    }

}