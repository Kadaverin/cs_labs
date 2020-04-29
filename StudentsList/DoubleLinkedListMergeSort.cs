using System;
using System.Collections.Generic;
using System.Text;

namespace StudentsList
{
    class DoubleLinkedListMergeSort <T> where T : IComparable<T>
    {
        public static DoubleLinkedList<T>.Node MergeSort(DoubleLinkedList<T>.Node head, Func<T, T, bool> isFirstBefore)
        {
            int listSize = 1, numMerges;
            if (ReferenceEquals(head?.Next, null)) return head;

            do
            {
                numMerges = 0;
                var left = head;
                var tail = head = null;

                while (!ReferenceEquals(left, null))
                {
                    numMerges++;
                    var right = left;
                    var leftSize = 0;
                    var rightSize = listSize;

                    while (!ReferenceEquals(right, null) && leftSize < listSize)
                    {
                        leftSize++;
                        right = right.Next;
                    }

                    while (leftSize > 0 || rightSize > 0 && !ReferenceEquals(right, null))
                    {
                        DoubleLinkedList<T>.Node next;
                        if (rightSize == 0 || ReferenceEquals(right, null) || left.Value.CompareTo(right.Value) < 0)
                        {
                            next = left;
                            left = left.Next;
                            leftSize--;
                        }
                        else
                        {
                            next = right;
                            right = right.Next;
                            rightSize--;
                        }

                        if (!ReferenceEquals(tail, null)) tail.Next = next;
                        else head = next;

                        next.Prev = tail;
                        tail = next;
                    }

                    left = right;
                }

                tail.Next = null;
                listSize <<= 1;
            } while (numMerges > 1);

            return head;
        }
    }
}
