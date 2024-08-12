using System;
using UnityEngine;

namespace Editor.InteractionMatrix
{
    [CreateAssetMenu(fileName = "InteractionGridSettings", menuName = "Settings/Interaction Grid", order = 0)]
    public class InteractionGridSettings : ScriptableObject
    {
        [HideInInspector]
        public GridData gridData;
        [Header("Naming")]
        [Tooltip("Change the title of the InteractionGridWindow, and names of the x and y axis.")]
        public string title = "Interaction Grid";
        public string xAxisLabel;
        public string yAxisLabel;
        [Header("Column/Row Labels:")]
        [Tooltip("These should be the types you want to show in the matrix." +
                 " Columns and Rows should have the same labels." +
                 " You can modify this to add/remove labels but should have at least 2.")]
        public string[] labels;
        [Header("Relationship Options:")]
        [Tooltip("You should have at least 2 options here. Validation is setup for options 0 and 2.")]
        public string[] options;
        [Header("Validation Options:")]
        [Tooltip("This is meant to balance strengths and weaknesses.\n " +
                 "Set the amounts for options 0 and 2 in your options setting.")]
        public int optionZeroAmount = 1;
        public int optionTwoAmount = 1;
    }

    [Serializable]
    public class GridData
    {
        public int[] gridValues;
        public int size;
    }
}