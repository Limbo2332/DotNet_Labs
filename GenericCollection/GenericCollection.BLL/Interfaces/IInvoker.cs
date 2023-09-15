using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.Collections;

namespace GenericCollection.BLL.Interfaces
{
    public interface IInvoker
    {
        public void AddCommand(BaseCommand command);
        public void ExecuteCommand(int index);
        public MyLinkedList<BaseCommand> GetCommands();
        public int GetCommandsCount();
    }
}
