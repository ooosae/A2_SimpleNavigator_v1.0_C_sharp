using s21_graph;

namespace s21_graph_algorithms;

public class AntColonyPathFinder {
  private const int _MinStepsCount = 60;
  // algorithm parameters
  public int StepsCount { get; private set; }
  public double AmountOfPheromone { get; }
  public double InitAmountOfPheromone { get; }
  public double InfluenceDistanceRate { get; }
  public double InfluencePheromoneRate { get; }
  public double PheromoneEvaporationCoefficient { get; }
  public int? RandomSeed { get; }

  // Start data
  private Graph _graph = new();
  private int? _startVertex;

  // solving artefacts
  private Random _random = new();
  private double[,] _desirabilityOfTransition = new double[,] {};
  private double[,] _pheromone = new double[,] {};
  private List<int> _bestPath = new();
  private double _bestLength;

  public AntColonyPathFinder(int stepsCount = 0, double influencePheromoneRate = 1,
                             double influenceDistanceRate = 1.5,
                             double pheromoneEvaporationCoefficient = 0.2,
                             double amountOfPheromone = 1, double initAmountOfPheromone = 1,
                             int? randomSeed = null) {
    StepsCount = stepsCount;
    InfluenceDistanceRate = influenceDistanceRate;
    InfluencePheromoneRate = influencePheromoneRate;
    PheromoneEvaporationCoefficient = pheromoneEvaporationCoefficient;
    AmountOfPheromone = amountOfPheromone;
    InitAmountOfPheromone = initAmountOfPheromone;
    RandomSeed = randomSeed;

    ThrowIfAlgParamsAreWrong();
  }

  public TsmResult GetPath(Graph graph, int? startVertex = null) {
    _graph = graph;
    _startVertex = startVertex;
    if (StepsCount == 0) {
      StepsCount = Math.Max(_MinStepsCount, graph.VertexCount * graph.VertexCount);
    }

    InitStartState();
    int counter = 0;
    while (counter++ < StepsCount) {
      ColonyStep();
    }

    if (_bestPath.Count == 0) {
      throw new ArgumentException("It is impossible to solve the problem with a given graph.");
    }

    return new TsmResult(_bestPath, _bestLength);
  }

  private void ColonyStep() {
    var deltaPheromone = new double[_pheromone.GetLength(0), _pheromone.GetLength(1)];
    if (_startVertex is null) {
      for (int startVertex = 1; startVertex <= _graph.VertexCount; startVertex++) {
        AntRun(startVertex, deltaPheromone);
      }
    } else {
      for (int i = 1; i <= _graph.VertexCount; i++) {
        AntRun((int)_startVertex, deltaPheromone);
      }
    }

    UpdatePheromone(deltaPheromone);
  }

  private void AntRun(int startVertex, double[,] deltaPheromone) {
    List<int> path = [startVertex];
    int pathLength = 0;
    bool[] visited = new bool[_graph.VertexCount + 1];
    visited[startVertex] = true;
    int counter = 1;
    while (++counter <= _graph.VertexCount) {
      int from = path[^1];
      int to = ChooseNextVertex(from, visited);
      if (to <= 0) {
        return;
      }

      path.Add(to);
      pathLength += _graph[from, to];
      visited[to] = true;
    }
    int lastVertex = path[^1];
    if (_graph[lastVertex, startVertex] == 0) {
      return;  //если из последней добавленной вершины невозможно попасть в начальную точку,
               //ничего не обновляем
    }

    path.Add(startVertex);
    pathLength += _graph[lastVertex, startVertex];

    UpdateDeltaPheromone(path, pathLength, deltaPheromone);
    if (pathLength <= _bestLength) {
      _bestLength = pathLength;
      _bestPath = path;
    }
  }

  private void UpdateDeltaPheromone(List<int> path, int pathLength, double[,] deltaPheromone) {
    for (int i = 1; i < path.Count; i++) {
      int from = path[i - 1];
      int to = path[i];
      deltaPheromone[from, to] += AmountOfPheromone / pathLength;
    }
  }

