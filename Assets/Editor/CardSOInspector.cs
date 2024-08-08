using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            CardSO card = (CardSO)target;
            serializedObject.Update();
            GUILayout.BeginVertical(GUILayout.Width(300));
            EditorGUIUtility.labelWidth = 80;
            if (GUILayout.Button("Open in Card Editor"))
            {
                CardEditor instance = (CardEditor)EditorWindow.GetWindow(typeof(CardEditor));
                instance.OpenCardInEditor(card);
            }
            EditorGUILayout.PropertyField(CardType);

            CardTypes cardTypes = (CardTypes)CardType.enumValueIndex;
            EditorGUILayout.PropertyField(CardName);
            ArtWork.objectReferenceValue = EditorGUILayout.ObjectField("Artwork",ArtWork.objectReferenceValue, typeof(Texture2D),false);
            
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
                    EditorGUILayout.PropertyField(Attack, GUILayout.Height(40));
                    EditorGUILayout.Space(10, true);
                    EditorGUILayout.PropertyField(HitPoints, GUILayout.Height(40));
                    EditorGUILayout.Space(10, true);
                    EditorGUILayout.PropertyField(Speed, GUILayout.Height(40));
                    EditorGUILayout.Space(10, true);
                    EditorGUILayout.PropertyField(Focus, GUILayout.Height(40));
                    EditorGUILayout.Space(10, true);
                    if (cardTypes == CardTypes.Hunter)
                    {
                        EditorGUILayout.PropertyField(UpgradeSlots, GUILayout.Height(40));
                        EditorGUILayout.Space(10, true);
                    }
                    break;
                case CardTypes.Upgrade:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
           
            if (ArtWork.objectReferenceValue is Texture2D artworkTexture)
            {
                GUILayout.Label("Artwork Preview:");
                Rect rect = GUILayoutUtility.GetRect(400 * 0.75f,225 * 0.75f);
                EditorGUI.DrawPreviewTexture(rect, artworkTexture);
                EditorGUILayout.LabelField(artworkTexture.name);
            }
            GUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}