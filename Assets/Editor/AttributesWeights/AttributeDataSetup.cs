using System;
using Editor.CardEditor;
using UnityEditor;
using UnityEngine;

namespace Editor.AttributesWeights
{
    public class AttributeDataSetup : EditorWindow
    {
        public AttributeSettings attributeSettings;
        // Class variables
        private CardTypes _cardType;
        private float _attackWeight = 0.0f;
        private float _exploreWeight = 0.0f;
        private float _focusWeight = 0.0f;
        private float _hitPointsWeight = 0.0f;
        private float _keywordsWeight = 0.0f;
        private float _speedWeight = 0.0f;
        private float _upgradeSlotsWeight = 0.0f;
        
        // GUI variables
        private Rect _topAreaRect;
        private Rect _statInputAreaRect;
        private Rect _buttonAreaRect;
        private Rect _bottomAreaRect;
        
        [MenuItem("Tools/Utilities/Attribute Data Setup")]
        private static void ShowWindow()
        {
            AttributeDataSetup window = GetWindow<AttributeDataSetup>();
            window.titleContent = new GUIContent("Attribute Data Setup");
            window.position = new Rect(50, 50, 400, 300);
            window.Show();
        }

        private void OnGUI()
        {
            SetupAreaRects();
            DrawTopArea();
            DrawStatInputArea();
            DrawButtonArea();
            DrawBottomArea();
        }
        private void SetupAreaRects()
        {
            _topAreaRect = new Rect(20, 5, position.width, position.height * 0.1f);
            _statInputAreaRect = new Rect(20, 50, position.width * 0.5f, position.height * 0.65f);
            _buttonAreaRect = new Rect(_statInputAreaRect.width + 50,50,
                position.width * 0.33f, position.height * 0.75f);
            _bottomAreaRect = new Rect(20, _statInputAreaRect.height + 20, position.width * 0.66f,
                position.height * 0.25f);
        }

        private void DrawTopArea()
        {
            GUILayout.BeginArea(_topAreaRect);
            DrawEnumPopup();
            GUILayout.EndArea();
        }

        private void DrawEnumPopup()
        {
            _cardType = (CardTypes)EditorGUILayout.EnumPopup("Card Type", _cardType,
                GUILayout.Width(300));
        }

        private void DrawStatInputArea()
        {
            GUILayout.BeginArea(_statInputAreaRect);
            DrawEditableFields();
            GUILayout.EndArea();
        }

        private void DrawButtonArea()
        {
            GUILayout.BeginArea(_buttonAreaRect);
            if (GUILayout.Button("Save Data", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                // Save to Settings Asset
            }
            GUILayout.Space(50);
            if (GUILayout.Button("Show Grid", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                // Save to Settings Asset
            }
            GUILayout.EndArea();
        }

        private void DrawBottomArea()
        {
            GUILayout.BeginArea(_bottomAreaRect);
            attributeSettings =
                EditorGUILayout.ObjectField("Attribute Settings", attributeSettings, typeof(AttributeSettings),
                        false) as
                    AttributeSettings;
            GUILayout.EndArea();
        }

        private void DrawEditableFields()
        {
             switch (_cardType)
            {
                case CardTypes.TBD:
                        break;
                case CardTypes.Environment:
                    DrawEnvironmentFields();
                    break;
                case CardTypes.Action:
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                    DrawActionEquipmentUpgradeFields();
                    break;
                case CardTypes.Character_Ally:
                case CardTypes.Character_Hunter:
                case CardTypes.Creature:
                case CardTypes.Boss:
                    DrawCharacterLikeFields();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
}

        private void DrawEnvironmentFields()
        {
            DrawFloatField("Explore Weight", ref _exploreWeight);
            GUILayout.Space(5);
            DrawFloatField("Keywords Weight", ref _keywordsWeight);
        }

        private void DrawActionEquipmentUpgradeFields()
        {
            DrawFloatField("Keywords Weight", ref _keywordsWeight);
        }

        private void DrawCharacterLikeFields()
        {
            DrawFloatField("Attack Weight", ref _attackWeight);
            GUILayout.Space(5);
            DrawFloatField("Focus Weight", ref _focusWeight);
            GUILayout.Space(5);
            DrawFloatField("Hit Points Weight", ref _hitPointsWeight);
            GUILayout.Space(5);
            DrawFloatField("Speed Weight", ref _speedWeight);
            GUILayout.Space(5);
            if (_cardType == CardTypes.Character_Hunter)
            {
                DrawFloatField("Upgrade Slots Weight", ref _upgradeSlotsWeight);
                GUILayout.Space(5);
            }
            DrawFloatField("Keywords Weight", ref _keywordsWeight);
        }

        private void DrawFloatField(string label, ref float value)
        {
            value = EditorGUILayout.FloatField(label, value, GUILayout.Width(200));
           
        }
        
        // Send Data To AttributeSettings Asset
        // need to send CardStatWeight(name, weight) to AttributeWeightsData with the CardType
        // Card Types should already be populated in the Attribute Settings Asset
        // settings.attributeData
    }
}