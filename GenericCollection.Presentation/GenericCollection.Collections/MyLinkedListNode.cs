namespace GenericCollection.Collections
{
    /// <summary>
    /// Element of linked list
    /// </summary>
    /// <typeparam name="T">Type of linked list</typeparam>
    public class MyLinkedListNode<T>
    {
        public MyLinkedListNode(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
        public MyLinkedListNode<T> Previous { get; set; } = null!;
        public MyLinkedListNode<T> Next { get; set; } = null!;
    }
}
