using GenericCollection.Collections;
using NUnit.Framework;

namespace GenericCollection.Tests
{
    public class CollectionAddingTests
    {
        [TestCaseSource(typeof(TestData), nameof(TestData.OneItemTestCase))]
        public void Check_CollectionAdd_Item<T>(T value)
        {
            // Arrange
            var collection = new MyLinkedList<T>();

            // Act
            collection.Add(value);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(collection, Is.Not.Null);
                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.Last, Is.Not.Null);

                Assert.That(collection.Count, Is.EqualTo(1));
                Assert.That(collection.First!.Value, Is.EqualTo(value));
                Assert.That(collection.Last!.Value, Is.EqualTo(value));

                Assert.That(collection.First, Is.EqualTo(collection.Last));
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.TwoItemsTestCases))]
        public void Check_CollectionAdd_TwoItems<T>(T firstValue, T secondValue)
        {
            // Arrange
            var collection = new MyLinkedList<T>();

            // Act
            collection.Add(firstValue);
            collection.Add(secondValue);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(collection, Is.Not.Null);

                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.First!.Previous, Is.Null);
                Assert.That(collection.First!.Next, Is.Not.Null);

                Assert.That(collection.Last, Is.Not.Null);
                Assert.That(collection.Last!.Next, Is.Null);
                Assert.That(collection.Last!.Previous, Is.Not.Null);

                Assert.That(collection.Count, Is.EqualTo(2));

                Assert.That(collection.First.Value, Is.EqualTo(firstValue));
                Assert.That(collection.First!.Next!.Value, Is.EqualTo(secondValue));

                Assert.That(collection.Last.Value, Is.EqualTo(secondValue));
                Assert.That(collection.Last!.Previous!.Value, Is.EqualTo(firstValue));

                Assert.That(collection.First, Is.Not.EqualTo(collection.Last));
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.ThreeItemsTestCases))]
        public void Check_CollectionAdd_ThreeItems<T>(T firstValue, T secondValue, T thirdValue)
        {
            // Arrange
            var collection = new MyLinkedList<T>();

            // Act
            collection.Add(firstValue);
            collection.Add(secondValue);
            collection.Add(thirdValue);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(collection, Is.Not.Null);

                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.First!.Previous, Is.Null);
                Assert.That(collection.First!.Next, Is.Not.Null);

                Assert.That(collection.Find(1), Is.Not.Null);
                Assert.That(collection.Find(1)!.Previous, Is.Not.Null);
                Assert.That(collection.Find(1)!.Next, Is.Not.Null);

                Assert.That(collection.Last, Is.Not.Null);
                Assert.That(collection.Last!.Next, Is.Null);
                Assert.That(collection.Last!.Previous, Is.Not.Null);

                Assert.That(collection.Count, Is.EqualTo(3));

                Assert.That(collection.First.Value, Is.EqualTo(firstValue));
                Assert.That(collection.First!.Next!.Value, Is.EqualTo(secondValue));

                Assert.That(collection.Find(1)!.Value, Is.EqualTo(secondValue));

                Assert.That(collection.Find(1)!.Next, Is.EqualTo(collection.Last));
                Assert.That(collection.Find(1)!.Next!.Value, Is.EqualTo(thirdValue));

                Assert.That(collection.Find(1)!.Previous, Is.EqualTo(collection.First));
                Assert.That(collection.Find(1)!.Previous!.Value, Is.EqualTo(firstValue));

                Assert.That(collection.Last.Value, Is.EqualTo(thirdValue));
                Assert.That(collection.Last!.Previous!.Value, Is.EqualTo(secondValue));
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.TwoItemsTestCases))]
        public void Check_CollectionAdd_TwoItemsToStart<T>(T firstValue, T secondValue)
        {
            // Arrange
            var collection = new MyLinkedList<T>();

            // Act
            collection.AddFirst(firstValue);
            collection.AddFirst(secondValue);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(collection, Is.Not.Null);

                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.First!.Previous, Is.Null);
                Assert.That(collection.First!.Next, Is.Not.Null);

                Assert.That(collection.Last, Is.Not.Null);
                Assert.That(collection.Last!.Next, Is.Null);
                Assert.That(collection.Last!.Previous, Is.Not.Null);

                Assert.That(collection.Count, Is.EqualTo(2));

                Assert.That(collection.First.Value, Is.EqualTo(secondValue));
                Assert.That(collection.First!.Next!.Value, Is.EqualTo(firstValue));

                Assert.That(collection.Last.Value, Is.EqualTo(firstValue));
                Assert.That(collection.Last!.Previous!.Value, Is.EqualTo(secondValue));

                Assert.That(collection.First, Is.Not.EqualTo(collection.Last));
            });
        }
    }
}
