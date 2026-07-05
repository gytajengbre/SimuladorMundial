using System;

// 1. El bloque básico: Un nodo que almacena un elemento y un puntero al siguiente
public class Nodo<T>
{
    public T Data { get; set; }
    public Nodo<T> Siguiente { get; set; }

    public Nodo(T data)
    {
        Data = data;
        Siguiente = null;
    }
}

// 2. La estructura de la Lista Enlazada Dinámica
public class ListaEnlazada<T>
{
    private Nodo<T> cabeza;
    public int Contar { get; private set; } // Guarda el tamaño total de la lista

    public ListaEnlazada()
    {
        cabeza = null;
        Contar = 0;
    }

    // Método para agregar un elemento al final de la lista
    public void Agregar(T item)
    {
        Nodo<T> nuevoNodo = new Nodo<T>(item);
        if (cabeza == null)
        {
            cabeza = nuevoNodo;
        }
        else
        {
            Nodo<T> actual = cabeza;
            while (actual.Siguiente != null)
            {
                actual = actual.Siguiente;
            }
            actual.Siguiente = nuevoNodo;
        }
        Contar++;
    }

    // Método para obtener un elemento mediante un índice (como si fuera un arreglo)
    public T Obtener(int indice)
    {
        if (indice < 0 || indice >= Contar)
            throw new IndexOutOfRangeException("Índice fuera de rango en la lista enlazada.");

        Nodo<T> actual = cabeza;
        for (int i = 0; i < indice; i++)
        {
            actual = actual.Siguiente;
        }
        return actual.Data;
    }
    // Método para intercambiar los datos de dos índices (útil para algoritmos de ordenamiento)
    public void Intercambiar(int indiceA, int indiceB)
    {
        if (indiceA < 0 || indiceA >= Contar || indiceB < 0 || indiceB >= Contar)
            throw new IndexOutOfRangeException("Índices fuera de rango para intercambio.");

        Nodo<T> actual = cabeza;
        Nodo<T> nodoA = null;
        Nodo<T> nodoB = null;

        // Buscamos ambos nodos en un solo recorrido lineal
        for (int i = 0; i <= Math.Max(indiceA, indiceB); i++)
        {
            if (i == indiceA) nodoA = actual;
            if (i == indiceB) nodoB = actual;
            actual = actual.Siguiente;
        }

        // Intercambiamos las referencias de los datos contenidos
        T temporal = nodoA.Data;
        nodoA.Data = nodoB.Data;
        nodoB.Data = temporal;
    }
}


// El molde con los datos de cada selección nacional
public class Equipo
{
    public string Nombre { get; set; }
    public double Elo { get; set; }
    
    // Estadísticas necesarias para la Fase de Grupos y desempates
    public int Puntos { get; set; }
    public int GolesFavor { get; set; }
    public int GolesContra { get; set; }
    
    // Propiedad calculada automáticamente: Diferencia de Goles (DG = GF - GC)
    public int DiferenciaGoles => GolesFavor - GolesContra;

    public Equipo(string nombre, double elo)
    {
        Nombre = nombre;
        Elo = elo;
        Puntos = 0;
        GolesFavor = 0;
        GolesContra = 0;
    }
}

// Representa a cada uno de los 12 grupos del mundial
public class Grupo
{
    public char Letra { get; set; } // 'A', 'B', 'C', etc.
    public ListaEnlazada<Equipo> Equipos { get; set; }

    public Grupo(char letra)
    {
        Letra = letra;
        Equipos = new ListaEnlazada<Equipo>();
    }
}