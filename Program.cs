using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== SIMULADOR COMPLETO DEL MUNDIAL 2026 ===");
        
        ListaEnlazada<Grupo> losGrupos = CargadorEquipos.InicializarGruposOficiales();
        GestorTorneo.SimularFaseDeGrupos(losGrupos);

        ListaEnlazada<Equipo> losMejoresTerceros = GestorTorneo.ResolverMatrizTerceros(losGrupos);
        ListaEnlazada<PartidoEliminatorio> dieciseisavos = GestorTorneo.GenerarLlaves16avos(losGrupos, losMejoresTerceros);

        Console.WriteLine("\n--- SIMULANDO DIECISEISAVOS DE FINAL ---");
        ListaEnlazada<Equipo> clasificadosOctavos = new ListaEnlazada<Equipo>();
        for (int i = 0; i < dieciseisavos.Contar; i++)
        {
            PartidoEliminatorio partido = dieciseisavos.Obtener(i);
            partido.Jugar();
            Console.WriteLine($"Partido {partido.NumeroPartido}: {partido.ResultadoTexto}");
            clasificadosOctavos.Agregar(partido.Ganador);
        }

        Console.WriteLine("\n--- SIMULANDO OCTAVOS DE FINAL ---");
        ListaEnlazada<PartidoEliminatorio> octavos = new ListaEnlazada<PartidoEliminatorio>();
        int numPartido = 89;
        for (int i = 0; i < clasificadosOctavos.Contar; i += 2)
        {
            octavos.Agregar(new PartidoEliminatorio(numPartido++, clasificadosOctavos.Obtener(i), clasificadosOctavos.Obtener(i + 1)));
        }

        ListaEnlazada<Equipo> clasificadosCuartos = new ListaEnlazada<Equipo>();
        for (int i = 0; i < octavos.Contar; i++)
        {
            PartidoEliminatorio partido = octavos.Obtener(i);
            partido.Jugar();
            Console.WriteLine($"Partido {partido.NumeroPartido}: {partido.ResultadoTexto}");
            clasificadosCuartos.Agregar(partido.Ganador);
        }

        Console.WriteLine("\n--- SIMULANDO CUARTOS DE FINAL ---");
        ListaEnlazada<PartidoEliminatorio> cuartos = new ListaEnlazada<PartidoEliminatorio>();
        for (int i = 0; i < clasificadosCuartos.Contar; i += 2)
        {
            cuartos.Agregar(new PartidoEliminatorio(numPartido++, clasificadosCuartos.Obtener(i), clasificadosCuartos.Obtener(i + 1)));
        }

        ListaEnlazada<Equipo> clasificadosSemis = new ListaEnlazada<Equipo>();
        for (int i = 0; i < cuartos.Contar; i++)
        {
            PartidoEliminatorio partido = cuartos.Obtener(i);
            partido.Jugar();
            Console.WriteLine($"Partido {partido.NumeroPartido}: {partido.ResultadoTexto}");
            clasificadosSemis.Agregar(partido.Ganador);
        }

        Console.WriteLine("\n--- SIMULANDO SEMIFINALES ---");
        ListaEnlazada<PartidoEliminatorio> semifinales = new ListaEnlazada<PartidoEliminatorio>();
        for (int i = 0; i < clasificadosSemis.Contar; i += 2)
        {
            semifinales.Agregar(new PartidoEliminatorio(numPartido++, clasificadosSemis.Obtener(i), clasificadosSemis.Obtener(i + 1)));
        }

        ListaEnlazada<Equipo> finalistas = new ListaEnlazada<Equipo>();
        for (int i = 0; i < semifinales.Contar; i++)
        {
            PartidoEliminatorio partido = semifinales.Obtener(i);
            partido.Jugar();
            Console.WriteLine($"Partido {partido.NumeroPartido}: {partido.ResultadoTexto}");
            finalistas.Agregar(partido.Ganador);
        }

        Console.WriteLine("\n=========================================");
        Console.WriteLine(" !!! GRAN FINAL DEL MUNDIAL 2026 !!!");
        Console.WriteLine("=========================================");

        PartidoEliminatorio granFinal = new PartidoEliminatorio(104, finalistas.Obtener(0), finalistas.Obtener(1));
        granFinal.Jugar();
        Console.WriteLine($"\n[FINAL] {granFinal.ResultadoTexto}");

        Equipo campeon = granFinal.Ganador;
        Equipo subcampeon = granFinal.Ganador == granFinal.EquipoA ? granFinal.EquipoB : granFinal.EquipoA;

        Console.WriteLine($"\n🏆 ¡EL NUEVO CAMPEÓN DEL MUNDO ES: {campeon.Nombre.ToUpper()}! 🏆");
        Console.WriteLine($"🥈 Subcampeón: {subcampeon.Nombre}");
        Console.WriteLine("\n=========================================");
    }
}