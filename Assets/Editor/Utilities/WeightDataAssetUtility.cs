using Editor.AttributesWeights;
using Editor.CardData;
using UnityEditor;
using UnityEngine;

namespace Editor.Utilities
{
    public static class WeightDataAssetUtility
    {
        private const string ALLY_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Ally_Weights.asset";

        private const string BOSS_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Boss_Weights.asset";

        private const string CREATURE_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Creature_Weights.asset";
        private const string ENVIRONMENT_WEIGHT_ASSET_PATH = "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Environment_Weights.asset";
        private const string GEAR_WEIGHT_ASSET_PATH = "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Gear_Weights.asset";
        private const string HUNTER_WEIGHT_ASSET_PATH = "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Hunter_Weights.asset";

        private const string KEYWORD_ONLY_WEIGHT_ASSET_PATH =
            "Assets/Data/Scriptable Objects/Card Stats/Weight Data/Keyword-Only_Weights.asset";
        
        
        
        public static void SetWeightData(CardDataSO card)
        {
            WeightContainer weights = GetWeightContainer();
            if (ReferenceEquals(card.WeightData, null))
            {
                Debug.LogError($"{nameof(card.WeightData)} is null.");
            }
            card.WeightData = weights;
        }
        
        private static WeightContainer GetWeightContainer()
        {
            /*return cardType switch
            {
                CardTypes.Character_Ally => AssetDatabase.LoadAssetAtPath<WeightContainer>(ALLY_WEIGHT_ASSET_PATH),
                CardTypes.Environment => AssetDatabase.LoadAssetAtPath<WeightContainer>(ENVIRONMENT_WEIGHT_ASSET_PATH),
                CardTypes.Creature => AssetDatabase.LoadAssetAtPath<WeightContainer>(CREATURE_WEIGHT_ASSET_PATH),
                CardTypes.Boss => AssetDatabase.LoadAssetAtPath<WeightContainer>(BOSS_WEIGHT_ASSET_PATH),
                CardTypes.Character_Hunter => AssetDatabase.LoadAssetAtPath<WeightContainer>(HUNTER_WEIGHT_ASSET_PATH),
                CardTypes.Gear_Equipment=> AssetDatabase.LoadAssetAtPath<WeightContainer>(GEAR_WEIGHT_ASSET_PATH),
                CardTypes.Gear_Upgrade => AssetDatabase.LoadAssetAtPath<WeightContainer>(GEAR_WEIGHT_ASSET_PATH),
                _ => AssetDatabase.LoadAssetAtPath<WeightContainer>(KEYWORD_ONLY_WEIGHT_ASSET_PATH)
            };*/
            // TODO: Need to refactor Weight system.
            return null;
        }
        
    }
}