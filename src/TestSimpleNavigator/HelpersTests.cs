using s21_helpers;

namespace TestSimpleNavigator;

public class HelpersTests {
  [Fact]
  public void TestStack() {
    s21_helpers.Containers.Stack stack = new();
    Assert.Equal(0, stack.Count());

    stack.Push(1);
    Assert.Equal(1, stack.Count());
    Assert.Equal(1, stack.Top());
    Assert.Equal(1, stack.Pop());

    Assert.Equal(0, stack.Count());

    Assert.Throws<InvalidOperationException>(() => stack.Pop());
    Assert.Throws<InvalidOperationException>(() => stack.Top());

    stack.Dispose();
    Assert.Throws<ObjectDisposedException>(() => stack.Count());

    stack = new();
  }

  [Fact]
  public void TestQueue() {
    s21_helpers.Containers.Queue queue = new();
    Assert.Equal(0, queue.Count());

    queue.Push(1);
    Assert.Equal(1, queue.Count());
    Assert.Equal(1, queue.Front());
    Assert.Equal(1, queue.Back());

    queue.Push(2);
    Assert.Equal(2, queue.Count());
    Assert.Equal(1, queue.Front());
    Assert.Equal(2, queue.Back());

    Assert.Equal(1, queue.Pop());

    Assert.Equal(1, queue.Count());
    Assert.Equal(2, queue.Front());
    Assert.Equal(2, queue.Back());

    Assert.Equal(2, queue.Pop());

    Assert.Throws<InvalidOperationException>(() => queue.Pop());
    Assert.Throws<InvalidOperationException>(() => queue.Front());
    Assert.Throws<InvalidOperationException>(() => queue.Back());

    queue.Dispose();
    Assert.Throws<ObjectDisposedException>(() => queue.Count());

    queue = new();
  }

  [Fact]
  public void SequenceEqual() {
    int[,] a = new int[,] { { 1, 2 }, { 3, 4 } };
    int[,] b = new int[,] { { 1, 2 }, { 3, 4 } };
    int[,] c = new int[,] { { 1, 2 }, { 3, 5 } };
    int[,] d = new int[,] { { 1, 2, 3 }, { 3, 5, 6 } };
    int[,] e = new int[,] { { 1, 2 }, { 3, 5 }, { 6, 7 } };
    int[,]? f = null;
    int[,] g = new int[,] {};
    int[,] h = new int[,] {};

    Assert.True(a.SequenceEqual(a));
    Assert.True(a.SequenceEqual(b));
    Assert.True(g.SequenceEqual(h));
    Assert.False(a.SequenceEqual(c));
    Assert.False(a.SequenceEqual(d));
    Assert.False(a.SequenceEqual(g));
    Assert.False(a.SequenceEqual(e));
    Assert.False(d.SequenceEqual(e));

    Assert.Throws<ArgumentNullException>(() => a.SequenceEqual(f));
    Assert.Throws<ArgumentNullException>(() => f.SequenceEqual(f));
    Assert.Throws<ArgumentNullException>(() => f.SequenceEqual(a));
  }
}