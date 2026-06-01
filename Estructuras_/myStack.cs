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
    this._value = value;
    this._next = null;
  }
}
public class Stack<T>
{
  private MyNode<T>? head;
  public MyNode<T>? Head
  {
    get { return head; }
    set { head = value; }
  }

  public Stack()
  {
    head = null;
  }

  public virtual void Push(T value)
  {                                     // head[5] -> [2] -> null
    var newNode = new MyNode<T>(value); // [10] -> null
    newNode.Next = head;                // [10] -> head[5] -> [2]->null
    head = newNode;                    // head[10]->[5]->[2]->null
  }

  public virtual bool IsEmpty()
  {
    return head == null;
  }

  public virtual T Pop()
  {
    if (IsEmpty())
      throw new InvalidOperationException("La pila está vacía");
    T value = head!.Value; // value = 10
    head = head.Next; // head = [5] (head.Next.Value = 5)
    return value;
  }

  public virtual T Peek()
  {
    if (IsEmpty())
      throw new InvalidOperationException("La pila está vacía");
    return head!.Value;
  }

  public int Size()
  {
    var count = 0;
    var current = this.head;
    while (current != null)
    {
      count++;
      current = current.Next;
    }
    return count;
  }
}