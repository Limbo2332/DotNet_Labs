using GenericCollection.BLL.Interfaces;
using GenericCollection.Collections;
using System.Windows.Input;

namespace GenericCollection.BLL.Services
{
    public class Invoker : IInvoker
    {
        private MyLinkedList<ICommand> commands = new MyLinkedList<ICommand>();

        public MyLinkedList<ICommand> GetCommands() => commands;

        public void AddCommand(ICommand command) => commands.Add(command);

        public void ExecuteCommand(int index)
        {
        }
    }
}
