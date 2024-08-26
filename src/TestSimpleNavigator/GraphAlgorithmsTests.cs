using System.Diagnostics;
using s21_graph;
using s21_graph_algorithms;
using s21_helpers;

namespace TestSimpleNavigator;

public class GraphAlgorithmsTests {
#region Graphs

  private static Graph Odd() => new Graph(new int[,] { { 0, 2, 3, 4, 5, 6 },
                                                       { 1, 0, 0, 0, 0, 0 },
                                                       { 1, 0, 0, 0, 0, 0 },
                                                       { 1, 0, 0, 0, 0, 6 },
                                                       { 1, 0, 0, 0, 0, 6 },
                                                       { 1, 0, 0, 4, 5, 0 } });

  private static Graph OddWithLoop() => new Graph(new int[,] { { 7, 2, 3, 4, 5, 6 },
                                                               { 1, 7, 0, 0, 0, 0 },
                                                               { 1, 0, 7, 0, 0, 0 },
                                                               { 1, 0, 0, 0, 0, 6 },
                                                               { 1, 0, 0, 0, 0, 6 },
                                                               { 1, 0, 0, 4, 5, 7 } });
  private static Graph OddFull() => new Graph(new int[,] { { 0, 5, 5, 5, 5, 1 },
                                                           { 5, 0, 5, 5, 1, 5 },
                                                           { 1, 5, 0, 5, 5, 5 },
                                                           { 5, 1, 5, 0, 5, 5 },
                                                           { 5, 5, 1, 5, 0, 5 },
                                                           { 5, 5, 5, 1, 5, 0 } });
  private static Graph OddFullWithLoop() => new Graph(new int[,] { { 1, 5, 5, 5, 5, 1 },
                                                                   { 5, 0, 5, 5, 1, 5 },
                                                                   { 1, 5, 0, 5, 5, 5 },
                                                                   { 5, 1, 5, 0, 5, 5 },
                                                                   { 5, 5, 1, 5, 1, 5 },
                                                                   { 5, 5, 5, 1, 5, 5 } });

  private static Graph SpecialFromMaterials() => new Graph(new int[,] {
    { 0, 29, 20, 21, 16, 31, 100, 12, 4, 31, 18 },
    { 29, 0, 15, 29, 28, 40, 72, 21, 29, 41, 12 },
    { 20, 15, 0, 15, 14, 25, 81, 9, 23, 27, 13 },
    { 21, 29, 15, 0, 4, 12, 92, 12, 25, 13, 25 },
    { 16, 28, 14, 4, 0, 16, 94, 9, 20, 16, 22 },
    { 31, 40, 25, 12, 16, 0, 95, 24, 36, 3, 37 },
    { 100, 72, 81, 92, 94, 95, 0, 90, 101, 99, 84 },
    { 12, 21, 9, 12, 9, 24, 90, 0, 15, 25, 13 },
    { 4, 29, 23, 25, 20, 36, 101, 15, 0, 35, 18 },
    { 31, 41, 27, 13, 16, 3, 99, 25, 35, 0, 38 },
    { 18, 12, 13, 25, 22, 37, 84, 13, 18, 38, 0 }
  });
  private static Graph SpecialDirected1() => new Graph(new int[,] { { 0, 1, 1, 1, 1, 1 },
                                                                    { 2, 0, 1, 1, 1, 1 },
                                                                    { 2, 2, 0, 1, 1, 1 },
                                                                    { 2, 2, 2, 0, 1, 1 },
                                                                    { 2, 2, 2, 2, 0, 1 },
                                                                    { 1, 2, 2, 2, 2, 0 } });
  private static Graph SpecialDirected2() => new Graph(new int[,] { { 0, 2, 2, 1, 3, 1 },
                                                                    { 2, 0, 1, 5, 2, 7 },
                                                                    { 2, 2, 0, 3, 1, 1 },
                                                                    { 2, 2, 2, 0, 3, 1 },
                                                                    { 1, 2, 2, 2, 0, 1 },
                                                                    { 2, 1, 2, 2, 2, 0 } });

  private static Graph SingleVertex() => new Graph(new int[,] { { 0 } });
  private static Graph SingleVertexLoop() => new Graph(new int[,] { { 7 } });

  private static Graph TwoVerticesLoop() => new Graph(new int[,] { { 0, 2 }, { 2, 10 } });

  private static Graph TwoDisconnectedVertices() => new Graph(new int[,] { { 0, 0 }, { 0, 0 } });

  private static Graph TwoConnectedVertices() => new Graph(new int[,] { { 0, 1 }, { 1, 0 } });

  private static Graph ManyVerticesOneDisconnected() => new Graph(new int[,] {
    { 0, 1, 1, 1, 0 }, { 1, 0, 1, 1, 0 }, { 1, 1, 0, 1, 0 }, { 1, 1, 1, 0, 0 }, { 0, 0, 0, 0, 0 }
  });

  private static Graph ManyVerticesAllConnected() => new Graph(new int[,] { { 0, 1, 1, 1, 1, 1 },
                                                                            { 1, 0, 1, 1, 1, 1 },
                                                                            { 1, 1, 0, 1, 1, 1 },
                                                                            { 1, 1, 1, 0, 1, 1 },
                                                                            { 1, 1, 1, 1, 0, 1 },
                                                                            { 1, 1, 1, 1, 1, 0 } });

  private static Graph Three() => new Graph(new int[,] { { 0, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 1, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0 },
                                                         { 0, 1, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0 },
                                                         { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 1, 1 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 } });

  private static Graph Full() => new Graph(new int[,] {
    { 0, 1, 1, 1 }, { 1, 0, 1, 1 }, { 1, 1, 0, 1 }, { 1, 1, 1, 0 }
  });

