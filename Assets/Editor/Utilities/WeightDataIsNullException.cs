using System;

namespace Editor.Utilities
{
    public class WeightDataIsNullException: Exception
    {
        public WeightDataIsNullException(){}
        public WeightDataIsNullException(string message) : base(message){}
        public WeightDataIsNullException(string message, Exception inner) : base(message, inner){}
        
    }
}