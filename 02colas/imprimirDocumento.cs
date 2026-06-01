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
    else
    {
      T value = first!.Value;
      first = first.Next;
      if (first == null)
      {
        last = null;
      }
      return value;
    }
  }

  public virtual T Front()
  {
    return IsEmpty() ? default : First!.Value;
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

public interface IDocumento
{
  string Nombre { get; set; }
  int Paginas { get; set; }
  string Usuario { get; set; }
}

public class Documento : IDocumento
{
  public string Nombre { get; set; }
  public int Paginas { get; set; }
  public string Usuario { get; set; }

  public Documento(string nombre, int paginas, string usuario)
  {
    Nombre = nombre;
    Paginas = paginas;
    Usuario = usuario;
  }

  public override string ToString()
  {
    return $"{Nombre}, {Paginas} págs. por {Usuario}";
  }
}

public class Impresora
{
  private MyQueue<IDocumento> cola;
  public MyQueue<IDocumento> Cola
  {
    get { return cola; }
    set { cola = value; }
  }

  private bool estaImprimiendo;
  public bool EstaImprimiendo
  {
    get { return estaImprimiendo; }
    set { estaImprimiendo = value; }
  }

  public Impresora()
  {
    this.cola = new MyQueue<IDocumento>();
    this.estaImprimiendo = false;
  }

  public virtual void AgregarTrabajo(IDocumento doc)
  {
    Cola.Enqueue(doc);
    Console.WriteLine($"{doc.Nombre}, {doc.Paginas} págs. agregado a la cola de impresio por {doc.Usuario}");

    var trabajosEspera = Cola.Size();
    Console.WriteLine($"Hay {trabajosEspera} trabajos en espera.");
  }

  public virtual void ImprimirUno()
  {
    if (Cola.IsEmpty())
    {
      Console.WriteLine("La cola de impresión está vacía");
      EstaImprimiendo = false;
      return;
    }
    var doc = Cola.Dequeue();
    Console.WriteLine($"Imprimiendo {doc.Nombre}, {doc.Paginas} págs. agregado a la cola de impresio por {doc.Usuario}");
    EstaImprimiendo = true;

  }

  public virtual void ImprimirTodo()
  {
    while (!Cola.IsEmpty())
    {
      var doc = Cola.Dequeue();
      EstaImprimiendo = true;
      Console.WriteLine($"Imprimiendo {doc.Nombre}, {doc.Paginas} págs. agregado a la cola de impresio por {doc.Usuario}");

      var trabajosEspera = Cola.Size();
      Console.WriteLine($"Hay {trabajosEspera} trabajos restantes.");

    }
    if (Cola.IsEmpty())
    {
      Console.WriteLine("No hay trabajos pendientes");
      EstaImprimiendo = false;
    }
  }

  public virtual void ImprimirPrimero()
  {
    Console.WriteLine(Cola.Front());
  }
}

public class Program
{
  public static void Main()
  {
    Impresora impresoraHP = new Impresora();
    Documento documentoUno = new Documento("Croma en los Andes", 123, "Croma");
    Documento documentoDos = new Documento("Croma en los Alpes", 123, "Croma");
    Documento documentoTres = new Documento("Croma en los Alamos", 123, "Croma");
    impresoraHP.AgregarTrabajo(documentoUno);
    impresoraHP.AgregarTrabajo(documentoDos);
    impresoraHP.AgregarTrabajo(documentoTres);
    Console.WriteLine("--------------");
    impresoraHP.ImprimirPrimero();
    Console.WriteLine("--------------");
    impresoraHP.ImprimirUno();
    impresoraHP.ImprimirTodo();
    impresoraHP.ImprimirUno();

  }
}