using System.Collections.Generic;
using Editor.CardData;
using Editor.CardData.CardTypes;
using Editor.CardData.Stats;
using Editor.KeywordSystem;
using UnityEngine;

namespace Editor.Utilities
{
    public interface ICardDataAssetUtility
    {
        List<CardDataSO> AllCardData { get; }
        KeywordManager KeywordManager { get; set; }
        string[] CardAssetGuids { get; }
        CardTypeDataSO CardTypeData { get; set; }
        string CardName { get; set; }
        Texture2D Artwork { get; set; }
        string CardText { get; set; }
        int CardCost { get; set; }
        void LoadKeywordManagerAsset();
        void RefreshKeywordsList();
        void CreateNewCard(List<CardStat> newStats);
        void SaveExistingCard(List<CardStat> stats);
        void UnloadCard();
        void LoadCardFromFile();
    }
}