namespace s21_helpers.Containers;

public interface IQueue {
  public int Count();

  public void Push(int item);

  public int Pop();

  public int Front();

  public int Back();
}
