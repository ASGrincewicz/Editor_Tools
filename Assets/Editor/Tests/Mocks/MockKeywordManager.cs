using System.Collections.Generic;

namespace Editor.Tests.Mocks
{
    public class MockKeywordManager
    {
        public List<Keyword> KeywordList = new ();

        public void AddKeyword(Keyword keyword)
        {
            if (!KeywordList.Exists(k => k.KeywordName == keyword.KeywordName)) // Prevent duplicates
            {
                KeywordList.Add(keyword);
            }
        }

        public void RemoveKeyword(Keyword keyword)
        {
            KeywordList.RemoveAll(k => k.KeywordName == keyword.KeywordName);
        }

        public Keyword[] GetKeywords()
        {
            return KeywordList.ToArray();
        }

        public void Initialize(Keyword[] initialKeywords)
        {
            KeywordList = new List<Keyword>(initialKeywords);
        }
    }
}