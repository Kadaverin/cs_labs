using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsList
{
    class DoubleLinkedListMergeSort <T> where T : IComparable<T>
    {
        public static Node<T> MergeSort(ref Node<T> node, Func<T, T, bool> isSmaller)
        {
            if (node is null || node.Next is null)
            {
                return node;
            }
            Node<T> second = SplitList(node);

            // Recur for left and right halves 
            node = MergeSort(ref node, isSmaller);
            second = MergeSort(ref second, isSmaller);

            // Merge the two sorted halves 
            return MergeLists(node, second, isSmaller);
        }

        private static Node<T> MergeLists(Node<T> first, Node<T> second, Func<T, T, bool> isSmaller)
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
            if (isSmaller(first.Value, second.Value))
            {
                first.Next = MergeLists(first.Next, second, isSmaller);
                first.Next.Prev = first;
                first.Prev = null;
                return first;
            }
            else
            {
                second.Next = MergeLists(first, second.Next, isSmaller);
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
