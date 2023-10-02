﻿using GenericCollection.Collections;
using NUnit.Framework;

namespace GenericCollection.Tests
{
    public class CollectionRemovingTests
    {
        public void Check_CollectionRemoveItem_WhenEmptyCollection()
        {
            //Arrange
            var collection = new MyLinkedList<int>();

            //Act
            var result = collection.Remove(1);

            //Assert
            Assert.That(result, Is.False);
        }

        public void Check_CollectionRemoveItem_WhenNoItemInCollection()
        {
            //Arrange
            var collection = new MyLinkedList<int>() { 1, 2, 3 };

            //Act
            var result = collection.Remove(0);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.False);
                Assert.That(collection, Has.Count.EqualTo(3));
            });
        }

        [TestCase(1)]
        [TestCase('h')]
        [TestCase("Hello")]
        [TestCase(true)]
        [TestCase(27.4F)]
        public void Check_CollectionRemoveItem_WhenOneItemInCollection<T>(T value)
        {
            //Arrange
            var collection = new MyLinkedList<T>{
                value,
            };

            //Act
            var result = collection.Remove(value);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(collection, Has.Count.EqualTo(0));

                Assert.That(collection.First, Is.Null);

                Assert.That(collection.Last, Is.Null);
            });
        }

        [TestCase(1, 2)]
        [TestCase('h', '1')]
        [TestCase("Hello", "World")]
        [TestCase(true, true)]
        [TestCase(27.4F, 65.4F)]
        public void Check_CollectionRemoveItem_WhenTwoItemsInCollection<T>(T firstValue, T secondValue)
        {
            //Arrange
            var collection = new MyLinkedList<T>{
                firstValue,
                secondValue
            };

            //Act
            var result = collection.Remove(secondValue);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(collection, Has.Count.EqualTo(1));

                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.First!.Next, Is.Null);
                Assert.That(collection.First!.Previous, Is.Null);

                Assert.That(collection.Last, Is.Not.Null);
                Assert.That(collection.Last!.Next, Is.Null);
                Assert.That(collection.Last!.Previous, Is.Null);
            });
        }

        [TestCase(1, 2)]
        [TestCase('h', '1')]
        [TestCase("Hello", "World")]
        [TestCase(true, true)]
        [TestCase(27.4F, 65.4F)]
        public void Check_CollectionRemoveFirstItem_WhenTwoItemsInCollection<T>(T firstValue, T secondValue)
        {
            //Arrange
            var collection = new MyLinkedList<T>{
                firstValue,
                secondValue
            };

            //Act
            var result = collection.RemoveFirst();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(collection, Has.Count.EqualTo(1));

                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.First!.Next, Is.Null);
                Assert.That(collection.First!.Previous, Is.Null);

                Assert.That(collection.Last, Is.Not.Null);
                Assert.That(collection.Last!.Next, Is.Null);
                Assert.That(collection.Last!.Previous, Is.Null);
            });
        }

        [TestCase(1, 2, 3)]
        [TestCase('h', '1', 'h')]
        [TestCase("Hello", "World", "!")]
        [TestCase(true, true, false)]
        [TestCase(27.4F, 65.4F, 14.0F)]
        public void Check_CollectionRemoveItem_InTheStart<T>(T firstValue, T secondValue, T thirdValue)
        {
            //Arrange
            var collection = new MyLinkedList<T>()
            {
                firstValue,
                secondValue,
                thirdValue
            };

            //Act
            var result = collection.RemoveFirst();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);

                Assert.That(collection, Has.Count.EqualTo(2));

                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.First!.Previous, Is.Null);
                Assert.That(collection.First!.Next, Is.Not.Null);
                Assert.That(collection.First!.Value, Is.EqualTo(secondValue));

                Assert.That(collection.Last, Is.Not.Null);
                Assert.That(collection.Last!.Next, Is.Null);
                Assert.That(collection.Last!.Previous, Is.Not.Null);
                Assert.That(collection.Last!.Value, Is.EqualTo(thirdValue));
            });
        }

        [TestCase(1, 2, 3)]
        [TestCase('h', '1', 'h')]
        [TestCase("Hello", "World", "!")]
        [TestCase(true, true, false)]
        [TestCase(27.4F, 65.4F, 14.0F)]
        public void Check_CollectionRemoveItems_InTheStart_And_InTheEnd<T>(T firstValue, T secondValue, T thirdValue)
        {
            //Arrange
            var collection = new MyLinkedList<T>()
            {
                firstValue,
                secondValue,
                thirdValue
            };

            //Act
            var firstResult = collection.RemoveFirst();
            var secondResult = collection.RemoveLast();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstResult, Is.True);
                Assert.That(secondResult, Is.True);

                Assert.That(collection, Has.Count.EqualTo(1));

                Assert.That(collection.First, Is.Not.Null);
                Assert.That(collection.First!.Previous, Is.Null);
                Assert.That(collection.First!.Next, Is.Null);
                Assert.That(collection.First!.Value, Is.EqualTo(secondValue));

                Assert.That(collection.Last, Is.Not.Null);
                Assert.That(collection.Last!.Next, Is.Null);
                Assert.That(collection.Last!.Previous, Is.Null);
                Assert.That(collection.Last!.Value, Is.EqualTo(secondValue));
            });
        }
    }
}
