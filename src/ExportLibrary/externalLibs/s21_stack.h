#ifndef SRC_S21_STACK_H_
#define SRC_S21_STACK_H_

#include "s21_list.h"

namespace s21 {
template <typename T>
class stack {
 public:
  /**
   * Тип элемента стэка.
   * T определяет тип элемента
   */
  using value_type = T;

  /**
   * Тип элемента стэка.
   * T& определяет тип ссылки на элеммент
   */
  using reference = T&;

  /**
   * Тип элемента стэка.
   * const T& определяет тип ссылки на константу
   */
  using const_reference = const T&;

  /**
   * size_t определяет тип размера контейнера
   */
  using size_type = std::size_t;

  /**
   * Конструктор по умолчанию.
   * Создаёт пустой стэк.
   */
  stack() : list_() {}

  /**
   * Списки инициализаторов конструкторов.
   * Создаёт список, инициализированный с помощью std::initializer_list
   */
  stack(std::initializer_list<value_type> const& items) : list_(items) {}

  /**
   * Конструктор копирования
   */
  stack(const stack& s) : list_(s.list_) {}

  /**
   * Конструктор перемещения
   */
  stack(stack&& s) : list_(std::move(s.list_)) {}

  /**
   * Десктруктор
   */
  ~stack() = default;

  /**
   * Перегрузка оператора присваивания для объекта копирования
   */
  stack& operator=(const stack& s) {
    list_ = s.list_;
    return *this;
  }

  /**
   * Перегрузка оператора присваивания для объекта перемещения
   */
  stack& operator=(stack&& s) noexcept {
    list_ = std::move(s.list_);
    return *this;
  }

  /**
   * Метод для доступа к первому элементу
   */
  reference top() noexcept { return list_.back(); }

  /**
   * Метод для доступа к первому элементу без изменений
   */
  const_reference top() const noexcept { return list_.back(); }

  /**
   * Метод проверяет пустой ли стек
   */
  bool empty() const noexcept { return list_.empty(); }

  /**
   * Метод возвращает размер стека
   */
  size_type size() const noexcept { return list_.size(); }

  /**
   * Метод вставляет элемент в начало стека
   */
  void push(const_reference value) { list_.push_back(value); }

  /**
   * Метод удаляет первый элемент
   */
  void pop() { list_.pop_back(); }

  /**
   * Метод меняет местами элементы двух стеков
   */
  void swap(stack& other) { list_.swap(other.list_); }

  /**
   * Метод добавляет новый элемент в начало контейнера
   */
  template <typename... Args>
  void insert_many_front(Args&&... args) {
    list_.insert_many_front(std::forward<Args>(args)...);
  }

 private:
  s21::list<T> list_;
};
}  // namespace s21

#endif  // SRC_S21_STACK_H_
