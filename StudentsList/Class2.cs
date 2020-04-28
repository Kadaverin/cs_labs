using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsList
{
    class DoubleLinkedListMergeSort <T> where T : IComparable, ICloneable
    {
        public static Node<T> MergeSort(Node<T> node, Func<T, T, int> comparator)
        {
            if (node == null || node.Next == null)
            {
                return node;
            }
            Node<T> second = SplitList(node);

            // Recur for left and right halves 
            node = MergeSort(node, comparator);
            second = MergeSort(second, comparator);

            // Merge the two sorted halves 
            return MergeLists(node, second, comparator);
        }

        private static Node<T> MergeLists(Node<T> first, Node<T> second, Func<T, T, int> comparator)
        {
            // If first linked list is empty 
            if (first == null)
            {
                return second;
            }

            // If second linked list is empty 
            if (second == null)
            {
                return first;
            }

            // Pick the smaller value 
            if (first.Value.CompareTo(second.Value) == -1)
            {
                first.Next = MergeLists(first.Next, second, comparator);
                first.Next.Prev = first;
                first.Prev = null;
                return first;
            } 
            else
            {
                second.Next = MergeLists(first, second.Next, comparator);
                second.Next.Prev = second;
                second.Prev = null;
                return second;
            }
        }


        private static Node<T> SplitList(Node<T> head)
        {
            Node<T> fast = head, slow = head;
            while (fast.Next != null && fast.Next.Next != null)
            {
                fast = fast.Next.Next;
                slow = slow.Next;
            }
            Node<T> temp = slow.Next;
            slow.Next = null;
            return temp;
        }
    }
}
