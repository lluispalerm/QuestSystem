using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public static class QuestConstants
    {
        public static readonly string RESOURCES_PATH = "Assets/Resources";
        public static readonly string RESOURCES_NAME = "Resources";
        public static readonly string MISIONS_FOLDER = "Assets/Resources/Missions";
        public static readonly string MISIONS_NAME = "Missions";
        public static readonly string PARENT_PATH = "Assets";
        public static readonly string SAVE_FILE_PATH = Application.persistentDataPath + "/saves/" + "savefile" + ".save";
        public static readonly string SAVE_FILE_FOLDER = Application.persistentDataPath + "/saves";
        public static readonly string QUEST_LOG_NAME = "TheQuestLog";
        public static readonly string NODES_TEMP_FOLDER_NAME = "NodesTemp";
        public static readonly string NODES_FOLDER_NAME = "Nodes";
    }
}

