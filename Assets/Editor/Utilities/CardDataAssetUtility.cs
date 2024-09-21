using System.Collections.Generic;
using Editor.CardData;
using UnityEditor;

namespace Editor.Utilities
{
    public static class CardDataAssetUtility
    {
        public static string[] CardAssetGUIDs = LoadAllCardDataByGUID();
        public static List<CardDataSO> AllCardData = LoadAllCardData();

        private static List<CardDataSO> LoadAllCardData()
        {
            // Find all CardDataSO assets
           
            List<CardDataSO> cardDataList = new();
            foreach (string guid in CardAssetGUIDs)
            {
                CardDataSO cardData = LoadCardDataByGuid(guid);
                if (cardData != null)
                {
                    cardDataList.Add(cardData);
                }
            }
            return cardDataList;
        }

        public static string[] LoadAllCardDataByGUID()
        {
            string[] guids = AssetDatabase.FindAssets("t:CardDataSO");
            return guids;
        }

        public static CardDataSO LoadCardDataByGuid(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<CardDataSO>(path);
        }
    }
}