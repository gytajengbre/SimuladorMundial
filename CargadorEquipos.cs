using System;

public static class CargadorEquipos
{
    public static ListaEnlazada<Grupo> InicializarGruposOficiales()
    {
        ListaEnlazada<Grupo> listaGrupos = new ListaEnlazada<Grupo>();

        for (char c = 'A'; c <= 'L'; c++)
        {
            listaGrupos.Agregar(new Grupo(c));
        }

        InyectarEquipoAGrupo(listaGrupos, 'A', "México", 1600);
        InyectarEquipoAGrupo(listaGrupos, 'A', "Sudáfrica", 1355);
        InyectarEquipoAGrupo(listaGrupos, 'A', "Corea del Sur", 1530);
        InyectarEquipoAGrupo(listaGrupos, 'A', "República Checa", 1475);

        InyectarEquipoAGrupo(listaGrupos, 'B', "Canadá", 1400);
        InyectarEquipoAGrupo(listaGrupos, 'B', "Bosnia y Her.", 1310);
        InyectarEquipoAGrupo(listaGrupos, 'B', "Qatar", 1480);
        InyectarEquipoAGrupo(listaGrupos, 'B', "Suiza", 1570);

        InyectarEquipoAGrupo(listaGrupos, 'C', "Brasil", 1776);
        InyectarEquipoAGrupo(listaGrupos, 'C', "Marruecos", 1620);
        InyectarEquipoAGrupo(listaGrupos, 'C', "Haití", 1280);
        InyectarEquipoAGrupo(listaGrupos, 'C', "Escocia", 1450);

        InyectarEquipoAGrupo(listaGrupos, 'D', "Estados Unidos", 1610);
        InyectarEquipoAGrupo(listaGrupos, 'D', "Paraguay", 1375);
        InyectarEquipoAGrupo(listaGrupos, 'D', "Australia", 1515);
        InyectarEquipoAGrupo(listaGrupos, 'D', "Turquía", 1510);

        InyectarEquipoAGrupo(listaGrupos, 'E', "Alemania", 1590);
        InyectarEquipoAGrupo(listaGrupos, 'E', "Curazao", 1260);
        InyectarEquipoAGrupo(listaGrupos, 'E', "Costa de Marfil", 1460);
        InyectarEquipoAGrupo(listaGrupos, 'E', "Ecuador", 1495);

        InyectarEquipoAGrupo(listaGrupos, 'F', "Países Bajos", 1724);
        InyectarEquipoAGrupo(listaGrupos, 'F', "Japón", 1580);
        InyectarEquipoAGrupo(listaGrupos, 'F', "Suecia", 1500);
        InyectarEquipoAGrupo(listaGrupos, 'F', "Túnez", 1440);

        InyectarEquipoAGrupo(listaGrupos, 'G', "Bélgica", 1744);
        InyectarEquipoAGrupo(listaGrupos, 'G', "Egipto", 1470);
        InyectarEquipoAGrupo(listaGrupos, 'G', "Irán", 1565);
        InyectarEquipoAGrupo(listaGrupos, 'G', "Nueva Zelanda", 1200);

        InyectarEquipoAGrupo(listaGrupos, 'H', "España", 1772);
        InyectarEquipoAGrupo(listaGrupos, 'H', "Cabo Verde", 1340);
        InyectarEquipoAGrupo(listaGrupos, 'H', "Arabia Saudita", 1380);
        InyectarEquipoAGrupo(listaGrupos, 'H', "Uruguay", 1660);

        InyectarEquipoAGrupo(listaGrupos, 'I', "Francia", 1840);
        InyectarEquipoAGrupo(listaGrupos, 'I', "Senegal", 1550);
        InyectarEquipoAGrupo(listaGrupos, 'I', "Irak", 1360);
        InyectarEquipoAGrupo(listaGrupos, 'I', "Noruega", 1410);

        InyectarEquipoAGrupo(listaGrupos, 'J', "Argentina", 1860);
        InyectarEquipoAGrupo(listaGrupos, 'J', "Argelia", 1415);
        InyectarEquipoAGrupo(listaGrupos, 'J', "Austria", 1520);
        InyectarEquipoAGrupo(listaGrupos, 'J', "Jordania", 1320);

        InyectarEquipoAGrupo(listaGrupos, 'K', "Portugal", 1741);
        InyectarEquipoAGrupo(listaGrupos, 'K', "RD Congo", 1330);
        InyectarEquipoAGrupo(listaGrupos, 'K', "Uzbekistán", 1335);
        InyectarEquipoAGrupo(listaGrupos, 'K', "Colombia", 1721);

        InyectarEquipoAGrupo(listaGrupos, 'L', "Inglaterra", 1785);
        InyectarEquipoAGrupo(listaGrupos, 'L', "Croacia", 1640);
        InyectarEquipoAGrupo(listaGrupos, 'L', "Ghana", 1350);
        InyectarEquipoAGrupo(listaGrupos, 'L', "Panamá", 1420);

        return listaGrupos;
    }

    private static void InyectarEquipoAGrupo(ListaEnlazada<Grupo> lista, char letra, string nombre, double elo)
    {
        for (int i = 0; i < lista.Contar; i++)
        {
            Grupo g = lista.Obtener(i);
            if (g.Letra == letra)
            {
                g.Equipos.Agregar(new Equipo(nombre, elo) { GrupoOrigen = letra });
                break;
            }
        }
    }
}