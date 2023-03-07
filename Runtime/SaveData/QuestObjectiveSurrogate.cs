using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

namespace QuestSystem.SaveSystem
{
    public class QuestObjectiveSurrogate : ISerializationSurrogate
    {
        public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
        {
            QuestObjective qo = (QuestObjective)obj;

            info.AddValue("keyName", qo.keyName);
            info.AddValue("isCompleted", qo.isCompleted);
            info.AddValue("maxItems", qo.maxItems);
            info.AddValue("actualItems", qo.actualItems);
            info.AddValue("description", qo.description);
        }

        public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
        {
            QuestObjective qo = (QuestObjective)obj;

            qo.keyName = (string)info.GetValue("keyName", typeof(string));
            qo.isCompleted = (bool)info.GetValue("isCompleted", typeof(bool));
            qo.maxItems = (int)info.GetValue("maxItems", typeof(int));
            qo.actualItems = (int)info.GetValue("actualItems", typeof(int));
            qo.description = (string)info.GetValue("description", typeof(string));

            obj = qo;
            return obj;
        }


    }
}