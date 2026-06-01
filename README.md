# Programación II - Unidad 3: Estructuras de Datos

> _"Si ha elegido las estructuras de datos correctas y las cosas están bien organizadas, los algoritmos casi siempre serán cortos y simples."_ — Rob Pike

---

## 1. Introducción: ¿Por qué estructuras dinámicas?

En esta unidad estudiamos el manejo de **estructuras de datos dinámicas**.

**¿Por qué dinámicas?** A diferencia de los vectores y matrices (cuyo tamaño es fijo y no varía durante la ejecución del programa), las estructuras dinámicas pueden **crecer o reducirse** a medida que se ejecuta el programa.

### 1.1 Cómo se organiza la memoria de un proceso

Cuando un proceso se ejecuta, se divide en dos grandes segmentos: **código** y **datos**. A su vez, el segmento de datos se subdivide en:

| Segmento             | Descripción                                                                                              |
| -------------------- | -------------------------------------------------------------------------------------------------------- |
| **Código**           | Contiene el código ejecutable del programa                                                               |
| **Datos**            | Contiene variables locales, globales y estáticas                                                         |
| **Stack (Pila)**     | Utilizado para "apilar" las llamadas a funciones, sus parámetros y variables locales                     |
| **Heap (Montículo)** | Contiene variables cuya reserva de memoria es dinámica. Este segmento puede crecer o disminuir su tamaño |

```
┌─────────────────────────────┐
│  Datos de control (PCB)     │
├─────────────────────────────┤
│  Segmento de Código         │
├─────────────────────────────┤
│  Datos                      │
│  Stack                      │  ← Segmento de Datos
│  Heap                       │
└─────────────────────────────┘
```

#### Stack (Pila de llamadas)

El **stack** es la porción de memoria que el sistema operativo utiliza para almacenar las llamadas a función y crear las variables locales y parámetros de cada función.

- Funciona con algoritmo **LIFO** (_Last In, First Out_ — último en entrar, primero en salir).
- Cuando invocamos una función, ésta (junto con sus parámetros y variables locales) se crea en el stack.
- Cuando la función retorna, todas las variables creadas en el stack son **destruidas automáticamente**, liberando el espacio.

> **💡 Clarificación:** El stack es automático. No tenés que reservar ni liberar memoria manualmente. El sistema operativo lo hace por vos cada vez que llamás o retornás de una función.

#### Heap (Montículo)

Las **estructuras dinámicas** estarán alojadas en el segmento denominado **heap**.

- Es parte de la memoria RAM asignada al proceso en ejecución.
- A diferencia del stack, **no hay ningún algoritmo automático** para almacenar los datos.
- La responsabilidad de la **reserva y liberación de memoria** recae en el programador.

> **💡 Nota sobre lenguajes modernos:** En lenguajes de bajo nivel como C o C++, el programador debe reservar y liberar memoria del heap manualmente (usando `malloc`/`free` o `new`/`delete`). En lenguajes de alto nivel como **Java**, **Python**, **JavaScript** o **TypeScript**, existen mecanismos automáticos de gestión de memoria (como el _garbage collector_), por lo que no es necesario hacerlo a mano.

---

## 2. El Tipo de Dato Nodo (Node)

Todas las estructuras dinámicas que estudiaremos (pilas, colas y listas) se construyen a partir de un elemento básico llamado **Nodo**.

Un nodo es simplemente un objeto que contiene:

1. **Un valor** (el dato que queremos guardar)
2. **Una referencia** (puntero) al siguiente nodo

```
┌──────────────────────┬──────────────────────┐
│   _value: T          │   _next: ???         │
│   (el dato real)     │   (¿a dónde apunta?) │
└──────────────────────┴──────────────────────┘
* _value guarda el dato (un número, un string, un objeto).
* _next no guarda un dato. Guarda la dirección de la siguiente caja en la cadena.
```

### Implementación genérica del Nodo

Usamos **tipos genéricos (generics)** para que el nodo pueda almacenar cualquier tipo de dato: números, strings, objetos, etc.

```typescript
class MyNode<T> {
  private _value: T;
  private _next: MyNode<T> | null;

  constructor(value: T) {
    this._value = value;
    this._next = null;
  }

  // Getter para obtener el valor
  public get value(): T {
    return this._value;
  }

  // Getter para obtener el siguiente nodo
  public get next(): MyNode<T> | null {
    return this._next;
  }

  // Setter para enlazar con otro nodo
  public set next(n: MyNode<T> | null) {
    this._next = n;
  }
}
```

```csharp
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
```

### Ejemplo práctico: Creando nodos

```typescript
// Creamos nodos que almacenan strings
const nodo1 = new MyNode<string>("Hola");
const nodo2 = new MyNode<string>("Mundo");

// Enlazamos el nodo1 con el nodo2
nodo1.next = nodo2;

// Recorremos manualmente
let actual: MyNode<string> | null = nodo1;
while (actual !== null) {
  console.log(actual.value); // Imprime: Hola, Mundo
  actual = actual.next;
}
```

