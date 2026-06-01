public class MyNode<T>
{
  private T _value;
  public T Value
  {
    get { return _value; }
    set { _value = value; }
  }

  private MyNode<T>? _next;
  public MyNode<T>? Next
  {
    get { return _next; }
    set { _next = value; }
  }

  public MyNode(T value)
  {
    _value = value;
    _next = null;
  }
}

public class MyQueue<T>
{
  private MyNode<T> first;
  public MyNode<T> First
  {
    get { return first; }
    set { first = value; }
  }

  private MyNode<T> last;
  public MyNode<T> Last
  {
    get { return last; }
    set { last = value; }
  }

  public MyQueue()
  {
    first = null;
    last = null;
  }

  public bool IsEmpty()
  {
    return first == null && last == null;
  }

  public virtual void Enqueue(T value)
  {
    var newNode = new MyNode<T>(value); //[10] -> null
    if (IsEmpty())
    {
      first = newNode; // [10]-->null
    }
    else
    {
      last.Next = newNode; // [8]->(last)[9]->(apuntaba a null)agrego[10]->null
    }
    last = newNode; //[10] //last.Value = 10 // last.Next = null
  }

  public virtual T Dequeue()
  {
    if (IsEmpty())
    {
      throw new InvalidOperationException("La cola está vacía");
    }
    T value = first.Value;
    first = first.Next;
    if (first == null)
    {
      last = null;
    }
    return value;
  }

  public virtual T Front()
  {
    if (IsEmpty())
      throw new InvalidOperationException("La cola está vacía");

    return first!.Value;
  }

  public virtual int Size()
  {
    var count = 0;
    var current = first;
    while (current != null)
    {
      count++;
      current = current.Next;
    }
    return count;
  }
}