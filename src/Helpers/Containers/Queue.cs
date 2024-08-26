using ExportLibrary;

namespace s21_helpers.Containers;

public class Queue : IQueue, IDisposable {
  IntPtr _items;
  private bool _disposed = false;

  public Queue() {
    _items = ExportQueue.CreateQueue();
  }

  public int Count() {
    ThrowIfDisposed();
    return ExportQueue.QueueSize(_items);
  }

  public void Push(int item) {
    ThrowIfDisposed();
    ExportQueue.QueuePush(_items, item);
  }

  public int Pop() {
    ThrowIfDisposed();
    ThrowIfQueueIsEmpty();
    int result = ExportQueue.QueueFront(_items);
    ExportQueue.QueuePop(_items);
    return result;
  }

  public int Front() {
    ThrowIfDisposed();
    ThrowIfQueueIsEmpty();
    return ExportQueue.QueueFront(_items);
  }

  public int Back() {
    ThrowIfDisposed();
    ThrowIfQueueIsEmpty();
    return ExportQueue.QueueBack(_items);
  }

  public void Dispose() {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  protected virtual void Dispose(bool disposing) {
    if (!_disposed) {
      if (_items != IntPtr.Zero) {
        ExportQueue.DeleteQueue(_items);
        _items = IntPtr.Zero;
      }
      _disposed = true;
    }
  }

  ~Queue() {
    Dispose(false);
  }

  private void ThrowIfDisposed() {
    if (_disposed) {
      throw new ObjectDisposedException(nameof(Queue));
    }
  }

  private void ThrowIfQueueIsEmpty() {
    if (ExportQueue.QueueSize(_items) == 0) {
      throw new InvalidOperationException("Queue is empty.");
    }
  }
}