```csharp
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

```

```plain
nodo1 ("Hola")  →  nodo2 ("Mundo")  →  null
   │                    │
   └─ _next ────────────┘              └─ _next

> El null del último nodo es la señal de que la cadena terminó. Es como el punto final de una oración.
```

```typescript
let actual: MyNode<number> | null = a;
```

```
Guarda la referencia (la dirección de memoria) al objeto nodo completo:
actual ──→  ┌────────────────────────┐
            │  _value: 10            │
            │  _next: ───────┐       │
            └────────────────┼───────┘
                             │
                             ▼
                    ┌────────────────────────┐
                    │  _value: 20            │
                    │  _next: ───────┐       │
                    └────────────────┼───────┘
                                     │
                                     ▼
                            ┌────────────────────────┐
                            │  _value: 30            │
                            │  _next: null           │
                            └────────────────────────┘
Por eso podés hacer actual.value (accedés al dato) y actual.next (accedés al siguiente objeto).
```

> **💡 Concepto clave:** Los nodos son como eslabones de una cadena. Cada uno "sabe" quién es el siguiente, pero no tiene idea de quién es el anterior ni de cuántos nodos hay en total. La estructura completa se forma siguiendo estos enlaces uno por uno.

---

## 3. Pilas (Stack)

### 3.1 Concepto

Las **pilas** son estructuras de datos que almacenan y recuperan sus elementos siguiendo un orden estricto: **LIFO** (_Last In, First Out_ — último en entrar, primero en salir).

**Definición formal:** Una pila es una colección de elementos de un mismo tipo de dato a los que solo se puede acceder (introducir y eliminar) por un único lugar denominado **cabeza de pila** (también llamado _tope_ o _top_).

#### Analogía práctica

Imaginá una **pila de platos** o una **pila de libros**:

- El primer plato que ponés en la mesa queda **abajo de todo**.
- El último plato que ponés queda **arriba de todo**.
- Cuando necesitás un plato, solo podés agarrar el que está **arriba** (el último que pusiste).

```
        ┌─────────┐  ← Agrego/Elimino elementos por acá (TOPE)
        │ Libro N │
        ├─────────┤
        │   ...   │
        ├─────────┤
        │ Libro 2 │
        ├─────────┤
        │ Libro 1 │  ← Primer elemento en entrar, último en salir
        └─────────┘
```

#### ¿Cuándo usar pilas?

- Para **invertir el orden** de un conjunto de elementos.
- Para conocer el **historial de acciones** (deshacer/rehacer en editores de texto).
- Para el **historial de navegación** de páginas web (botón "atrás").
- Para evaluar **expresiones matemáticas** (notación polaca inversa).
- Para algoritmos de **backtracking** (volver atrás en laberintos, sudokus, etc.).

> **⚠️ Importante:** Las operaciones de lectura sobre una pila son **destructivas**. Para leer un elemento, debemos removerlo de la pila. No podemos "ver" el elemento del medio sin sacar primero todos los que están arriba.

### 3.2 Implementación de la Pila

```typescript
class MyNode<T> {
  public value: T;
  public next: MyNode<T> | null;

  constructor(value: T) {
    this.value = value;
    this.next = null;
  }
}

class Stack<T> {
  private head: MyNode<T> | null;

  constructor() {
    this.head = null;
  }

  /**
   * Push: Inserta un elemento en el tope de la pila
   * Complejidad: O(1)
   */
  public push(value: T): void {
    const newNode = new MyNode<T>(value);
    newNode.next = this.head; // El nuevo nodo apunta al anterior tope
    this.head = newNode; // El nuevo nodo ahora es el tope
  }

  /**
   * Pop: Elimina y devuelve el elemento del tope de la pila
   * Complejidad: O(1)
   */
  public pop(): T | null {
    if (this.isEmpty()) {
      return null; // La pila está vacía
    }
    const value = this.head!.value; // Guardamos el valor del tope
    this.head = this.head!.next; // El tope ahora es el siguiente
    return value;
  }

  /**
   * Peek: Devuelve el elemento del tope SIN eliminarlo
   * (Operación extra, no destructiva)
   */
  public peek(): T | null {
    return this.isEmpty() ? null : this.head!.value;
  }

  /**
   * isEmpty: Verifica si la pila está vacía
   */
  public isEmpty(): boolean {
    return this.head === null;
  }

  /**
   * size: Devuelve la cantidad de elementos
   * (Operación extra para facilitar debugging)
   */
  public size(): number {
    let count = 0;
    let current = this.head;
    while (current !== null) {
      count++;
      current = current.next;
    }
    return count;
  }
}
```

### 3.3 Operaciones sobre una Pila

