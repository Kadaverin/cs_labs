using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsList
{
    class DoubleLinkedListMergeSort <T> where T : IComparable
    {
        public static Node<T> MergeSort(Node<T> node, Func<T, T, bool> predicate)
        {
            if (node is null || node.Next is null)
            {
                return node;
            }
            Node<T> second = SplitList(node);

            // Recur for left and right halves 
            node = MergeSort(node, predicate);
            second = MergeSort(second, predicate);

            // Merge the two sorted halves 
            return MergeLists(node, second, predicate);
        }

        private static Node<T> MergeLists(Node<T> first, Node<T> second, Func<T, T, bool> predicate)
        {
            // If first linked list is empty 
            if (first is null)
            {
                return second;
            }

            // If second linked list is empty 
            if (second is null)
            {
                return first;
            }

            // Pick the smaller value 
            if (predicate(first.Value, second.Value))
            {
                first.Next = MergeLists(first.Next, second, predicate);
                first.Next.Prev = first;
                first.Prev = null;
                return first;
            }
            else
            {
                second.Next = MergeLists(first, second.Next, predicate);
                second.Next.Prev = second;
                second.Prev = null;
                return second;
            }
        }


        private static Node<T> SplitList(Node<T> head)
        {
            Node<T> fast = head, slow = head;
            while (fast.Next is object && fast.Next.Next is object)
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
