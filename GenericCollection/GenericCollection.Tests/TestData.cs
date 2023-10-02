using NUnit.Framework;

namespace GenericCollection.Tests
{
    internal class TestData
    {
        internal static IEnumerable<TestCaseData> OneItemTestCase()
        {
            yield return new TestCaseData(1);
            yield return new TestCaseData('h');
            yield return new TestCaseData("Hello");
            yield return new TestCaseData(true);
            yield return new TestCaseData(27.4F);
        }

        internal static IEnumerable<TestCaseData> TwoItemsTestCases()
        {
            yield return new TestCaseData(1, 2);
            yield return new TestCaseData('h', '1');
            yield return new TestCaseData("Hello", "World");
            yield return new TestCaseData(true, true);
            yield return new TestCaseData(27.4F, 65.4F);
        }

        internal static IEnumerable<TestCaseData> ThreeItemsTestCases()
        {
            yield return new TestCaseData(1, 2, 3);
            yield return new TestCaseData('h', '1', 'h');
            yield return new TestCaseData("Hello", "World", "!");
            yield return new TestCaseData(true, true, false);
            yield return new TestCaseData(27.4F, 65.4F, 14.0F);
        }
    }
}
