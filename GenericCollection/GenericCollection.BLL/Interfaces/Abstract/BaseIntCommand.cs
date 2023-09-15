using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.BLL.Interfaces.Abstract
{
    public abstract class BaseIntCommand : BaseCommand
    {
        protected readonly IIntLinkedListRepository _repository;

        protected BaseIntCommand(ICheckData checkData, IWriter writer, IIntLinkedListRepository repository) : base(checkData, writer)
        {
            _repository = repository;
        }
    }
}
