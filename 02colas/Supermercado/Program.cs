public class MyNode<T>
{
  private T _value;
  public T Value
  {
    get { return _value; }
    set { _value = value; }
  }

  private MyNode<T>? next;
  public MyNode<T>? Next
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
public class MyQueue<T>
{
  private MyNode<T>? first;
  public MyNode<T>? First
  {
    get { return first; }
    set { first = value; }
  }

  private MyNode<T>? last;
  public MyNode<T>? Last
  {
    get { return last; }
    set { last = value; }
  }

  public MyQueue()
  {
    this.first = null;
    this.last = null;
  }

  public bool IsEmpty()
  {
    return first == null && last == null;
  }

  public virtual void Enqueue(T value)
  {
    var newNode = new MyNode<T>(value);
    if (IsEmpty())
    {
      first = newNode;
    }
    else
    {
      last!.Next = newNode;
    }
    last = newNode;
  }

  public virtual T Dequeue()
  {
    if (IsEmpty())
    {
      throw new InvalidOperationException("La cola está vacía");
    }
    var value = first!.Value;
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
    {
      throw new InvalidOperationException("La cola está vacía");
    }
    else
    {
      return first!.Value;
    }
  }

  public virtual int Size()
  {
    int count = 0;
    var current = first;

    while (current != null)
    {
      count++;
      current = current.Next;
    }

    return count;
  }
}

public enum TipoCliente
{
  Normal,
  Mayor,
  Excelencia
}
public class Cliente
{
  public TipoCliente TipoCliente { get; set; }

  public Cliente(TipoCliente tipoCliente)
  {
    TipoCliente = tipoCliente;
  }
}
public class Supermercado
{
  private MyQueue<Cliente> mayores;
  public MyQueue<Cliente> Mayores
  {
    get { return mayores; }
    set { mayores = value; }
  }

  private MyQueue<Cliente> excelencia;
  public MyQueue<Cliente> Excelencia
  {
    get { return excelencia; }
    set { excelencia = value; }
  }

  private MyQueue<Cliente> normales;
  public MyQueue<Cliente> Normales
  {
    get { return normales; }
    set { normales = value; }
  }


  public Supermercado()
  {
    this.mayores = new MyQueue<Cliente>();
    this.excelencia = new MyQueue<Cliente>();
    this.normales = new MyQueue<Cliente>();
  }

  public bool IsEmptySupermercado()
  {
    return Mayores.IsEmpty() && Excelencia.IsEmpty() && Normales.IsEmpty();
  }

  public virtual void AddClientQueue(Cliente cliente)
  {
    switch (cliente.TipoCliente)
    {
      case TipoCliente.Mayor:
        mayores.Enqueue(cliente);
        break;
      case TipoCliente.Excelencia:
        excelencia.Enqueue(cliente);
        break;
      case TipoCliente.Normal:
        normales.Enqueue(cliente);
        break;
      default:
        Console.WriteLine("Cliente no registrado");
        break;
    }
  }

  public virtual Cliente RemoveClient()
  {
    if (IsEmptySupermercado())
    {
      throw new InvalidOperationException("La cola está vacía");
    }

    if (!Mayores.IsEmpty())
    {
      return Mayores.Dequeue();
    }

    if (!Excelencia.IsEmpty())
    {
      return Excelencia.Dequeue();
    }

    return Normales.Dequeue();
  }

  public virtual void RemoveAllClients()
  {
    while (!IsEmptySupermercado())
    {
      var proxCliente = RemoveClient();
      var tipoCliente = proxCliente.TipoCliente;
      Console.WriteLine(tipoCliente);
    }
    Console.WriteLine("Fin de fiesta");
  }
}

public class Program
{
  public static void Main()
  {
    Supermercado superMayorista = new Supermercado();
    Cliente clienteMayorUno = new Cliente(TipoCliente.Mayor);
    Cliente clienteExcelenciaUno = new Cliente(TipoCliente.Excelencia);
    Cliente clienteNormalUno = new Cliente(TipoCliente.Normal);
    Cliente clienteMayorDos = new Cliente(TipoCliente.Mayor);
    Cliente clienteExcelenciaDos = new Cliente(TipoCliente.Excelencia);
    Cliente clienteNormalDos = new Cliente(TipoCliente.Normal);

    superMayorista.AddClientQueue(clienteMayorUno);
    superMayorista.AddClientQueue(clienteExcelenciaUno);
    superMayorista.AddClientQueue(clienteNormalUno);
    superMayorista.AddClientQueue(clienteMayorDos);
    superMayorista.AddClientQueue(clienteExcelenciaDos);
    superMayorista.AddClientQueue(clienteNormalDos);

    var cliente = superMayorista.RemoveClient();
    Console.WriteLine(cliente.TipoCliente);
    Console.WriteLine("-----------------");
    superMayorista.RemoveAllClients();

  }
}