  private static Graph Line() => new Graph(new int[,] {
    { 0, 1, 0, 0, 0 }, { 1, 0, 1, 0, 0 }, { 0, 1, 0, 1, 0 }, { 0, 0, 1, 0, 1 }, { 0, 0, 0, 1, 0 }
  });

  private static Graph Star() => new Graph(new int[,] {
    { 0, 1, 1, 1, 1 }, { 1, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0 }, { 1, 0, 0, 0, 0 }
  });

  private static Graph OneWayDirected() => new Graph(new int[,] {
    { 0, 10, 2, 100 }, { 0, 0, 0, 1 }, { 0, 0, 0, 20 }, { 0, 0, 0, 0 }
  });

  private static Graph Directed() => new Graph(new int[,] {
    { 0, 10, 2, 100 }, { 0, 0, 0, 1 }, { 0, 0, 0, 20 }, { 1, 0, 0, 0 }
  });

  private static Graph Disconnected() => new Graph(new int[,] {
    { 0, 1, 0, 0 }, { 1, 0, 0, 0 }, { 0, 0, 0, 1 }, { 0, 0, 1, 0 }
  });
#endregion

#region DepthFirstSearch
  public static IEnumerable<object[]> GetGraphsForDfs() {
    yield return new object[] { Three(), 1, new int[] { 1, 2, 4, 8, 9, 10, 11, 12, 5, 3, 6, 7 } };
    yield return new object[] { Three(), 2, new int[] { 2, 1, 3, 6, 7, 4, 8, 9, 10, 11, 12, 5 } };
    yield return new object[] { Full(), 1, new int[] { 1, 2, 3, 4 } };
    yield return new object[] { Full(), 2, new int[] { 2, 1, 3, 4 } };
    yield return new object[] { Line(), 1, new int[] { 1, 2, 3, 4, 5 } };
    yield return new object[] { Line(), 3, new int[] { 3, 2, 1, 4, 5 } };
    yield return new object[] { Star(), 1, new int[] { 1, 2, 3, 4, 5 } };
    yield return new object[] { Star(), 4, new int[] { 4, 1, 2, 3, 5 } };
    yield return new object[] { OneWayDirected(), 1, new int[] { 1, 2, 4, 3 } };
    yield return new object[] { OneWayDirected(), 2, new int[] { 2, 4 } };
    yield return new object[] { Directed(), 1, new int[] { 1, 2, 4, 3 } };
    yield return new object[] { Directed(), 2, new int[] { 2, 4, 1, 3 } };
  }

  [Theory]
  [MemberData(nameof(GetGraphsForDfs))]
  public void DepthFirstSearch_ShouldReturnCorrectOrder(Graph graph, int startVertex,
                                                        int[] expected) {
    // Act
    var result = graph.DepthFirstSearch(startVertex);

    // Assert
    Assert.Equal(expected, result);
  }
#endregion

#region BreadthFirstSearch
  public static IEnumerable<object[]> GetGraphsForBfs() {
    yield return new object[] { Three(), 1, new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 } };
    yield return new object[] { Three(), 2, new int[] { 2, 1, 4, 5, 3, 8, 9, 6, 7, 10, 11, 12 } };
    yield return new object[] { Full(), 1, new int[] { 1, 2, 3, 4 } };
    yield return new object[] { Full(), 2, new int[] { 2, 1, 3, 4 } };
    yield return new object[] { Line(), 1, new int[] { 1, 2, 3, 4, 5 } };
    yield return new object[] { Line(), 3, new int[] { 3, 2, 4, 1, 5 } };
    yield return new object[] { Star(), 1, new int[] { 1, 2, 3, 4, 5 } };
    yield return new object[] { Star(), 4, new int[] { 4, 1, 2, 3, 5 } };
    yield return new object[] { OneWayDirected(), 1, new int[] { 1, 2, 3, 4 } };
    yield return new object[] { OneWayDirected(), 2, new int[] { 2, 4 } };
    yield return new object[] { Directed(), 1, new int[] { 1, 2, 3, 4 } };
    yield return new object[] { Directed(), 2, new int[] { 2, 4, 1, 3 } };
  }

  [Theory]
  [MemberData(nameof(GetGraphsForBfs))]
  public void BreadthFirstSearch_ShouldReturnCorrectOrder(Graph graph, int startVertex,
                                                          int[] expected) {
    // Act
    var result = graph.BreadthFirstSearch(startVertex);

    // Assert
    Assert.Equal(expected, result);
  }
#endregion

