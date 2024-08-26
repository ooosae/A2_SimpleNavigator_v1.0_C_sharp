using s21_graph;

namespace ConsoleInterface;

internal class UI {
  private Graph? _graph = null;
  private bool _continue = true;
  internal bool RunUI() {
    try {
      Console.WriteLine("Choose an option:");
      Console.WriteLine("1. Load graph from file");
      Console.WriteLine("2. Traverse graph in breadth");
      Console.WriteLine("3. Traverse graph in depth");
      Console.WriteLine("4. Find shortest path between two vertices");
      Console.WriteLine("5. Find shortest paths between all pairs of vertices");
      Console.WriteLine("6. Find minimum spanning tree");
      Console.WriteLine("7. Solve the Traveling Salesman Problem");
      Console.WriteLine("8. Export graph to dot.");
      Console.WriteLine("9. Exit");

      var choice = Console.ReadLine();
      switch (choice) {
        case "1":
          LoadGraphMenuPoint();
          break;
        case "2":
          BreadhTraverseMenuPoint();
          break;
        case "3":
          DepthTraverseMenuPoint();
          break;
        case "4":
          ShortestPathBetweenTwoVerticesMenuPoint();
          break;
        case "5":
          ShortestPathsBetweenAllVerticesMenuPoint();
          break;
        case "6":
          MinimumSpanningTreeMenuPoint();
          break;
        case "7":
          SolveTravelingSalesmanProblemMenuPoint();
          break;
        case "8":
          ExportToDotMenuPoint();
          break;
        case "9":
          ExitMenuPoint();
          break;
        default:
          Console.WriteLine("Invalid option. Try again.");
          break;
      }
    } catch (Exception ex) {
      Console.WriteLine($"Operation cannot be done! {ex.Message}");
    }

    Console.WriteLine("");
    return _continue;
  }

  private void ExportToDotMenuPoint() {
    ThrowIfGraphIsNull();
    Console.Write("Set filename: ");
    string? fileName = Console.ReadLine();
    if (string.IsNullOrEmpty(fileName) || !fileName!.All(char.IsLetterOrDigit)) {
      Console.WriteLine("Import failed. Invalid file name.");
      return;
    }
    Controller.ExportToDot(_graph!, fileName! + ".dot");
    Console.WriteLine("Import succeed.");
  }

  private void ExitMenuPoint() {
    Console.WriteLine("Goodbye!");
    _continue = false;
  }

  private void LoadGraphMenuPoint() {
    Console.Write("Enter file path: ");
    var filePath = Console.ReadLine() ?? "";
    _graph = Controller.LoadGraphFromFile(filePath);
    Console.WriteLine($"Loaded.\n{_graph}");
  }

  private void BreadhTraverseMenuPoint() {
    ThrowIfGraphIsNull();
    Console.Write("Enter start vertex: ");

    int vertex = int.Parse(Console.ReadLine() ?? "");

    var result = Controller.BreadthFirstTraversal(_graph!, vertex);
    WarnIfNotAllVerticiesAreReachable(result);
    Console.WriteLine("Breadth First Traversal:");
    Console.WriteLine(string.Join(" -> ", result));
  }

  private void WarnIfNotAllVerticiesAreReachable(int[] result) {
    if (result.Length < _graph!.VertexCount) {
      Console.WriteLine("WARNING! Not all vertices of the graph are reachable.");
    }
  }

  private void DepthTraverseMenuPoint() {
    ThrowIfGraphIsNull();
    Console.Write("Enter start vertex: ");
    int start_vertex = int.Parse(Console.ReadLine() ?? "");
    var result = Controller.DepthFirstTraversal(_graph!, start_vertex);
    WarnIfNotAllVerticiesAreReachable(result);
    Console.WriteLine("Depth First Traversal:");
    Console.WriteLine(string.Join(" -> ", result));
  }

  private void ShortestPathBetweenTwoVerticesMenuPoint() {
    ThrowIfGraphIsNull();
    Console.Write("Enter start vertex: ");
    int start = int.Parse(Console.ReadLine() ?? "");
    Console.Write("Enter finish vertex: ");
    int finish = int.Parse(Console.ReadLine() ?? "");

    var result = Controller.ShortestPathBetweenVertices(_graph!, start, finish);

    Console.WriteLine($"Length of shortest Path from {start} to {finish}: {result}.");
  }

  private void ShortestPathsBetweenAllVerticesMenuPoint() {
    ThrowIfGraphIsNull();
    var result = Controller.ShortestPathsBetweenAllVertices(_graph!);
    Console.WriteLine("Shortest Paths Between All Vertices:");
    OuputMatrix(result);
  }

  private void MinimumSpanningTreeMenuPoint() {
    ThrowIfGraphIsNull();
    var result = Controller.MinimumSpanningTree(_graph!);
    Console.WriteLine("Minimum Spanning Tree:");
    OuputMatrix(result);
  }

  private void SolveTravelingSalesmanProblemMenuPoint() {
    ThrowIfGraphIsNull();
    var result = Controller.SolveTravelingSalesmanProblem(_graph!);
    Console.WriteLine("Traveling Salesman Problem:");
    Console.WriteLine($"Route: {string.Join(" -> ", result.Vertices)}");
    Console.WriteLine($"Distance: {result.Distance}");
  }

  private void ThrowIfGraphIsNull() {
    if (_graph is null || _graph.VertexCount == 0) {
      throw new InvalidOperationException("Graph is not loaded. Please load a graph first.");
    }
  }

  private static void OuputMatrix(int[,] result) {
    for (int i = 0; i < result.GetLength(0); i++) {
      for (int j = 0; j < result.GetLength(1); j++) {
        Console.Write($"{result[i, j]} ");
      }
      Console.WriteLine("");
    }
  }
}
