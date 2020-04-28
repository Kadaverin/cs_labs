using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace StudentsList
{
   public class TwowayList<T> : ICloneable, IEnumerable, IComparable<TwowayList<T>> where T : IComparable, ICloneable
    {
        public Node<T> Head { get; private set; }
        public Node<T> CurrentNode { get; set; }
        public uint Size { get; private set; }

        public bool IsSorted { get; private set; } = false;

        public void PushFront(T value)
        {
            var newNode = new Node<T>() { Value = value };

            if (Size == 0)
            {
                newNode.Prev = newNode.Next = newNode;
                Head = CurrentNode = newNode;
            }
            else
            {
                newNode.Prev = Head.Prev;
                newNode.Next = Head;
                Head.Prev.Next = newNode;
                Head.Prev = newNode;
                Head = CurrentNode = newNode;
                IsSorted = false;
            }

            ++Size;
        }

        public void PushBack(T value)
        {
            var newNode = new Node<T>() { Value = value };

            if (Size == 0)
            {
                newNode.Prev = newNode.Next = newNode;
                Head = CurrentNode = newNode;

            }
            else
            {
                newNode.Prev = Head.Prev;
                newNode.Next = Head;
                Head.Prev.Next = newNode;
                Head.Prev = newNode;
                CurrentNode = newNode;
                IsSorted = false;
            }

            ++Size;
        }

        public bool PushSorted(T value, Func<T, T, bool> compareFunc)
        {
            if (IsSorted)
            {
                var newNode = new Node<T>() {Value = value};
                var foundNode = Find(node => compareFunc(newNode, node));
                if (foundNode is null)
                {
                    PushBack(value);
                    return true;
                }

                foundNode.Prev.Next = newNode;
                newNode.Prev = foundNode.Prev.Next;
                newNode.Next = foundNode;
                foundNode.Prev = newNode;
            }
            else
            {
                PushBack(value);
                SortCurrent(compareFunc);
                return true;
            }

            return false;
        }

        public bool DeleteValue(T value)
        {
            if (Head == null)
            {
                return false;
            }

            var foundNode = Find(value);
           
            if(foundNode == null)
            {
                return false;
            }

            if (foundNode.CompareTo(Head) == 0)
            {
                var headPrev = Head.Prev;
                Head = Head.Next;
                headPrev.Next = Head;
            }
            else if (foundNode.CompareTo(Head.Prev) == 0)
            {
                Head.Prev = Head.Prev.Prev;
                Head.Prev.Next = Head;
            }
            else
            {
                foundNode.Prev.Next = foundNode.Next;
                foundNode.Next.Prev = foundNode.Prev;
            }

            --Size;

            return true;
        }


        public Node<T> Find(T value)
        {
            return Find(node => node.Value.CompareTo(value) == 0);
        }

        public Node<T> Find(Func<Node<T>, bool> compareFunc)
        {
            if (Head == null)
            {
                return null;
            }

            Node<T> foundElement = null;
            foreach(Node<T> element in this)
            {
                if (compareFunc(element))
                {
                    foundElement = element;
                    break;
                }
            }

            return foundElement;
        }


        public void ClearAll()
        {
            Head = CurrentNode = null;
            IsSorted = false;
            Size = 0;
        }


       public void Sort(Func<T, T, int> comparator )
        {
            DoubleLinkedListMergeSort<T>.MergeSort(Head, comparator);
        }

        public bool SortCurrent(Func<T, T, bool> compareFunc)
        {
            if (Head == null || CurrentNode == null)
            {
                return false;
            }

            if (IsSorted)
            {
                var tempNode = Head;

                do
                {
                    if (compareFunc(tempNode, CurrentNode))
                    {
                        tempNode = tempNode.Next;
                    }
                    else
                    {
                        var currNodeValue = CurrentNode.Value;
                        CurrentNode.Value = tempNode.Value;
                        tempNode.Value = currNodeValue;

                    }

                } while (tempNode != Head);
            }
            else
            {
                Sort(compareFunc);
            }

            return true;
        }

        public bool DeleteCurrent()
        {
            if (Head == null || CurrentNode == null)
            {
                return false;
            }

            if (CurrentNode == Head.Prev)
            {
                Head.Prev = Head.Prev.Prev;
            }
            else if (CurrentNode == Head)
            {
                Head.Prev.Next = Head.Next;
                Head = Head.Next;
            }

            CurrentNode.Prev.Next = CurrentNode.Next;
            CurrentNode.Next.Prev = CurrentNode.Prev;
            CurrentNode = null;

            IsSorted = false;
            --Size;

            return true;
        }


        public void MoveCurrentToHead()
        {
            CurrentNode = Head;
        }

        public void MoveCurrentToTail()
        {
            CurrentNode = Head.Prev;
        }

        #region Interfaces & Operators
        public object Clone()
        {
            var newList = new TwowayList<T>();
         
            foreach (Node<T> element in this)
            {
                newList.PushBack((T)element.Clone());
            }

            return newList;
        }
        public int CompareTo(TwowayList<T> obj)
        {
            return CompareLists(this, obj);
        }

        public static int CompareLists(TwowayList<T> firstList, TwowayList<T> secondList)
        {
            var isFirstListNull = firstList is null;


            var isSecondListNull = secondList is null;


            if (isFirstListNull && isSecondListNull)
            {
                return 0;
            }
            else
            {
                if (isFirstListNull)
                {
                    return -1;
                }

                if (isSecondListNull)
                {
                    return 1;
                }
            }

            return firstList.Size == secondList.Size ? 0 : firstList.Size > secondList.Size ? 1 : -1;
        }
        public override bool Equals(object obj)
        {
            if (GetType() != obj?.GetType())
            {
                return false;
            }
            var secondList = (TwowayList<T>)obj;

            return CompareLists(this, secondList) == 0;
        }

        public override int GetHashCode()
        {
            int hashResult = 0;

            foreach (Node<T> node in this)
            {
                hashResult += node.GetHashCode();
            }

            hashResult += BitConverter.ToInt32(BitConverter.GetBytes(Size), 0);

            return hashResult;
        }

        #region Operators
        public static bool operator ==(TwowayList<T> left, TwowayList<T> right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(TwowayList<T> left, TwowayList<T> right)
        {
            return !(left == right);
        }

        public static bool operator <(TwowayList<T> left, TwowayList<T> right)
        {
            return left is null ? right is object : left.CompareTo(right) < 0;
        }

        public static bool operator <=(TwowayList<T> left, TwowayList<T> right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(TwowayList<T> left, TwowayList<T> right)
        {
            return left is object && left.CompareTo(right) > 0;
        }

        public static bool operator >=(TwowayList<T> left, TwowayList<T> right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        #endregion Operators

        public static bool operator !(TwowayList<T> list) => list?.Size == 0;

        public static TwowayList<T> operator ++(TwowayList<T> list)
        {
            if (list != null && list.CurrentNode != null)
            {
                list.CurrentNode = list.CurrentNode.Next;
            }

            return list;
        }

        public static TwowayList<T> operator --(TwowayList<T> list)
        {
            if (list != null && list.CurrentNode != null)
            {
                list.CurrentNode = list.CurrentNode.Prev;
            }

            return list;
        }


        public IEnumerator GetEnumerator()
        {
            return new ListEnumerator(Head);
        }

        public class ListEnumerator : IEnumerator
        {
            private Node<T> _currentNode;
            private Node<T> _headNode;
            private bool _firstUse = true;
            public object Current { get; set; }

            public ListEnumerator(Node<T> headNode)
            {
                _currentNode = headNode;
                _headNode = headNode;
            }

            public bool MoveNext()
            {
                if (_currentNode == null || _headNode == null)
                {
                    return false;
                }

                Current = _currentNode;
                _currentNode = _currentNode.Next;

                if (_firstUse)
                {
                    _firstUse = false;
                    return true;
                }

                var compareResult = !IsEqualNodes(_headNode, (Node<T>)Current);

                if (!compareResult)
                {
                    _firstUse = true;
                }

                return compareResult;
            }

            public void Reset()
            {
                if (_currentNode == null || _headNode == null)
                {
                    throw new NullReferenceException("Invalid nodes");
                }
                _currentNode = _headNode;
                _firstUse = true;
            }

            public bool IsEqualNodes(Node<T> node1, Node<T> node2)
            {
                return
                    (node1 is null && node2 is null)
                    || ReferenceEquals(node1, node2)
                    || (ReferenceEquals(node1.Prev, node2.Prev) && ReferenceEquals(node1.Next, node2.Next));
            }
        }

        
        #endregion
    }
}
