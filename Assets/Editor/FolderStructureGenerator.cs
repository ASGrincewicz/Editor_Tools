using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class FolderStructureGenerator
    {
        [MenuItem("Tools/Create Project Folders")]
        private static void CreateFolders()
        {
            // Define your folder hierarchy here
            string[] folderHierarchy = {
                "Assets/Animations",
                "Assets/Audio",
                "Assets/Audio/Music",
                "Assets/Audio/Sounds",
                "Assets/Editor",
                "Assets/Materials",
                "Assets/Art",
                "Assets/Art/Characters",
                "Assets/Art/Characters/Player",
                "Assets/Art/Characters/Player/Materials",
                "Assets/Art/Characters/Player/Models",
                "Assets/Art/Characters/Player/Sprites",
                "Assets/Art/Characters/Player/Prefabs",
                "Assets/Art/Characters/Player/Textures",
                "Assets/Art/Characters/AI",
                "Assets/Art/Characters/AI/Materials",
                "Assets/Art/Characters/AI/Models",
                "Assets/Art/Characters/AI/Sprites",
                "Assets/Art/Characters/AI/Prefabs",
                "Assets/Art/Characters/AI/Textures",
                "Assets/Art/Props",
                "Assets/Art/Props/Materials",
                "Assets/Art/Props/Models",
                "Assets/Art/Props/Sprites",
                "Assets/Art/Props/Prefabs",
                "Assets/Art/Props/Textures",
                "Assets/Art/Levels",
                "Assets/Art/Levels/Materials",
                "Assets/Art/Levels/Sprites",
                "Assets/Art/Levels/Prefabs",
                "Assets/Art/Levels/Textures",
                "Assets/Art/UI",
                "Assets/Art/UI/Materials",
                "Assets/Art/UI/Sprites",
                "Assets/Art/UI/Prefabs",
                "Assets/Art/UI/Textures",
                "Assets/Scenes",
                "Assets/Code",
                "Assets/Code/Scripts/Player",
                "Assets/Code/Scripts/AI",
                "Assets/Code/Scripts/Utilities",
                "Assets/Code/Shaders",
                "Assets/Documents/",
                "Assets/Documents/Wiki",
                "Assets/Documents/Concept",
                "Assets/Documents/Design",
                "Assets/Data",
                "Assets/Data/Scriptable Objects",
                "Assets/Data/Scriptable Objects/Characters/Player",
                "Assets/Data/Scriptable Objects/Characters/AI",
                "Assets/Data/Scriptable Objects/Props",
                "Assets/Data/Scriptable Objects/Level",
                "Assets/Data/Scriptable Objects/UI",
                "Assets/Data/Scriptable Objects/Utilities",
                "Assets/Data/Art",
                "Assets/Data/Art/Sprites",
                "Assets/Data/Art/Textures",
                "Assets/Data/Art/Images",
                "Assets/Data/Text",
                "Assets/Data/Audio"


            };

            foreach (string folderPath in folderHierarchy)
            {
                string messagePrefix;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                    messagePrefix = "Created folder: ";
                }
                else
                {
                    messagePrefix = "Folder already exists: ";
                }
                string message = messagePrefix + folderPath;
                Debug.Log(message);
            }
            AssetDatabase.Refresh();
        }
    }
}