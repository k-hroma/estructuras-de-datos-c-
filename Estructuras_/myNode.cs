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
public class Program
{
  public static void Main()
  {

    MyNode<string> nodo1 = new MyNode<string>("Hola");

    MyNode<string> nodo2 = new MyNode<string>("Mundo");

    nodo1.Next = nodo2;

    Console.WriteLine(nodo1.Value);         // Hola
    Console.WriteLine(nodo1.Next.Value);    // Mundo

    var actual = nodo1;
    while (actual != null)
    {
      Console.WriteLine(actual.Value);
      actual = actual.Next;
    }

  }
}

