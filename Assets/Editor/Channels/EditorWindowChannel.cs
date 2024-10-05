using System;
using Editor.CardData;
using Editor.CardEditor;
using Editor.CostCalculator;
using Editor.KeywordSystem;
using Editor.SetDesigner;
using UnityEditor;
using UnityEngine;

namespace Editor.Channels
{
    [CreateAssetMenu(fileName = "Card Editor Channel", menuName = "Channels/Card Editor Channel", order = 0)]
    public class EditorWindowChannel : ScriptableObject
    {
        public Action<CardDataSO> OnCardEditorWindowRequested;
        public Action<CardDataSO> OnCostCalculatorWindowRequested;
        public Action OnKwywordEditorWindowRequested;
        public Action OnCardSetEditorWindowRequested;

        public void RaiseCardEditorWindowRequestedEvent(CardDataSO cardData)
        {
            EditorWindow.GetWindow<CardEditorWindow>();
            OnCardEditorWindowRequested?.Invoke(cardData);
        }

        public void RaiseCostCalculatorWindowRequestedEvent(CardDataSO cardData)
        {
            EditorWindow.GetWindow<CostCalculatorWindow>();
            OnCostCalculatorWindowRequested?.Invoke(cardData);
        }

        public void RaiseKeyWordEditorWindowRequestedEvent()
        {
            EditorWindow.GetWindow<KeywordEditorWindow>();
            OnKwywordEditorWindowRequested?.Invoke();
        }

        public void RaiseCardSetEditorWindowRequestedEvent()
        {
            EditorWindow.GetWindow<CardSetEditorWindow>();
            OnCardSetEditorWindowRequested?.Invoke();
        }
    }
}