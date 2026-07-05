using System;

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

public class ListaEnlazada<T>
{
    private Nodo<T> cabeza;
    public int Contar { get; private set; }

    public ListaEnlazada()
    {
        cabeza = null;
        Contar = 0;
    }

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

    public void Intercambiar(int indiceA, int indiceB)
    {
        if (indiceA < 0 || indiceA >= Contar || indiceB < 0 || indiceB >= Contar)
            throw new IndexOutOfRangeException("Índices fuera de rango para intercambio.");

        if (indiceA == indiceB) return;

        Nodo<T> actual = cabeza;
        Nodo<T> nodoA = null;
        Nodo<T> nodoB = null;

        int maxIndice = indiceA > indiceB ? indiceA : indiceB;

        for (int i = 0; i <= maxIndice; i++)
        {
            if (i == indiceA) nodoA = actual;
            if (i == indiceB) nodoB = actual;
            actual = actual.Siguiente;
        }

        T temporal = nodoA.Data;
        nodoA.Data = nodoB.Data;
        nodoB.Data = temporal;
    }
}

public class PartidoEliminatorio
{
    public int NumeroPartido { get; set; }
    public Equipo EquipoA { get; set; }
    public Equipo EquipoB { get; set; }
    public Equipo Ganador { get; set; }
    public string ResultadoTexto { get; set; }

    public PartidoEliminatorio(int numero, Equipo a, Equipo b)
    {
        NumeroPartido = numero;
        EquipoA = a;
        EquipoB = b;
        ResultadoTexto = "Pendiente";
    }

    public void Jugar()
    {
        var (gA, gB) = MotorEstocastico.SimularPartido(EquipoA, EquipoB);
        string penalesInfo = "";

        if (gA > gB)
        {
            Ganador = EquipoA;
        }
        else if (gB > gA)
        {
            Ganador = EquipoB;
        }
        else
        {
            bool ganaA = MotorEstocastico.SimularPenalesBernoulli(EquipoA, EquipoB);
            if (ganaA)
            {
                Ganador = EquipoA;
                penalesInfo = " (p)";
                gA += 1;
            }
            else
            {
                Ganador = EquipoB;
                penalesInfo = " (p)";
                gB += 1;
            }
        }
        ResultadoTexto = $"{EquipoA.Nombre} {gA} - {gB} {EquipoB.Nombre}{penalesInfo}";
    }
}

public class Equipo
{
    public char GrupoOrigen { get; set; }
    public string Nombre { get; set; }
    public double Elo { get; set; }
    public int Puntos { get; set; }
    public int GolesFavor { get; set; }
    public int GolesContra { get; set; }
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

public class Grupo
{
    public char Letra { get; set; }
    public ListaEnlazada<Equipo> Equipos { get; set; }

    public Grupo(char letra)
    {
        Letra = letra;
        Equipos = new ListaEnlazada<Equipo>();
    }
}