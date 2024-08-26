using System.Text;
using s21_helpers;

namespace s21_graph;

public class Graph : IEquatable<Graph?> {
  private const string WrongFileMessage = "Wrong file.";
  private int[,] _adjacencyMatrix = new int[,] {};
  private int _vertexCount;

  public int MinPossibleValue => 0;
  public int MaxPossibleValue => int.MaxValue - 1;

  public int VertexCount => _vertexCount;

  public int this[int i, int j] {
    get {
      ThrowIfNoContent();
      ThrowIfIndexOutOfRange(i, j);
      return _adjacencyMatrix![i - 1, j - 1];
    }
    set {
      ThrowIfNoContent();
      ThrowIfIndexOutOfRange(i, j);
      ThrowIfValueOutOrRange(value);

      if (value != _adjacencyMatrix![i - 1, j - 1]) {
        _adjacencyMatrix![i - 1, j - 1] = value;
      }
    }
  }

  public Graph() : this(new int[,] {}) {}

  public Graph(int[,] adjacencyMatrix) {
    if (adjacencyMatrix is null) {
      InitEmptyGraph();
      return;
    }
    if (adjacencyMatrix.GetLength(0) != adjacencyMatrix.GetLength(1)) {
      throw new ArgumentException("Adjacency matrix should be square.");
    }
    for (int i = 0; i < adjacencyMatrix.GetLength(0); i++) {
      for (int j = 0; j < adjacencyMatrix.GetLength(1); j++) {
        ThrowIfValueOutOrRange(adjacencyMatrix[i, j]);
      }
    }

    _vertexCount = adjacencyMatrix.GetLength(0);
    _adjacencyMatrix = (int[,])adjacencyMatrix.Clone();
  }

  // Loads the graph from a file containing the adjacency matrix
  public void LoadGraphFromFile(string filename) {
    string[] lines = File.ReadAllLines(filename);
    if (lines is null || lines.Length == 0 || lines[0].Split(' ').Length != 1 ||
        !int.TryParse(lines[0], out _vertexCount) || _vertexCount <= 1 ||
        lines.Length != _vertexCount + 1) {
      NullifyAndThrowIfWrongFile();
    }

    _adjacencyMatrix = new int[_vertexCount, _vertexCount];

    for (int i = 0; i < _vertexCount; i++) {
      var values = lines![i + 1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
      if (values.Length != _vertexCount) {
        NullifyAndThrowIfWrongFile();
      }

      for (int j = 0; j < _vertexCount; j++) {
        if (!int.TryParse(values[j], out _adjacencyMatrix[i, j]) || _adjacencyMatrix[i, j] < 0) {
          NullifyAndThrowIfWrongFile();
        }
      }
    }
  }

  // Exports the graph to a DOT file
  public void ExportGraphToDot(string filename) {
    var sb = new StringBuilder().AppendLine("graph G {");

    for (int i = 0; i < _vertexCount; i++) {
      sb.AppendLine($"  {i + 1};");
    }

    bool isUndirected = IsUndirected();
    string separator = isUndirected ? " -- " : " --> ";

    for (int i = 0; i < _vertexCount; i++) {
      for (int j = i * Convert.ToInt32(isUndirected); j < _vertexCount;
           j++)  // j = i to avoid duplicate edges in undirected graph
      {
        if (_adjacencyMatrix![i, j] != 0) {
          sb.AppendLine($"  {i + 1}{separator}{j + 1};");
        }
      }
    }

    sb.AppendLine("}");

    File.WriteAllText(filename, sb.ToString());
  }

  private void ThrowIfValueOutOrRange(int value) {
    if (value < MinPossibleValue || MaxPossibleValue < value) {
      throw new ArgumentOutOfRangeException("Catnot set negative value.");
    }
  }

  private void ThrowIfIndexOutOfRange(int i, int j) {
    if (i <= 0 || _vertexCount < i || j <= 0 || _vertexCount < j) {
      throw new IndexOutOfRangeException("Index is out of range.");
    }
  }

  private void ThrowIfNoContent() {
    if (_vertexCount == 0) {
      throw new NullReferenceException("Graph conteins nothing.");
    }
  }

#region overrides
  public override string? ToString() {
    if (_adjacencyMatrix.Length == 0 || _vertexCount == 0) {
      return null;
    }

    var sb = new StringBuilder().AppendLine($"Graph with {VertexCount} vertices.");

    for (int i = 0; i < _vertexCount; i++) {
      for (int j = 0; j < _vertexCount; j++) {
        sb.Append($"{_adjacencyMatrix[i, j]} ");
      }
      if (i != _vertexCount - 1) {
        sb.AppendLine("");
      }
    }

    return sb.ToString();
  }

  public override bool Equals(object? obj) {
    return Equals(obj as Graph);
  }

  public bool Equals(Graph? other) {
    return other is not null && VertexCount == other.VertexCount &&
           _adjacencyMatrix.SequenceEqual(other._adjacencyMatrix);
  }

  public override int GetHashCode() {
    int hash = 17;
    hash = hash * 31 + _vertexCount.GetHashCode();
    for (int i = 0; i < _vertexCount; i++) {
      for (int j = 0; j < _vertexCount; j++) {
        hash = hash * 31 + _adjacencyMatrix[i, j].GetHashCode();
      }
    }

    return hash;
  }
#endregion

  public bool IsUndirected() {
    for (int i = 0; i < _vertexCount; i++) {
      for (int j = i + 1; j < _vertexCount; j++) {
        if (_adjacencyMatrix[i, j] != _adjacencyMatrix[j, i]) {
          return false;
        }
      }
    }

    return true;  // Consider an empty or null matrix as undirected too
  }

  private void NullifyAndThrowIfWrongFile() {
    InitEmptyGraph();
    throw new FormatException(WrongFileMessage);
  }

  private void InitEmptyGraph() {
    _adjacencyMatrix = new int[,] {};
    _vertexCount = 0;
  }
}
