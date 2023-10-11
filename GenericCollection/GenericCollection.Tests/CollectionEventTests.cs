using GenericCollection.Collections;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace GenericCollection.Tests
{
    [TestFixture]
    public class CollectionEventTests
    {
        [Test]
        public void Check_Event_WhenAddedNewElement()
        {
            // Arrange
            var collection = new MyLinkedList<int>();
            bool eventTriggered = false;

            // Act
            collection.ItemAdded += (e) => { eventTriggered = true; };
            collection.Add(1);

            // Assert
            Assert.That(eventTriggered, Is.True);
        }

        [Test]
        public void Check_Event_WhenRemoveElement()
        {
            // Arrange
            var collection = new MyLinkedList<int>() { 1, 2, 3 };
            bool eventTriggered = false;

            // Act
            collection.ItemRemoved += (e) => { eventTriggered = true; };
            collection.Remove(1);

            // Assert
            Assert.That(eventTriggered, Is.True);
        }

        [Test]
        public void Check_Event_WhenClearElements()
        {
            // Arrange
            var collection = new MyLinkedList<int>() { 1, 2, 3 };
            bool eventTriggered = false;

            // Act
            collection.CollectionCleared += () => { eventTriggered = true; };
            collection.Clear();

            // Assert
            Assert.That(eventTriggered, Is.True);
        }
    }
}
