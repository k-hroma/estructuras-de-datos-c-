public class MyNode<T>
{
  private T _value;
  public T Value
  {
    get { return _value; }
    set { _value = value; }
  }

  private MyNode<T> next;
  public MyNode<T> Next
  {
    get { return next; }
    set { next = value; }
  }

  public MyNode(T value)
  {
    this._value = value;
    this.next = null;
  }
}

public class MyList<T> where T : IComparable<T>
{
  private MyNode<T> head;
  public MyNode<T> Head
  {
    get { return head; }
    set { head = value; }
  }

  public MyList()
  {
    this.head = null;
  }

  //Push: Inserta un nodo al FINAL de la lista
  public virtual void Push(T value)
  {
    var newNode = new MyNode<T>(value);

    if (head == null)
    {
      head = newNode;
    }
    else
    {
      var headAux = head;
      while (headAux.Next != null)
      {
        headAux = headAux.Next;
      }
      headAux.Next = newNode;
    }

  }
  //Pop: Elimina y devuelve el ÚLTIMO nodo

  public virtual T Pop()
  {
    if (this.head == null)
    {
      return null;
    }
    //si solo hay un elemento
    if (this.head.Next == null)
    {
      var value = this.head.Value;
      this.head = null;
      return value;
    }

    var headAux = this.head;
    var previous = null;

    while (headAux.Next != null)
    {
      previous = headAux;
      headAux = headAux.Next;
    }

    previous.Next = null;

    return headAux.Value;


  }


}