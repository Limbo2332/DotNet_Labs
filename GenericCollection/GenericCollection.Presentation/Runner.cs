using GenericCollection.BLL.Commands.IntCommands;
using GenericCollection.BLL.Helpers;
using GenericCollection.BLL.Interfaces;
using GenericCollection.BLL.Services;
using GenericCollection.DAL.Repositories.Interfaces;

namespace GenericCollection.Presentation
{
    public class Runner : IRunner
    {
        private readonly IWriter _writer;
        private readonly ICheckData _checkData;
        private readonly IInvoker _invoker;
        private readonly IIntLinkedListRepository _repository;

        public Runner(IWriter writer, ICheckData checkData, IInvoker invoker, IIntLinkedListRepository repository)
        {
            _writer = writer;
            _checkData = checkData;
            _invoker = invoker;
            _repository = repository;
        }

        public void Run()
        {
            InvokerRuns();

            int commandsNumber = _invoker.GetCommandsCount();

            while (true)
            {
                int choiseNumber = _checkData.CheckData<int>($"Enter choise number (1-{commandsNumber}). To exit - input {commandsNumber + 1}",
                                                             $"Please, enter choise number (1-{commandsNumber}). To exit - input {commandsNumber + 1}");

                if (choiseNumber == commandsNumber + 1
                 || choiseNumber < 0
                 || choiseNumber > commandsNumber)
                {
                    return;
                }

                try
                {
                    _invoker.ExecuteCommand(choiseNumber - 1);
                }
                catch (Exception ex)
                {
                    _writer.Write(ConsoleColor.Red, ex.Message);
                }
            }
        }

        private void InvokerRuns()
        {
            _invoker.AddCommand(new GetIntCollection(_checkData, _writer, _repository));
            _invoker.AddCommand(new AddToIntCollection(_checkData, _writer, _repository));
            _invoker.AddCommand(new RemoveFromIntCollection(_checkData, _writer, _repository));
            _invoker.AddCommand(new GetItemByIndexFromIntCollection(_checkData, _writer, _repository));
            _invoker.AddCommand(new ClearIntCollection(_checkData, _writer, _repository));

            CommandMenu.GenerateMenu(_invoker.GetCommands());
            CommandMenu.PrintMenu(_writer);
        }
    }
}