| Operación                | Descripción                             | Complejidad |
| ------------------------ | --------------------------------------- | ----------- |
| **create** (constructor) | Inicializa la pila vacía                | O(1)        |
| **push** (insertar)      | Agrega un elemento en el tope           | O(1)        |
| **pop** (eliminar)       | Remueve y devuelve el elemento del tope | O(1)        |
| **isEmpty**              | Verifica si la pila está vacía          | O(1)        |
| **peek**                 | Muestra el tope sin removerlo           | O(1)        |

### 3.4 Ejemplo práctico completo: Invirtiendo palabras

```typescript
// Ejemplo: Usar una pila para invertir una palabra

function invertirPalabra(palabra: string): string {
  const pila = new Stack<string>();

  // Paso 1: Apilamos cada letra
  for (const letra of palabra) {
    pila.push(letra);
  }

  console.log(`Tamaño de la pila: ${pila.size()}`); // 5 para "Hola"

  // Paso 2: Desapilamos (sale en orden inverso)
  let palabraInvertida = "";
  while (!pila.isEmpty()) {
    palabraInvertida += pila.pop();
  }

  return palabraInvertida;
}

// Prueba
console.log(invertirPalabra("Hola")); // "aloH"
console.log(invertirPalabra("Mundo")); // "odnuM"
console.log(invertirPalabra("12345")); // "54321"
```

### 3.5 Ejemplo práctico: Historial de navegación

```typescript
// Simulamos el botón "Atrás" de un navegador

class Navegador {
  private historial: Stack<string>;
  private paginaActual: string;

  constructor() {
    this.historial = new Stack<string>();
    this.paginaActual = "about:blank";
  }

  navegarA(url: string): void {
    // Guardamos la página actual en el historial antes de ir a la nueva
    this.historial.push(this.paginaActual);
    this.paginaActual = url;
    console.log(`→ Navegando a: ${url}`);
  }

  atras(): void {
    if (this.historial.isEmpty()) {
      console.log("❌ No hay páginas en el historial");
      return;
    }
    this.paginaActual = this.historial.pop()!;
    console.log(`← Volviendo a: ${this.paginaActual}`);
  }

  verPaginaActual(): string {
    return this.paginaActual;
  }
}

// Simulación
const chrome = new Navegador();
chrome.navegarA("google.com"); // → Navegando a: google.com
chrome.navegarA("facebook.com"); // → Navegando a: facebook.com
chrome.navegarA("youtube.com"); // → Navegando a: youtube.com

chrome.atras(); // ← Volviendo a: facebook.com
chrome.atras(); // ← Volviendo a: google.com
chrome.atras(); // ← Volviendo a: about:blank
chrome.atras(); // ❌ No hay páginas en el historial
```

---

## 4. Colas (Queue)

### 4.1 Concepto

Las **colas** son estructuras de datos que almacenan y recuperan sus elementos siguiendo un orden estricto: **FIFO** (_First In, First Out_ — primero en entrar, primero en salir).

**Definición formal:** Una cola es una colección de elementos que se insertan por un extremo (llamado **final** o _tail/rear_) y se extraen por el otro extremo (llamado **frente** o _head/front_).

#### Analogía práctica

Imaginá una **fila de personas** esperando para ser atendidas en un banco:

- La primera persona que llega es la **primera en ser atendida**.
- Las nuevas personas se colocan al **final** de la fila.
- Nadie se "cuela" en el medio; el orden de llegada se respeta estrictamente.

```
   Ingresan →  ┌─────┬─────┬─────┬─────┬─────┬─────┐  → Salen
               │  6  │  5  │  4  │  3  │  2  │  1  │
               └─────┴─────┴─────┴─────┴─────┴─────┘
                  ↑                                   ↑
                FINAL                              FRENTE
```

#### ¿Cuándo usar colas?

- Para modelar **filas de espera** (bancos, supermercados, call centers).
- Para **colas de impresión** (los trabajos se procesan en orden de llegada).
- Para **buffers de datos** (streaming de video, lectura de archivos).
- Para algoritmos de **búsqueda en anchura** (BFS) en grafos.
- Para sistemas de **mensajería** y procesamiento de tareas asíncronas.

> **⚠️ Importante:** Al igual que las pilas, las operaciones de lectura sobre una cola son **destructivas**. Para leer un elemento, debemos removerlo del frente.

### 4.2 Implementación de la Cola

