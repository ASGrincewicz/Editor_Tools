using System;
using UnityEngine;

namespace Editor.InteractionMatrix
{
    [CreateAssetMenu(fileName = "InteractionGridSettings", menuName = "Interaction Grid/Settings", order = 0)]
    public class InteractionGridSettings : ScriptableObject
    {
        [HideInInspector]
        public GridData gridData;
        public string title = "Interaction Grid";
        public string xAxisLabel;
        public string yAxisLabel;
        public string[] labels;
        public string[] options;
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