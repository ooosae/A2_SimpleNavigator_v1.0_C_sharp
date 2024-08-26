using s21_graph;

namespace s21_graph_algorithms;

public static partial class GraphAlgorithms {
  public static int[] DepthFirstSearch(this Graph graph, int start_vertex) {
    ThrowIfVertexIsOutOfRange(graph, start_vertex);
    List<int> result = [start_vertex];
    HashSet<int> visited = [start_vertex];
    s21_helpers.Containers.Stack path = new();
    path.Push(start_vertex);
    int from = start_vertex;
    int to;
    // пока не посетили все вершины
    while (visited.Count != graph.VertexCount && path.Count() != 0) {
      int counter = 0;
      // перебираем все возможные точки назначения
      for (to = 1; to <= graph.VertexCount; to++) {
        // если вершина еще не посещена и достижима, спускаемся в нее
        if (!visited.Contains(to) && graph[from, to] > 0) {
          path.Push(to);
          visited.Add(to);
          result.Add(to);
          from = to;
          break;
        }
        counter++;
      }
      // Если нет достижимых вершин, возвращаемся на уровень выше
      if (counter == graph.VertexCount) {
        if (path.Count() <= 1) {
          break;
        }

        path.Pop();
        from = path.Top();
      }
    }

    return result.ToArray();
  }

  public static int[] BreadthFirstSearch(this Graph graph, int start_vertex) {
    ThrowIfVertexIsOutOfRange(graph, start_vertex);
    List<int> result = [start_vertex];
    HashSet<int> visited = [start_vertex];
    s21_helpers.Containers.Queue path = new();

    path.Push(start_vertex);
    // пока не посетили все вершины
    while (path.Count() != 0) {
      // выталкиваем первую вершину, ставим ее стартовой
      int from = path.Pop();

      // заталкиваем в очередь все непосещенные вершины со следующего уровня от стартовой
      for (int to = 1; to <= graph.VertexCount; to++) {
        if (!visited.Contains(to) && graph[from, to] > 0) {
          path.Push(to);
          visited.Add(to);
          result.Add(to);
        }
      }
    }

    return result.ToArray();
  }

  // searching for the shortest path between two Vertices in a graph using Dijkstra's algorithm.
  // The function accepts as input the numbers of two Vertices and
  // returns a numerical result equal to the smallest Distance between them.
  public static int GetShortestPathBetweenVertices(this Graph graph, int start, int finish) {
    ThrowIfVertexIsOutOfRange(graph, start);
    ThrowIfVertexIsOutOfRange(graph, finish);
    ApplyDijkstraAlgorithm(graph, start, out int[] distance, out int[] _, out bool[] visited);
    if (!visited[finish]) {
      throw new Exception($"Vertex {finish} is not reachable from {start}.");
    }
    return distance[finish];
  }

  // Floyd-Warshall algorithm RETURNS DISTANCES
  public static int[,] GetShortestPathsBetweenAllVertices(this Graph graph) {
    InitDistanceAndNextMatrices(graph, out int[,] distance, out int[,] next, out int inf);
    FillDistanceAndNextMatrices(distance, next);
    TurnInfDistanceToNegative(distance, inf);
    return distance;
  }

  // — searching for the minimal spanning tree in a graph using Prim's algorithm.
  // As a result, the function should return the adjacency matrix for the minimal spanning tree.
  public static int[,] GetLeastSpanningTree(this Graph graph) {
    // На вход алгоритма подаётся связный неориентированный граф.
    if (!graph.IsConnectedAndUndirected()) {
      throw new ArgumentException($"Graph must be connected and undirected.");
    }

    var result = new int[graph.VertexCount, graph.VertexCount];

    int firstVertex = 1;
    HashSet<int> used = [firstVertex];
    PriorityQueue<(int I, int J), int> priorityQueue = new();
    AddEdgesToNonUsedVerticesToPriorityQueue(graph, firstVertex, used, priorityQueue);

    while (used.Count < graph.VertexCount) {
      (int UsedVertex, int NewVertex) edge;
      int distance;
      do {
        priorityQueue.TryDequeue(out edge, out distance);
      } while (used.Contains(edge.NewVertex));

      result[edge.UsedVertex - 1, edge.NewVertex - 1] = distance;
      result[edge.NewVertex - 1, edge.UsedVertex - 1] = distance;
      used.Add(edge.NewVertex);
      AddEdgesToNonUsedVerticesToPriorityQueue(graph, edge.NewVertex, used, priorityQueue);
    }
    return result;
  }

  public static TsmResult SolveTravelingSalesmanProblem(this Graph graph) {
    if (!graph.TravelingSalesmanCriterion()) {
      throw new ArgumentException(
          "There is no solution to the Traveling Salesman Problem for the graph.");
    }
    AntColonyPathFinder antColonyPathFinder = new AntColonyPathFinder(randomSeed: 21);
    TsmResult result = antColonyPathFinder.GetPath(graph, 1);

    return result;
  }

  public static int[] GetShortestPathDijkstraAlg(this Graph graph, int start, int finish) {
    ThrowIfVertexIsOutOfRange(graph, start);
    ThrowIfVertexIsOutOfRange(graph, finish);
    ApplyDijkstraAlgorithm(graph, start, out int[] distance, out int[] previous,
                           out bool[] visited);

    List<int> path = [];
    if (visited[finish]) {
      path = GetRetracedPathForDijkstraAlg(finish, previous);
    }

    return path.ToArray();
  }

