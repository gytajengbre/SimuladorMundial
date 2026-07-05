using System;

public static class GestorTorneo
{
    // 1. Mantiene el algoritmo de distribución que ya probamos
    public static ListaEnlazada<Grupo> CrearGrupos(ListaEnlazada<Equipo> todosLosEquipos)
    {
        ListaEnlazada<Grupo> listaGrupos = new ListaEnlazada<Grupo>();

        for (int i = 0; i < 12; i++)
        {
            char letraGrupo = (char)('A' + i);
            listaGrupos.Agregar(new Grupo(letraGrupo));
        }

        for (int i = 0; i < todosLosEquipos.Contar; i++)
        {
            Equipo eq = todosLosEquipos.Obtener(i);
            int indiceGrupoTarget = i % 12;
            
            Grupo grupoDestino = listaGrupos.Obtener(indiceGrupoTarget);
            grupoDestino.Equipos.Agregar(eq);
        }

        return listaGrupos;
    }

    // 2. NUEVO MÉTODO: Simula todos los partidos de la Fase de Grupos (Round Robin)
    public static void SimularFaseDeGrupos(ListaEnlazada<Grupo> listaGrupos)
    {
        // Recorremos los 12 grupos uno por uno
        for (int g = 0; g < listaGrupos.Contar; g++)
        {
            Grupo grupoActual = listaGrupos.Obtener(g);
            
            // Algoritmo de todos contra todos (6 combinaciones únicas para 4 elementos)
            for (int i = 0; i < grupoActual.Equipos.Contar; i++)
            {
                for (int j = i + 1; j < grupoActual.Equipos.Contar; j++)
                {
                    Equipo equipoA = grupoActual.Equipos.Obtener(i);
                    Equipo equipoB = grupoActual.Equipos.Obtener(j);

                    // Ejecutamos la matemática de Poisson que creamos antes
                    var (golesA, golesB) = MotorEstocastico.SimularPartido(equipoA, equipoB);

                    // Actualizamos estadísticas de goles de forma acumulativa
                    equipoA.GolesFavor += golesA;
                    equipoA.GolesContra += golesB;
                    
                    equipoB.GolesFavor += golesB;
                    equipoB.GolesContra += golesA;

                    // Asignación de puntos según reglas de la FIFA
                    if (golesA > golesB)
                    {
                        equipoA.Puntos += 3;
                    }
                    else if (golesB > golesA)
                    {
                        equipoB.Puntos += 3;
                    }
                    else
                    {
                        equipoA.Puntos += 1;
                        equipoB.Puntos += 1;
                    }
                }
            }
        }
    }
    // 3. NUEVO MÉTODO: Ordena los equipos de un grupo usando Bubble Sort con criterios FIFA
    public static void OrdenarGrupo(Grupo grupo)
    {
        int n = grupo.Equipos.Contar;
        
        // Algoritmo de Burbuja tradicional adaptado para los nodos de la lista
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                Equipo eqJ = grupo.Equipos.Obtener(j);
                Equipo eqJ1 = grupo.Equipos.Obtener(j + 1);

                bool intercambiar = false;

                // Criterio 1: Comparar por Puntos
                if (eqJ.Puntos < eqJ1.Puntos)
                {
                    intercambiar = true;
                }
                else if (eqJ.Puntos == eqJ1.Puntos)
                {
                    // Criterio 2: Desempate por Diferencia de Goles (DG)
                    if (eqJ.DiferenciaGoles < eqJ1.DiferenciaGoles)
                    {
                        intercambiar = true;
                    }
                    else if (eqJ.DiferenciaGoles == eqJ1.DiferenciaGoles)
                    {
                        // Criterio 3: Desempate por Goles a Favor (GF)
                        if (eqJ.GolesFavor < eqJ1.GolesFavor)
                        {
                            intercambiar = true;
                        }
                    }
                }

                // Si se cumple alguna condición de intercambio, hacemos el 'swap' de los datos
                if (intercambiar)
                {
                    grupo.Equipos.Intercambiar(j, j + 1);
                }
            }
        }
    }
    // 4. NUEVO MÉTODO: Procesa el cierre de la fase de grupos y extrae los 32 clasificados
    public static ListaEnlazada<Equipo> ObtenerClasificadosFaseFinal(ListaEnlazada<Grupo> listaGrupos)
    {
        ListaEnlazada<Equipo> clasificadosDirectos = new ListaEnlazada<Equipo>();
        ListaEnlazada<Equipo> todosLosTerceros = new ListaEnlazada<Equipo>();

        // Primero, ordenamos cada grupo y separamos los puestos
        for (int i = 0; i < listaGrupos.Contar; i++)
        {
            Grupo g = listaGrupos.Obtener(i);
            OrdenarGrupo(g); // Ordenamos los 4 equipos de este grupo

            // El 1° y 2° lugar van directo a la bolsa de clasificados
            clasificadosDirectos.Agregar(g.Equipos.Obtener(0));
            clasificadosDirectos.Agregar(g.Equipos.Obtener(1));

            // El 3° lugar va a una lista temporal para competir contra los demás terceros
            todosLosTerceros.Agregar(g.Equipos.Obtener(2));
        }

        // Ordenamos la lista de todos los terceros usando una lógica de burbuja idéntica
        // Creamos un "grupo virtual" para reusar nuestro algoritmo de ordenamiento
        Grupo grupoVirtualTerceros = new Grupo('X') { Equipos = todosLosTerceros };
        OrdenarGrupo(grupoVirtualTerceros);

        // Juntamos los 24 directos con los mejores 8 terceros
        ListaEnlazada<Equipo> los32Elegidos = new ListaEnlazada<Equipo>();
        
        for (int i = 0; i < clasificadosDirectos.Contar; i++)
        {
            los32Elegidos.Agregar(clasificadosDirectos.Obtener(i));
        }
        
        for (int i = 0; i < 8; i++) // Solo tomamos los primeros 8 de la lista ordenada de terceros
        {
            los32Elegidos.Agregar(todosLosTerceros.Obtener(i));
        }

        return los32Elegidos;
    }
    // 5. NUEVO MÉTODO: Simula una ronda completa de eliminación directa y devuelve a los ganadores
    public static ListaEnlazada<Equipo> SimularRondaEliminatoria(ListaEnlazada<Equipo> competidores, string nombreRonda)
    {
        ListaEnlazada<Equipo> ganadores = new ListaEnlazada<Equipo>();
        Console.WriteLine($"\n=========================================");
        Console.WriteLine($" SIMULANDO: {nombreRonda.ToUpper()}");
        Console.WriteLine($"=========================================");

        // Avanzamos de 2 en 2 para cruzar las parejas consecutivas
        for (int i = 0; i < competidores.Contar; i += 2)
        {
            Equipo eA = competidores.Obtener(i);
            Equipo eB = competidores.Obtener(i + 1);

            // 90 minutos reglamentarios
            var (g01, g02) = MotorEstocastico.SimularPartido(eA, eB);
            
            Console.Write($" -> {eA.Nombre} {g01} - {g02} {eB.Nombre}");

            if (g01 > g02)
            {
                Console.WriteLine($" | Clasifica: {eA.Nombre}");
                ganadores.Agregar(eA);
            }
            else if (g02 > g01)
            {
                Console.WriteLine($" | Clasifica: {eB.Nombre}");
                ganadores.Agregar(eB);
            }
            else
            {
                // Desempate por Penales (Bernoulli)
                Console.Write(" [Empate! Ir a Penales] ->");
                bool ganaA = MotorEstocastico.SimularPenalesBernoulli(eA, eB);
                
                if (ganaA)
                {
                    Console.WriteLine($" Ganador Penales: {eA.Nombre}");
                    ganadores.Agregar(eA);
                }
                else
                {
                    Console.WriteLine($" Ganador Penales: {eB.Nombre}");
                    ganadores.Agregar(eB);
                }
            }
        }
        return ganadores;
    }
}