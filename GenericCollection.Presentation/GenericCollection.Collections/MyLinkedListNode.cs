namespace GenericCollection.Collections
{
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
