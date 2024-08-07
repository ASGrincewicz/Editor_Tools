using System;
using System.Collections.Generic;
using UnityEditor;
namespace Editor
{
    [CustomEditor(typeof(CardSO))]
    public class CardSOInspector: UnityEditor.Editor
    {
        private const string CardTypePropertyName = "_cardType";
        private const string CardNamePropertyName = "_cardName";
        private const string ArtWorkPropertyName = "_artWork";
        private const string AttackStatPropertyName = "_attack";
        private const string HitPointsStatPropertyName = "_hitPoints";
        private const string SpeedStatPropertyName = "_speed";
        private const string FocusStatPropertyName = "_focus";
        private const string UpgradeSlotsPropertyName = "_upgradeSlots";
        private const string CardTextPropertyName = "_cardText";
        private SerializedProperty CardType;
        private SerializedProperty CardName;
        private SerializedProperty ArtWork;
        private SerializedProperty Attack;
        private SerializedProperty HitPoints;
        private SerializedProperty Speed;
        private SerializedProperty Focus;
        private SerializedProperty UpgradeSlots;
        private SerializedProperty CardText;
        private void OnEnable()
        {
            CardType = serializedObject.FindProperty(CardTypePropertyName);
            CardName = serializedObject.FindProperty(CardNamePropertyName);
            ArtWork = serializedObject.FindProperty(ArtWorkPropertyName);
            Attack = serializedObject.FindProperty(AttackStatPropertyName);
            HitPoints = serializedObject.FindProperty(HitPointsStatPropertyName);
            Speed = serializedObject.FindProperty(SpeedStatPropertyName);
            Focus = serializedObject.FindProperty(FocusStatPropertyName);
            UpgradeSlots = serializedObject.FindProperty(UpgradeSlotsPropertyName);
            CardText = serializedObject.FindProperty(CardTextPropertyName);
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(CardType);

            CardTypes cardTypes = (CardTypes)CardType.enumValueIndex;
            EditorGUILayout.PropertyField(CardName);
            EditorGUILayout.PropertyField(ArtWork);
            EditorGUILayout.PropertyField(CardText);
            switch (cardTypes)
            {
                case CardTypes.TBD:
                    break;
                case CardTypes.Action:
                    break;
                case CardTypes.Environment:
                    break;
                case CardTypes.Equipment:
                    break;
                case CardTypes.Boss:
                case CardTypes.Character:
                case CardTypes.Creature:
                case CardTypes.Hunter:
                    EditorGUILayout.PropertyField(Attack);
                    EditorGUILayout.PropertyField(HitPoints);
                    EditorGUILayout.PropertyField(Speed);
                    EditorGUILayout.PropertyField(Focus);
                    if (cardTypes == CardTypes.Hunter)
                    {
                        EditorGUILayout.PropertyField(UpgradeSlots);
                    }
                    break;
                case CardTypes.Upgrade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            
            
            
            

            serializedObject.ApplyModifiedProperties();
        }
    }
}