using System;

public static class CargadorEquipos
{
    public static ListaEnlazada<Equipo> Obtener48Equipos()
    {
        ListaEnlazada<Equipo> lista = new ListaEnlazada<Equipo>();

        // Registramos las 48 selecciones clasificadas ordenadas por ELO inicial (Datos del PDF de la cátedra)
        lista.Agregar(new Equipo("Argentina", 2120));
        lista.Agregar(new Equipo("Francia", 2040));
        lista.Agregar(new Equipo("España", 2020));
        lista.Agregar(new Equipo("Inglaterra", 2000));
        lista.Agregar(new Equipo("Brasil", 1980));
        lista.Agregar(new Equipo("Bélgica", 1960));
        lista.Agregar(new Equipo("Países Bajos", 1940));
        lista.Agregar(new Equipo("Portugal", 1920));
        lista.Agregar(new Equipo("Italia", 1900));
        lista.Agregar(new Equipo("Alemania", 1880));
        lista.Agregar(new Equipo("Uruguay", 1860));
        lista.Agregar(new Equipo("Croacia", 1840));
        lista.Agregar(new Equipo("Marruecos", 1820));
        lista.Agregar(new Equipo("Colombia", 1800));
        lista.Agregar(new Equipo("EE. UU.", 1780));
        lista.Agregar(new Equipo("México", 1760));
        lista.Agregar(new Equipo("Japón", 1740));
        lista.Agregar(new Equipo("Senegal", 1720));
        lista.Agregar(new Equipo("Irán", 1700));
        lista.Agregar(new Equipo("Dinamarca", 1680));
        lista.Agregar(new Equipo("Suiza", 1660));
        lista.Agregar(new Equipo("Corea del Sur", 1640));
        lista.Agregar(new Equipo("Australia", 1620));
        lista.Agregar(new Equipo("Ucrania", 1600));
        lista.Agregar(new Equipo("Austria", 1580));
        lista.Agregar(new Equipo("Suecia", 1560));
        lista.Agregar(new Equipo("Ecuador", 1540));
        lista.Agregar(new Equipo("Polonia", 1520));
        lista.Agregar(new Equipo("Gales", 1500));
        lista.Agregar(new Equipo("Hungría", 1480));
        lista.Agregar(new Equipo("Serbia", 1460));
        lista.Agregar(new Equipo("Túnez", 1440));
        lista.Agregar(new Equipo("Chile", 1420));
        lista.Agregar(new Equipo("Argelia", 1400));
        lista.Agregar(new Equipo("Canadá", 1380));
        lista.Agregar(new Equipo("Perú", 1360));
        lista.Agregar(new Equipo("Nigeria", 1340));
        lista.Agregar(new Equipo("Egipto", 1320));
        lista.Agregar(new Equipo("Camerún", 1300));
        lista.Agregar(new Equipo("Costa de Marfil", 1280));
        lista.Agregar(new Equipo("Costa Rica", 1260));
        lista.Agregar(new Equipo("Paraguay", 1240));
        lista.Agregar(new Equipo("Mali", 1220));
        lista.Agregar(new Equipo("Qatar", 1200));
        lista.Agregar(new Equipo("Arabia Saudita", 1180));
        lista.Agregar(new Equipo("Panamá", 1160));
        lista.Agregar(new Equipo("Jamaica", 1140));
        lista.Agregar(new Equipo("Nueva Zelanda", 1100));

        return lista;
    }
}