#region GetShortestPathDijkstraAlg
  public static IEnumerable<object[]> GetShortestPathDijkstraAlgGraphs() {
    yield return new object[] { Three(), 1, 12, new int[] { 1, 2, 4, 9, 12 } };
    yield return new object[] { Three(), 1, 5, new int[] { 1, 2, 5 } };
    yield return new object[] { Full(), 1, 4, new int[] { 1, 4 } };
    yield return new object[] { Line(), 1, 5, new int[] { 1, 2, 3, 4, 5 } };
    yield return new object[] { Star(), 1, 5, new int[] { 1, 5 } };
    yield return new object[] { Star(), 4, 5, new int[] { 4, 1, 5 } };
    yield return new object[] { OneWayDirected(), 1, 4, new int[] { 1, 2, 4 } };
    yield return new object[] { OneWayDirected(), 2, 4, new int[] { 2, 4 } };
    yield return new object[] { Directed(), 1, 4, new int[] { 1, 2, 4 } };
    yield return new object[] { Directed(), 2, 4, new int[] { 2, 4 } };
    yield return new object[] { Directed(), 2, 3, new int[] { 2, 4, 1, 3 } };

    yield return new object[] { Directed(), 3, 3, new int[] { 3 } };
    yield return new object[] { Full(), 2, 2, new int[] { 2 } };
    yield return new object[] { SingleVertex(), 1, 1, new int[] { 1 } };

    yield return new object[] { TwoDisconnectedVertices(), 1, 2, new int[] {} };
    yield return new object[] { TwoConnectedVertices(), 1, 2, new int[] { 1, 2 } };
    yield return new object[] { ManyVerticesOneDisconnected(), 1, 5, new int[] {} };
    yield return new object[] { ManyVerticesOneDisconnected(), 1, 2, new int[] { 1, 2 } };
    yield return new object[] { ManyVerticesOneDisconnected(), 1, 4, new int[] { 1, 4 } };
    yield return new object[] { ManyVerticesAllConnected(), 1, 6, new int[] { 1, 6 } };
  }

  [Theory]
  [MemberData(nameof(GetShortestPathDijkstraAlgGraphs))]
  public void GetShortestPathDijkstraAlg_ShouldReturnCorrectPath(Graph graph, int start, int finish,
                                                                 int[] expected) {
    // Act
    var result = graph.GetShortestPathDijkstraAlg(start, finish);

    // Assert
    Assert.Equal(expected, result);
  }

  public static IEnumerable<object[]> GetInvalidVertexGraphs() {
    yield return new object[] { SingleVertex(), 0, 1 };
    yield return new object[] { SingleVertex(), 1, 2 };
    yield return new object[] { TwoConnectedVertices(), -1, 2 };
    yield return new object[] { TwoConnectedVertices(), 1, 3 };
    yield return new object[] { ManyVerticesAllConnected(), 1, 7 };
    yield return new object[] { ManyVerticesAllConnected(), 7, 1 };
  }

  [Theory]
  [MemberData(nameof(GetInvalidVertexGraphs))]
  public void GetShortestPathDijkstraAlg_InvalidVertices_ShouldThrowException(Graph graph,
                                                                              int start,
                                                                              int finish) {
    // Act & Assert
    Assert.Throws<IndexOutOfRangeException>(() => graph.GetShortestPathDijkstraAlg(start, finish));
  }

#endregion

#region GetShortestPathBetweenVertices
  public static IEnumerable<object[]> GetShortestPathBetweenVerticesGraphs() {
    yield return new object[] { Three(), 1, 12, 4 };
    yield return new object[] { Three(), 1, 5, 2 };
    yield return new object[] { Full(), 1, 4, 1 };
    yield return new object[] { Line(), 1, 5, 4 };
    yield return new object[] { Star(), 1, 5, 1 };
    yield return new object[] { Star(), 4, 5, 2 };
    yield return new object[] { OneWayDirected(), 1, 4, 11 };
    yield return new object[] { OneWayDirected(), 2, 4, 1 };
    yield return new object[] { Directed(), 1, 4, 11 };
    yield return new object[] { Directed(), 2, 4, 1 };
    yield return new object[] { Directed(), 2, 3, 4 };
    yield return new object[] { SpecialFromMaterials(), 1, 9, 4 };
    yield return new object[] { SpecialFromMaterials(), 1, 7, 100 };
    yield return new object[] { SpecialFromMaterials(), 1, 4, 20 };

    yield return new object[] { Directed(), 3, 3, 0 };
    yield return new object[] { Full(), 2, 2, 0 };
    yield return new object[] { SingleVertex(), 1, 1, 0 };

    yield return new object[] { TwoConnectedVertices(), 1, 2, 1 };
    yield return new object[] { ManyVerticesOneDisconnected(), 1, 2, 1 };
    yield return new object[] { ManyVerticesOneDisconnected(), 1, 4, 1 };
    yield return new object[] { ManyVerticesAllConnected(), 1, 6, 1 };
    yield return new object[] { SingleVertexLoop(), 1, 1, 0 };
    yield return new object[] { TwoVerticesLoop(), 1, 1, 0 };
    yield return new object[] { TwoVerticesLoop(), 2, 2, 0 };
  }

  [Theory]
  [MemberData(nameof(GetShortestPathBetweenVerticesGraphs))]
  public void GetShortestPathBetweenVertices_ShouldReturnCorrectPath(Graph graph, int start,
                                                                     int finish, int expected) {
    // Act
    var result = graph.GetShortestPathBetweenVertices(start, finish);
    Debug.WriteLine($"{graph}:\nfrom{start} to {finish}: expected = {expected}, result = {result}");
    // Assert
    Assert.Equal(expected, result);
  }

  public static IEnumerable<object[]> GetShortestPathBetweenVerticesNoPathGraphs() {
    yield return new object[] { TwoDisconnectedVertices(), 1, 2 };
    yield return new object[] { ManyVerticesOneDisconnected(), 1, 5 };
  }

  [Theory]
  [MemberData(nameof(GetShortestPathBetweenVerticesNoPathGraphs))]
  public void GetShortestPathBetweenVertices_WhenPathNotExists_ThrowExeption(Graph graph, int start,
                                                                             int finish) {
    // Act && Assert
    Assert.Throws<Exception>(() => graph.GetShortestPathBetweenVertices(start, finish));
  }
#endregion

