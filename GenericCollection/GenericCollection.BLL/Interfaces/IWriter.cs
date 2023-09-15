using GenericCollection.Collections;

namespace GenericCollection.BLL.Interfaces
{
    public interface IWriter
    {
        void Write(ConsoleColor color = ConsoleColor.Gray, string value = "");
        void WriteMyLinkedList<T>(MyLinkedList<T> list);
    }
}
