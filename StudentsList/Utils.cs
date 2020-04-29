using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsList
{
    public class Utils
    {
        public static object Clone(object element)
        {
            return element is ICloneable ? ((ICloneable)element).Clone() : element;
        }

        public static void Swap<T>(ref Node<T> A, ref Node<T> B) where T : IComparable<T>
        {
            var p = A;
            A = B;
            B = p;

            B.Next = A.Next;
            B.Prev = A.Prev;

            A.Next = p.Next;
            A.Prev = p.Prev;
        }
    }
}
