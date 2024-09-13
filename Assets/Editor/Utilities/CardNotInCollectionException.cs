using System;

namespace Editor.Utilities
{
    public class CardNotInCollectionException: Exception
    {
        public CardNotInCollectionException(){}
        public CardNotInCollectionException(string message) : base(message){}
        public CardNotInCollectionException(string message, Exception inner) : base(message, inner){}
    }
}