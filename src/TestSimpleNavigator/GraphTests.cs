using s21_graph;

namespace TestSimpleNavigator;

public class GraphTests {
  [Fact]
  public void Constructor_ShouldThrowArgumentException_WhenAdjacencyMatrixIsNotSquare() {
    // Arrange
    int[,] invalidMatrix = { { 0, 1, 0 }, { 1, 0, 1 } };

    // Act & Assert
    Assert.Throws<ArgumentException>(() => new Graph(invalidMatrix));
  }

  [Fact]
  public void Constructor_ShouldcreateEmptyGraph_WhenAdjacencyMatrixIsNull() {
    // Arrange
    int[,] adjacencyMatrix = null;

    // Act
    var graph = new Graph(adjacencyMatrix);

    // Assert
    Assert.True(graph.VertexCount == 0);
  }

  [Fact]
  public void LoadGraphFromFile_ShouldLoadCorrectly_WhenFileIsValid() {
    // Arrange
    string tempFile = Path.GetTempFileName();
    File.WriteAllLines(tempFile, new[] {
      "11", "0   29  20  21  16  31  100 12  4   31  18",
      "29  0   15  29  28  40  72  21  29  41  12", "20  15  0   15  14  25  81  9   23  27  13",
      "21  29  15  0   4   12  92  12  25  13  25", "16  28  14  4   0   16  94  9   20  16  22",
      "31  40  25  12  16  0   95  24  36  3   37", "100 72  81  92  94  95  0   90  101 99  84",
      "12  21  9   12  9   24  90  0   15  25  13", "4   29  23  25  20  36  101 15  0   35  18",
      "31  41  27  13  16  3   99  25  35  0   38", "18  12  13  25  22  37  84  13  18  38  0"
    });
    var expectedGraph = new Graph(new int[,] { { 0, 29, 20, 21, 16, 31, 100, 12, 4, 31, 18 },
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

    var graph = new Graph();

    // Act
    graph.LoadGraphFromFile(tempFile);

    // Assert
    Assert.Equal(11, graph.VertexCount);
    Assert.Equal(expectedGraph, graph);
  }

  [Fact]
  public void LoadGraphFromFile_ShouldThrowFormatException_WhenEmptyFile() {
    // Arrange
    string tempFile = Path.GetTempFileName();
    File.WriteAllLines(tempFile, Array.Empty<string>());

    var graph = new Graph();

    // Act & Assert
    Assert.Throws<FormatException>(() => graph.LoadGraphFromFile(tempFile));
  }

  public static IEnumerable<object[]> GetInvalidGraphData() {
    yield return new object[] { new[] { "3", "0 1 0", "1 x 1", "0 1 0" } };   // Invalid character
    yield return new object[] { new[] { "-3", "0 1 0", "1 0 1", "0 1 0" } };  // Negative size
    yield return new object[] { new[] { "3", "0 -1 0", "1 0 1", "0 1 0" } };  // Negative element
    yield return new object[] { new[] { "d", "0 1 0", "1 0 1",
                                        "0 1 0" } };                    // Invalid character as size
    yield return new object[] { new[] { "0 1 0", "1 0 1", "0 1 0" } };  // No size
    yield return new object[] { new[] { "5", "0 1 0", "1 0 1", "0 1 0" } };    // Incorrect size
    yield return new object[] { new[] { "2", "0 1 0", "1 0 1", "0 1 0" } };    // Incorrect size
    yield return new object[] { new[] { "3", "0 1 0 5", "1 0 1", "0 1 0" } };  // Incorrect size
    yield return new object[] { new[] { "3", "0 1", "1 0 1", "0 1 0" } };      // Incorrect size
    yield return new object[] { new[] { "3", "1 0 1", "0 1 1", "1 0 1",
                                        "0 1 0" } };  // Incorrect size
    yield return new object[] { new[] { "3" } };      // Incorrect size
  }

  [Theory]
  [MemberData(nameof(GetInvalidGraphData))]
  public void LoadGraphFromFile_ShouldThrowFormatException_WhenFileIsInvalid(string[] lines) {
    // Arrange
    string tempFile = Path.GetTempFileName();
    File.WriteAllLines(tempFile, lines);

    var graph = new Graph();

    // Act & Assert
    Assert.Throws<FormatException>(() => graph.LoadGraphFromFile(tempFile));
  }

  [Fact]
  public void ExportGraphToDot_ShouldCreateValidDotFile_ForDirectedGraph() {
    // Arrange
    string tempFile = Path.GetTempFileName();
    var graph = CreateDirectedGraph3Vertex();

    // Act
    graph.ExportGraphToDot(tempFile);

    // Assert
    Assert.True(File.Exists(tempFile));  // Check if file was created

    string[] lines = File.ReadAllLines(tempFile);
    Assert.Equal(10, lines.Length);
    Assert.Contains("graph G {", lines);  // Check for the graph opening line
    Assert.Contains("}", lines);          // Check for the graph closing line

    // Check for each edge in the graph
    Assert.Contains("  1 --> 2;", lines);
    Assert.Contains("  2 --> 1;", lines);
    Assert.Contains("  2 --> 3;", lines);
    Assert.Contains("  3 --> 1;", lines);
    Assert.Contains("  3 --> 2;", lines);
  }

  [Fact]
  public void ExportGraphToDot_ShouldCreateValidDotFile_ForUndirectedGraph() {
    // Arrange
    string tempFile = Path.GetTempFileName();
    var graph = CreateUndirectedGraph3Vertex();

    // Act
    graph.ExportGraphToDot(tempFile);

    // Assert
    Assert.True(File.Exists(tempFile));  // Check if file was created

    string[] lines = File.ReadAllLines(tempFile);
    Assert.Equal(8, lines.Length);
    Assert.Contains("graph G {", lines);  // Check for the graph opening line
    Assert.Contains("}", lines);          // Check for the graph closing line

    // Check for each edge in the graph
    Assert.Contains("  1 -- 2;", lines);
    Assert.Contains("  1 -- 3;", lines);
    Assert.Contains("  2 -- 3;", lines);
  }

  [Fact]
  public void AccesByIndexSet_ShouldChangeValue() {
    // Arrange
    var graph = CreateDirectedGraph3Vertex();
    var expectedGraph = new Graph(new int[,] { { 21, 1, 0 }, { 5, 0, 2 }, { 1, 1, 0 } });
    int expectedVal = 21;

    // Act
    graph[1, 1] = expectedVal;

    // Assert
    Assert.Equal(expectedVal, graph[1, 1]);
    Assert.Equal(expectedGraph, graph);
  }

  [Fact]
  public void AccesByIndexSet_ValueOutOrRange_ShouldThrowArgumentOutOfRangeException() {
    // Arrange
    var graph = CreateDirectedGraph3Vertex();

    // Act & Assert
    Assert.Throws<ArgumentOutOfRangeException>(() => graph[1, 1] = -21);
    Assert.Throws<ArgumentOutOfRangeException>(() => graph[1, 1] = int.MaxValue);
  }

  [Fact]
  public void AccesByIndexSetGet_IndexOutOfRange_ShouldThrowIndexOutOfRangeException() {
    // Arrange
    var graph = CreateDirectedGraph3Vertex();

    // Act & Assert
    Assert.Throws<IndexOutOfRangeException>(() => graph[-1, 1] = 21);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, -1] = 21);
    Assert.Throws<IndexOutOfRangeException>(() => graph[0, 1] = 21);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, 0] = 21);
    Assert.Throws<IndexOutOfRangeException>(() => graph[4, 1] = 21);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, 4] = 21);
    Assert.Throws<IndexOutOfRangeException>(() => graph[10, 1] = 21);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, 10] = 21);

    Assert.Throws<IndexOutOfRangeException>(() => graph[-1, 1]);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, -1]);
    Assert.Throws<IndexOutOfRangeException>(() => graph[0, 1]);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, 0]);
    Assert.Throws<IndexOutOfRangeException>(() => graph[4, 1]);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, 4]);
    Assert.Throws<IndexOutOfRangeException>(() => graph[10, 1]);
    Assert.Throws<IndexOutOfRangeException>(() => graph[1, 10]);
  }

  [Fact]
  public void AccesByIndexSet_NoContent_ShouldThrowArgumentNullReferenceException() {
    // Arrange
    var graph1 = new Graph();

    // Act & Assert
    Assert.Throws<NullReferenceException>(() => graph1[1, 1] = 3);

    // Arrange
    var graph2 = new Graph(new int[,] {});

    // Act & Assert
    Assert.Throws<NullReferenceException>(() => graph2[1, 1] = 3);
  }

  [Fact]
  public void ToString_EmptyGraph_ShouldReturnNull() {
    // Arrange
    var graph = new Graph();

    // Act & Assert
    Assert.True(string.IsNullOrEmpty(graph.ToString()));
  }

  [Fact]
  public void Equals_OtherType_ShouldReturnFalse() {
    // Arrange
    var graph = new Graph();

    // Act & Assert
    Assert.False(graph.Equals(1));
  }

  [Fact]
  public void Equals_EmptyGraphs_ShouldReturnTrue() {
    // Arrange
    var graph1 = new Graph();
    var graph2 = new Graph();
    var graph3 = new Graph(new int[,] {});

    // Act & Assert
    Assert.True(graph1.Equals(graph2));
    Assert.True(graph2.Equals(graph1));
    Assert.True(graph1.Equals(graph3));
    Assert.True(graph3.Equals(graph1));
  }

  [Fact]
  public void Equals_OneEmptyGraph_ShouldReturnFalse() {
    // Arrange
    var graph1 = new Graph();
    var graph2 = new Graph(new int[,] { { 0, 1 }, { 1, 2 } });
    var graph3 = new Graph(new int[,] {});

    // Act & Assert
    Assert.False(graph1.Equals(graph2));
    Assert.False(graph2.Equals(graph1));
    Assert.False(graph3.Equals(graph2));
    Assert.False(graph2.Equals(graph3));
  }

  [Fact]
  public void HashSet() {
    // Arrange
    var graph1 = new Graph();
    var graph2 = new Graph();

    var graph3 = new Graph(new int[,] {});
    var graph4 = new Graph(new int[,] {});

    var graph5 = new Graph(new int[,] { { 0, 1 }, { 1, 2 } });
    var graph6 = new Graph(new int[,] { { 0, 1 }, { 1, 2 } });

    var graph7 = new Graph(new int[,] { { 1, 1 }, { 1, 2 } });

    var graph8 = new Graph(new int[,] { { 0, 1, 1 }, { 1, 2, 1 }, { 1, 2, 2 } });

    // Act & Assert
    Assert.Equal(graph1.GetHashCode(), graph1.GetHashCode());
    Assert.Equal(graph1.GetHashCode(), graph2.GetHashCode());
    Assert.Equal(graph1.GetHashCode(), graph3.GetHashCode());
    Assert.NotEqual(graph1.GetHashCode(), graph5.GetHashCode());

    Assert.Equal(graph3.GetHashCode(), graph3.GetHashCode());
    Assert.Equal(graph4.GetHashCode(), graph3.GetHashCode());
    Assert.NotEqual(graph3.GetHashCode(), graph5.GetHashCode());

    Assert.Equal(graph5.GetHashCode(), graph5.GetHashCode());
    Assert.Equal(graph5.GetHashCode(), graph6.GetHashCode());
    Assert.NotEqual(graph5.GetHashCode(), graph7.GetHashCode());
    Assert.NotEqual(graph5.GetHashCode(), graph8.GetHashCode());
  }

  // Helper method to create a directed graph for testing
  private Graph CreateDirectedGraph3Vertex() {
    int[,] adjacencyMatrix = { { 0, 1, 0 }, { 5, 0, 2 }, { 1, 1, 0 } };

    return new Graph(adjacencyMatrix);
  }

  // Helper method to create an undirected graph for testing
  private Graph CreateUndirectedGraph3Vertex() {
    int[,] adjacencyMatrix = { { 0, 5, 1 }, { 5, 0, 7 }, { 1, 7, 0 } };

    return new Graph(adjacencyMatrix);
  }
}
