using System;
using System.Linq;
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
                    DrawKeywordOnlyFields();
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
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
                    SetCardStatWeight(attributeSettings.keywordOnlyCardStatWeights, _keywordsWeight, 0);
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                   SetGearCardStatWeights();
                    break;
                case CardTypes.Boss:
                    SetBossCardStatWeights();
                    break;
                case CardTypes.Character_Ally:
                    SetCharacterAllyStatWeights();
                    break;
                case CardTypes.Character_Hunter:
                    SetHunterCardStatWeights();
                    break;
                case CardTypes.Creature:
                    SetCreatureCardStatWeights();
                    break;
                case CardTypes.Environment:
                    SetEnvironmentCardStatWeights();
                    break;
                case CardTypes.Starship:
                case CardTypes.TBD:
                    // No-op
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_cardType), _cardType, null);
            }
        }

        private void SetCardStatWeight(CardStatWeight[] cardStatWeights, float weight, int index)
        {
            cardStatWeights[index].statWeight = weight;
        }

        private void SetMultipleCardStatWeights(CardStatWeight[] cardStatWeights, params (float weight, int index)[] weights)
        {
            foreach ((float weight, int index) in weights)
            {
                SetCardStatWeight(cardStatWeights, weight, index);
            }
        }

        private void SetBossCardStatWeights()
        {
            SetMultipleCardStatWeights(attributeSettings.bossCardStatWeights,
                (_attackWeight, 0),
                (_focusWeight, 1),
                (_hitPointsWeight, 2),
                (_speedWeight, 3),
                (_keywordsWeight, 4)
            );
        }

        private void SetCharacterAllyStatWeights()
        {
            SetMultipleCardStatWeights(attributeSettings.allyCardStatWeights,
                (_attackWeight, 0),
                (_focusWeight, 1),
                (_hitPointsWeight, 2),
                (_speedWeight, 3),
                (_keywordsWeight, 4)
            );
        }

        private void SetHunterCardStatWeights()
        {
            SetMultipleCardStatWeights(attributeSettings.hunterCardStatWeights,
                (_attackWeight, 0),
                (_focusWeight, 1),
                (_hitPointsWeight, 2),
                (_speedWeight, 3),
                (_keywordsWeight, 4),
                (_upgradeSlotsWeight, 5)
            );
        }

        private void SetCreatureCardStatWeights()
        {
            SetMultipleCardStatWeights(attributeSettings.creatureCardStatWeights,
                (_attackWeight, 0),
                (_focusWeight, 1),
                (_hitPointsWeight, 2),
                (_speedWeight, 3),
                (_keywordsWeight, 4)
            );
        }

        private void SetEnvironmentCardStatWeights()
        {
            SetMultipleCardStatWeights(attributeSettings.environmentCardStatWeights,
                (_exploreWeight, 0),
                (_keywordsWeight, 1)
            );
        }

        private void SetGearCardStatWeights()
        {
            SetMultipleCardStatWeights(attributeSettings.gearCardStatWeights,
                (_attackWeight, 0),
                (_focusWeight, 1),
                (_hitPointsWeight, 2),
                (_speedWeight, 3),
                (_keywordsWeight, 4)
            );
        }
        private void SetWeights(float[] weights) 
        {
            _attackWeight = weights[0];
            _focusWeight = weights[1];
            _hitPointsWeight = weights[2];
            _speedWeight = weights[3];
            _keywordsWeight = weights[4];
            if (weights.Length > 5) 
            {
                _upgradeSlotsWeight = weights[5];
            }
        }

        private void LoadWeightData()
        {
            switch (_cardType)
            {
                case CardTypes.Action:
                    _keywordsWeight = attributeSettings.keywordOnlyCardStatWeights[0].statWeight;
                    break;
                case CardTypes.Gear_Equipment:
                case CardTypes.Gear_Upgrade:
                    SetWeights(attributeSettings.gearCardStatWeights.Select(w => w.statWeight).ToArray());
                    break;
                case CardTypes.Boss:
                    SetWeights(attributeSettings.bossCardStatWeights.Select(w => w.statWeight).ToArray());
                    break;
                case CardTypes.Character_Ally:
                    SetWeights(attributeSettings.allyCardStatWeights.Select(w => w.statWeight).ToArray());
                    break;
                case CardTypes.Character_Hunter:
                    SetWeights(attributeSettings.hunterCardStatWeights.Select(w => w.statWeight).ToArray());
                    break;
                case CardTypes.Creature:
                    SetWeights(attributeSettings.creatureCardStatWeights.Select(w => w.statWeight).ToArray());
                    break;
                case CardTypes.Environment:
                    _exploreWeight = attributeSettings.environmentCardStatWeights[0].statWeight;
                    _keywordsWeight = attributeSettings.environmentCardStatWeights[1].statWeight;
                    break;
                case CardTypes.Starship:
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