using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace StudentsList
{
    public class DoubleLinkedList<T> : ICloneable, IEnumerable<T>, IComparable<DoubleLinkedList<T>> where T : IComparable<T>
    {
        public DoubleLinkedList() { }
        protected Node<T> Head { get; set; }
        protected Node<T> CurrentNode { get; set; }
        public int Length { get; private set; } = 0;

        public static DoubleLinkedList<T> Of(IEnumerable<T> source)
        {
            if (source is null) throw new ArgumentNullException("Can not clone list from 'null'");

            var list = new DoubleLinkedList<T>();

            foreach (T element in source)
            {
                var elem = element is ICloneable ? ((ICloneable)element).Clone() : element;

                list.Push((T)elem);
            }

            return list;
        }

        public static DoubleLinkedList<T> Of(params T[] source)
        {
            return Of(source);
        }

        public object Clone()
        {
            return Of(this);
        }

        public void Push(T value)
        {
            AddAfter(value, Head?.Prev);
        }

        public void Unshift(T value)
        {
            Push(value);

            Head = Head.Prev;
        }

        public void PutAt(T data, int index)
        {
            var nodeToAddAfter = GetNode(index - 1);

            AddAfter(data, nodeToAddAfter);
        }

        public T Get(int index)
        {
            return GetNode(index).Value;
        }

        private Node<T> GetNode(int index)
        {
            if (index < 0)
            {
                throw new IndexOutOfRangeException("Argument 'index' can not be negative");
            }

            if (index >= Length)
            {
                throw new IndexOutOfRangeException("Argument 'index' can not be greater or equal to length of list");
            }

            Node<T> node = Head; ;

            for (var i = 0; i < index; i++) node = node.Next;

            return node;
        }

        public bool Includes(T data)
        {
            return Includes(elem => elem.CompareTo(data) == 0);
        }

        public bool Includes(Func<T, bool> predicate)
        {
            Node<T> node = FindNode(predicate);

            return !(node is null);
        }

        public T Find(Func<T, bool> predicate)
        {
            Node<T> node = FindNode(predicate);

            return node is null ? default : node.Value;
        }

        private Node<T> FindNode(T data)
        {
            return FindNode(elem => elem.CompareTo(data) == 0);
        }

        private Node<T> FindNode(Func<T, bool> predicate)
        {
            Node<T> temp = Head;

            for (int i = 0; i < Length; i++)
            {
                if (predicate(temp.Value)) return temp;
                temp = temp.Next;
            }

            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = Head;
            for (var i = 0; i < Length; i++)
            {
                yield return node.Value;
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T data, bool allEntries = false)
        {
            return Remove(val => val.CompareTo(data) == 0, allEntries);
        }

        public bool Remove(Func<T, bool> predicate, bool allEntries = false)
        {
            if (Head is null)
            {
                return false;
            }

            bool isDeleted = false;
            Node<T> temp = Head.Prev;

            do
            {
                if (predicate(temp.Next.Value))
                {
                    DeleteNode(temp.Next);
                    isDeleted = true;

                    if (!allEntries) break;
                }
                temp = temp.Next;
            } while (temp.Next != Head);

            return isDeleted;
        }

        private void DeleteNode(Node<T> target)
        {
            if (Length == 1) Head = null;
            else
            {
                target.Prev.Next = target.Next;
                target.Next.Prev = target.Prev;
            }
            if (ReferenceEquals(target, Head) && Length > 1)
            {
                Head = target.Next;
            }

            Length--;
        }

        public void Clear()
        {
            Head = CurrentNode = null;
            Length = 0;
        }

        public void Sort(Func<T, T, bool> isSmaller)
        {
            var temp = Head;
            temp.Prev.Next = null;
            Head = DoubleLinkedListMergeSort<T>.MergeSort(ref temp, isSmaller);
        }

        public void Sort(bool isAsc)
        {
           var isSmaller = isAsc ? (Func<T, T, bool>)((el1, el2) => el1.CompareTo(el2) == -1)
                : ((el1, el2) => el1.CompareTo(el2) == 1);

            Sort(isSmaller);
        }


        public void Sort()
        {
            Sort(true);
        }

        private void AddFirst(T data)
        {
            Head = new Node<T>
            {
                Value = data
            };
            Head.Next = Head;
            Head.Prev = Head;

            Length++;
        }

        private void AddAfter(T data, Node<T> target)
        {
            if (Head is null)
            {
                AddFirst(data);
                return;
            }

            Node<T> newNode = new Node<T>
            {
                Value = data,

                Next = target.Next
            };
            target.Next.Prev = newNode;

            target.Next = newNode;
            newNode.Prev = target;

            Length++;
        }

        public static int Compare(DoubleLinkedList<T> first, DoubleLinkedList<T> second)
        {
            bool isFirstNull = first is null;
            bool isSecondNull = second is null;

            if (isFirstNull && isSecondNull) return 0;

            if (isFirstNull) return -1;

            if (isSecondNull) return 1;

            return first.Length > second.Length ? 1 : first.Length == second.Length ? 0 : -1;
        }

        public int CompareTo(DoubleLinkedList<T> obj)
        {
            return Compare(this, obj);
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj?.GetType())
            {
                return false;
            }

            return Compare(this, (DoubleLinkedList<T>)obj) == 0;
        }

        #region overloaded operators

        public static bool operator ==(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) == 0;
        }

        public static bool operator !=(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return !(left == right);
        }

        public static bool operator <(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) == -1;
        }

        public static bool operator <=(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return left < right || left == right;
        }

        public static bool operator >(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) == 1;
        }

        public static bool operator >=(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return left > right || left == right;
        }

        public static bool operator !(DoubleLinkedList<T> list) => list.Length == 0;

        public static DoubleLinkedList<T> operator ++(DoubleLinkedList<T> list)
        {
            if (list.CurrentNode is object)
            {
                list.CurrentNode = list.CurrentNode.Next;
            }

            return list;
        }

        public static DoubleLinkedList<T> operator --(DoubleLinkedList<T> list)
        {
            if (list.CurrentNode is object)
            {
                list.CurrentNode = list.CurrentNode.Prev;
            }

            return list;
        }

        #endregion

        #region CurrentNode hanlers
        public void DeleteCurrent()
        {
            if (CurrentNode is object) DeleteNode(CurrentNode);
        }

        public void MoveCurrentToHead()
        {
            CurrentNode = Head;
        }

        public void MoveCurrentToTail()
        {
            CurrentNode = Head.Prev;
        }

        public bool SortCurrent(Func<T, T, bool> isSmaller)
        {
            if (Length > 1 && !(CurrentNode is null))
            {
                var tempNode = Head;

                for (int i = 0; i < Length; i++)
                {
                    if (isSmaller(tempNode.Value, CurrentNode.Value))
                    {
                        Swap(tempNode, CurrentNode);

                        return true;
                    }
                    tempNode = tempNode.Next;
                }
            }
            return false;
        }

        public bool SortCurrent()
        {
            return SortCurrent((val, curVal) => val.CompareTo(curVal) == 1);
        }

        #endregion

        private void Swap(Node<T> A, Node<T> B)
        {
            var p = A;
            A = B;
            B = p;

            B.Next = A.Next;
            B.Prev = A.Prev;

            A.Next = p.Next;
            A.Prev = p.Prev;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Head, CurrentNode, Length);
        }
    }
}