```typescript
class MyNode<T> {
  public value: T;
  public next: MyNode<T> | null;

  constructor(value: T) {
    this.value = value;
    this.next = null;
  }
}

class Queue<T> {
  private first: MyNode<T> | null; // Frente (de donde sale)
  private last: MyNode<T> | null; // Final (donde entra)

  constructor() {
    this.first = null;
    this.last = null;
  }

  /**
   * Enqueue: Agrega un elemento al final de la cola
   * Complejidad: O(1)
   */
  public enqueue(value: T): void {
    const newNode = new MyNode<T>(value);

    if (this.isEmpty()) {
      // Si está vacía, el nuevo nodo es tanto el primero como el último
      this.first = newNode;
    } else {
      // El último nodo actual apunta al nuevo
      this.last!.next = newNode;
    }

    this.last = newNode; // El nuevo nodo ahora es el último
  }

  /**
   * Dequeue: Elimina y devuelve el elemento del frente
   * Complejidad: O(1)
   */
  public dequeue(): T | null {
    if (this.isEmpty()) {
      return null; // La cola está vacía
    }

    const value = this.first!.value; // Guardamos el valor del frente
    this.first = this.first!.next; // El frente avanza al siguiente

    // Si sacamos el último elemento, last también debe ser null
    if (this.first === null) {
      this.last = null;
    }

    return value;
  }

  /**
   * Front: Devuelve el elemento del frente SIN eliminarlo
   */
  public front(): T | null {
    return this.isEmpty() ? null : this.first!.value;
  }

  /**
   * isEmpty: Verifica si la cola está vacía
   */
  public isEmpty(): boolean {
    return this.first === null && this.last === null;
  }

  /**
   * size: Devuelve la cantidad de elementos
   */
  public size(): number {
    let count = 0;
    let current = this.first;
    while (current !== null) {
      count++;
      current = current.next;
    }
    return count;
  }
}
```

### 4.3 Operaciones sobre una Cola

| Operación                | Descripción                               | Complejidad |
| ------------------------ | ----------------------------------------- | ----------- |
| **create** (constructor) | Inicializa la cola vacía                  | O(1)        |
| **enqueue** (agregar)    | Inserta un elemento al final              | O(1)        |
| **dequeue** (eliminar)   | Remueve y devuelve el elemento del frente | O(1)        |
| **front**                | Muestra el frente sin removerlo           | O(1)        |
| **isEmpty**              | Verifica si la cola está vacía            | O(1)        |

> **💡 Por qué mantenemos dos punteros (first y last):** Si solo tuviéramos `first`, agregar un elemento al final nos obligaría a recorrer toda la cola desde el principio hasta encontrar el último nodo. Con `last`, podemos enlazar directamente en O(1).

### 4.4 Ejemplo práctico completo: Cola de impresión

```typescript
// Simulamos una cola de impresión en una oficina

interface Documento {
  nombre: string;
  paginas: number;
  usuario: string;
}

class Impresora {
  private cola: Queue<Documento>;
  private estaImprimiendo: boolean;

  constructor() {
    this.cola = new Queue<Documento>();
    this.estaImprimiendo = false;
  }

  agregarTrabajo(doc: Documento): void {
    this.cola.enqueue(doc);
    console.log(
      `📄 "${doc.nombre}" (${doc.paginas} págs.) agregado a la cola por ${doc.usuario}`,
    );
    console.log(`   Trabajos en espera: ${this.cola.size()}`);
  }

  imprimirSiguiente(): void {
    if (this.cola.isEmpty()) {
      console.log("✅ No hay trabajos pendientes. La impresora está libre.");
      this.estaImprimiendo = false;
      return;
    }

    const doc = this.cola.dequeue()!;
    this.estaImprimiendo = true;
    console.log(
      `🖨️  Imprimiendo: "${doc.nombre}" de ${doc.usuario} (${doc.paginas} páginas)...`,
    );
    console.log(`   Trabajos restantes: ${this.cola.size()}`);
  }

  verEstado(): void {
    if (this.cola.isEmpty()) {
      console.log("Estado: Libre");
    } else {
      const siguiente = this.cola.front();
      console.log(`Estado: Ocupada. Siguiente: "${siguiente?.nombre}"`);
    }
  }
}

// Simulación
const impresoraHP = new Impresora();

impresoraHP.agregarTrabajo({
  nombre: "Factura.pdf",
  paginas: 2,
  usuario: "Juan",
});
impresoraHP.agregarTrabajo({
  nombre: "Contrato.docx",
  paginas: 15,
  usuario: "María",
});
impresoraHP.agregarTrabajo({
  nombre: "Foto.jpg",
  paginas: 1,
  usuario: "Pedro",
});

impresoraHP.imprimirSiguiente(); // Factura (Juan)
impresoraHP.imprimirSiguiente(); // Contrato (María)
impresoraHP.imprimirSiguiente(); // Foto (Pedro)
impresoraHP.imprimirSiguiente(); // No hay trabajos
```

### 4.5 Ejemplo práctico: Atención en un banco