  private static bool TravelingSalesmanCriterion(this Graph graph) {
    if (graph.IsFull()) {
      return true;
    }

    int oddVerticesCount = 0;
    int incomeVerticiesCount = 0;
    for (int col = 1; col <= graph.VertexCount; col++) {
      for (int row = 1; row <= graph.VertexCount; row++) {
        incomeVerticiesCount = 0;
        if (col != row && graph[row, col] > 0) {
          incomeVerticiesCount++;
        }
      }
      oddVerticesCount += (incomeVerticiesCount % 2 == 1) ? 1 : 0;
      if (oddVerticesCount > 2) {
        return false;
      }
    }
    return true;
  }
  private static bool IsFull(this Graph graph) {
    for (int i = 1; i < graph.VertexCount; i++) {
      for (int j = 1; j < graph.VertexCount; j++) {
        if (i != j && graph[i, j] <= 0) {
          return false;
        }
      }
    }
    return true;
  }

  // part of  Floyd-Warshall algorithm
  private static void InitDistanceAndNextMatrices(Graph graph, out int[,] distance, out int[,] next,
                                                  out int infDist) {
    int matricesSize = graph.VertexCount;
    distance = new int[matricesSize, matricesSize];
    next = new int[graph.VertexCount, graph.VertexCount];
    infDist = graph.MaxPossibleValue;
    for (int i = 0; i < matricesSize; i++) {
      for (int j = 0; j < matricesSize; j++) {
        if (i != j) {
          distance[i, j] = graph[i + 1, j + 1] > 0 ? graph[i + 1, j + 1] : infDist;
          next[i, j] = graph[i + 1, j + 1] > 0 ? j + 1 : 0;
        }
      }
    }
  }

  // part of Floyd-Warshall algorithm
  private static void FillDistanceAndNextMatrices(int[,] distance, int[,] next) {
    int matricesSize = distance.GetLength(0);

    for (int k = 0; k < matricesSize; k++) {
      for (int i = 0; i < matricesSize; i++) {
        for (int j = 0; j < matricesSize; j++) {
          if (i != j && distance[i, k] + distance[k, j] > 0 &&  // <-- overflow checking
              distance[i, j] > distance[i, k] + distance[k, j]) {
            distance[i, j] = distance[i, k] + distance[k, j];
            next[j, j] = next[j, k];
          }
        }
      }
    }
  }

  private static void TurnInfDistanceToNegative(int[,] distance, int inf) {
    for (int i = 0; i < distance.GetLength(0); i++) {
      for (int j = 0; j < distance.GetLength(1); j++) {
        if (distance[i, j] == inf) {
          distance[i, j] = -1;
        }
      }
    }
  }

  // part of Dijkstra Algorithm
  private static void ApplyDijkstraAlgorithm(this Graph graph, int start, out int[] distance,
                                             out int[] previous, out bool[] visited) {
    ThrowIfVertexIsOutOfRange(graph, start);
    int infDist = graph.MaxPossibleValue + 1;
    distance = Enumerable.Repeat(infDist, graph.VertexCount + 1)
                   .ToArray();  // чтобы не париться с нумерацией добавили 1 элемент
    previous = new int[distance.Length];
    visited = Enumerable.Repeat(false, distance.Length).ToArray();
    visited[0] = true;
    distance[start] = 0;
    previous[start] = 0;

    int current = start;
    while (distance[current] != infDist) {
      UpdateNightboursDist(graph, current, distance, previous);
      visited[current] = true;
      current = GetNearestNonVisited(visited, distance);
    }
  }

  private static List<int> GetRetracedPathForDijkstraAlg(int finish, int[] previous) {
    List<int> path = [];
    int current = finish;
    while (current != 0) {
      path.Add(current);
      current = previous[current];
    }

    path.Reverse();

    return path;
  }

  private static int GetNearestNonVisited(bool[] visited, int[] distances) {
    int minDistance = int.MaxValue;
    int nearestVertex = 0;

    for (int i = 0; i < visited.Length; i++) {
      if (!visited[i] && distances[i] < minDistance) {
        minDistance = distances[i];
        nearestVertex = i;
      }
    }

    return nearestVertex;
  }

  private static void UpdateNightboursDist(Graph graph, int vertex, int[] distances,
                                           int[] previous) {
    for (int i = 1; i <= graph.VertexCount; i++) {
      if (i != vertex && graph[vertex, i] > 0) {
        int currentDistance = distances[vertex] + graph[vertex, i];
        if (currentDistance < distances[i]) {
          distances[i] = currentDistance;
          previous[i] = vertex;
        }
      }
    }
  }

  private static void ThrowIfVertexIsOutOfRange(Graph graph, int start_vertex) {
    if (graph is null || start_vertex < 1 || graph.VertexCount < start_vertex) {
      throw new IndexOutOfRangeException("Vertex is out of range.");
    }
  }

  private static bool IsConnectedAndUndirected(this Graph graph) {
    return graph.IsUndirected() && graph.BreadthFirstSearch(1).Length == graph.VertexCount;
  }

  // For Prime Alg
  private static void AddEdgesToNonUsedVerticesToPriorityQueue(
      Graph graph, int startVertex, HashSet<int> used,
      PriorityQueue<(int I, int J), int> priorityQueue) {
    for (int j = 1; j <= graph.VertexCount; j++) {
      if (graph[startVertex, j] != 0 && !used.Contains(j)) {
        priorityQueue.Enqueue((startVertex, j), graph[startVertex, j]);
      }
    }
  }
}
