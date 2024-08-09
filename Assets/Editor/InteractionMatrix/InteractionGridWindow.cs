using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor.InteractionMatrix
{
    public class InteractionGridWindow : EditorWindow
    {
        public InteractionGridSettings settings;
        private const float AXIS_LABEL_WIDTH = 115.0f;
        private const float COL_WIDTH = 100.0f;
        private const float INDENT = 50.0f;
        private const float TITLE_HEIGHT = 20.0f;
        private StringBuilder _message = new StringBuilder();

        [MenuItem("Tools/Interaction Grid")]
        public static void Init()
        {
            EditorWindow window = GetWindow<InteractionGridWindow>("Interaction Matrix");
            window.position = new Rect(100f, 100f, 900f, 300f);
            window.Show();
        }

        private void OnGUI()
        {
            // check if gridData has been initialized, if not - initialize it.
            if (ReferenceEquals(settings.gridData, null) || settings.gridData.size != settings.labels.Length)
            {
                settings.gridData = new GridData
                {
                    size = settings.labels.Length,
                    gridValues = new int[settings.labels.Length * settings.labels.Length]
                };
            }

            GUILayout.Space(TITLE_HEIGHT);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(settings.title, EditorStyles.largeLabel, GUILayout.Height(TITLE_HEIGHT));
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(INDENT);
            
            EditorGUILayout.LabelField($"{settings.xAxisLabel}\\{settings.yAxisLabel}", EditorStyles.boldLabel,  GUILayout.Width(AXIS_LABEL_WIDTH));
            for (int j = 0; j < settings.labels.Length; j++)
            {
                GUILayout.Label(settings.labels[j], GUILayout.Width(COL_WIDTH));
            }
            EditorGUILayout.EndHorizontal();
        
            for (int i = 0; i < settings.labels.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("", GUILayout.Width(INDENT));
                GUILayout.Label(settings.labels[i], GUILayout.Width(COL_WIDTH)); // Row label
               
                for (int j = 0; j < settings.labels.Length; j++)
                {
                    int index = i * settings.gridData.size + j;
                    settings.gridData.gridValues[index] = EditorGUILayout.Popup(settings.gridData.gridValues[index], settings.options, GUILayout.Width(COL_WIDTH));
                }
                EditorGUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Validate Grid", GUILayout.Width(200)))
            {
                ValidateGrid();
            }
            GUILayout.Label($"Message: {_message}", EditorStyles.whiteLargeLabel);
        }
        
        private void ValidateGrid()
        {
            _message.Clear();
            int size = settings.gridData.size;
            for (int i = 0; i < size; i++)
            {
                int countZero = 0;
                int countTwo = 0;

                // Validate row
                for (int j = 0; j < size; j++)
                {
                    int index = i * size + j;
                    if (settings.gridData.gridValues[index] == 0) countZero++;
                    if (settings.gridData.gridValues[index] == 2) countTwo++;
                }
                if (countZero != settings.optionZeroAmount || countTwo != settings.optionTwoAmount)
                {
                    _message.Append($"{settings.labels[i]} does not have exactly {settings.optionZeroAmount} {settings.options[0]} and {settings.optionTwoAmount} {settings.options[2]}!");
                    return;
                }
            }
            _message.Append("Everything looks good!");
        }
    }
}