```typescript
// Simulamos la fila de un banco con turnos

class Banco {
  private fila: Queue<string>;
  private contadorTurnos: number;

  constructor() {
    this.fila = new Queue<string>();
    this.contadorTurnos = 0;
  }

  sacarTurno(cliente: string): string {
    this.contadorTurnos++;
    const turno = `T${this.contadorTurnos.toString().padStart(3, "0")}`;
    this.fila.enqueue(turno);
    console.log(
      `🎫 ${cliente} obtuvo el turno ${turno}. Hay ${this.fila.size()} personas en espera.`,
    );
    return turno;
  }

  atenderSiguiente(): void {
    if (this.fila.isEmpty()) {
      console.log("🏦 No hay clientes esperando. El cajero puede descansar.");
      return;
    }
    const turno = this.fila.dequeue()!;
    console.log(
      `✅ Atendiendo turno ${turno}. Quedan ${this.fila.size()} en espera.`,
    );
  }

  cerrar(): void {
    console.log("🔒 Cerrando banco...");
    while (!this.fila.isEmpty()) {
      const turno = this.fila.dequeue()!;
      console.log(`   ⚠️ Turno ${turno} quedó sin atender.`);
    }
    console.log("Banco cerrado.");
  }
}

// Simulación
const bancoNacion = new Banco();

bancoNacion.sacarTurno("Ana"); // T001
bancoNacion.sacarTurno("Luis"); // T002
bancoNacion.sacarTurno("Carla"); // T003

bancoNacion.atenderSiguiente(); // Atiende T001
bancoNacion.sacarTurno("Roberto"); // T004
bancoNacion.atenderSiguiente(); // Atiende T002
bancoNacion.atenderSiguiente(); // Atiende T003
bancoNacion.atenderSiguiente(); // Atiende T004
bancoNacion.atenderSiguiente(); // No hay clientes
```

---

## 5. Listas Enlazadas (Linked List)

### 5.1 Concepto

Una **lista enlazada** es una estructura de datos que maneja una colección de elementos dispuestos uno detrás de otro, en la que cada elemento se conecta al siguiente mediante un puntero.

#### Diferencias clave con Pilas y Colas

| Característica  | Pila        | Cola        | Lista Enlazada                                   |
| --------------- | ----------- | ----------- | ------------------------------------------------ |
| Orden de acceso | LIFO        | FIFO        | **Libre** — cualquier orden                      |
| Inserción       | Solo tope   | Solo final  | **Cualquier posición**                           |
| Eliminación     | Solo tope   | Solo frente | **Cualquier nodo**                               |
| Lectura         | Destructiva | Destructiva | **No destructiva** — podemos recorrer sin borrar |

#### ¿Cuándo usar listas enlazadas?

- Cuando necesitás **insertar o eliminar en cualquier posición** frecuentemente.
- Cuando no conocés de antemano la cantidad de elementos.
- Cuando necesitás mantener los elementos **ordenados**.
- Cuando necesitás evitar **duplicados**.
- Como base para construir estructuras más complejas (listas de listas, tablas hash, etc.).

### 5.2 Implementación de la Lista Enlazada

