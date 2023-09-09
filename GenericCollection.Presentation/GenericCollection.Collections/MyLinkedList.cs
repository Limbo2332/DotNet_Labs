using System.Collections;

namespace GenericCollection.Collections
{
    /// <summary>
    /// Linked list 
    /// </summary>
    /// <typeparam name="T">Type of linked list</typeparam>
    public class MyLinkedList<T> : IEnumerable<T>
    {
        private MyLinkedListNode<T> _firstElement = null!;
        private MyLinkedListNode<T> _lastElement = null!;

        public IEnumerator<T> GetEnumerator()
        {
            MyLinkedListNode<T> current = _firstElement;

            while (current is not null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
             return ((IEnumerable<T>)this).GetEnumerator();
        }
    }
}
