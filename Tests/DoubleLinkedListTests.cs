using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsList;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Tests
{
    [TestClass]
    public class DoubleLinkedListTests
    {

        [TestMethod]
        public void SortShouldWorkWithLargeAmountOfElements()
        {
            var list = new DoubleLinkedList<int>();

            // create not sorted random elements
            for (int i = 0; i < 50000; i++)
            {
                list.Push(i);
                list.Unshift(i - 3);
            }

            list.Sort();
        }

        [TestMethod]
        public void SortShouldWorkAscendingByDefault()
        {
            var list = DoubleLinkedList<int>.Of(new[] { 3, 1, 2, 4 });

            list.Sort();

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(2, list.Get(1));
            Assert.AreEqual(3, list.Get(2));
            Assert.AreEqual(4, list.Get(3));
        }

       
        [TestMethod]
        public void SortShouldSortDescWhenPassIsAscAsFalse()
        {
            var list = DoubleLinkedList<int>.Of(new[] { 3, 1, 2, 4 });

            list.Sort(false);

            Assert.AreEqual(4, list.Get(0));
            Assert.AreEqual(3, list.Get(1));
            Assert.AreEqual(2, list.Get(2));
            Assert.AreEqual(1, list.Get(3));
        }

        [TestMethod]
        public void SortShouldWorkWithisFirstBeforePredicate()
        {
            var list = DoubleLinkedList<int>.Of(new[] { 3, 1, 2, 4 });

            // sort in a way that each next element not greather than 2
            list.Sort((el1, el2) => Math.Abs(el2 - el1) >= 2);

            Assert.AreEqual(2, list.Get(0));
            Assert.AreEqual(4, list.Get(1));
            Assert.AreEqual(3, list.Get(2));
            Assert.AreEqual(1, list.Get(3));
        }



        [TestMethod]
        public void GetShouldReturnDataByIndex()
        {
            var list = DoubleLinkedList<int>.Of(1, 2, 3);

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(2, list.Get(1));
            Assert.AreEqual(3, list.Get(2));
        }

        [TestMethod]
        public void UnshiftShouldAddElementsToListBegin()
        {
            var list = new DoubleLinkedList<int>();

            list.Unshift(1);
            list.Unshift(2);
            list.Unshift(3);

            Assert.AreEqual(3, list.Length);

            Assert.AreEqual(3, list.Get(0));
            Assert.AreEqual(2, list.Get(1));
            Assert.AreEqual(1, list.Get(2));
        }

        [TestMethod]
        public void PushShouldAddElementsToListEnd()
        {
            var list = new DoubleLinkedList<int>();

            list.Push(1);
            list.Push(2);
            list.Push(3);

            Assert.AreEqual(3, list.Length);

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(2, list.Get(1));
            Assert.AreEqual(3, list.Get(2));
        }

        [TestMethod]
        public void ListShouldBeEmptyBeDefault()
        {
            var list1 = DoubleLinkedList<int>.Of(new int[] { });
            var list2 = new DoubleLinkedList<double>();


            Assert.AreEqual(0, list1.Length);
            Assert.IsTrue(!list1);

            Assert.AreEqual(0, list2.Length);
            Assert.IsTrue(!list2);
        }


        [TestMethod]
        public void CloneShouldWorkWithValueTypes()
        {
            var list = DoubleLinkedList<int>.Of(new int[] { 1, 2 });

            DoubleLinkedList<int> cloned = (DoubleLinkedList<int>)list.Clone();

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(2, list.Get(1));

            Assert.AreEqual(1, cloned.Get(0));
            Assert.AreEqual(2, cloned.Get(1));

            Assert.AreEqual(cloned.Get(0), list.Get(0));
            Assert.AreEqual(cloned.Get(1), list.Get(1));
        }

        [TestMethod]
        public void CloneShouldCloneAllClonableEmements()
        {
            float FIRST_STUDENT_GRADE = 50;
            float MODIFIED_FIRST_STUDENT_GRADE = 100;

            var student1 = new Student("A", "B", "C", 1995, FIRST_STUDENT_GRADE);
            var student2 = new Student("X", "Y", "Z", 2000);

            DoubleLinkedList<Student> list = DoubleLinkedList<Student>.Of(student1, student2);

            var cloned = (DoubleLinkedList<Student>)list.Clone();

            Assert.AreEqual(student1, list.Get(0));
            Assert.AreEqual(student2, list.Get(1));

            Assert.AreEqual(cloned.Get(0), list.Get(0));
            Assert.AreEqual(cloned.Get(1), list.Get(1));

            list.Get(0).AverageGrade = MODIFIED_FIRST_STUDENT_GRADE;

            Assert.AreEqual(MODIFIED_FIRST_STUDENT_GRADE, list.Get(0).AverageGrade);
            Assert.AreEqual(FIRST_STUDENT_GRADE, cloned.Get(0).AverageGrade);
        }

        public void CloneAndOfhouldCloneSourceListCurrentNode()
        {
            var list = DoubleLinkedList<int>.Of(1, 2, 3);

            list.MoveCurrentToHead(); // current node refers to 1
            list++; // current node refers to 2

            DoubleLinkedList<int> cloned1 = (DoubleLinkedList<int>)list.Clone();
            DoubleLinkedList<int> cloned2 = DoubleLinkedList<int>.Of(list);

            Assert.AreEqual(2, cloned1.Current());
            Assert.AreEqual(2, cloned2.Current());
        }

        [TestMethod]
        public void PutAtShoulInsertDataByIndex()
        {
            var list = DoubleLinkedList<int>.Of(1, 3, 4);

            uint INDEX = 1;
            int NEW_DATA = 2;

            list.PutAt(NEW_DATA, INDEX);

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(NEW_DATA, list.Get(INDEX));
            Assert.AreEqual(3, list.Get(2));
            Assert.AreEqual(4, list.Get(3));
        }


        [TestMethod]
        public void IncludesShouldWorkWithDataAndPredicate()
        {
            var list = DoubleLinkedList<int>.Of(-1, 2, 3, 4);

            Assert.IsTrue(list.Includes(2));
            Assert.IsFalse(list.Includes(1));

            Assert.IsTrue(list.Includes(el => Math.Abs(el) == 1));
            Assert.IsTrue(list.Includes(el => el < 0));
            Assert.IsFalse(list.Includes(el => el > 5));
        }

        [TestMethod]
        public void FindShouldReturnDataByPredicate()
        {
            var list = DoubleLinkedList<int>.Of(1, 2, -3);

            Assert.AreEqual(-3, list.Find(el => el < 0));
        }

        [TestMethod]
        public void RemoveShouldRemoveByData()
        {
            var list = DoubleLinkedList<int>.Of(-1, 0, 1, -1);

            list.Remove(-1);

            Assert.AreEqual(3, list.Length);

            Assert.AreEqual(0, list.Get(0));
            Assert.AreEqual(1, list.Get(1));
            Assert.AreEqual(-1, list.Get(2));
        }

        [TestMethod]
        public void RemoveShouldRemoveAddEntriesByData()
        {
            var list = DoubleLinkedList<int>.Of(-1, 0, 1, -1);

            list.Remove(-1, true);

            Assert.AreEqual(2, list.Length);

            Assert.AreEqual(0, list.Get(0));
            Assert.AreEqual(1, list.Get(1));
        }

        [TestMethod]
        public void RemoveReturnBooleandTellsIfSomeElementWasRemoved()
        {
            var list = DoubleLinkedList<int>.Of(-1, 0, 1);

            int NON_EXISTING_VALUE_IN_LIST = 10000;

            Assert.IsTrue(list.Remove(-1, true));
            Assert.IsFalse(list.Remove(NON_EXISTING_VALUE_IN_LIST, false));
        }

        [TestMethod]
        public void RemoveShouldRemoveByPredicate()
        {
            var list = DoubleLinkedList<int>.Of(-1, 0, 1, -1);

            list.Remove(el => el < 0, true);

            Assert.AreEqual(2, list.Length);

            Assert.AreEqual(0, list.Get(0));
            Assert.AreEqual(1, list.Get(1));
        }

        [TestMethod]
        public void ClearShouldClearTheList()
        {
            var list = DoubleLinkedList<int>.Of(-1, 0, 1);

            list.Clear();

            Assert.IsTrue(!list);
            Assert.AreEqual(0, list.Length);
        }

        [TestMethod]
        public void CompareToShouldWork()
        {
            var list = DoubleLinkedList<int>.Of(-1, 0, 1);

            var cloned = (DoubleLinkedList<int>)list.Clone();

            Assert.AreEqual(list, cloned);
            Assert.AreEqual(0, list.CompareTo(cloned));

            list.Clear();

            Assert.AreNotEqual(list, cloned);
            Assert.AreEqual(-1, list.CompareTo(cloned));

            Assert.AreEqual(1, cloned.CompareTo(null));
            Assert.AreEqual(1, list.CompareTo(null));
        }


        [TestMethod]
        public void StaticCompareShouldWork()
        {
            var list = DoubleLinkedList<int>.Of(-1, 0);
            var list2 = DoubleLinkedList<int>.Of(1);

            var clonedList1 = (DoubleLinkedList<int>)list.Clone();

            Assert.AreEqual(-1, DoubleLinkedList<int>.Compare(null, list));
            Assert.AreEqual(0, DoubleLinkedList<int>.Compare(null, null));
            Assert.AreEqual(1, DoubleLinkedList<int>.Compare(list, null));

            Assert.AreEqual(-1, DoubleLinkedList<int>.Compare(list2, list));
            Assert.AreEqual(0, DoubleLinkedList<int>.Compare(list, list));
            Assert.AreEqual(0, DoubleLinkedList<int>.Compare(list, clonedList1));
            Assert.AreEqual(1, DoubleLinkedList<int>.Compare(list, list2));
        }


        [TestMethod]
        public void EnumeratorShouldIterateByValues()
        {
            var list = DoubleLinkedList<int>.Of(1, 2, 3);
            var list2 = new DoubleLinkedList<int>();

            foreach(int num in list)
            {
                list2.Push(num);
            }

            Assert.AreEqual(1, list2.Get(0));
            Assert.AreEqual(2, list2.Get(1));
            Assert.AreEqual(3, list2.Get(2));
        }

        [TestMethod]
        public void ShouldNotAllovedToCreateFromNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => DoubleLinkedList<int>.Of(null));
        }


        [TestMethod]
        public void ShouldThrowIndexOutOfBoundsWheTryingGetElementByUnexistingIndex()
        {
            var list = new DoubleLinkedList<int>();

            Assert.ThrowsException<IndexOutOfRangeException>(() => { list.Get(0); });
            Assert.ThrowsException<IndexOutOfRangeException>(() => { list.Get(10000); });
        }


        [TestMethod]
        public void ShouldSortCurrent()
        {
            var list = DoubleLinkedList<int>.Of(4, 1, 2);

            list.MoveCurrentToHead();
            list.SortCurrent();

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(2, list.Get(1));
            Assert.AreEqual(4, list.Get(2));
        }

    }
}
