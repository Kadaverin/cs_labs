using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentsList;


namespace Tests
{
    [TestClass]
    public class DoubleLinkedListTests
    {
        [TestMethod]
        public void UnshiftShouldAddElementsToListBegin()
        {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>();

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
            DoubleLinkedList<int> list = new DoubleLinkedList<int>();

            list.Push(1);
            list.Push(2);
            list.Push(3);

            Assert.AreEqual(3, list.Length);

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(2, list.Get(1));
            Assert.AreEqual(3, list.Get(2));
        }

        [TestMethod]
        public void SortShouldWorkAscendingByDefault()
        {
            DoubleLinkedList<int> list = new DoubleLinkedList<int>();

            list.Push(3);
            list.Push(1);
            list.Push(2);
            list.Push(4);

            list.Sort();

            Assert.AreEqual(1, list.Get(0));
            Assert.AreEqual(2, list.Get(1));
            Assert.AreEqual(3, list.Get(2));
            Assert.AreEqual(4, list.Get(3));
        }


    }
}
