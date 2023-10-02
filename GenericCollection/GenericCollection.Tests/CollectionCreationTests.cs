using GenericCollection.Collections;
using NUnit.Framework;

namespace GenericCollection.Tests
{
    public class CollectionCreationTests
    {
        private static readonly List<IEnumerable<object>> itemsCollection = new List<IEnumerable<object>>
        {
            new List<object> {1, 2, 3, 4},
            new List<object> {true, false, false, true, false},
            new List<object> {"", "Hello", "Boom", "World", "1", "3"},
            new List<object> { new List<object> { } },
            new List<object> {6.7M, 7.8M},
        };

        [Test]
        public void Check_CollectionCreationIfNoItems()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MyLinkedList<object>(Enumerable.Empty<object>()));
        }

        [Test]
        public void Check_CollectionCreationIfNullCollection()
        {
            // Assert
            Assert.Throws<ArgumentNullException>(() => new MyLinkedList<object>(null));
        }

        [TestCaseSource(nameof(itemsCollection))]
        public void Check_CollectionCreationIfItems<T>(IEnumerable<T> items)
        {
            // Act
            var collection = new MyLinkedList<T>(items);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(collection, Is.Not.Null);
                Assert.That(collection, Has.Count.EqualTo(items.Count()));
            });
        }
    }
}