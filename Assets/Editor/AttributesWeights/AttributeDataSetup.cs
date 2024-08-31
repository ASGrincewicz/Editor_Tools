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
            
            if (GUILayout.Button("Load Data", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                ResetLocalWeightData();
                LoadWeightData();
            }
            if (GUILayout.Button("Save Data", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
               ValidateWeightInput();
            }
            if (GUILayout.Button("Reset Values", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                ResetLocalWeightData();
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
                case CardTypes.Starship:
                        break;
                case CardTypes.Environment:
                    DrawEnvironmentFields();
                    break;
                case CardTypes.Action:
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                    DrawKeywordOnlyFields();
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

        private void DrawKeywordOnlyFields()
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

        private void ResetLocalWeightData()
        {
            _attackWeight = 0;
            _exploreWeight = 0;
            _focusWeight = 0;
            _hitPointsWeight = 0;
            _speedWeight = 0;
            _upgradeSlotsWeight = 0;
            _keywordsWeight = 0;
        }

        private void SaveWeightData()
        {
            switch (_cardType)
            {
                case CardTypes.Action:
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                    attributeSettings.keywordOnlyCardStatWeights[0].statWeight = _keywordsWeight;
                    break;
                case CardTypes.Boss:
                    attributeSettings.bossCardStatWeights[0].statWeight = _attackWeight;
                    attributeSettings.bossCardStatWeights[1].statWeight = _focusWeight;
                    attributeSettings.bossCardStatWeights[2].statWeight = _hitPointsWeight;
                    attributeSettings.bossCardStatWeights[3].statWeight = _speedWeight;
                    attributeSettings.bossCardStatWeights[4].statWeight = _keywordsWeight;
                    break;
                case CardTypes.Character_Ally:
                    attributeSettings.allyCardStatWeights[0].statWeight = _attackWeight;
                    attributeSettings.allyCardStatWeights[1].statWeight = _focusWeight;
                    attributeSettings.allyCardStatWeights[2].statWeight = _hitPointsWeight;
                    attributeSettings.allyCardStatWeights[3].statWeight = _speedWeight;
                    attributeSettings.allyCardStatWeights[4].statWeight = _keywordsWeight;
                    break;
                case CardTypes.Character_Hunter:
                    attributeSettings.hunterCardStatWeights[0].statWeight = _attackWeight;
                    attributeSettings.hunterCardStatWeights[1].statWeight = _focusWeight;
                    attributeSettings.hunterCardStatWeights[2].statWeight = _hitPointsWeight;
                    attributeSettings.hunterCardStatWeights[3].statWeight = _speedWeight;
                    attributeSettings.hunterCardStatWeights[4].statWeight = _upgradeSlotsWeight;
                    attributeSettings.hunterCardStatWeights[5].statWeight = _keywordsWeight;
                    break;
                case CardTypes.Creature:
                    attributeSettings.creatureCardStatWeights[0].statWeight = _attackWeight;
                    attributeSettings.creatureCardStatWeights[1].statWeight = _focusWeight;
                    attributeSettings.creatureCardStatWeights[2].statWeight = _hitPointsWeight;
                    attributeSettings.creatureCardStatWeights[3].statWeight = _speedWeight;
                    attributeSettings.creatureCardStatWeights[4].statWeight = _keywordsWeight;
                    break;
                case CardTypes.Environment:
                    attributeSettings.environmentCardStatWeights[0].statWeight = _exploreWeight;
                    attributeSettings.environmentCardStatWeights[1].statWeight = _keywordsWeight;
                    break;
                case CardTypes.Starship:
                    break;
                case CardTypes.TBD:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void LoadWeightData()
        {
             switch (_cardType)
            {
                case CardTypes.Action:
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                    _keywordsWeight = attributeSettings.keywordOnlyCardStatWeights[0].statWeight;
                    break;
                case CardTypes.Boss:
                    _attackWeight = attributeSettings.bossCardStatWeights[0].statWeight;
                    _focusWeight = attributeSettings.bossCardStatWeights[1].statWeight;
                    _hitPointsWeight = attributeSettings.bossCardStatWeights[2].statWeight;
                    _speedWeight = attributeSettings.bossCardStatWeights[3].statWeight;
                    _keywordsWeight = attributeSettings.bossCardStatWeights[4].statWeight;
                    break;
                case CardTypes.Character_Ally:
                    _attackWeight = attributeSettings.allyCardStatWeights[0].statWeight;
                    _focusWeight = attributeSettings.allyCardStatWeights[1].statWeight;
                    _hitPointsWeight = attributeSettings.allyCardStatWeights[2].statWeight;
                    _speedWeight = attributeSettings.allyCardStatWeights[3].statWeight;
                    _keywordsWeight = attributeSettings.allyCardStatWeights[4].statWeight;
                    break;
                case CardTypes.Character_Hunter:
                    _attackWeight = attributeSettings.hunterCardStatWeights[0].statWeight;
                    _focusWeight = attributeSettings.hunterCardStatWeights[1].statWeight;
                    _hitPointsWeight = attributeSettings.hunterCardStatWeights[2].statWeight;
                    _speedWeight = attributeSettings.hunterCardStatWeights[3].statWeight;
                    _upgradeSlotsWeight = attributeSettings.hunterCardStatWeights[4].statWeight;
                    _keywordsWeight = attributeSettings.hunterCardStatWeights[5].statWeight;
                    break;
                case CardTypes.Creature:
                    _attackWeight = attributeSettings.creatureCardStatWeights[0].statWeight;
                    _focusWeight = attributeSettings.creatureCardStatWeights[1].statWeight;
                    _hitPointsWeight = attributeSettings.creatureCardStatWeights[2].statWeight;
                    _speedWeight = attributeSettings.creatureCardStatWeights[3].statWeight;
                    _keywordsWeight = attributeSettings.creatureCardStatWeights[4].statWeight;
                    break;
                case CardTypes.Environment:
                    _exploreWeight = attributeSettings.environmentCardStatWeights[0].statWeight;
                    _keywordsWeight = attributeSettings.environmentCardStatWeights[1].statWeight;
                    break;
                case CardTypes.Starship:
                    break;
                case CardTypes.TBD:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DisplayMessage(string message)
        {
            Debug.Log(message);
        }

        private bool SumWeights()
        {
            float sum = _attackWeight + _exploreWeight + _focusWeight + _hitPointsWeight + _speedWeight +
                        _upgradeSlotsWeight + _keywordsWeight;
            return Math.Abs(1.0 - sum) < 0.001;
        }

        private void ValidateWeightInput()
        {
            if (SumWeights())
            {
                SaveWeightData();
                DisplayMessage("All Good!");
                ResetLocalWeightData();
            }
            else
            {
                DisplayMessage("The total value of all weights must be 1.0!");
            }
        }
        // need to send CardStatWeight(name, weight) to AttributeWeightsData with the CardType
        // Card Types should already be populated in the Attribute Settings Asset
        // settings.attributeData
    }
}