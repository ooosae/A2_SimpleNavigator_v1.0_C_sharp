#ifndef SRC_S21_QUEUE_H_
#define SRC_S21_QUEUE_H_

#include "s21_list.h"

namespace s21 {
template <typename T>
class queue {
 public:
  /**
   * Тип элемента очереди.
   * T определяет тип элемента
   */
  using value_type = T;

  /**
   * Тип элемента очереди.
   * T& определяет тип ссылки на элеммент
   */
  using reference = T &;

  /**
   * Тип элемента списка.
   * const T& определяет тип ссылки на константу
   */
  using const_reference = const T &;

  /**
   * size_t определяет тип размера контейнера
   */
  using size_type = std::size_t;

  /**
   * Конструктор по умолчанию
   * Создаёт пустую очередь
   */
  queue() : list_() {}

  /**
   * Списки инициализаторов конструкторов.
   * Создаёт список, инициализированный с помощью std::initializer_list
   */
  queue(std::initializer_list<value_type> const &items) : list_(items) {}

  /**
   * Конструктор копирования
   */
  queue(const queue &q) : list_(q.list_) {}

  /**
   * Конструктор перемещения
   */
  queue(queue &&q) : list_(std::move(q.list_)) {}

  /**
   * Деструктор
   */
  ~queue() = default;

  /**
   * Перегрузка оператора присваивания для объекта копирования
   */
  queue &operator=(const queue &q) {
    list_ = q.list_;
    return *this;
  }

  /**
   * Перегрузка оператора присваивания для объекта перемещения
   */
  queue &operator=(queue &&q) noexcept {
    list_ = std::move(q.list_);
    return *this;
  }

  /**
   * Метод для доступа к первому элементу
   */
  reference front() noexcept { return list_.front(); }

  /**
   * Метод для доступа к первому элементу без изменений
   */
  const_reference front() const noexcept { return list_.front(); }

  /**
   * Метод для доступа к последнему элементу
   */
  reference back() noexcept { return list_.back(); }

  /**
   * Метод для доступа к последнему элементу без изменений
   */
  const_reference back() const noexcept { return list_.back(); }

  /**
   * Метод проверяет пустая ли очередь
   */
  bool empty() const noexcept { return list_.empty(); }

  /**
   * Метод возвращает размер очереди
   */
  size_type size() const noexcept { return list_.size(); }

  /**
   * Метод вставляет элемент в конец очереди
   */
  void push(const_reference value) { list_.push_back(value); }

  /**
   * Метод удаляет первый элемент очереди
   */
  void pop() { list_.pop_front(); }

  /**
   * Метод меняет местами элементы двух очередей
   */
  void swap(queue &other) { list_.swap(other.list_); }

  /**
   * Метод добавляет новый элемент в конец контейрнера
   */
  template <typename... Args>
  void insert_many_back(Args &&...args) {
    list_.insert_many_back(std::forward<Args>(args)...);
  }

 private:
  s21::list<T> list_;
};
}  // namespace s21

#endif  // SRC_S21_QUEUE_H_
