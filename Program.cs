using System;
using Spectre.Console;

class Program
{
    private static ListaEnlazada<Grupo> _losGrupos = null!;
    private static ListaEnlazada<Equipo> _mejoresTerceros = null!;
    private static ListaEnlazada<PartidoEliminatorio> _dieciseisavos = null!;
    private static ListaEnlazada<PartidoEliminatorio> _octavos = null!;
    private static ListaEnlazada<PartidoEliminatorio> _cuartos = null!;
    private static ListaEnlazada<PartidoEliminatorio> _semifinales = null!;
    private static PartidoEliminatorio _granFinal = null!;

    private static bool _gruposSimulados = false;
    private static bool _eliminatoriasSimuladas = false;

    static void Main(string[] args)
    {
        _losGrupos = CargadorEquipos.InicializarGruposOficiales();

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("MUNDIAL 2026")
                    .Centered()
                    .Color(Color.Green));

            AnsiConsole.Write(new Rule("[yellow]SIMULADOR ESTOCÁSTICO INTERACTIVO[/]").Centered());
            Console.WriteLine();

            var opcion = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Seleccione una opción del menú:")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "1. Ver Configuración Inicial de Grupos",
                        "2. Simular Fase de Grupos",
                        "3. Mostrar Tablas de Posiciones",
                        "4. Ver Clasificados (Mejores Terceros)",
                        "5. Simular Fase Eliminatoria (Play-offs)",
                        "6. Consultar Estadísticas de un Equipo",
                        "7. Salir del Programa"
                    }));

            switch (opcion)
            {
                case "1. Ver Configuración Inicial de Grupos":
                    MostrarConfiguracionGrupos();
                    break;
                case "2. Simular Fase de Grupos":
                    SimularFaseGrupos();
                    break;
                case "3. Mostrar Tablas de Posiciones":
                    MostrarTablasPosiciones();
                    break;
                case "4. Ver Clasificados (Mejores Terceros)":
                    MostrarSeccionTerceros();
                    break;
                case "5. Simular Fase Eliminatoria (Play-offs)":
                    SimularPlayOffs();
                    break;
                case "6. Consultar Estadísticas de un Equipo":
                    MenuConsultarEquipo();
                    break;
                case "7. Salir del Programa":
                    AnsiConsole.MarkupLine("[bold red]Saliendo del simulador... ¡Éxito en la defensa del proyecto![/]");
                    return;
            }

            AnsiConsole.MarkupLine("\n[gray]Presione cualquier tecla para regresar al menú principal...[/]");
            Console.ReadKey(true);
        }
    }

    private static void MostrarConfiguracionGrupos()
    {
        AnsiConsole.Write(new Rule("[blue]CONFIGURACIÓN OFICIAL DE GRUPOS[/]"));
        for (int i = 0; i < _losGrupos.Contar; i++)
        {
            Grupo g = _losGrupos.Obtener(i);
            var tabla = new Table().Title($"[bold yellow]GRUPO {g.Letra}[/]").Border(TableBorder.Rounded);
            tabla.AddColumn("[bold]Equipo[/]");
            tabla.AddColumn("[bold]ELO Inicial[/]");

            for (int e = 0; e < g.Equipos.Contar; e++)
            {
                Equipo eq = g.Equipos.Obtener(e);
                tabla.AddRow(eq.Nombre, eq.Elo.ToString("F0"));
            }
            AnsiConsole.Write(tabla);
            Console.WriteLine();
        }
    }

    private static void SimularFaseGrupos()
    {
        if (_gruposSimulados)
        {
            AnsiConsole.MarkupLine("[yellow]⚠ La Fase de Grupos ya fue simulada previamente.[/]");
            return;
        }

        AnsiConsole.Status()
            .Start("Simulando partidos de Fase de Grupos mediante Distribución de Poisson...", ctx =>
            {
                GestorTorneo.SimularFaseDeGrupos(_losGrupos);
                _mejoresTerceros = GestorTorneo.ResolverMatrizTerceros(_losGrupos);
                _dieciseisavos = GestorTorneo.GenerarLlaves16avos(_losGrupos, _mejoresTerceros);
                System.Threading.Thread.Sleep(1500);
            });

        _gruposSimulados = true;
        AnsiConsole.MarkupLine("[bold green]✓ Fase de Grupos simulada con éxito matemáticamente y posiciones calculadas.[/]");
    }

    private static void MostrarTablasPosiciones()
    {
        if (!_gruposSimulados)
        {
            AnsiConsole.MarkupLine("[red]❌ Debe simular la Fase de Grupos primero (Opción 2).[/]");
            return;
        }

        AnsiConsole.Write(new Rule("[blue]TABLAS DE POSICIONES FINALES (FASE DE GRUPOS)[/]"));
        for (int i = 0; i < _losGrupos.Contar; i++)
        {
            Grupo g = _losGrupos.Obtener(i);
            var tabla = new Table().Title($"[bold yellow]GRUPO {g.Letra}[/]").Border(TableBorder.Square);
            tabla.AddColumn("[bold]Pos[/]");
            tabla.AddColumn("[bold]Equipo[/]");
            tabla.AddColumn("[bold]Pts[/]");
            tabla.AddColumn("[bold]GF[/]");
            tabla.AddColumn("[bold]GC[/]");
            tabla.AddColumn("[bold]DG[/]");

            for (int e = 0; e < g.Equipos.Contar; e++)
            {
                Equipo eq = g.Equipos.Obtener(e);
                string color = e < 2 ? "green" : (e == 2 ? "yellow" : "red");
                tabla.AddRow($"[{color}]{e + 1}[/]", $"[{color}]{eq.Nombre}[/]", eq.Puntos.ToString(), eq.GolesFavor.ToString(), eq.GolesContra.ToString(), eq.DiferenciaGoles.ToString());
            }
            AnsiConsole.Write(tabla);
            Console.WriteLine();
        }
    }

    private static void MostrarSeccionTerceros()
    {
        if (!_gruposSimulados)
        {
            AnsiConsole.MarkupLine("[red]❌ Debe simular la Fase de Grupos primero (Opción 2).[/]");
            return;
        }

        var tabla = new Table().Title("[bold yellow]RANKING DE MEJORES TERCEROS RELEVADOS (MATRIZ FIFA)[/]").Border(TableBorder.Rounded);
        tabla.AddColumn("[bold]Asignación[/]");
        tabla.AddColumn("[bold]Equipo[/]");
        tabla.AddColumn("[bold]Grupo Origen[/]");
        tabla.AddColumn("[bold]Pts[/]");
        tabla.AddColumn("[bold]DG[/]");
        tabla.AddColumn("[bold]GF[/]");

        for (int i = 0; i < _mejoresTerceros.Contar; i++)
        {
            Equipo eq = _mejoresTerceros.Obtener(i);
            tabla.AddRow($"Asignado {i + 1}", eq.Nombre, eq.GrupoOrigen.ToString(), eq.Puntos.ToString(), eq.DiferenciaGoles.ToString(), eq.GolesFavor.ToString());
        }
        AnsiConsole.Write(tabla);
    }

    private static void SimularPlayOffs()
    {
        if (!_gruposSimulados)
        {
            AnsiConsole.MarkupLine("[red]❌ Debe simular la Fase de Grupos primero (Opción 2).[/]");
            return;
        }

        if (_eliminatoriasSimuladas)
        {
            AnsiConsole.MarkupLine("[yellow]⚠ Las fases eliminatorias ya han sido procesadas.[/]");
            MostrarResultadosFinales();
            return;
        }

        // 1. Dieciseisavos
        AnsiConsole.Write(new Rule("[yellow]DIECISEISAVOS DE FINAL (Ronda de 32)[/]"));
        ListaEnlazada<Equipo> clasificadosOctavos = new ListaEnlazada<Equipo>();
        var tabla16 = new Table().Border(TableBorder.Minimal);
        tabla16.AddColumn("Partido").AddColumn("Resultado Oficial Estocástico");

        for (int i = 0; i < _dieciseisavos.Contar; i++)
        {
            PartidoEliminatorio p = _dieciseisavos.Obtener(i);
            p.Jugar();
            tabla16.AddRow($"P{p.NumeroPartido}", p.ResultadoTexto);
            clasificadosOctavos.Agregar(p.Ganador);
        }
        AnsiConsole.Write(tabla16);

        // 2. Octavos
        _octavos = new ListaEnlazada<PartidoEliminatorio>();
        int numP = 89;
        for (int i = 0; i < clasificadosOctavos.Contar; i += 2)
        {
            _octavos.Agregar(new PartidoEliminatorio(numP++, clasificadosOctavos.Obtener(i), clasificadosOctavos.Obtener(i + 1)));
        }

        AnsiConsole.Write(new Rule("[yellow]OCTAVOS DE FINAL[/]"));
        ListaEnlazada<Equipo> clasificadosCuartos = new ListaEnlazada<Equipo>();
        var tabla8 = new Table().Border(TableBorder.Minimal);
        tabla8.AddColumn("Partido").AddColumn("Resultado Oficial Estocástico");

        for (int i = 0; i < _octavos.Contar; i++)
        {
            PartidoEliminatorio p = _octavos.Obtener(i);
            p.Jugar();
            tabla8.AddRow($"P{p.NumeroPartido}", p.ResultadoTexto);
            clasificadosCuartos.Agregar(p.Ganador);
        }
        AnsiConsole.Write(tabla8);

        // 3. Cuartos
        _cuartos = new ListaEnlazada<PartidoEliminatorio>();
        for (int i = 0; i < clasificadosCuartos.Contar; i += 2)
        {
            _cuartos.Agregar(new PartidoEliminatorio(numP++, clasificadosCuartos.Obtener(i), clasificadosCuartos.Obtener(i + 1)));
        }

        AnsiConsole.Write(new Rule("[yellow]CUARTOS DE FINAL[/]"));
        ListaEnlazada<Equipo> clasificadosSemis = new ListaEnlazada<Equipo>();
        var tabla4 = new Table().Border(TableBorder.Minimal);
        tabla4.AddColumn("Partido").AddColumn("Resultado Oficial Estocástico");

        for (int i = 0; i < _cuartos.Contar; i++)
        {
            PartidoEliminatorio p = _cuartos.Obtener(i);
            p.Jugar();
            tabla4.AddRow($"P{p.NumeroPartido}", p.ResultadoTexto);
            clasificadosSemis.Agregar(p.Ganador);
        }
        AnsiConsole.Write(tabla4);

        // 4. Semifinales
        _semifinales = new ListaEnlazada<PartidoEliminatorio>();
        for (int i = 0; i < clasificadosSemis.Contar; i += 2)
        {
            _semifinales.Agregar(new PartidoEliminatorio(numP++, clasificadosSemis.Obtener(i), clasificadosSemis.Obtener(i + 1)));
        }

        AnsiConsole.Write(new Rule("[yellow]SEMIFINALES[/]"));
        ListaEnlazada<Equipo> finalistas = new ListaEnlazada<Equipo>();
        var tabla2 = new Table().Border(TableBorder.Minimal);
        tabla2.AddColumn("Partido").AddColumn("Resultado Oficial Estocástico");

        for (int i = 0; i < _semifinales.Contar; i++)
        {
            PartidoEliminatorio p = _semifinales.Obtener(i);
            p.Jugar();
            tabla2.AddRow($"P{p.NumeroPartido}", p.ResultadoTexto);
            finalistas.Agregar(p.Ganador);
        }
        AnsiConsole.Write(tabla2);

        // 5. Gran Final
        _granFinal = new PartidoEliminatorio(104, finalistas.Obtener(0), finalistas.Obtener(1));
        _granFinal.Jugar();

        _eliminatoriasSimuladas = true;
        MostrarResultadosFinales();
    }

    private static void MostrarResultadosFinales()
    {
        AnsiConsole.Write(new Rule("[bold gold1]🚀 GRAN FINAL DEL MUNDIAL 2026 🚀[/]"));
        AnsiConsole.MarkupLine($"\n   [bold white]Marcador Oficial Final del Partido 104:[/]");
        AnsiConsole.MarkupLine($"   [bold yellow]→ {_granFinal.ResultadoTexto} ←[/]\n");

        Equipo campeon = _granFinal.Ganador;
        Equipo subcampeon = _granFinal.Ganador == _granFinal.EquipoA ? _granFinal.EquipoB : _granFinal.EquipoA;

        var panelCam = new Panel(new Markup($"[bold gold1]🏆 ¡CAMPEÓN DEL MUNDO: {campeon.Nombre.ToUpper()}! 🏆[/]\n[white]🥈 Subcampeón: {subcampeon.Nombre}[/]"))
        {
            Border = BoxBorder.Double,
            Padding = new Padding(2, 1, 2, 1)
        };
        AnsiConsole.Write(panelCam);
    }

    private static void MenuConsultarEquipo()
    {
        string nombreBuscado = AnsiConsole.Ask<string>("Ingrese el nombre del país a consultar (ej. [yellow]México[/], [yellow]Brasil[/]):");
        Equipo? encontrado = null;
        char grupoLetra = ' ';

        for (int i = 0; i < _losGrupos.Contar; i++)
        {
            Grupo g = _losGrupos.Obtener(i);
            for (int e = 0; e < g.Equipos.Contar; e++)
            {
                Equipo eq = g.Equipos.Obtener(e);
                if (string.Equals(eq.Nombre, nombreBuscado, StringComparison.OrdinalIgnoreCase))
                {
                    encontrado = eq;
                    grupoLetra = g.Letra;
                    break;
                }
            }
            if (encontrado != null) break;
        }

        if (encontrado == null)
        {
            AnsiConsole.MarkupLine("[bold red]❌ El equipo especificado no forma parte de los 48 clasificados oficiales del torneo.[/]");
            return;
        }

        var panel = new Panel(new Markup(
            $"[bold green]Ficha Técnica de Selección[/]\n\n" +
            $"[bold white]País:[/] {encontrado.Nombre}\n" +
            $"[bold white]Grupo Asignado:[/] Grupo {grupoLetra}\n" +
            $"[bold white]Puntaje ELO Base:[/] {encontrado.Elo:F0}\n" +
            $"[bold white]Puntos en Fase de Grupos:[/] {encontrado.Puntos}\n" +
            $"[bold white]Goles a Favor:[/] {encontrado.GolesFavor}\n" +
            $"[bold white]Goles en Contra:[/] {encontrado.GolesContra}\n" +
            $"[bold white]Diferencia de Goles:[/] {(encontrado.DiferenciaGoles >= 0 ? "+" : "")}{encontrado.DiferenciaGoles}"
        ))
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1, 2, 1)
        };
        AnsiConsole.Write(panel);
    }
}