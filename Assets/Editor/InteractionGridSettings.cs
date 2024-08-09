using UnityEngine;

namespace Editor
{
    [CreateAssetMenu(fileName = "InteractionGridSettings", menuName = "Interaction Grid/Settings", order = 0)]
    public class InteractionGridSettings : ScriptableObject
    {
        public string title = "Interaction Grid";
        public string[] labels;
        public string[] options;
    }
}