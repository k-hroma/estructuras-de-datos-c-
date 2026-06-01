using System.ComponentModel;
using Microsoft.VisualBasic;

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
    this.head = null;
  }
  public virtual bool IsEmpty()
  {
    return head == null;
  }
  public virtual void Push(T value)
  {
    var newNode = new MyNode<T>(value);
    newNode.Next = head;
    this.head = newNode;
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

public class History
{
  private MyStack<string> urls;

  private MyStack<string>? aux;

  public History()
  {
    urls = new MyStack<string>();
  }

  public void AddUrl(string url)
  {
    urls.Push(url);
  }

  public string RemoveLastUrl()
  {
    return urls.Pop();
  }

  public bool IsEmpty()
  {
    return urls.IsEmpty();
  }

  public void PrintHistorial()
  {
    aux = new MyStack<string>();

    while (!urls.IsEmpty())
    {
      var url = urls.Pop();

      aux.Push(url);

      Console.WriteLine(url);
    }

    Console.WriteLine("-------------------");

    while (!aux.IsEmpty())
    {
      var url = aux.Pop();

      urls.Push(url);

      Console.WriteLine(url);
    }
  }
}

public class Browser
{
  private string currentUrl;

  public string CurrentUrl
  {
    get { return currentUrl; }
    set { currentUrl = value; }
  }

  private History history;
  public History History
  {
    get { return history; }
    set { history = value; }
  }

  public Browser(string url)
  {
    history = new History();

    currentUrl = url;

    history.AddUrl(currentUrl);
  }

  public bool IsEmpty()
  {
    return history.IsEmpty();
  }
  public void Visit(string url)
  {
    currentUrl = url;

    history.AddUrl(currentUrl);
  }

  public string Back()
  {
    currentUrl = history.RemoveLastUrl();

    return currentUrl;
  }

  public void PrintHistorial()
  {
    history.PrintHistorial();
  }
}

public static class Utils
{
  public static void Print(History historial)
  {
    var printHistorial = historial;
    var aux = new MyStack<string>();

    while (!printHistorial.IsEmpty())
    {
      var url = printHistorial.RemoveLastUrl();
      aux.Push(url);
      Console.WriteLine(url);
    }
    Console.WriteLine("-------------------");

    while (!aux.IsEmpty())
    {
      var url = aux.Pop();
      printHistorial.AddUrl(url);
      Console.WriteLine(url);
    }

  }
}
public class Program
{
  public static void Main()
  {
    Browser miBrowser = new Browser("www.google.com");
    miBrowser.Visit("www.croma.com");
    miBrowser.Visit("www.fuks.com");
    var back = miBrowser.Back();
    Console.WriteLine(back);
    miBrowser.Visit("www.mex.com");
    miBrowser.PrintHistorial();
    Utils.Print(miBrowser.History);
  }
}