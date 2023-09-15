using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.BLL.Commands.IntCommands
{
    public class GetIntCollection : BaseIntCommand
    {
        public GetIntCollection(ICheckData checkData, IWriter writer, IIntLinkedListRepository repository) 
            : base(checkData, writer, repository)
        {
        }

        public override string Name => "Get items of int collection";

        public override void Execute()
        {
            var collection = _repository.GetCollection();

            if(collection.Count == 0)
            {
                _writer.Write(ConsoleColor.Red, "No items in collection");
            }
            else
            {
                _writer.WriteMyLinkedList(collection);
            }
        }
    }
}
