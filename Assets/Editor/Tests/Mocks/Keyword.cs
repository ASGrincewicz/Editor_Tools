namespace Editor.Tests.Mocks
{
    public class Keyword
    {
        public string KeywordName { get; set; }
        public string Definition { get; set; }
        
        public int KeywordValue { get; set; }

        // Simulating a method that checks if the keyword is a default one.
        public bool IsDefault()
        {
            return KeywordName.StartsWith("Default");
        }
    }
}