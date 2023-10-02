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

        [TestCase(1, 2, 3)]
        [TestCase('h', '1', 'h')]
        [TestCase("Hello", "World", "!")]
        [TestCase(true, true, false)]
        [TestCase(27.4F, 65.4F, 14.0F)]
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

        [TestCase(1, 2, 3)]
        [TestCase('h', '1', 'h')]
        [TestCase("Hello", "World", "!")]
        [TestCase(true, true, false)]
        [TestCase(27.4F, 65.4F, 14.0F)]
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

        [TestCase(1, 2, 3)]
        [TestCase('h', '1', 'h')]
        [TestCase("Hello", "World", "!")]
        [TestCase(true, true, false)]
        [TestCase(27.4F, 65.4F, 14.0F)]
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

        public void CheckIf_ItemNotInCollection()
        {
            //Arrange
            var collection = new MyLinkedList<int>();

            //Act
            var result = collection.Contains(1);

            //Assert
            Assert.That(result, Is.False);
        }
    }
}
