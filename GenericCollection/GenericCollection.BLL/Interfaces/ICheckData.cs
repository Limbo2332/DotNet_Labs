namespace GenericCollection.BLL.Interfaces
{
    public interface ICheckData
    {
        T CheckData<T>(string inputTextGreeting, string errorMessage) where T : IParsable<T>;

        string CheckStringData(string inputTextGreeting, string errorMessage);
    }
}
