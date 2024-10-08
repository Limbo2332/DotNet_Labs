﻿using GenericCollection.Collections;
using NUnit.Framework;

namespace GenericCollection.Tests
{
    [TestFixture]
    public class CollectionClearingTests
    {
        [Test]
        public void Check_ClearCollection_WhenNoItems()
        {
            //Arrange
            var collection = new MyLinkedList<int>();

            //Act
            collection.Clear();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(collection, Has.Count.EqualTo(0));
                Assert.That(collection.First, Is.Null);
                Assert.That(collection.Last, Is.Null);
            });
        }

        [TestCaseSource(typeof(TestData), nameof(TestData.OneItemTestCase))]
        public void Check_ClearCollection_WhenItems<T>(T value)
        {
            //Arrange
            var collection = new MyLinkedList<T>() { value };

            //Act
            collection.Clear();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(collection, Has.Count.EqualTo(0));
                Assert.That(collection.First, Is.Null);
                Assert.That(collection.Last, Is.Null);
            });
        }
    }
}
