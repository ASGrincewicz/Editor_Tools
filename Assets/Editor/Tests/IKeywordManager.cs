using System.Collections.Generic;
using Editor.Tests.Mocks;

namespace Editor.Tests
{
    public interface IKeywordManager
    {
        List<Keyword> KeywordList { get; set; }
    }
}