using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.BLL.Commands.IntCommands
{
    public class AddToIntCollection : BaseIntCommand
    {
        public AddToIntCollection(ICheckData checkData, IWriter writer, IIntLinkedListRepository repository) : base(checkData, writer, repository)
        {
        }

        public override string Name => "Add new item to collection";

        public override void Execute()
        {
            int newValueToAdd = _checkData.CheckData<int>("Enter new int value to add into collection.",
                                                              "Please, enter valid int data.");

            _repository.Add(newValueToAdd, OnAddEvent);
        }

        private void OnAddEvent(int value) =>
            _writer.Write(ConsoleColor.Green, $"New item with value {value} was successfully added to collection");
    }
}
