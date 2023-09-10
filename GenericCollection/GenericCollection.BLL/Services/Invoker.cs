using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.Collections;

namespace GenericCollection.BLL.Services
{
    public class Invoker : IInvoker
    {
        private const string NO_COMMAND_ERROR_MESSAGE = "Command doesn't exist";

        private MyLinkedList<BaseCommand> commands = new MyLinkedList<BaseCommand>();

        public MyLinkedList<BaseCommand> GetCommands() => commands;

        public void AddCommand(BaseCommand command) => commands.Add(command);

        public void ExecuteCommand(int index)
        {
            var command = commands[index];

            if (command is null)
            {
                throw new ArgumentNullException(NO_COMMAND_ERROR_MESSAGE);
            }

            command.Value.Execute();
        }
    }
}