```typescript
class MyNode<T> {
  public value: T;
  public next: MyNode<T> | null;

  constructor(value: T) {
    this.value = value;
    this.next = null;
  }
}

class LinkedList<T> {
  private head: MyNode<T> | null;

  constructor() {
    this.head = null;
  }

  // ═══════════════════════════════════════════════════════
  // OPERACIONES BÁSICAS
  // ═══════════════════════════════════════════════════════

  /**
   * Push: Inserta un nodo al FINAL de la lista
   * Complejidad: O(n) — debe recorrer hasta el final
   */
  public push(value: T): MyNode<T> {
    const newNode = new MyNode<T>(value);

    if (this.head === null) {
      this.head = newNode;
    } else {
      let current = this.head;
      while (current.next !== null) {
        current = current.next;
      }
      current.next = newNode;
    }

    return newNode;
  }
  /**
   * var headAux = head;

    head ----┐
            ↓
          [10] -> [20] -> [30]
            ↑
    headAux -┘

  * headAux = headAux.Next;
  head
  ↓
  [10] -> [20] -> [30]

  headAux
            ↓
          [20] -> [30]
  
  */

  /**
   * Pop: Elimina y devuelve el ÚLTIMO nodo
   * Complejidad: O(n)
   */
  public pop(): T | null {
    if (this.head === null) return null;

    // Si solo hay un elemento
    if (this.head.next === null) {
      const value = this.head.value;
      this.head = null;
      return value;
    }

    let current = this.head;
    let previous: MyNode<T> | null = null;

    while (current.next !== null) {
      previous = current;
      current = current.next;
    }

    previous!.next = null;
    return current.value;
  }

  /**
   * InsertFirst: Inserta un nodo al PRINCIPIO de la lista
   * Complejidad: O(1)
   */
  public insertFirst(value: T): MyNode<T> {
    const newNode = new MyNode<T>(value);
    newNode.next = this.head;
    this.head = newNode;
    return newNode;
  }

  /**
   * RemoveFirst: Elimina y devuelve el PRIMER nodo
   * Complejidad: O(1)
   */
  public removeFirst(): T | null {
    if (this.head === null) return null;

    const value = this.head.value;
    this.head = this.head.next;
    return value;
  }

  // ═══════════════════════════════════════════════════════
  // OPERACIONES DE BÚSQUEDA
  // ═══════════════════════════════════════════════════════

  /**
   * Search: Busca un nodo por su valor
   * Complejidad: O(n)
   */
  public search(value: T): MyNode<T> | null {
    let current = this.head;

    while (current !== null) {
      if (current.value === value) {
        return current;
      }
      current = current.next;
    }

    return null; // No encontrado
  }

  /**
   * Includes: Verifica si un valor existe (versión booleana de search)
   */
  public includes(value: T): boolean {
    return this.search(value) !== null;
  }

  // ═══════════════════════════════════════════════════════
  // OPERACIONES DE ELIMINACIÓN ESPECÍFICA
  // ═══════════════════════════════════════════════════════

  /**
   * Delete: Elimina el nodo que contiene un valor específico
   * Complejidad: O(n)
   */
  public delete(value: T): T | null {
    if (this.head === null) return null;

    // Si el valor está en el primer nodo
    if (this.head.value === value) {
      return this.removeFirst();
    }

    let current = this.head;
    let previous: MyNode<T> | null = null;

    while (current !== null && current.value !== value) {
      previous = current;
      current = current.next;
    }

    // Si no se encontró
    if (current === null) return null;

    // "Salteamos" el nodo a eliminar
    previous!.next = current.next;
    return current.value;
  }

  // ═══════════════════════════════════════════════════════
  // OPERACIONES DE ORDENAMIENTO
  // ═══════════════════════════════════════════════════════

  /**
   * InsertOrdered: Inserta manteniendo orden ascendente
   * Requiere que T sea comparable (números, strings)
   * Complejidad: O(n)
   */
  public insertOrdered(value: T): MyNode<T> {
    const newNode = new MyNode<T>(value);

    // Caso 1: Lista vacía o valor menor al primero
    if (this.head === null || this.head.value > value) {
      newNode.next = this.head;
      this.head = newNode;
      return newNode;
    }

    // Caso 2: Buscar posición correcta
    let current = this.head;
    while (current.next !== null && current.next.value < value) {
      current = current.next;
    }

    newNode.next = current.next;
    current.next = newNode;
    return newNode;
  }

  /**
   * InsertUnique: Inserta ordenado SOLO si no existe previamente
   * Devuelve el nodo existente si ya estaba, o el nuevo si se insertó
   */
  public insertUnique(value: T): MyNode<T> {
    const existing = this.search(value);
    if (existing !== null) {
      return existing; // Ya existe, no insertamos
    }
    return this.insertOrdered(value);
  }

  /**
   * Sort: Ordena la lista completa
   * Estrategia: Vaciar la lista e ir insertando ordenadamente en una auxiliar
   */
  public sort(): void {
    const sortedList = new LinkedList<T>();

    while (!this.isEmpty()) {
      const value = this.removeFirst()!;
      sortedList.insertOrdered(value);
    }

    this.head = sortedList.head;
  }

  // ═══════════════════════════════════════════════════════
  // OPERACIONES AUXILIARES
  // ═══════════════════════════════════════════════════════

  /**
   * isEmpty: Verifica si la lista está vacía
   */
  public isEmpty(): boolean {
    return this.head === null;
  }

  /**
   * Clear: Vacía completamente la lista
   */
  public clear(): void {
    this.head = null; // El garbage collector se encarga del resto
  }

  /**
   * toArray: Devuelve un array con los valores (para facilitar visualización)
   */
  public toArray(): T[] {
    const result: T[] = [];
    let current = this.head;
    while (current !== null) {
      result.push(current.value);
      current = current.next;
    }
    return result;
  }

  /**
   * size: Devuelve la cantidad de elementos
   */
  public size(): number {
    let count = 0;
    let current = this.head;
    while (current !== null) {
      count++;
      current = current.next;
    }
    return count;
  }
}
```

### 5.3 Resumen de Operaciones

| Operación         | Descripción                     | Complejidad |
| ----------------- | ------------------------------- | ----------- |
| **create**        | Inicializa lista vacía          | O(1)        |
| **push**          | Inserta al final                | O(n)        |
| **pop**           | Elimina el último               | O(n)        |
| **insertFirst**   | Inserta al principio            | O(1)        |
| **removeFirst**   | Elimina el primero              | O(1)        |
| **insertOrdered** | Inserta manteniendo orden       | O(n)        |
| **insertUnique**  | Inserta ordenado sin duplicados | O(n)        |
| **search**        | Busca un valor                  | O(n)        |
| **delete**        | Elimina un valor específico     | O(n)        |
| **sort**          | Ordena toda la lista            | O(n²)       |
| **clear**         | Vacía la lista                  | O(1)        |
| **isEmpty**       | Verifica si está vacía          | O(1)        |

> **💡 Por qué insertFirst es O(1) y push es O(n):** Insertar al principio solo requiere crear un nodo y hacer que apunte al `head` actual. Insertar al final requiere recorrer toda la lista desde el principio hasta encontrar el último nodo. Si necesitás agregar muchos elementos al final, conviene mantener un puntero `tail` (como en las colas) o usar `insertFirst` y luego invertir.

