namespace s21_helpers.Containers;

public interface IStack {
  public int Count();

  public void Push(int item);

  public int Pop();

  public int Top();
}
