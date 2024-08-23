using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.KeywordSystem
{
    [CreateAssetMenu(fileName = "KeywordManager", menuName = "Config/Keyword Manager", order = 0)]
    public class KeywordManager : ScriptableObject
    {
        public List<Keyword> keywordList;


        private void OnValidate()
        {
            if (keywordList == null)
                return;

            // Create a set to keep track of items we've seen
            HashSet<string> seenItems = new HashSet<string>();

            // List to store indices of duplicates
            List<int> duplicateIndices = new List<int>();

            for (int i = 0; i < keywordList.Count; i++)
            {
                Keyword currentItem = keywordList[i];

                // Skip entries with empty or null names and handle them separately
                if (string.IsNullOrEmpty(currentItem.keywordName))
                {
                    continue;
                }

                if (seenItems.Contains(currentItem.keywordName))
                {
                    // Track index to handle duplicate items later
                    duplicateIndices.Add(i);
                }
                else
                {
                    seenItems.Add(currentItem.keywordName);
                }
            }

            // Remove duplicates at the tracked indices
            duplicateIndices.Reverse(); // Reverse to remove from the end to avoid disrupting earlier indices
            foreach (int index in duplicateIndices)
            {
                Debug.LogWarning($"Duplicate item found and removed: {keywordList[index].keywordName} at index {index}");
                keywordList.RemoveAt(index);
            }

            // Ensure there is one empty slot for new entries
            bool hasEmptySlot = false;
            foreach (var keyword in keywordList)
            {
                if (string.IsNullOrEmpty(keyword.keywordName))
                {
                    hasEmptySlot = true;
                    break;
                }
            }

            if (!hasEmptySlot)
            {
                keywordList.Add(new Keyword { keywordName = string.Empty });
            }
        }
    }
}
