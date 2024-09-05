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
        private WeightType _weightType;
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
            _weightType = (WeightType)EditorGUILayout.EnumPopup("Weight Type", _weightType,
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
             switch (_weightType)
            {
                case WeightType.None:
                case WeightType.Starship:
                        break;
                case WeightType.Environment:
                    DrawEnvironmentFields();
                    break;
                case WeightType.Keyword:
                    DrawKeywordOnlyFields();
                    break;
                case WeightType.Gear:
                case WeightType.Ally:
                case WeightType.Boss:
                case WeightType.Creature:
                case WeightType.Hunter:
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
            if (_weightType == WeightType.Hunter)
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

        private void SetWeightsAccordingToWeightType()
        {
            switch (_weightType)
            {
                case WeightType.Ally:
                    SaveWeightsToAssets(attributeSettings.allyCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Boss:
                    SaveWeightsToAssets(attributeSettings.bossCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Creature:
                    SaveWeightsToAssets(attributeSettings.creatureCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Environment:
                    SaveWeightsToAssets(attributeSettings.environmentCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Gear:
                    SaveWeightsToAssets(attributeSettings.gearCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Hunter:
                    SaveWeightsToAssets(attributeSettings.hunterCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Keyword:
                    SaveWeightsToAssets(attributeSettings.keywordOnlyCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Starship:
                case WeightType.None:
                    // No-op
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_weightType), _weightType, null);
            }
        }

        

        private void SaveWeightsToAssets(CardStatWeight[] weights)
        {
            weights[0].statWeight = _attackWeight;
            weights[1].statWeight = _exploreWeight;
            weights[2].statWeight = _focusWeight;
            weights[3].statWeight = _hitPointsWeight;
            weights[4].statWeight = _speedWeight;
            weights[5].statWeight = _upgradeSlotsWeight;
            weights[6].statWeight = _keywordsWeight;
        }
        
        private void SetWeightsFromAssets(CardStatWeight[] weights) 
        {
            _attackWeight = weights[0].statWeight;
            _exploreWeight = weights[1].statWeight;
            _focusWeight = weights[2].statWeight;
            _hitPointsWeight = weights[3].statWeight;
            _speedWeight = weights[4].statWeight;
            _upgradeSlotsWeight = weights[5].statWeight;
            _keywordsWeight = weights[6].statWeight;
        }
        
        private void SaveWeightData()
        {
            SetWeightsAccordingToWeightType();
            EditorUtility.SetDirty(attributeSettings);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void LoadWeightData()
        {
            switch (_weightType)
            {
                case WeightType.Ally:
                    SetWeightsFromAssets(attributeSettings.allyCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Boss:
                    SetWeightsFromAssets(attributeSettings.bossCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Creature:
                    SetWeightsFromAssets(attributeSettings.creatureCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Environment:
                    SetWeightsFromAssets(attributeSettings.environmentCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Gear:
                    SetWeightsFromAssets(attributeSettings.gearCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Hunter:
                    SetWeightsFromAssets(attributeSettings.hunterCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Keyword:
                    SetWeightsFromAssets(attributeSettings.keywordOnlyCardStatWeights.cardStatWeights);
                    break;
                case WeightType.Starship:
                case WeightType.None:
                    // No-op
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_weightType), _weightType, null);
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