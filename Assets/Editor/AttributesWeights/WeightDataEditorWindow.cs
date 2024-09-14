using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Editor.AttributesWeights
{
    public class WeightDataEditorWindow : EditorWindow
    {
        [FormerlySerializedAs("attributeSettings")] public AttributeSettings _attributeSettings;

        private WeightType WeightType { get; set; }
        private WeightAttributes _weightAttributes;

        private Rect TopAreaRect { get; set; }
        private Rect StatInputAreaRect { get; set; }
        private Rect ButtonAreaRect { get; set; }
        private Rect BottomAreaRect { get; set; }
        
        
        [MenuItem("Tools/Utilities/Stat Weight Data Setup")]
        private static void ShowWindow()
        {
            WeightDataEditorWindow window = GetWindow<WeightDataEditorWindow>();
            window.titleContent = new GUIContent("Stat Weight Data Setup");
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
            TopAreaRect = new Rect(20, 5, position.width, position.height * 0.1f);
            StatInputAreaRect = new Rect(20, 50, position.width * 0.5f, position.height * 0.65f);
            ButtonAreaRect = new Rect(StatInputAreaRect.width + 50, 50, position.width * 0.33f, position.height * 0.75f);
            BottomAreaRect = new Rect(20, StatInputAreaRect.height + 20, position.width * 0.66f, position.height * 0.25f);
        }

        private void DrawTopArea()
        {
            GUILayout.BeginArea(TopAreaRect);
            WeightType = (WeightType)EditorGUILayout.EnumPopup("Weight Type", WeightType, GUILayout.Width(300));
            GUILayout.EndArea();
        }

        private void DrawStatInputArea()
        {
            GUILayout.BeginArea(StatInputAreaRect);
            DrawStatFields();
            GUILayout.EndArea();
        }

        private void DrawButtonArea()
        {
            GUILayout.BeginArea(ButtonAreaRect);

            if (GUILayout.Button("Load Data", GUILayout.Width(ButtonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                ResetWeights();
                LoadWeightData();
            }
            if (GUILayout.Button("Save Data", GUILayout.Width(ButtonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                SaveWeightData();
            }
            if (GUILayout.Button("Reset Values", GUILayout.Width(ButtonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                ResetWeights();
            }

            GUILayout.EndArea();
        }

        private void DrawBottomArea()
        {
            GUILayout.BeginArea(BottomAreaRect);
            _attributeSettings = EditorGUILayout.ObjectField("Attribute Settings", _attributeSettings, typeof(AttributeSettings), false) as AttributeSettings;
            GUILayout.EndArea();
        }

        private void DrawStatFields()
        {
            try
            {
                switch (WeightType)
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
                        DrawCharacterFields();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (ArgumentOutOfRangeException exception)
            {
                Debug.LogException(exception);
            }
            
        }

        private void DrawEnvironmentFields()
        {
            DrawFloatField("Explore Weight", ref _weightAttributes.exploreWeight);
            GUILayout.Space(5);
            DrawFloatField("Keywords Weight", ref _weightAttributes.keywordsWeight);
        }

        private void DrawKeywordOnlyFields()
        {
            DrawFloatField("Keywords Weight", ref _weightAttributes.keywordsWeight);
        }

        private void DrawCharacterFields()
        {
            DrawFloatField("Attack Weight", ref _weightAttributes.attackWeight);
            GUILayout.Space(5);
            DrawFloatField("Focus Weight", ref _weightAttributes.focusWeight);
            GUILayout.Space(5);
            DrawFloatField("Hit Points Weight", ref _weightAttributes.hitPointsWeight);
            GUILayout.Space(5);
            DrawFloatField("Speed Weight", ref _weightAttributes.speedWeight);
            GUILayout.Space(5);
            if (WeightType == WeightType.Hunter)
            {
                DrawFloatField("Upgrade Slots Weight", ref _weightAttributes.upgradeSlotsWeight);
                GUILayout.Space(5);
            }
            DrawFloatField("Keywords Weight", ref _weightAttributes.keywordsWeight);
        }

        private void DrawFloatField(string label, ref float value)
        {
            value = EditorGUILayout.FloatField(label, value, GUILayout.Width(200));
        }

        private void ResetWeights()
        {
            _weightAttributes = new WeightAttributes();
        }

        private void LoadWeightData()
        {
            CardStatWeight[] weights = GetWeightsByType();
            if (weights != null)
            {
                _weightAttributes = new WeightAttributes
                {
                    attackWeight = weights[0].statWeight,
                    exploreWeight = weights[1].statWeight,
                    focusWeight = weights[2].statWeight,
                    hitPointsWeight = weights[3].statWeight,
                    speedWeight = weights[4].statWeight,
                    upgradeSlotsWeight = weights[5].statWeight,
                    keywordsWeight = weights[6].statWeight
                };
            }
        }

        private void SaveWeightData()
        {
            CardStatWeight[] weights = GetWeightsByType();
            if (weights != null)
            {
                weights[0].statWeight = _weightAttributes.attackWeight;
                weights[1].statWeight = _weightAttributes.exploreWeight;
                weights[2].statWeight = _weightAttributes.focusWeight;
                weights[3].statWeight = _weightAttributes.hitPointsWeight;
                weights[4].statWeight = _weightAttributes.speedWeight;
                weights[5].statWeight = _weightAttributes.upgradeSlotsWeight;
                weights[6].statWeight = _weightAttributes.keywordsWeight;

                foreach (CardStatWeight weight in weights)
                {
                    EditorUtility.SetDirty(weight);
                }

                EditorUtility.SetDirty(_attributeSettings);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private CardStatWeight[] GetWeightsByType()
        {
            return WeightType switch
            {
                WeightType.Ally => _attributeSettings.allyCardStatWeights.cardStatWeights,
                WeightType.Boss => _attributeSettings.bossCardStatWeights.cardStatWeights,
                WeightType.Creature => _attributeSettings.creatureCardStatWeights.cardStatWeights,
                WeightType.Environment => _attributeSettings.environmentCardStatWeights.cardStatWeights,
                WeightType.Gear => _attributeSettings.gearCardStatWeights.cardStatWeights,
                WeightType.Hunter => _attributeSettings.hunterCardStatWeights.cardStatWeights,
                WeightType.Keyword => _attributeSettings.keywordOnlyCardStatWeights.cardStatWeights,
                _ => Array.Empty<CardStatWeight>()
            };
        }

        private struct WeightAttributes
        {
            public float attackWeight;
            public float exploreWeight;
            public float focusWeight;
            public float hitPointsWeight;
            public float speedWeight;
            public float upgradeSlotsWeight;
            public float keywordsWeight;
        }
    }
}