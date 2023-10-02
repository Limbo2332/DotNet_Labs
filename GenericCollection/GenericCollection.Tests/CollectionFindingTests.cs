using GenericCollection.Collections;
using NUnit.Framework;

namespace GenericCollection.Tests
{
    public class CollectionFindingTests
    {
        [Test]
        public void FindItem_WhenNoItems()
        {
            //Arrange
            var collection = new MyLinkedList<bool>();

            //Act
            var item = collection.Find(true);

            //Assert
            Assert.That(item, Is.Null);
        }

        [Test]
        public void FindItem_NotInItems()
        {
            //Arrange
            var collection = new MyLinkedList<bool> { false, false, false };

            //Act
            var item = collection.Find(true);

            //Assert
            Assert.That(item, Is.Null);
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.ThreeItemsTestCases))]
        public void FindItem_InItems<T>(T firstValue, T secondValue, T thirdValue)
        {
            //Arrange
            var collection = new MyLinkedList<T> { firstValue, secondValue, thirdValue };

            //Act
            var item = collection.Find(value: secondValue);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(item, Is.Not.Null);
                Assert.That(item!.Value, Is.EqualTo(secondValue));
            });
        }

        [TestCase(-1)]
        [TestCase(int.MaxValue)]
        [TestCase(4)]
        public void FindItem_WhenWrongIndex(int index)
        {
            //Arrange
            var collection = new MyLinkedList<int> { 1, 2, 3 };

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.Find(index: index));
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.ThreeItemsTestCases))]
        public void FindItem_ByIndex_WhenInCollection<T>(T firstValue, T secondValue, T thirdValue)
        {
            //Arrange
            var collection = new MyLinkedList<T> { firstValue, secondValue, thirdValue };

            //Act
            var item = collection.Find(index: 1);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(item, Is.Not.Null);
                Assert.That(item!.Value, Is.EqualTo(secondValue));
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.ThreeItemsTestCases))]
        public void CheckIf_ItemInCollection<T>(T firstValue, T secondValue, T thirdValue)
        {
            //Arrange
            var collection = new MyLinkedList<T> { firstValue, secondValue, thirdValue };

            //Act
            var firstResult = collection.Contains(firstValue);
            var secondResult = collection.Contains(secondValue);
            var thirdResult = collection.Contains(thirdValue);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstResult, Is.True);
                Assert.That(secondResult, Is.True);
                Assert.That(thirdResult, Is.True);
            });
        }

        [Test]
        public void CheckIf_ItemNotInCollection()
        {
            //Arrange
            var collection = new MyLinkedList<int>();

            //Act
            var result = collection.Contains(1);

            //Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Check_Indexer_IfNoInCollection()
        {
            //Arrange
            var collection = new MyLinkedList<int>();

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _ = collection[1]);
        }

        [Test]
        public void Check_Indexer_IfInCollection()
        {
            //Arrange
            var collection = new MyLinkedList<int>() { 1, 2, 3 };

            //Act
            var result = collection[1];

            //Assert
            Assert.That(result, Is.EqualTo(2));
        }
    }
}