### 5.4 Ejemplo práctico completo: Lista de contactos

```typescript
// Sistema de gestión de contactos telefónicos

interface Contacto {
  nombre: string;
  telefono: string;
  email: string;
}

class AgendaTelefonica {
  private contactos: LinkedList<Contacto>;

  constructor() {
    this.contactos = new LinkedList<Contacto>();
  }

  agregarContacto(contacto: Contacto): void {
    // Verificamos si ya existe por nombre
    const existe = this.contactos
      .toArray()
      .some((c) => c.nombre === contacto.nombre);

    if (existe) {
      console.log(`⚠️ El contacto "${contacto.nombre}" ya existe.`);
      return;
    }

    this.contactos.push(contacto);
    console.log(`✅ Contacto "${contacto.nombre}" agregado.`);
  }

  buscarContacto(nombre: string): Contacto | null {
    const found = this.contactos.toArray().find((c) => c.nombre === nombre);
    return found || null;
  }

  eliminarContacto(nombre: string): void {
    const contacto = this.contactos.toArray().find((c) => c.nombre === nombre);
    if (contacto) {
      this.contactos.delete(contacto);
      console.log(`🗑️ Contacto "${nombre}" eliminado.`);
    } else {
      console.log(`❌ Contacto "${nombre}" no encontrado.`);
    }
  }

  listarContactos(): void {
    const lista = this.contactos.toArray();
    if (lista.length === 0) {
      console.log("📭 Agenda vacía.");
      return;
    }

    console.log("📒 === AGENDA TELEFÓNICA ===");
    lista.forEach((c, i) => {
      console.log(`${i + 1}. ${c.nombre} — ${c.telefono} — ${c.email}`);
    });
  }
}

// Simulación
const miAgenda = new AgendaTelefonica();

miAgenda.agregarContacto({
  nombre: "Juan Pérez",
  telefono: "555-0101",
  email: "juan@mail.com",
});
miAgenda.agregarContacto({
  nombre: "María García",
  telefono: "555-0202",
  email: "maria@mail.com",
});
miAgenda.agregarContacto({
  nombre: "Carlos López",
  telefono: "555-0303",
  email: "carlos@mail.com",
});

miAgenda.listarContactos();

const busqueda = miAgenda.buscarContacto("María García");
console.log("Buscado:", busqueda);

miAgenda.eliminarContacto("Juan Pérez");
miAgenda.listarContactos();
```

### 5.5 Ejemplo práctico: Lista ordenada sin duplicados

```typescript
// Sistema de puntuaciones de un juego (ordenadas de mayor a menor)

class TablaPuntuaciones {
  private puntuaciones: LinkedList<number>;

  constructor() {
    this.puntuaciones = new LinkedList<number>();
  }

  registrarPuntuacion(puntos: number): void {
    // Insertamos ordenadamente (de menor a mayor, luego invertimos visualmente)
    this.puntuaciones.insertOrdered(puntos);
    console.log(`🏆 Puntuación ${puntos} registrada.`);
  }

  mostrarTop(n: number = 5): void {
    const todas = this.puntuaciones.toArray();
    // Orden descendente para el ranking
    const ordenadas = [...todas].sort((a, b) => b - a);

    console.log("🏅 === RANKING ===");
    ordenadas.slice(0, n).forEach((p, i) => {
      console.log(`   ${i + 1}° lugar: ${p} puntos`);
    });
  }

  mejorPuntuacion(): number | null {
    const todas = this.puntuaciones.toArray();
    if (todas.length === 0) return null;
    return Math.max(...todas);
  }
}

// Simulación
const juego = new TablaPuntuaciones();

juego.registrarPuntuacion(1500);
juego.registrarPuntuacion(3200);
juego.registrarPuntuacion(800);
juego.registrarPuntuacion(3200); // Duplicado — se ignora si usamos insertUnique
juego.registrarPuntuacion(4500);

juego.mostrarTop(3);
console.log(`Mejor puntuación: ${juego.mejorPuntuacion()}`);
```

### 5.6 Ejemplo práctico: Playlist de música