#region GetShortestPathsBetweenAllVertices
#region GetShortestPathsBetweenAllVerticesData

  public static IEnumerable<object[]> GetGraphsForFloydWarshallSingleVertexLoop() {
    yield return new object[] { SingleVertexLoop(), new int[1, 1] { { 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallSingleVertex() {
    yield return new object[] { SingleVertex(), new int[1, 1] { { 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallTwoDisconnectedVertices() {
    yield return new object[] { TwoDisconnectedVertices(), new int[2, 2] { { 0, -1 }, { -1, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallTwoConnectedVertices() {
    yield return new object[] { TwoConnectedVertices(), new int[2, 2] { { 0, 1 }, { 1, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallTwoVerticesLoop() {
    yield return new object[] { TwoVerticesLoop(), new int[2, 2] { { 0, 2 }, { 2, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallManyVerticesOneDisconnected() {
    yield return new object[] { ManyVerticesOneDisconnected(),
                                new int[5, 5] { { 0, 1, 1, 1, -1 },
                                                { 1, 0, 1, 1, -1 },
                                                { 1, 1, 0, 1, -1 },
                                                { 1, 1, 1, 0, -1 },
                                                { -1, -1, -1, -1, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallManyVerticesAllConnected() {
    yield return new object[] { ManyVerticesAllConnected(),
                                new int[6, 6] { { 0, 1, 1, 1, 1, 1 },
                                                { 1, 0, 1, 1, 1, 1 },
                                                { 1, 1, 0, 1, 1, 1 },
                                                { 1, 1, 1, 0, 1, 1 },
                                                { 1, 1, 1, 1, 0, 1 },
                                                { 1, 1, 1, 1, 1, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallThree() {
    yield return new object[] { Three(),
                                new int[12, 12] { { 0, 1, 1, 2, 2, 2, 2, 3, 3, 4, 4, 4 },
                                                  { 1, 0, 2, 1, 1, 3, 3, 2, 2, 3, 3, 3 },
                                                  { 1, 2, 0, 3, 3, 1, 1, 4, 4, 5, 5, 5 },
                                                  { 2, 1, 3, 0, 2, 4, 4, 1, 1, 2, 2, 2 },
                                                  { 2, 1, 3, 2, 0, 4, 4, 3, 3, 4, 4, 4 },
                                                  { 2, 3, 1, 4, 4, 0, 2, 5, 5, 6, 6, 6 },
                                                  { 2, 3, 1, 4, 4, 2, 0, 5, 5, 6, 6, 6 },
                                                  { 3, 2, 4, 1, 3, 5, 5, 0, 2, 3, 3, 3 },
                                                  { 3, 2, 4, 1, 3, 5, 5, 2, 0, 1, 1, 1 },
                                                  { 4, 3, 5, 2, 4, 6, 6, 3, 1, 0, 2, 2 },
                                                  { 4, 3, 5, 2, 4, 6, 6, 3, 1, 2, 0, 2 },
                                                  { 4, 3, 5, 2, 4, 6, 6, 3, 1, 2, 2, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallFull() {
    yield return new object[] {
      Full(), new int[4, 4] { { 0, 1, 1, 1 }, { 1, 0, 1, 1 }, { 1, 1, 0, 1 }, { 1, 1, 1, 0 } }
    };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallLine() {
    yield return new object[] { Line(), new int[5, 5] { { 0, 1, 2, 3, 4 },
                                                        { 1, 0, 1, 2, 3 },
                                                        { 2, 1, 0, 1, 2 },
                                                        { 3, 2, 1, 0, 1 },
                                                        { 4, 3, 2, 1, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallStar() {
    yield return new object[] { Star(), new int[5, 5] { { 0, 1, 1, 1, 1 },
                                                        { 1, 0, 2, 2, 2 },
                                                        { 1, 2, 0, 2, 2 },
                                                        { 1, 2, 2, 0, 2 },
                                                        { 1, 2, 2, 2, 0 } } };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallOneWayDirected() {
    yield return new object[] {
      OneWayDirected(),
      new int[4, 4] { { 0, 10, 2, 11 }, { -1, 0, -1, 1 }, { -1, -1, 0, 20 }, { -1, -1, -1, 0 } }
    };
  }

  public static IEnumerable<object[]> GetGraphsForFloydWarshallDirected() {
    yield return new object[] {
      Directed(),
      new int[4, 4] { { 0, 10, 2, 11 }, { 2, 0, 4, 1 }, { 21, 31, 0, 20 }, { 1, 11, 3, 0 } }
    };
  }
#endregion

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallSingleVertex))]
  public void GetShortestPathsBetweenAllVertices_SingleVertex_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallSingleVertexLoop))]
  public void GetShortestPathsBetweenAllVertices_SingleVertexLoop_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallTwoDisconnectedVertices))]
  public void GetShortestPathsBetweenAllVertices_TwoDisconnectedVertices_ShouldReturnCorrectPaths1(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallTwoConnectedVertices))]
  public void GetShortestPathsBetweenAllVertices_TwoConnectedVertices_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallTwoVerticesLoop))]
  public void GetShortestPathsBetweenAllVertices_TwoVerticesLoop_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallManyVerticesOneDisconnected))]
  public void
  GetShortestPathsBetweenAllVertices_ManyVerticesOneDisconnected_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallManyVerticesAllConnected))]
  public void GetShortestPathsBetweenAllVertices_ManyVerticesAllConnected_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallThree))]
  public void GetShortestPathsBetweenAllVertices_Three_ShouldReturnCorrectPaths(Graph graph,
                                                                                int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallFull))]
  public void GetShortestPathsBetweenAllVertices_Full_ShouldReturnCorrectPaths(Graph graph,
                                                                               int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallLine))]
  public void GetShortestPathsBetweenAllVertices_Line_ShouldReturnCorrectPaths(Graph graph,
                                                                               int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallStar))]
  public void GetShortestPathsBetweenAllVertices_Star_ShouldReturnCorrectPaths(Graph graph,
                                                                               int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallOneWayDirected))]
  public void GetShortestPathsBetweenAllVertices_OneWayDirected_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  [Theory]
  [MemberData(nameof(GetGraphsForFloydWarshallDirected))]
  public void GetShortestPathsBetweenAllVertices_Directed_ShouldReturnCorrectPaths(
      Graph graph, int[,] expected) {
    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
  }

  private void GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(Graph graph,
                                                                               int[,] expected) {
    // Act
    var result = graph.GetShortestPathsBetweenAllVertices();

    // Assert
    for (int i = 0; i < result.GetLength(0); i++) {
      for (int j = 0; j < result.GetLength(1); j++) {
        Debug.Write(result[i, j]);
        Debug.Write(" ");
      }
      Debug.Write("\n");
    }
    Assert.True(expected.SequenceEqual(result));
  }
#endregion

#region GetLeastSpanningTree
#region Data

  private static int[,] ForPrim() => new int[,] {
    { 0, 9, 1, 8, 1 }, { 9, 0, 1, 0, 0 }, { 1, 1, 0, 2, 1 }, { 8, 0, 2, 0, 9 }, { 1, 0, 1, 9, 0 }
  };

  private static int[,] ForPrimResult() => new int[,] {
    { 0, 0, 1, 0, 1 }, { 0, 0, 1, 0, 0 }, { 1, 1, 0, 2, 0 }, { 0, 0, 2, 0, 0 }, { 1, 0, 0, 0, 0 }
  };

  private static int[,] ForPrimWithLoop() => new int[,] {
    { 1, 9, 1, 8, 1 }, { 9, 0, 1, 0, 0 }, { 1, 1, 0, 2, 1 }, { 8, 0, 2, 7, 9 }, { 1, 0, 1, 9, 0 }
  };
#endregion

  [Fact]
  public void GetLeastSpanningTree_CorrectGraph_ShouldReturnCorrectTree() {
    var graph = new Graph(ForPrim());

    // Act
    var result = graph.GetLeastSpanningTree();

    // Assert
    Assert.Equal(ForPrimResult(), result);
  }

  [Fact]
  public void GetLeastSpanningTree_CorrectGraphWithLoop_ShouldReturnCorrectTree() {
    var graph = new Graph(ForPrimWithLoop());

    // Act
    var result = graph.GetLeastSpanningTree();

    // Assert
    Assert.Equal(ForPrimResult(), result);
  }

  [Fact]
  public void GetLeastSpanningTree_DirectedGraph_ShouldThrowArgumentException() {
    var graph = Directed();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.GetLeastSpanningTree());
  }

  [Fact]
  public void GetLeastSpanningTree_DisconnectedGraph_ShouldThrowArgumentException() {
    var graph = Disconnected();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.GetLeastSpanningTree());
  }
#endregion

#region SolveTravelingSalesmanProblem

  [Fact]
  public void SolveTravelingSalesmanProblem_OddFull_ShouldReturnCorrectResult() {
    // Arrange
    var graph = OddFull();
    var expected = new TsmResult(new List<int> { 1, 6, 4, 2, 5, 3, 1 }, 6.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Debug.WriteLine($"{string.Join(" ", expected.Vertices)}\n{string.Join(" ", result.Vertices)}");
    Assert.Equal(expected.Distance, result.Distance);
    Assert.True(expected.Vertices.SequenceEqual(result.Vertices));
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_OddFullWithLoop_ShouldReturnCorrectResult() {
    // Arrange
    var graph = OddFullWithLoop();
    var expected = new TsmResult(new List<int> { 1, 6, 4, 2, 5, 3, 1 }, 6.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Debug.WriteLine($"{string.Join(" ", expected.Vertices)}\n{string.Join(" ", result.Vertices)}");
    Assert.Equal(expected.Distance, result.Distance);
    Assert.True(expected.Vertices.SequenceEqual(result.Vertices));
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_SpecialDirected1_ShouldReturnCorrectResult() {
    // Arrange
    var graph = SpecialDirected1();
    var expected = new TsmResult(new List<int> { 1, 2, 3, 4, 5, 6, 1 }, 6.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Debug.WriteLine($"{string.Join(" ", expected.Vertices)}\n{string.Join(" ", result.Vertices)}");
    Assert.Equal(expected.Distance, result.Distance);
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_SpecialDirected2_ShouldReturnCorrectResult() {
    // Arrange
    var graph = SpecialDirected2();
    var expected = new TsmResult(new List<int> { 1, 4, 6, 2, 3, 5, 1 }, 6.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Debug.WriteLine($"{string.Join(" ", expected.Vertices)}\n{string.Join(" ", result.Vertices)}");
    Assert.Equal(expected.Distance, result.Distance);
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_SpecialFromMaterials_ShouldReturnCorrectResult() {
    // Arrange
    var graph = SpecialFromMaterials();
    var expected = new TsmResult(new List<int> { 1, 8, 5, 4, 10, 6, 3, 7, 2, 11, 9, 1 }, 253.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Debug.WriteLine($"{string.Join(" ", expected.Vertices)}\n{string.Join(" ", result.Vertices)}");
    Assert.Equal(expected.Distance, result.Distance);
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_TwoConnectedVertices_ShouldReturnCorrectResult() {
    // Arrange
    var graph = TwoConnectedVertices();
    var expected = new TsmResult(new List<int> { 1, 2, 1 }, 2.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
    Assert.Equal(expected.Distance, result.Distance);
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_ManyVerticesAllConnected_ShouldReturnCorrectResult() {
    // Arrange
    var graph = ManyVerticesAllConnected();
    var expected = new TsmResult(new List<int> { 1, 2, 3, 4, 5, 6, 1 }, 6.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
    Assert.Equal(expected.Distance, result.Distance);
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_Full_ShouldReturnCorrectResult() {
    // Arrange
    var graph = Full();
    var expected = new TsmResult(new List<int> { 1, 2, 3, 4, 1 }, 4.0);

    // Act
    var result = graph.SolveTravelingSalesmanProblem();

    // Assert
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
    Assert.Equal(expected.Distance, result.Distance);
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_Odd_ShouldThrowException() {
    // Arrange
    var graph = Odd();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_OddWithLoop_ShouldThrowException() {
    // Arrange
    var graph = OddWithLoop();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_SingleVertex_ShouldThrowException() {
    // Arrange
    var graph = SingleVertex();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_TwoDisconnectedVertices_ShouldThrowException() {
    // Arrange
    var graph = TwoDisconnectedVertices();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_ManyVerticesOneDisconnected_ShouldThrowException() {
    // Arrange
    var graph = ManyVerticesOneDisconnected();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_Three_ShouldThrowException() {
    // Arrange
    var graph = Three();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_Line_ShouldThrowException() {
    // Arrange
    var graph = Star();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_Star_ShouldThrowException() {
    // Arrange
    var graph = Line();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_OneWayDirected_ShouldThrowException() {
    // Arrange
    var graph = OneWayDirected();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }

  [Fact]
  public void SolveTravelingSalesmanProblem_Directed_ShouldThrowException() {
    // Arrange
    var graph = Directed();

    // Act & Assert
    Assert.Throws<ArgumentException>(() => graph.SolveTravelingSalesmanProblem());
  }
#endregion

#region AntColonyPathFinder

  [Fact]
  public void GetPath_SpecialDirected1FromVertex2_ShouldReturnCorrectResult() {
    // Arrange
    AntColonyPathFinder acpf = new(randomSeed: 21);
    var graph = SpecialDirected1();
    var expected = new TsmResult(new List<int> { 1, 2, 3, 4, 5, 6, 1 }, 6.0);

    // Act
    var result = acpf.GetPath(graph, 1);

    // Assert
    Debug.WriteLine($"{string.Join(" ", expected.Vertices)}\n{string.Join(" ", result.Vertices)}");
    Assert.Equal(expected.Distance, result.Distance);
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
  }

  [Fact]
  public void GetPath_SpecialDirected1FromVertexNull_ShouldReturnCorrectResult() {
    // Arrange
    AntColonyPathFinder acpf = new();
    var graph = SpecialDirected1();
    var expected = new TsmResult(new List<int> { 1, 2, 3, 4, 5, 6, 1 }, 6.0);

    // Act
    var result = acpf.GetPath(graph);

    // Assert
    Debug.WriteLine($"{string.Join(" ", expected.Vertices)}\n{string.Join(" ", result.Vertices)}");
    Assert.Equal(expected.Distance, result.Distance);
    Assert.Equal(expected.Vertices.Count, result.Vertices.Count);
  }

  [Fact]
  public void Constructor_WrongPameters_ShouldThrowArgumentException() {
    // Act & Assert
    Assert.Throws<ArgumentException>(() => new AntColonyPathFinder(stepsCount: -1));
    Assert.Throws<ArgumentException>(() => new AntColonyPathFinder(amountOfPheromone: -5));
    Assert.Throws<ArgumentException>(() => new AntColonyPathFinder(initAmountOfPheromone: -5));
    Assert.Throws<ArgumentException>(() => new AntColonyPathFinder(influenceDistanceRate: 0.8));
    Assert.Throws<ArgumentException>(() => new AntColonyPathFinder(influenceDistanceRate: 5.5));
    Assert.Throws<ArgumentException>(() => new AntColonyPathFinder(influencePheromoneRate: -0.5));
    Assert.Throws<ArgumentException>(() => new AntColonyPathFinder(influencePheromoneRate: 5.5));
    Assert.Throws<ArgumentException>(
        () => new AntColonyPathFinder(pheromoneEvaporationCoefficient: -0.5));
    Assert.Throws<ArgumentException>(
        () => new AntColonyPathFinder(pheromoneEvaporationCoefficient: 1.5));
  }

#endregion

#region GetShortestPathsBetweenAllVerticesOld
//#region GetShortestPathsBetweenAllVerticesData

// public static IEnumerable<object[]> GetGraphsForFloydWarshallSingleVertexLoop()
//{
//     yield return new object[] { SingleVertexLoop(), new List<int>[2, 2]
//         {
//             { [], []},
//             { [], [1]}
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallSingleVertex()
//{
//     yield return new object[] { SingleVertex(), new List<int>[2, 2]
//         {
//             { [], []},
//             { [], [1]}
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallTwoDisconnectedVertices()
//{
//     yield return new object[] { TwoDisconnectedVertices(), new List<int>[3, 3]
//         {
//             { [], [], []},
//             { [], [1], []},
//             { [], [], [2]}
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallTwoConnectedVertices()
//{
//     yield return new object[] { TwoConnectedVertices(), new List<int>[3, 3]
//         {
//             { [], [], [] },
//             { [], [1], [1,2] },
//             { [], [2,1], [2] }
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallTwoVerticesLoop()
//{
//     yield return new object[] { TwoVerticesLoop(), new List<int>[3, 3]
//         {
//             { [], [], [] },
//             { [], [1], [1,2] },
//             { [], [2,1], [2] }
//         }
//     };
// }

// public static IEnumerable<object[]> GetGraphsForFloydWarshallManyVerticesOneDisconnected()
//{
//     yield return new object[] { ManyVerticesOneDisconnected(), new List<int>[6, 6]
//         {
//             { [], [], [], [], [], []},
//             { [], [1], [ 1,2 ], [1, 3 ], [1, 4 ] , [ ] },
//             { [], [2, 1 ], [2], [ 2, 3 ], [2, 4] , [ ] },
//             { [], [ 3, 1 ], [3, 2 ], [3], [3, 4 ], [ ] },
//             { [], [4, 1 ], [ 4, 2 ], [ 4, 3 ], [4], [ ]},
//             { [], [ ], [ ], [ ], [], [5]}
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallManyVerticesAllConnected()
//{
//     yield return new object[] { ManyVerticesAllConnected(), new List<int>[7, 7]
//         {
//             { [], [], [], [], [], [], [] },
//             { [], [1], [ 1,2 ], [1, 3 ], [1, 4 ] , [1, 5 ], [1, 6 ] },
//             { [], [2, 1 ], [2], [ 2, 3 ], [2, 4] , [2, 5 ], [2, 6 ] },
//             { [], [ 3, 1 ], [3, 2 ], [3], [3, 4 ], [3, 5 ], [3, 6 ] },
//             { [], [4, 1 ], [ 4, 2 ], [ 4, 3 ], [4], [4, 5 ], [4, 6 ] },
//             { [], [ 5,1 ], [5, 2 ], [5, 3 ], [5, 4 ], [5], [5, 6 ] },
//             { [], [6, 1 ], [ 6, 2 ], [ 6, 3 ], [ 6, 4 ], [ 6, 5 ], [6] }
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallThree()
//{
//     yield return new object[] { Three(), new List<int>[13, 13]
//         {
//             { [], [], [ ], [ ], [], [], [], [], [], [], [], [], [] },
//             { [], [1], [ 1, 2 ], [ 1, 3 ], [1, 2, 4], [1, 2, 5], [1, 3, 6], [1, 3, 7], [1, 2, 4
//             ,8], [1,2,4,9], [1,2,4,9,10], [1,2,4,9,11], [1, 2, 4, 9,12] }, { [], [ 2, 1 ], [2],
//             [2, 1, 3], [ 2, 4 ], [ 2, 5 ], [2,1, 3, 6], [2, 1, 3, 7], [2,4 ,8], [2,4,9],
//             [2,4,9,10], [2,4,9,11], [2, 4, 9, 12] }, { [], [ 3, 1 ], [3,1, 2], [3], [3,1, 2, 4],
//             [3,1, 2, 5], [ 3, 6 ], [ 3, 7 ], [3, 1, 2, 4 ,8], [3,1,2,4,9], [3,1,2,4,9,10],
//             [3,1,2,4,9,11], [3,1, 2, 4, 9,12] }, { [], [4, 2, 1], [ 4, 2 ], [4, 2, 1, 3], [4],
//             [4,2, 5], [4,2,1, 3, 6], [4,2, 1, 3, 7], [4 ,8], [4,9], [4,9,10], [4,9,11], [4, 9,
//             12] }, { [], [5, 2, 1], [ 5, 2 ], [5, 2, 1, 3], [5,2,4], [5], [5,2,1, 3, 6], [5,2, 1,
//             3, 7], [5,2,4 ,8], [5,2,4,9], [5,2,4,9,10], [5,2,4,9,11], [5,2,4, 9, 12] }, { [], [
//             6,3, 1 ], [6,3,1, 2], [6,3], [6,3,1, 2, 4], [6,3,1, 2, 5], [ 6 ], [ 6,3, 7 ], [6,3,
//             1, 2, 4 ,8], [6,3,1,2,4,9], [6,3,1,2,4,9,10], [6,3,1,2,4,9,11], [6,3,1, 2, 4, 9,12]
//             }, { [], [ 7,3, 1 ], [7,3,1, 2], [7,3], [7,3,1, 2, 4], [7,3,1, 2, 5], [ 7,3,6 ], [ 7
//             ], [7,3, 1, 2, 4 ,8], [7,3,1,2,4,9], [7,3,1,2,4,9,10], [7,3,1,2,4,9,11], [7,3,1, 2,
//             4, 9,12] }, { [], [8,4, 2, 1], [ 8,4, 2 ], [8,4, 2, 1, 3], [8,4], [8,4,2, 5],
//             [8,4,2,1, 3, 6], [8,4,2, 1, 3, 7], [8], [8,4,9], [8, 4,9,10], [8, 4,9,11], [8,4, 9,
//             12] }, { [], [9,4, 2, 1], [ 9,4, 2 ], [9,4, 2, 1, 3], [9,4], [9,4,2, 5], [9,4,2,1, 3,
//             6], [9,4,2, 1, 3, 7], [9,4,8], [9], [9,10], [9,11], [9, 12] }, { [], [10,9,4, 2, 1],
//             [ 10,9,4, 2 ], [10,9,4, 2, 1, 3], [10,9,4], [10,9,4,2, 5], [10,9,4,2,1, 3, 6],
//             [10,9,4,2, 1, 3, 7], [10,9,4,8], [10,9], [10], [10,9,11], [10,9, 12] }, { [],
//             [11,9,4, 2, 1], [ 11,9,4, 2 ], [11,9,4, 2, 1, 3], [11,9,4], [11,9,4,2, 5],
//             [11,9,4,2,1, 3, 6], [11,9,4,2, 1, 3, 7], [11,9,4,8], [11,9], [11,9,10], [11], [11,9,
//             12] }, { [], [12,9,4, 2, 1], [ 12,9,4, 2 ], [12,9,4, 2, 1, 3], [12,9,4], [12,9,4,2,
//             5], [12,9,4,2,1, 3, 6], [12,9,4,2, 1, 3, 7], [12,9,4,8], [12,9], [12,9,10],
//             [12,9,11], [12] }
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallFull()
//{
//     yield return new object[] { Full(), new List<int>[5, 5]
//         {
//             { [], [], [], [], [] },
//             { [], [1], [ 1,2 ], [1, 3 ], [1, 4 ] },
//             { [], [2, 1 ], [2], [ 2, 3 ], [2, 4] },
//             { [], [ 3, 1 ], [3, 2 ], [3], [3, 4 ] },
//             { [], [4, 1 ], [ 4, 2 ], [ 4, 3 ], [4] }
//         }
//     };
// }
// public static IEnumerable<object[]> GetGraphsForFloydWarshallLine()
//{

//    yield return new object[] { Line(), new List<int>[6, 6]
//        {
//            { [], [], [], [], [], [] },
//            { [], [ 1] , [ 1, 2 ], [ 1, 2, 3 ], [ 1, 2, 3, 4 ], [ 1, 2, 3, 4, 5 ] },
//            { [], [ 2, 1 ], [ 2 ], [ 2, 3 ], [ 2, 3, 4 ], [ 2, 3, 4, 5 ]          },
//            { [], [ 3, 2, 1 ], [ 3, 2 ], [ 3 ], [ 3, 4 ], [ 3, 4, 5 ]             },
//            { [], [ 4, 3, 2, 1], [ 4, 3, 2 ], [ 4, 3 ], [4], [ 4, 5]          },
//            { [], [ 5, 4, 3, 2, 1], [ 5, 4, 3, 2 ], [ 5, 4, 3 ], [ 5, 4 ], [5] }
//        }
//    };
//}
// public static IEnumerable<object[]> GetGraphsForFloydWarshallStar()
//{

//    yield return new object[] { Star(), new List<int>[6, 6]
//        {
//            { [], [], [], [], [], [] },
//            { [], [1], [ 1, 2 ], [ 1, 3 ], [ 1, 4 ], [ 1, 5 ] },
//            { [], [2,1], [2], [2,1,3], [2,1,4], [2,1,5]},
//            { [], [3,1], [3,1,2], [3], [3,1,4], [3,1,5]},
//            { [], [4,1], [4,1,2], [4,1,3], [4], [4,1,5]},
//            { [], [5,1], [5,1,2], [5,1,3], [5,1,4], [5]}
//        }
//    };
//}
// public static IEnumerable<object[]> GetGraphsForFloydWarshallOneWayDirected()
//{

//    yield return new object[] { OneWayDirected(), new List<int>[5, 5]
//        {
//            { [], [], [], [], [] },
//            { [], [1], [ 1, 2 ], [ 1, 3 ], [ 1, 2, 4 ] },
//            { [], [], [2], [], [ 2, 4 ] },
//            { [], [], [], [3], [ 3, 4 ]},
//            { [], [], [], [], [4] }
//        }
//    };
//}
// public static IEnumerable<object[]> GetGraphsForFloydWarshallDirected()
//{
//    yield return new object[] { Directed(), new List<int>[5, 5]
//        {
//            { [], [], [], [], [] },
//            { [], [1], [ 1, 2 ], [ 1, 3 ], [ 1, 2, 4 ] },
//            { [], [ 2, 4, 1 ], [2], [2, 4, 1, 3], [ 2, 4 ] },
//            { [], [ 3, 4, 1 ], [3, 4, 1, 2], [3], [ 3, 4 ] },
//            { [], [ 4, 1 ], [ 4, 1, 2 ], [ 4, 1, 3 ], [4] }
//        }
//    };
//}
//#endregion

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallSingleVertex))]
// public void GetShortestPathsBetweenAllVertices_SingleVertex_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallSingleVertexLoop))]
// public void GetShortestPathsBetweenAllVertices_SingleVertexLoop_ShouldReturnCorrectPaths(Graph
// graph, List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallTwoDisconnectedVertices))]
// public void
// GetShortestPathsBetweenAllVertices_TwoDisconnectedVertices_ShouldReturnCorrectPaths1(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallTwoConnectedVertices))]
// public void
// GetShortestPathsBetweenAllVertices_TwoConnectedVertices_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallTwoVerticesLoop))]
// public void GetShortestPathsBetweenAllVertices_TwoVerticesLoop_ShouldReturnCorrectPaths(Graph
// graph, List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallManyVerticesOneDisconnected))]
// public void
// GetShortestPathsBetweenAllVertices_ManyVerticesOneDisconnected_ShouldReturnCorrectPaths(Graph
// graph, List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallManyVerticesAllConnected))]
// public void
// GetShortestPathsBetweenAllVertices_ManyVerticesAllConnected_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallThree))]
// public void GetShortestPathsBetweenAllVertices_Three_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallFull))]
// public void GetShortestPathsBetweenAllVertices_Full_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallLine))]
// public void GetShortestPathsBetweenAllVertices_Line_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallStar))]
// public void GetShortestPathsBetweenAllVertices_Star_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallOneWayDirected))]
// public void GetShortestPathsBetweenAllVertices_OneWayDirected_ShouldReturnCorrectPaths(Graph
// graph, List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

//[Theory]
//[MemberData(nameof(GetGraphsForFloydWarshallDirected))]
// public void GetShortestPathsBetweenAllVertices_Directed_ShouldReturnCorrectPaths(Graph graph,
// List<int>[,] expected)
//{
//    GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(graph, expected);
//}

// private void GetShortestPathsBetweenAllVertices_ShouldReturnCorrectDistances(Graph graph,
// List<int>[,] expected)
//{
//     // Act
//     var result = graph.GetShortestPathsBetweenAllVertices();

//    // Assert
//    for (int i = 0; i < expected.GetLength(0); i++)
//    {
//        for (int j = 0; j < expected.GetLength(1); j++)
//        {
//            Debug.Write($"[{string.Join(" ", result[i, j])}] ");
//            Assert.True(expected[i, j].SequenceEqual(result[i, j]));
//        }
//        Debug.WriteLine("");
//    }
//}
#endregion
}
