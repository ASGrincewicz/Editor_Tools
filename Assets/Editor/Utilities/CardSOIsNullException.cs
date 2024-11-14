using System;

namespace Editor.Utilities
{
    public class CardSOIsNullException: Exception
    {
        public CardSOIsNullException() { }
        
        public CardSOIsNullException(string message):base(message){}
        
        public CardSOIsNullException(string message, Exception inner):base(message, inner){}
        
    }
}