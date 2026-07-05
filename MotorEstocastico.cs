using System;

public static class MotorEstocastico
{
    private static Random rand = new Random();
    private const double SCALE = 400.0;
    private const double G_AVG = 2.5;

    public static double CalcularDeltaElo(Equipo a, Equipo b)
    {
        return (a.Elo - b.Elo) / SCALE;
    }

    public static (double lambdaA, double lambdaB) CalcularLambdas(double deltaElo)
    {
        double lambdaA = Math.Max(0.05, (G_AVG + deltaElo) / 2.0);
        double lambdaB = Math.Max(0.05, (G_AVG - deltaElo) / 2.0);
        return (lambdaA, lambdaB);
    }

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

    public static (int golesA, int golesB) SimularPartido(Equipo a, Equipo b)
    {
        double deltaElo = CalcularDeltaElo(a, b);
        var (lambdaA, lambdaB) = CalcularLambdas(deltaElo);

        int golesA = SimularGolesPoisson(lambdaA);
        int golesB = SimularGolesPoisson(lambdaB);

        return (golesA, golesB);
    }

    public static bool SimularPenalesBernoulli(Equipo a, Equipo b)
    {
        double probabilidadA = a.Elo / (a.Elo + b.Elo);
        double disparoAleatorio = rand.NextDouble();

        return disparoAleatorio < probabilidadA;
    }
}