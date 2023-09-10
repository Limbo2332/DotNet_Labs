namespace GenericCollection.BLL.Interfaces.Abstract
{
    public abstract class BaseCommand
    {
        public abstract string Name { get; }

        public abstract void Execute();

        public override string ToString()
        {
            return Name;
        }
    }
}