```typescript
// Reproductor de música con lista de reproducción

class Cancion {
  constructor(
    public titulo: string,
    public artista: string,
    public duracion: number, // en segundos
  ) {}

  toString(): string {
    const min = Math.floor(this.duracion / 60);
    const seg = this.duracion % 60;
    return `"${this.titulo}" — ${this.artista} (${min}:${seg.toString().padStart(2, "0")})`;
  }
}

class ReproductorMusica {
  private playlist: LinkedList<Cancion>;
  private actual: MyNode<Cancion> | null;

  constructor() {
    this.playlist = new LinkedList<Cancion>();
    this.actual = null;
  }

  agregarCancion(cancion: Cancion): void {
    this.playlist.push(cancion);
    // Si es la primera canción, la ponemos como actual
    if (this.actual === null) {
      this.actual = this.playlist["head"]; // Accedemos al head
    }
    console.log(`🎵 Agregada: ${cancion.toString()}`);
  }

  reproducir(): void {
    if (this.actual === null) {
      console.log("⏹️ No hay canciones en la playlist");
      return;
    }
    console.log(`▶️ Reproduciendo: ${this.actual.value.toString()}`);
  }

  siguiente(): void {
    if (this.actual === null || this.actual.next === null) {
      console.log("⏭️ Fin de la playlist");
      return;
    }
    this.actual = this.actual.next;
    this.reproducir();
  }

  eliminarActual(): void {
    if (this.actual === null) return;

    const cancion = this.actual.value;
    this.playlist.delete(cancion);

    // Actualizamos el puntero
    this.actual = this.playlist["head"];
    console.log(`🗑️ Eliminada: ${cancion.titulo}`);
  }

  mostrarPlaylist(): void {
    const canciones = this.playlist.toArray();
    console.log("📋 === PLAYLIST ===");
    canciones.forEach((c, i) => {
      const marker = this.actual?.value === c ? "▶️ " : "   ";
      console.log(`${marker}${i + 1}. ${c.toString()}`);
    });
  }
}

// Simulación
const spotify = new ReproductorMusica();

spotify.agregarCancion(new Cancion("Bohemian Rhapsody", "Queen", 354));
spotify.agregarCancion(new Cancion("Imagine", "John Lennon", 183));
spotify.agregarCancion(new Cancion("Billie Jean", "Michael Jackson", 294));

spotify.mostrarPlaylist();
spotify.reproducir(); // Bohemian Rhapsody
spotify.siguiente(); // Imagine
spotify.siguiente(); // Billie Jean
```

---

## 6. Comparación final de estructuras

```
┌─────────────────┬─────────────────┬─────────────────┬─────────────────┐
│   Característica│     PILA        │      COLA       │  LISTA ENLAZADA │
├─────────────────┼─────────────────┼─────────────────┼─────────────────┤
│ Orden           │ LIFO            │ FIFO            │ Libre           │
│ Insertar        │ Solo tope       │ Solo final      │ Cualquier lado  │
│ Eliminar        │ Solo tope       │ Solo frente     │ Cualquier nodo  │
│ Buscar          │ No              │ No              │ Sí              │
│ Ordenar         │ No              │ No              │ Sí              │
│ Lectura         │ Destructiva     │ Destructiva     │ No destructiva  │
│ Complejidad     │ Simple          │ Simple          │ Flexible        │
│ Uso típico      │ Deshacer,       │ Filas,          │ Contactos,      │
│                 │ historial       │ impresión       │ playlists       │
└─────────────────┴─────────────────┴─────────────────┴─────────────────┘
```

---

## 7. Conceptos clave a recordar

1. **Heap vs Stack:** Las estructuras dinámicas viven en el **heap**. El **stack** es automático y se destruye al salir de la función.

2. **Nodo:** Es la unidad básica. Tiene un **valor** y una referencia al **siguiente**.

3. **Pila (LIFO):** El último en entrar es el primero en salir. Como una pila de platos.

4. **Cola (FIFO):** El primero en entrar es el primero en salir. Como una fila de banco.

5. **Lista:** Permite insertar, eliminar y buscar en **cualquier posición**. Es la más flexible.

6. **Complejidad temporal:**
   - Pilas y colas: todas las operaciones básicas son **O(1)** (constante).
   - Listas: buscar y eliminar por valor son **O(n)** (lineal), porque deben recorrerse.

7. **Punteros auxiliares:** Usar punteros como `previous` o `headAux` es esencial para no perder referencias mientras recorremos la estructura.

---

## 8. Bibliografía

- Luis Joyanes Aguilar, Ignacio Zahonero Martínez. _Programación en C_. Segunda Edición. España: McGRAW-HILL/INTERAMERICANA DE ESPAÑA, S.A.U., 2005. ISBN: 84-481-9844-1.
- Brian W. Kernighan, Rob Pike. _La práctica de la programación_. Pearson Educación. México (2000).
- [Debugging with CodeBlocks](http://wiki.codeblocks.org/index.php?title=Debugging_with_Code::Blocks)
- [Bibliotecas (Wikipedia)](<https://es.wikipedia.org/wiki/Biblioteca_(inform%C3%A1tica)>)
- [Enlace dinámico (Wikipedia)](https://es.wikipedia.org/wiki/Enlace_din%C3%A1mico)
- [Enlace estático (Wikipedia)](https://es.wikipedia.org/wiki/Enlace_est%C3%A1tico)

---

> **Documento generado a partir del material de la Unidad 3 de Programación II — TSSI.**
>
> Se han agregado ejemplos prácticos, clarificaciones conceptuales y código completo ejecutable para facilitar el aprendizaje.
