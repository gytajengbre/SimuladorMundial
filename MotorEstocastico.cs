using System;

public static class MotorEstocastico
{
    private static Random rand = new Random();
    private const double SCALE = 400.0; // Constante de escala del PDF
    private const double G_AVG = 2.5;    // Promedio global de goles por partido

    // 1. Calcula la brecha de rendimiento (Delta ELO)
    public static double CalcularDeltaElo(Equipo a, Equipo b)
    {
        return (a.Elo - b.Elo) / SCALE;
    }

    // 2. Calcula los goles esperados (Lambda) para cada equipo
    public static (double lambdaA, double lambdaB) CalcularLambdas(double deltaElo)
    {
        // Se usa Math.Max(0.05) para evitar valores negativos o extremadamente bajos
        double lambdaA = Math.Max(0.05, (G_AVG + deltaElo) / 2.0);
        double lambdaB = Math.Max(0.05, (G_AVG - deltaElo) / 2.0);
        return (lambdaA, lambdaB);
    }

    // 3. Generador de goles estocásticos usando el Algoritmo de Knuth para Poisson
    private static int SimularGolesPoisson(double lambda)
    {
        double L = Math.Exp(-lambda);
        int k = 0;
        double p = 1.0;

        do
        {
            k++;
            p *= rand.NextDouble();
        } while (p > L);

        return k - 1;
    }

    // 4. Simula un partido completo devolviendo los goles de cada uno
    public static (int golesA, int golesB) SimularPartido(Equipo a, Equipo b)
    {
        double deltaElo = CalcularDeltaElo(a, b);
        var (lambdaA, lambdaB) = CalcularLambdas(deltaElo);

        int golesA = SimularGolesPoisson(lambdaA);
        int golesB = SimularGolesPoisson(lambdaB);

        return (golesA, golesB);
    }
    // 5. NUEVO MÉTODO: Definición por Penales (Distribución de Bernoulli basada en ELO)
    public static bool SimularPenalesBernoulli(Equipo a, Equipo b)
    {
        double probabilidadA = a.Elo / (a.Elo + b.Elo);
        double disparoAleatorio = rand.NextDouble();

        // Si el número aleatorio es menor a su probabilidad, gana el Equipo A
        return disparoAleatorio < probabilidadA;
    }
}