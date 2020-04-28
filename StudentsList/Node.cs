using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsList
{
    public class Node<T> : IComparable<Node<T>>, ICloneable where T : IComparable, ICloneable
    {
        public T Value { get; set; }
        public Node<T> Prev { get; set; }
        public Node<T> Next { get; set; }

        public int CompareTo(Node<T> obj)
        {
            return CompareNodes(this, obj);
        }

        public static int CompareNodes(Node<T> node1, Node<T> node2)
        {
            if (ReferenceEquals(node1, node2))
            {
                return 0;
            }

            if (node1 is null) return -1;
            if (node2 is null) return 1;
            
            return node1.Value.CompareTo(node2.Value);
        }

        public override bool Equals(object obj)
        {
            // safe from obj is null becouse of ?. oerator
            if (GetType() != obj?.GetType())
            {
                return false;
            }

            return Equals((Node<T>)obj);
        }

        public bool Equals(Node<T> obj)
        {
            if (obj is null)  return false;
            
            if (!ReferenceEquals(Prev, obj.Prev) || !ReferenceEquals(Next, obj.Next))
            {
                return false;
            }

            return CompareNodes(this, obj) == 0;
        }

        public object Clone()
        {
            return typeof(T).IsValueType ? Value : Value.Clone();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Value, Prev, Next);
        }
    }

}
