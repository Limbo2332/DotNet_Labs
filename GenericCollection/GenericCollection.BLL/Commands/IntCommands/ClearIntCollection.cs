using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.BLL.Commands.IntCommands
{
    public class ClearIntCollection : BaseIntCommand
    {
        public ClearIntCollection(ICheckData checkData, IWriter writer, IIntLinkedListRepository repository) : base(checkData, writer, repository)
        {
            _repository.SetOnClearEvent(OnClearEvent);
        }

        public override string Name => "Clear all items of the collection";

        public override void Execute()
        {
            _repository.Clear();
        }

        private void OnClearEvent() =>
            _writer.Write(ConsoleColor.Green, "Collection was successfully cleared");
    }
}
