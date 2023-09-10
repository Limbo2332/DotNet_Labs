using GenericCollection.BLL.Interfaces;
using System.Globalization;

namespace GenericCollection.BLL.Services
{
    public class CheckData : ICheckData
    {
        private readonly IWriter _writer;

        public CheckData(IWriter writer)
        {
            _writer = writer;
        }

        /// <summary>
        /// Generic method to check input data, input data is mandatory
        /// </summary>
        /// <typeparam name="T">Type of input data</typeparam>
        /// <param name="inputTextGreeting">Message to help entering value</param>
        /// <param name="errorMessage">Error message if something goes wrong</param>
        /// <returns>Valid value</returns>
        T ICheckData.CheckData<T>(string inputTextGreeting, string errorMessage)
        {
            _writer.Write(ConsoleColor.Cyan, inputTextGreeting + "\n");
            string? inputText = Console.ReadLine();
            _writer.Write();

            T? type;

            while (!T.TryParse(inputText, CultureInfo.CurrentCulture, out type))
            {
                _writer.Write(ConsoleColor.Red, errorMessage + "\n");
                inputText = Console.ReadLine();
                _writer.Write();
            }

            return type;
        }

        /// <summary>
        /// Method to check string input data, input data is mandatory
        /// </summary>
        /// <param name="inputTextGreeting">Message to help entering value</param>
        /// <param name="errorMessage">Error message if something goes wrong</param>
        /// <returns>Valid string</returns>
        public string CheckStringData(string inputTextGreeting, string errorMessage)
        {
            _writer.Write(ConsoleColor.Green, inputTextGreeting + "\n");
            string? inputText = Console.ReadLine();
            _writer.Write();

            while (string.IsNullOrEmpty(inputText))
            {
                _writer.Write(ConsoleColor.Red, errorMessage + "\n");
                inputText = Console.ReadLine();
                _writer.Write();
            }

            return inputText;
        }
    }
}
