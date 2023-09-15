using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.BLL.Commands.IntCommands
{
    public class GetItemByIndexFromIntCollection : BaseIntCommand
    {
        public GetItemByIndexFromIntCollection(ICheckData checkData, IWriter writer, IIntLinkedListRepository repository) : base(checkData, writer, repository)
        {
        }

        public override string Name => "Write item value from the collection by given index";

        public override void Execute()
        {
            int indexOfItem = _checkData.CheckData<int>("Enter index of item you want to get",
                                                            "Please, enter valid int data.");

            try
            {
                int value = _repository.GetItemByIndex(indexOfItem);

                _writer.Write(ConsoleColor.Green, $"Value of the item by index {indexOfItem} is {value}");
            }
            catch (Exception ex)
            {
                _writer.Write(ConsoleColor.Red, ex.Message);
            }
        }
    }
}
