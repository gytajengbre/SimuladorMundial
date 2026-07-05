using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SIMULADOR COMPLETO DEL MUNDIAL 2026 ===");
        
        // 1. Fase de Grupos
        ListaEnlazada<Equipo> elMundial = CargadorEquipos.Obtener48Equipos();
        ListaEnlazada<Grupo> losGrupos = GestorTorneo.CrearGrupos(elMundial);
        GestorTorneo.SimularFaseDeGrupos(losGrupos);

        // 2. Filtrar los 32 clasificados
        ListaEnlazada<Equipo> ronda32 = GestorTorneo.ObtenerClasificadosFaseFinal(losGrupos);
        Console.WriteLine($"\nFase inicial cerrada. {ronda32.Contar} equipos listos en las llaves eliminatorias.");

        // 3. Correr las llaves de Play-offs consecutivamente
        ListaEnlazada<Equipo> ronda16 = GestorTorneo.SimularRondaEliminatoria(ronda32, "Dieciseisavos de Final");
        ListaEnlazada<Equipo> cuartos = GestorTorneo.SimularRondaEliminatoria(ronda16, "Octavos de Final");
        ListaEnlazada<Equipo> semis = GestorTorneo.SimularRondaEliminatoria(cuartos, "Cuartos de Final");
        ListaEnlazada<Equipo> finalistas = GestorTorneo.SimularRondaEliminatoria(semis, "Semifinales");

        // 4. GRAN FINAL
        Console.WriteLine($"\n=========================================");
        Console.WriteLine($" !!! GRAN FINAL DEL MUNDIAL 2026 !!!");
        Console.WriteLine($"=========================================");
        Equipo subcampeon;
        Equipo campeon;

        Equipo fA = finalistas.Obtener(0);
        Equipo fB = finalistas.Obtener(1);

        var (gfA, gfB) = MotorEstocastico.SimularPartido(fA, fB);
        Console.Write($"\n[FINAL] {fA.Nombre} {gfA} - {gfB} {fB.Nombre}");

        if (gfA > gfB)
        {
            campeon = fA;
            subcampeon = fB;
        }
        else if (gfB > gfA)
        {
            campeon = fB;
            subcampeon = fA;
        }
        else
        {
            Console.Write(" [Empate Histórico! Penales] ->");
            if (MotorEstocastico.SimularPenalesBernoulli(fA, fB))
            {
                campeon = fA;
                subcampeon = fB;
            }
            else
            {
                campeon = fB;
                subcampeon = fA;
            }
        }

        Console.WriteLine($"\n\n🏆 ¡EL NUEVO CAMPEÓN DEL MUNDO ES: {campeon.Nombre.ToUpper()}! 🏆");
        Console.WriteLine($"🥈 Subcampeón: {subcampeon.Nombre}");
        Console.WriteLine("\n=========================================");
    }
}