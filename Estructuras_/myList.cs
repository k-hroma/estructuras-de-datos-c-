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

  //=========================================================
  // OPERACIONES DE INSERCIÓN
  //=========================================================

  // InsertFirst: Inserta un nodo al comienzo de la lista
  public virtual void InsertFirst(T value)
  {
    var newNode = new MyNode<T>(value);

    newNode.Next = this.head;

    this.head = newNode;
  }

  // Push: Inserta un nodo al final de la lista
  public virtual void Push(T value)
  {
    var newNode = new MyNode<T>(value);

    if (head == null)
    {
      head = newNode;
    }
    else
    {
      // Puntero auxiliar para no perder la referencia al primer nodo
      var headAux = head;

      while (headAux.Next != null)
      {
        headAux = headAux.Next;
      }

      headAux.Next = newNode;
    }
  }

  // InsertOrdered: Inserta respetando el orden ascendente
  public virtual void InsertOrdered(T value)
  {
    var newNode = new MyNode<T>(value);

    // Lista vacía o inserción al comienzo
    if (head == null || value.CompareTo(head.Value) < 0)
    {
      newNode.Next = head;
      head = newNode;
      return;
    }

    var headAux = head;

    while (headAux.Next != null && headAux.Next.Value.CompareTo(value) < 0)
    {
      headAux = headAux.Next;
    }

    newNode.Next = headAux.Next;
    headAux.Next = newNode;
  }

  // InsertUnique:
  // Inserta el nodo únicamente si no existe.
  // Mantiene el orden de la lista.
  // Devuelve el nodo encontrado o insertado.
  public virtual MyNode<T> InsertUnique(T value)
  {
    var nodeFound = Search(value);

    if (nodeFound != null)
    {
      return nodeFound;
    }

    var newNode = new MyNode<T>(value);

    if (head == null || value.CompareTo(head.Value) < 0)
    {
      newNode.Next = head;
      head = newNode;
      return newNode;
    }

    var headAux = head;

    while (headAux.Next != null && headAux.Next.Value.CompareTo(value) < 0)
    {
      headAux = headAux.Next;
    }

    newNode.Next = headAux.Next;
    headAux.Next = newNode;

    return newNode;
  }

  //=========================================================
  // OPERACIONES DE BÚSQUEDA
  //=========================================================

  // Search:
  // Busca un nodo por valor y devuelve su referencia
  public virtual MyNode<T> Search(T value)
  {
    var headAux = this.head;

    while (headAux != null)
    {
      if (headAux.Value.CompareTo(value) == 0)
      {
        return headAux;
      }

      headAux = headAux.Next;
    }

    return null;
  }

  //=========================================================
  // OPERACIONES DE ELIMINACIÓN
  //=========================================================

  // RemoveFirst:
  // Elimina y devuelve el primer elemento de la lista
  public virtual T RemoveFirst()
  {
    if (head == null)
    {
      return default(T);
    }

    T value = head.Value;

    head = head.Next;

    return value;
  }

  // Pop:
  // Elimina y devuelve el último elemento de la lista
  public virtual T Pop()
  {
    if (this.head == null)
    {
      return default(T);
    }

    // Caso: existe un único nodo
    if (this.head.Next == null)
    {
      T value = this.head.Value;

      this.head = null;

      return value;
    }

    var headAux = this.head;
    MyNode<T> previous = null;

    while (headAux.Next != null)
    {
      previous = headAux;
      headAux = headAux.Next;
    }

    previous.Next = null;

    return headAux.Value;
  }

  // Delete:
  // Elimina el nodo que contiene el valor indicado
  public virtual bool Delete(T value)
  {
    if (head == null)
    {
      return false;
    }

    // Caso: el nodo a eliminar es el primero
    if (head.Value.CompareTo(value) == 0)
    {
      head = head.Next;
      return true;
    }

    var headAux = head;
    MyNode<T> previous = null;

    while (headAux != null)
    {
      if (headAux.Value.CompareTo(value) == 0)
      {
        previous.Next = headAux.Next;
        return true;
      }

      previous = headAux;
      headAux = headAux.Next;
    }

    return false;
  }

  //=========================================================
  // OPERACIONES GENERALES
  //=========================================================

  // Clear:
  // Vacía completamente la lista
  public virtual void Clear()
  {
    head = null;
  }

  // SortList:
  // Ordena la lista reutilizando
  // RemoveFirst e InsertOrdered
  public virtual void SortList()
  {
    var orderedList = new MyList<T>();

    while (this.head != null)
    {
      T value = this.RemoveFirst();

      orderedList.InsertOrdered(value);
    }

    this.head = orderedList.Head;
  }

  //=========================================================
  // MÉTODOS AUXILIARES
  //=========================================================

  // Imprime el contenido de la lista
  public virtual void Print()
  {
    var headAux = this.head;

    while (headAux != null)
    {
      Console.Write($"{headAux.Value} -> ");
      headAux = headAux.Next;
    }

    Console.WriteLine("null");
  }

  // Devuelve true si la lista está vacía
  public virtual bool IsEmpty()
  {
    return head == null;
  }
}