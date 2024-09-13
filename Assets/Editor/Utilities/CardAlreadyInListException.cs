using System;

namespace Editor.Utilities
{
    public class CardAlreadyInListException: Exception
    {
        public CardAlreadyInListException(){}
        public CardAlreadyInListException(string message):base(message){}
        public CardAlreadyInListException(string message, Exception inner):base(message,inner){}
    }
}