using System;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class InteractionGridWindow : EditorWindow
    {
        public InteractionGridSettings settings;
        private const float AXIS_LABEL_WIDTH = 115.0f;
        private const float COL_WIDTH = 100.0f;
        private const float INDENT = 50.0f;
        private const float TITLE_HEIGHT = 20.0f;
        private readonly int gridWidth = 5;
        private readonly int gridHeight = 5;
        private int[,] gridValues;

        private InteractionGridWindow()
        {
        gridValues = new int[gridHeight, gridWidth];
        }
        
        [MenuItem("Tools/Interaction Grid")]
        public static void Init()
        {
            EditorWindow window = GetWindow<InteractionGridWindow>("Interaction Matrix");
            window.position = new Rect(100f, 100f, 900f, 300f);
            window.Show();
        }

        private void OnGUI()
        {
            if (ReferenceEquals(settings,null))
            {
                EditorGUILayout.LabelField("No settings asset has been assigned.");
                return;
            }
            // Check if gridValues has the correct size, if not - resize it.
            if (settings.labels.Length != gridValues.GetLength(0) || settings.labels.Length != gridValues.GetLength(1))
            {
                ResizeGrid(settings.labels.Length, settings.labels.Length);
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
                    gridValues[i, j] = EditorGUILayout.Popup(gridValues[i, j], settings.options, GUILayout.Width(COL_WIDTH));
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        private void ResizeGrid(int rows, int cols)
        {
            // Create a new array
            int[,] newGrid = new int[rows,cols];
            for (int i = 0; i < rows; ++i)
            {
                // If this row existed in the old grid, copy its values to the new grid.
                if (i < gridValues.GetLength(0))
                {
                    for (int j = 0; j < Math.Min(cols, gridValues.GetLength(1)); ++j)
                    {
                        newGrid[i,j] = gridValues[i,j];
                    }
                }
            }
            // Replace the old grid with the new grid.
            gridValues = newGrid;
        }
    }
}