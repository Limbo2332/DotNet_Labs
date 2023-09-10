using GenericCollection.Collections;
using System.Windows.Input;

namespace GenericCollection.BLL.Interfaces
{
    public interface IInvoker
    {
        public void AddCommand(ICommand command);
        public void ExecuteCommand(int index);
        public MyLinkedList<ICommand> GetCommands();
    }
}
