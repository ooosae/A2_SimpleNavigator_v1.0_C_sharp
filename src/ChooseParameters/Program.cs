using s21_graph;
using s21_graph_algorithms;
namespace ChooseParameters;

internal class Program {
  private static double[] GenerateArray(double start, double end, double step) {
    var values = new List<double>();
    for (double value = start; value <= end; value += step) {
      values.Add(value);
    }
    return values.ToArray();
  }
  static void Main(string[] args) {
    Graph graph = new Graph(new int[,] { { 0, 29, 20, 21, 16, 31, 100, 12, 4, 31, 18 },
                                         { 29, 0, 15, 29, 28, 40, 72, 21, 29, 41, 12 },
                                         { 20, 15, 0, 15, 14, 25, 81, 9, 23, 27, 13 },
                                         { 21, 29, 15, 0, 4, 12, 92, 12, 25, 13, 25 },
                                         { 16, 28, 14, 4, 0, 16, 94, 9, 20, 16, 22 },
                                         { 31, 40, 25, 12, 16, 0, 95, 24, 36, 3, 37 },
                                         { 100, 72, 81, 92, 94, 95, 0, 90, 101, 99, 84 },
                                         { 12, 21, 9, 12, 9, 24, 90, 0, 15, 25, 13 },
                                         { 4, 29, 23, 25, 20, 36, 101, 15, 0, 35, 18 },
                                         { 31, 41, 27, 13, 16, 3, 99, 25, 35, 0, 38 },
                                         { 18, 12, 13, 25, 22, 37, 84, 13, 18, 38, 0 } });

    double[] amountOfPheromoneValues = GenerateArray(1, 20, 1);
    double[] initAmountOfPheromoneValues = GenerateArray(1, 20, 1);
    double[] influenceDistanceRateValues = GenerateArray(1, 5, 0.5);
    double[] influencePheromoneRateValues = GenerateArray(0.0, 5, 0.5);
    double[] pheromoneEvaporationCoefficientValues = GenerateArray(0.0, 1, 0.05);
    int counter = 0;

    foreach (var initAmountOfPheromone in initAmountOfPheromoneValues)
      foreach (var amountOfPheromone in amountOfPheromoneValues)
        foreach (var influencePhepomoneRate in influencePheromoneRateValues)
          foreach (var influenceDistanceRate in influenceDistanceRateValues)
            foreach (var pheromoneEvaporationCoefficient in pheromoneEvaporationCoefficientValues)

            {
              AntColonyPathFinder antColonyPathFinder = new AntColonyPathFinder(
                  amountOfPheromone: amountOfPheromone,
                  initAmountOfPheromone: initAmountOfPheromone,
                  influenceDistanceRate: influenceDistanceRate,
                  influencePhepomoneRate: influencePhepomoneRate,
                  pheromoneEvaporationCoefficient: pheromoneEvaporationCoefficient, randomSeed: 21);
              double d = antColonyPathFinder.GetPath(graph, 1).Distance;
              ++counter;
              if (d < 254) {
                Console.WriteLine($"******\n{counter}:{d}");
                                Console.WriteLine($"""
amountOfPheromone = {amountOfPheromone},
initAmountOfPheromone = {initAmountOfPheromone},
influenceDistanceRate = {influenceDistanceRate},
influencePhepomoneRate = {influencePhepomoneRate},
pheromoneEvaporationCoefficient = {
                  pheromoneEvaporationCoefficient}
""");
                                break;
              }
            }
    Console.WriteLine("End");
  }
}
