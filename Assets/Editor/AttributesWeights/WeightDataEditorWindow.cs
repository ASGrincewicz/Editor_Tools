using System;
using UnityEditor;
using UnityEngine;

namespace Editor.AttributesWeights
{
    public class WeightDataEditorWindow : EditorWindow
    {
        public AttributeSettings attributeSettings;

        private WeightType _weightType;
        private WeightAttributes _weightAttributes;

        private Rect _topAreaRect;
        private Rect _statInputAreaRect;
        private Rect _buttonAreaRect;
        private Rect _bottomAreaRect;

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
            _topAreaRect = new Rect(20, 5, position.width, position.height * 0.1f);
            _statInputAreaRect = new Rect(20, 50, position.width * 0.5f, position.height * 0.65f);
            _buttonAreaRect = new Rect(_statInputAreaRect.width + 50, 50, position.width * 0.33f, position.height * 0.75f);
            _bottomAreaRect = new Rect(20, _statInputAreaRect.height + 20, position.width * 0.66f, position.height * 0.25f);
        }

        private void DrawTopArea()
        {
            GUILayout.BeginArea(_topAreaRect);
            _weightType = (WeightType)EditorGUILayout.EnumPopup("Weight Type", _weightType, GUILayout.Width(300));
            GUILayout.EndArea();
        }

        private void DrawStatInputArea()
        {
            GUILayout.BeginArea(_statInputAreaRect);
            DrawStatFields();
            GUILayout.EndArea();
        }

        private void DrawButtonArea()
        {
            GUILayout.BeginArea(_buttonAreaRect);

            if (GUILayout.Button("Load Data", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                ResetWeights();
                LoadWeightData();
            }
            if (GUILayout.Button("Save Data", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                SaveWeightData();
            }
            if (GUILayout.Button("Reset Values", GUILayout.Width(_buttonAreaRect.width * 0.75f), GUILayout.Height(50)))
            {
                ResetWeights();
            }

            GUILayout.EndArea();
        }

        private void DrawBottomArea()
        {
            GUILayout.BeginArea(_bottomAreaRect);
            attributeSettings = EditorGUILayout.ObjectField("Attribute Settings", attributeSettings, typeof(AttributeSettings), false) as AttributeSettings;
            GUILayout.EndArea();
        }

        private void DrawStatFields()
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
                    DrawCharacterFields();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DrawEnvironmentFields()
        {
            DrawFloatField("Explore Weight", ref _weightAttributes.ExploreWeight);
            GUILayout.Space(5);
            DrawFloatField("Keywords Weight", ref _weightAttributes.KeywordsWeight);
        }

        private void DrawKeywordOnlyFields()
        {
            DrawFloatField("Keywords Weight", ref _weightAttributes.KeywordsWeight);
        }

        private void DrawCharacterFields()
        {
            DrawFloatField("Attack Weight", ref _weightAttributes.AttackWeight);
            GUILayout.Space(5);
            DrawFloatField("Focus Weight", ref _weightAttributes.FocusWeight);
            GUILayout.Space(5);
            DrawFloatField("Hit Points Weight", ref _weightAttributes.HitPointsWeight);
            GUILayout.Space(5);
            DrawFloatField("Speed Weight", ref _weightAttributes.SpeedWeight);
            GUILayout.Space(5);
            if (_weightType == WeightType.Hunter)
            {
                DrawFloatField("Upgrade Slots Weight", ref _weightAttributes.UpgradeSlotsWeight);
                GUILayout.Space(5);
            }
            DrawFloatField("Keywords Weight", ref _weightAttributes.KeywordsWeight);
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
                    AttackWeight = weights[0].statWeight,
                    ExploreWeight = weights[1].statWeight,
                    FocusWeight = weights[2].statWeight,
                    HitPointsWeight = weights[3].statWeight,
                    SpeedWeight = weights[4].statWeight,
                    UpgradeSlotsWeight = weights[5].statWeight,
                    KeywordsWeight = weights[6].statWeight
                };
            }
        }

        private void SaveWeightData()
        {
            CardStatWeight[] weights = GetWeightsByType();
            if (weights != null)
            {
                weights[0].statWeight = _weightAttributes.AttackWeight;
                weights[1].statWeight = _weightAttributes.ExploreWeight;
                weights[2].statWeight = _weightAttributes.FocusWeight;
                weights[3].statWeight = _weightAttributes.HitPointsWeight;
                weights[4].statWeight = _weightAttributes.SpeedWeight;
                weights[5].statWeight = _weightAttributes.UpgradeSlotsWeight;
                weights[6].statWeight = _weightAttributes.KeywordsWeight;

                foreach (CardStatWeight weight in weights)
                {
                    EditorUtility.SetDirty(weight);
                }

                EditorUtility.SetDirty(attributeSettings);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private CardStatWeight[] GetWeightsByType()
        {
            return _weightType switch
            {
                WeightType.Ally => attributeSettings.allyCardStatWeights.cardStatWeights,
                WeightType.Boss => attributeSettings.bossCardStatWeights.cardStatWeights,
                WeightType.Creature => attributeSettings.creatureCardStatWeights.cardStatWeights,
                WeightType.Environment => attributeSettings.environmentCardStatWeights.cardStatWeights,
                WeightType.Gear => attributeSettings.gearCardStatWeights.cardStatWeights,
                WeightType.Hunter => attributeSettings.hunterCardStatWeights.cardStatWeights,
                WeightType.Keyword => attributeSettings.keywordOnlyCardStatWeights.cardStatWeights,
                _ => null,
            };
        }

        private struct WeightAttributes
        {
            public float AttackWeight;
            public float ExploreWeight;
            public float FocusWeight;
            public float HitPointsWeight;
            public float SpeedWeight;
            public float UpgradeSlotsWeight;
            public float KeywordsWeight;
        }
    }
}