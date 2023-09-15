namespace GenericCollection.BLL.Interfaces.Abstract
{
    public abstract class BaseCommand
    {
        protected readonly ICheckData _checkData;
        protected readonly IWriter _writer;

        protected BaseCommand(ICheckData checkData, IWriter writer)
        {
            _checkData = checkData;
            _writer = writer;
        }

        public abstract string Name { get; }

        public abstract void Execute();

        public override string ToString()
        {
            return Name;
        }
    }
}
