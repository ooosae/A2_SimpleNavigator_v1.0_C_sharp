using ExportLibrary;

namespace s21_helpers.Containers;

public class Stack : IStack, IDisposable {
  IntPtr _items;
  private bool _disposed = false;

  public Stack() {
    _items = ExportStack.CreateStack();
  }

  public int Count() {
    ThrowIfDisposed();
    return ExportStack.StackSize(_items);
  }

  public void Push(int item) {
    ThrowIfDisposed();
    ExportStack.StackPush(_items, item);
  }

  public int Pop() {
    ThrowIfDisposed();
    ThrowIfStackIsEmpty();
    int result = ExportStack.StackTop(_items);
    ExportStack.StackPop(_items);
    return result;
  }

  public int Top() {
    ThrowIfDisposed();
    ThrowIfStackIsEmpty();
    return ExportStack.StackTop(_items);
  }

  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    if (!_disposed) {
      if (_items != IntPtr.Zero) {
        ExportStack.DeleteStack(_items);
        _items = IntPtr.Zero;
      }
      _disposed = true;
    }
  }

  ~Stack() {
    Dispose(false);
  }

  private void ThrowIfDisposed() {
    if (_disposed) {
      throw new ObjectDisposedException(nameof(Stack));
    }
  }

  private void ThrowIfStackIsEmpty() {
    if (ExportStack.StackSize(_items) == 0) {
      throw new InvalidOperationException("Stack is empty.");
    }
  }
}
