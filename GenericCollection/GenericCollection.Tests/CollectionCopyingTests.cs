using GenericCollection.Collections;
using NUnit.Framework;

namespace GenericCollection.Tests
{
    [TestFixture]
    public class CollectionCopyingTests
    {
        private static readonly object[][] WrongArrayAndIndexData =
        {
            new object[]
            {
                new int[3], 4
            },
            new object[]
            {
                new int[2], 3
            },
            new object[]
            {
                new int[5], -3
            },
        };

        [Test]
        public void CheckCopying_WhenNullArray()
        {
            //Arrange
            var collection = new MyLinkedList<int>();

            //Act
            int[]? array = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => collection.CopyTo(array, 1));
        }

        [TestCaseSource(nameof(WrongArrayAndIndexData))]
        public void CheckCopying_WhenWrongIndexOutOfArray(int[] array, int arrayIndex)
        {
            //Arrange
            var collection = new MyLinkedList<int>();

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(array, arrayIndex));
        }

        [Test]
        public void CheckCopying_WhenNoSpaceInArray()
        {
            //Arrange
            var collection = new MyLinkedList<int>() { 1, 2, 3, 4 };

            //Act
            int[]? array = new int[2];

            //Assert
            Assert.Throws<ArgumentException>(() => collection.CopyTo(array, 1));
        }

        [Test]
        public void CheckCopying_WhenArray()
        {
            //Arrange
            var collection = new MyLinkedList<int>() { 1, 2, 3, 4, 5 };
            int[] arrayFromZeroIndex = new int[5];
            int[] arrayFromFirstIndex = new int[6];
            int[] arrayFromSecondIndex = new int[7];
            int[] arrayFromThirdIndex = new int[8];
            int[] arrayFromFourthIndex = new int[9];

            //Act
            collection.CopyTo(arrayFromZeroIndex, 0);
            collection.CopyTo(arrayFromFirstIndex, 1);
            collection.CopyTo(arrayFromSecondIndex, 2);
            collection.CopyTo(arrayFromThirdIndex, 3);
            collection.CopyTo(arrayFromFourthIndex, 4);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(new int[5] { 1, 2, 3, 4, 5 }, Is.EqualTo(arrayFromZeroIndex));
                Assert.That(new int[6] { 0, 1, 2, 3, 4, 5 }, Is.EqualTo(arrayFromFirstIndex));
                Assert.That(new int[7] { 0, 0, 1, 2, 3, 4, 5 }, Is.EqualTo(arrayFromSecondIndex));
                Assert.That(new int[8] { 0, 0, 0, 1, 2, 3, 4, 5 }, Is.EqualTo(arrayFromThirdIndex));
                Assert.That(new int[9] { 0, 0, 0, 0, 1, 2, 3, 4, 5 }, Is.EqualTo(arrayFromFourthIndex));
            });
        }
    }
}
