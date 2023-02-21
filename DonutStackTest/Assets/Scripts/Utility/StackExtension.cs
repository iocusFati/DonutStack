using System.Collections.Generic;

namespace Utility
{
    public static class StackExtension
    {
        public static Stack<T> Clone<T>(this Stack<T> originalStack)
        {
            return new Stack<T>(new Stack<T>(originalStack));
        }
    }
}