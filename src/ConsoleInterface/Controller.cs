using s21_graph;
using s21_graph_algorithms;

namespace ConsoleInterface {
  internal static class Controller {
    public static int[] BreadthFirstTraversal(Graph graph, int start_vertex) {
      return graph.BreadthFirstSearch(start_vertex);
    }

    public static int[] DepthFirstTraversal(Graph graph, int start_vertex) {
      return graph.DepthFirstSearch(start_vertex);
    }

    public static int[,] MinimumSpanningTree(Graph graph) {
      return graph.GetLeastSpanningTree();
    }

    public static int ShortestPathBetweenVertices(Graph graph, int start, int finish) {
      return graph.GetShortestPathBetweenVertices(start, finish);
    }

    public static int[,] ShortestPathsBetweenAllVertices(Graph graph) {
      return graph.GetShortestPathsBetweenAllVertices();
    }

    public static TsmResult SolveTravelingSalesmanProblem(Graph graph) {
      return graph.SolveTravelingSalesmanProblem();
    }

    public static Graph LoadGraphFromFile(string filePath) {
      Graph graph = new();
      graph.LoadGraphFromFile(filePath);
      return graph;
    }

    public static void ExportToDot(Graph graph, string fileName) {
      graph.ExportGraphToDot(fileName);
    }
  }
}