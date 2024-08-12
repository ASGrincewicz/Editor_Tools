using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor.InteractionMatrix
{
    public class InteractionGridWindow : EditorWindow
    {
        public InteractionGridSettings settings;

        private const float AxisLabelWidth = 115.0f;
        private const float ColumnWidth = 100.0f;
        private const float Indent = 50.0f;
        private const float TitleHeight = 20.0f;

        private StringBuilder _message = new StringBuilder();

        [MenuItem("Tools/Interaction Grid")]
        public static void ShowWindow()
        {
            InteractionGridWindow window = GetWindow<InteractionGridWindow>("Interaction Matrix");
            window.position = new Rect(100f, 100f, 900f, 300f);
            window.Show();
        }

        private void OnGUI()
        {
            ValidateOrCreateGridData();

            DisplayTitle();
            DisplayLabels();
            DisplayGrid();
            DisplayValidationButton();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(settings);
            }

            GUILayout.Label($"Message: {_message}", EditorStyles.whiteLabel);
        }

        private void ValidateOrCreateGridData()
        {
            if (settings.gridData == null || settings.gridData.size != settings.labels.Length)
            {
                settings.gridData = new GridData
                {
                    size = settings.labels.Length,
                    gridValues = new int[settings.labels.Length * settings.labels.Length]
                };
            }
        }

        private void DisplayTitle()
        {
            GUILayout.Space(TitleHeight);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(settings.title, EditorStyles.largeLabel, GUILayout.Height(TitleHeight));
            EditorGUILayout.EndHorizontal();
        }

        private void DisplayLabels()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(Indent);

            EditorGUILayout.LabelField($"{settings.xAxisLabel}\\{settings.yAxisLabel}", EditorStyles.boldLabel, GUILayout.Width(AxisLabelWidth));
            foreach (string label in settings.labels)
            {
                GUILayout.Label(label, GUILayout.Width(ColumnWidth));
            }

            EditorGUILayout.EndHorizontal();
        }

        private void DisplayGrid()
        {
            for (int i = 0; i < settings.labels.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(string.Empty, GUILayout.Width(Indent));
                GUILayout.Label(settings.labels[i], GUILayout.Width(ColumnWidth)); // Row label

                for (int j = 0; j < settings.labels.Length; j++)
                {
                    int index = i * settings.gridData.size + j;
                    settings.gridData.gridValues[index] = EditorGUILayout.Popup(settings.gridData.gridValues[index], settings.options, GUILayout.Width(ColumnWidth));
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DisplayValidationButton()
        {
            if (!GUILayout.Button("Validate Grid", GUILayout.Width(200))) return;

            ValidateGrid();
        }

        private void ValidateGrid()
        {
            _message.Clear();
            int size = settings.gridData.size;
            for (int i = 0; i < size; i++)
            {
                (int countZero, int countTwo) = CountOptionsInRow(i);
                if (countZero != settings.optionZeroAmount || countTwo != settings.optionTwoAmount)
                {
                    _message.Append($"{settings.labels[i]} does not have exactly {settings.optionZeroAmount} {settings.options[0]} and {settings.optionTwoAmount} {settings.options[2]}!");
                    return;
                }
            }
            _message.Append("Everything looks good!");
        }

        private (int countZero, int countTwo) CountOptionsInRow(int rowIndex)
        {
            int countZero = 0;
            int countTwo = 0;

            for (int j = 0; j < settings.gridData.size; j++)
            {
                int index = rowIndex * settings.gridData.size + j;
                if (settings.gridData.gridValues[index] == 0) countZero++;
                if (settings.gridData.gridValues[index] == 2) countTwo++;
            }

            return (countZero, countTwo);
        }
    }
}