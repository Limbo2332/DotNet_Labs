using GenericCollection.BLL.Interfaces;
using GenericCollection.Collections;

namespace GenericCollection.BLL.Services
{
    public class Writer : IWriter
    {
        private const string EMPTY_LIST_MESSAGE = "List doesn't have elements";

        /// <summary>
        /// Writes to console with specified color
        /// </summary>
        /// <param name="color">Color of the text</param>
        /// <param name="value">Text value</param>
        public void Write(ConsoleColor color = ConsoleColor.Gray, string value = "")
        {
            Console.ForegroundColor = color;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public void WriteMyLinkedList<T>(MyLinkedList<T>? list)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            if(list is null)
            {
                Write(ConsoleColor.Red, EMPTY_LIST_MESSAGE);
                return;
            }

            foreach (T item in list)
            {
                Console.WriteLine(item!.ToString());
            }
        }
    }
}