  private int ChooseNextVertex(int current, bool[] visited) {
    double choise = _random.NextDouble();  //[0,1)
    double[]? probabilities = GetNightboursProbabilities(current, visited);

    if (probabilities is null) {
      return -1;
    }

    double[] cumProbabilitie =
        probabilities.Select((value, index) => probabilities.Take(index + 1).Sum())
            .ToArray();  // кумулятивные вероятности

    return Array.FindIndex(cumProbabilitie, p => p > choise);
  }

  private double[]? GetNightboursProbabilities(int from, bool[] visited) {
    var probability = new double[_graph.VertexCount + 1];
    double sum = 0;
    for (int to = 1; to <= _graph.VertexCount; to++) {
      if (_graph[from, to] > 0 && !visited[to]) {
        probability[to] = DesirabilityOfTransitionBetweenTwoVertices(from, to);
        sum += probability[to];
      }
    }
    return sum > 0 ? probability.Select(p => p / sum).ToArray() : null;
  }

  private double DesirabilityOfTransitionBetweenTwoVertices(int from, int to) {
    return Math.Pow(_pheromone[from, to], InfluencePheromoneRate) *
           Math.Pow(_desirabilityOfTransition[from, to], InfluenceDistanceRate);
  }

  private void UpdatePheromone(double[,] deltaPheromone) {
    for (int from = 1; from < _pheromone.GetLength(0); from++) {
      for (int to = 1; to < _pheromone.GetLength(1); to++) {
        _pheromone[from, to] =
            (1 - PheromoneEvaporationCoefficient) * _pheromone[from, to] + deltaPheromone[from, to];
      }
    }
  }

  private void InitStartState() {
    InitRandom();
    InitDesirabilityOfTransitionMatrix();
    InitPheromoneMatrix();

    _bestPath = new List<int>();
    _bestLength = int.MaxValue;
  }

  private void InitPheromoneMatrix() {
    _pheromone = new double[_graph.VertexCount + 1, _graph.VertexCount + 1];
    for (int from = 1; from <= _graph.VertexCount; from++) {
      for (int to = 1; to <= _graph.VertexCount; to++) {
        if (_graph[from, to] > 0 && from != to) {
          _pheromone[from, to] = InitAmountOfPheromone;
        }
      }
    }
  }

  private void InitDesirabilityOfTransitionMatrix() {
    _desirabilityOfTransition = new double[_graph.VertexCount + 1, _graph.VertexCount + 1];
    for (int from = 1; from <= _graph.VertexCount; from++) {
      for (int to = 1; to <= _graph.VertexCount; to++) {
        if (_graph[from, to] > 0 && from != to) {
          _desirabilityOfTransition[from, to] = 1.0 / _graph[from, to];
        }
      }
    }
  }

  private void InitRandom() {
    _random = RandomSeed is not null ? new Random((int)RandomSeed) : new Random();
  }

  private void ThrowIfAlgParamsAreWrong() {
    if (StepsCount < 0) {
      throw new ArgumentException("StepsCount must be equal or greater than 0.");
    }
    if (AmountOfPheromone < 0) {
      throw new ArgumentException("AmountOfPheromone must be equal or greater than 0.");
    }
    if (InitAmountOfPheromone < 0) {
      throw new ArgumentException("InitAmountOfPheromone must be equal or greater than 0.");
    }
    if (InfluenceDistanceRate < 1 || InfluenceDistanceRate > 5) {
      throw new ArgumentException("InfluenceDistanceRate must be between 1 and 5.");
    }
    if (InfluencePheromoneRate < 0 || InfluencePheromoneRate > 5) {
      throw new ArgumentException("InfluencePheromoneRate must be between 0 and 5.");
    }
    if (PheromoneEvaporationCoefficient < 0 || PheromoneEvaporationCoefficient > 1) {
      throw new ArgumentException("PheromoneEvaporationCoefficient must be between 0 and 1.");
    }
  }
}
