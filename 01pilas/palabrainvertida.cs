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
public class MyStack<T>
{
  private MyNode<T>? head;
  public MyNode<T>? Head
  {
    get { return head; }
    set { head = value; }
  }

  public MyStack()
  {
    head = null;
  }

  public virtual void Push(T value)
  {
    var newNode = new MyNode<T>(value);
    newNode.Next = head;
    this.head = newNode;
  }

  public virtual bool IsEmpty()
  {
    return head == null;
  }

  public virtual T Pop()
  {
    if (IsEmpty())
      throw new InvalidOperationException("La pila está vacía");
    T value = head!.Value;
    head = head.Next;
    return value;
  }

}

public static class WordUtils
{
  public static string InvertirPalabra(string palabra)
  {
    MyStack<char> pila = new MyStack<char>();

    foreach (char letra in palabra)
    {
      pila.Push(letra);
    }

    string palabraInvertida = "";

    while (!pila.IsEmpty())
    {
      palabraInvertida += pila.Pop();
    }

    return palabraInvertida;
  }
}
// Ejemplo de palabra invertida
public class Program
{
  public static void Main()
  {
    Console.WriteLine(WordUtils.InvertirPalabra("HOLA"));
  }
}