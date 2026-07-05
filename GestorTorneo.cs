using System;

public static class GestorTorneo
{
    public static void SimularFaseDeGrupos(ListaEnlazada<Grupo> listaGrupos)
    {
        for (int g = 0; g < listaGrupos.Contar; g++)
        {
            Grupo grupoActual = listaGrupos.Obtener(g);
            for (int i = 0; i < grupoActual.Equipos.Contar; i++)
            {
                for (int j = i + 1; j < grupoActual.Equipos.Contar; j++)
                {
                    Equipo equipoA = grupoActual.Equipos.Obtener(i);
                    Equipo equipoB = grupoActual.Equipos.Obtener(j);

                    var (golesA, golesB) = MotorEstocastico.SimularPartido(equipoA, equipoB);

                    equipoA.GolesFavor += golesA;
                    equipoA.GolesContra += golesB;
                    equipoB.GolesFavor += golesB;
                    equipoB.GolesContra += golesA;

                    if (golesA > golesB) { equipoA.Puntos += 3; }
                    else if (golesB > golesA) { equipoB.Puntos += 3; }
                    else { equipoA.Puntos += 1; equipoB.Puntos += 1; }
                }
            }
            OrdenarGrupo(grupoActual);
        }
    }

   public static void OrdenarGrupo(Grupo grupo)
    {
        int n = grupo.Equipos.Contar;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                Equipo eqJ = grupo.Equipos.Obtener(j);
                Equipo eqJ1 = grupo.Equipos.Obtener(j + 1);

                bool intercambiar = false;
                if (eqJ.Puntos < eqJ1.Puntos) 
                { 
                    intercambiar = true; 
                }
                else if (eqJ.Puntos == eqJ1.Puntos)
                {
                    if (eqJ.DiferenciaGoles < eqJ1.DiferenciaGoles) 
                    { 
                        intercambiar = true; 
                    }
                    else if (eqJ.DiferenciaGoles == eqJ1.DiferenciaGoles)
                    {
                        if (eqJ.GolesFavor < eqJ1.GolesFavor) 
                        { 
                            intercambiar = true; 
                        }
                    }
                }

                if (intercambiar) 
                { 
                    grupo.Equipos.Intercambiar(j, j + 1); 
                }
            }
        }
    }
    // ALGORITMO CODICIOSO: Filtra y distribuye los mejores terceros según la matriz de la FIFA
    public static ListaEnlazada<Equipo> ResolverMatrizTerceros(ListaEnlazada<Grupo> listaGrupos)
    {
        ListaEnlazada<Equipo> todosLosTerceros = new ListaEnlazada<Equipo>();
        for (int i = 0; i < listaGrupos.Contar; i++)
        {
            todosLosTerceros.Agregar(listaGrupos.Obtener(i).Equipos.Obtener(2)); // Puesto 3
        }

        // Ordenar los terceros de mejor a peor
        Grupo grupoVirtual = new Grupo('X') { Equipos = todosLosTerceros };
        OrdenarGrupo(grupoVirtual);

        // Lista de asignados finales (8 espacios correspondientes a las asignaciones de la 1 a la 8)
        ListaEnlazada<Equipo> tercerosAsignados = new ListaEnlazada<Equipo>();
        
        // Cadenas de restricciones del PDF (Páginas 4 y 5)
        string[] restricciones = new string[] {
            "ABCDF",   // Asignado 1 (Partido 74)
            "CDGH",    // Asignado 2 (Partido 77) - Nota: Se omite F por consistencia con el PDF
            "CEFHI",   // Asignado 3 (Partido 79)
            "EHIJK",   // Asignado 4 (Partido 80)
            "BEFIJ",   // Asignado 5 (Partido 81)
            "AEHIJ",   // Asignado 6 (Partido 82)
            "EFGIJ",   // Asignado 7 (Partido 85)
            "DEIJL"    // Asignado 8 (Partido 87)
        };

        bool[] yaAsignado = new bool[12];

        for (int r = 0; r < restricciones.Length; r++)
        {
            string gruposPermitidos = restricciones[r];
            bool encontrado = false;

            // Intentar buscar por orden de mérito respetando la compatibilidad de grupo
            for (int i = 0; i < todosLosTerceros.Contar; i++)
            {
                if (!yaAsignado[i])
                {
                    Equipo candidato = todosLosTerceros.Obtener(i);
                    if (gruposPermitidos.Contains(candidato.GrupoOrigen.ToString()))
                    {
                        tercerosAsignados.Agregar(candidato);
                        yaAsignado[i] = true;
                        encontrado = true;
                        break;
                    }
                }
            }

            // Mecanismo de Tolerancia a Fallos: Asignación directa si no hay compatibilidad pura
            if (!encontrado)
            {
                for (int i = 0; i < todosLosTerceros.Contar; i++)
                {
                    if (!yaAsignado[i])
                    {
                        tercerosAsignados.Agregar(todosLosTerceros.Obtener(i));
                        yaAsignado[i] = true;
                        break;
                    }
                }
            }
        }

        return tercerosAsignados;
    }

    // Construye los cruces exactos de 16vos de final estipulados en la página 3
    public static ListaEnlazada<PartidoEliminatorio> GenerarLlaves16avos(ListaEnlazada<Grupo> grupos, ListaEnlazada<Equipo> terceros)
    {
        ListaEnlazada<PartidoEliminatorio> partidos = new ListaEnlazada<PartidoEliminatorio>();

   // Añadimos el signo '!' al final de la asignación para indicarle a C# 
        // que garantizamos que el equipo no será nulo en tiempo de ejecución.
        Func<char, int, Equipo> eq = (letra, puesto) => {
            for (int i = 0; i < grupos.Contar; i++) {
                if (grupos.Obtener(i).Letra == letra) return grupos.Obtener(i).Equipos.Obtener(puesto - 1);
            }
            return null!;
        };

        // Inyección estricta de partidos (73 al 88)
        partidos.Agregar(new PartidoEliminatorio(73, eq('A', 2), eq('B', 2)));
        partidos.Agregar(new PartidoEliminatorio(74, eq('E', 1), terceros.Obtener(0))); // Asignado 1
        partidos.Agregar(new PartidoEliminatorio(75, eq('F', 1), eq('C', 2)));
        partidos.Agregar(new PartidoEliminatorio(76, eq('C', 1), eq('F', 2)));
        partidos.Agregar(new PartidoEliminatorio(77, eq('I', 1), terceros.Obtener(1))); // Asignado 2
        partidos.Agregar(new PartidoEliminatorio(78, eq('E', 2), eq('I', 2)));
        partidos.Agregar(new PartidoEliminatorio(79, eq('A', 1), terceros.Obtener(2))); // Asignado 3
        partidos.Agregar(new PartidoEliminatorio(80, eq('L', 1), terceros.Obtener(3))); // Asignado 4
        partidos.Agregar(new PartidoEliminatorio(81, eq('D', 1), terceros.Obtener(4))); // Asignado 5
        partidos.Agregar(new PartidoEliminatorio(82, eq('G', 1), terceros.Obtener(5))); // Asignado 6
        partidos.Agregar(new PartidoEliminatorio(83, eq('K', 2), eq('L', 2)));
        partidos.Agregar(new PartidoEliminatorio(84, eq('H', 1), eq('J', 2)));
        partidos.Agregar(new PartidoEliminatorio(85, eq('B', 1), terceros.Obtener(6))); // Asignado 7
        partidos.Agregar(new PartidoEliminatorio(86, eq('J', 1), eq('H', 2)));
        partidos.Agregar(new PartidoEliminatorio(87, eq('K', 1), terceros.Obtener(7))); // Asignado 8
        partidos.Agregar(new PartidoEliminatorio(88, eq('D', 2), eq('G', 2)));

        return partidos;
    }
}