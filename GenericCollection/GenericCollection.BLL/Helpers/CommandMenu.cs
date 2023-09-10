using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Interfaces.Abstract;
using GenericCollection.Collections;

namespace GenericCollection.BLL.Helpers
{
    public static class CommandMenu
    {
        private const string NO_COMMANDS_ERROR_MESSAGE = "There are no commands in menu";

        private static MyLinkedList<BaseCommand>? _commands;

        public static void GenerateMenu(IEnumerable<BaseCommand> commands)
        {
            _commands = new MyLinkedList<BaseCommand>(commands);
        }

        public static void PrintMenu(IWriter writer)
        {
            if (_commands is not null)
            {
                writer.WriteMyLinkedList(_commands);
            }
            else
            {
                writer.Write(ConsoleColor.Red, NO_COMMANDS_ERROR_MESSAGE);
            }
        }
    }
}
