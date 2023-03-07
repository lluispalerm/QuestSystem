using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace QuestSystem.SaveSystem
{
    public class QuestSaveSystem
    {
        public static string GetPath(string saveName)
        {
            return QuestConstants.SAVE_FILE_FOLDER + "/" + saveName + ".save";
        }

        public static bool Save(object saveData)
        {
            BinaryFormatter formatter = GetBinaryFormater();

            if (!Directory.Exists(QuestConstants.SAVE_FILE_FOLDER))
            {
                Directory.CreateDirectory(QuestConstants.SAVE_FILE_FOLDER);
            }

            string path = QuestConstants.SAVE_FILE_PATH;
            Debug.Log(path);
            FileStream file = File.Create(path);

            formatter.Serialize(file, saveData);
            Debug.Log("Saved");
            file.Close();

            return true;
        }

        public static object Load(string path)
        {
            Debug.Log(path);
            if (!File.Exists(path)) return null;

            BinaryFormatter formatter = GetBinaryFormater();

            FileStream file = File.Open(path, FileMode.Open);
            try
            {
                object save = formatter.Deserialize(file);
                file.Close();
                Debug.Log("Loaded");
                return save;
            }
            catch
            {
                Debug.LogErrorFormat("Faled to load file at {0}", path);
                file.Close();
                return null;
            }
        }

        /*
        public static object LoadFromName(string saveName)
        {
            return Load(GetPath(saveName));
        }*/

        public static BinaryFormatter GetBinaryFormater()
        {
            //Create binary formater
            BinaryFormatter formatter = new BinaryFormatter();


            //Define surrogates (especificacions de com tractar les dades)
            SurrogateSelector selector = new SurrogateSelector();

            QuestObjectiveSurrogate objectiveSurrogate = new QuestObjectiveSurrogate();
            NodeQuestSaveDataSurrogate nodeSurrogate = new NodeQuestSaveDataSurrogate();
            QuestSaveDataSurrogate questSurrogate = new QuestSaveDataSurrogate();
            QuestLogSaveDataSurrogate questLogSurrogate = new QuestLogSaveDataSurrogate();

            //selector.AddSurrogate(typeof(QuestObjective), new StreamingContext(StreamingContextStates.All), objectiveSurrogate);
            selector.AddSurrogate(typeof(NodeQuest), new StreamingContext(StreamingContextStates.All), nodeSurrogate);
            selector.AddSurrogate(typeof(Quest), new StreamingContext(StreamingContextStates.All), questSurrogate);
            selector.AddSurrogate(typeof(QuestLog), new StreamingContext(StreamingContextStates.All), questLogSurrogate);

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}