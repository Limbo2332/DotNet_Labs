using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.BLL.Commands.IntCommands
{
    public class RemoveFromIntCollection : BaseIntCommand
    {
        public RemoveFromIntCollection(ICheckData checkData, IWriter writer, IIntLinkedListRepository repository) : base(checkData, writer, repository)
        {
        }

        public override string Name => "Remove item from the collection";

        public override void Execute()
        {
            int valueToRemove = _checkData.CheckData<int>("Enter int value to remove from collection.",
                                                              "Please, enter valid int data.");

            bool isItemRemoved = _repository.Remove(valueToRemove, OnRemoveEvent);

            if(!isItemRemoved) 
            {
                _writer.Write(ConsoleColor.Red, $"Our collection doesn't contain the item with value {valueToRemove}");
            }
        }

        private void OnRemoveEvent(int value) =>
            _writer.Write(ConsoleColor.Green, $"Item with value {value} was successfully removed from collection");
    }
}
