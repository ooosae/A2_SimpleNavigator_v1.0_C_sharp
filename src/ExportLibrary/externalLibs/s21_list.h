#ifndef SRC_S21_LIST_H_
#define SRC_S21_LIST_H_

#include <initializer_list>
#include <iostream>
#include <limits>

namespace s21 {
template <typename T>
class list {
  class ListNode;
  class ListIterator;
  class ListConstIterator;

 public:
  /**
   * Тип элемента списка.
   * T определяет тип элемента
   */
  using value_type = T;

  /**
   * Тип элемента списка.
   * T& определяет тип ссылки на элеммент
   */
  using reference = T&;

  /**
   * Тип элемента списка.
   * const T& определяет тип ссылки на константу
   */
  using const_reference = const T&;

  /**
   * Внутренний класс ListIterator<T>.
   * Определяет тип для итерации по контейнеру
   */
  using iterator = ListIterator;

  /**
   * Внутренний класс ListIterator<T>.
   * Определяет тип константы для итерации по контейнеру
   */
  using const_iterator = ListConstIterator;

  /**
   * size_t определяет тип размера контейнера
   */
  using size_type = std::size_t;

  /**
   * Конструктор по умолчанию.
   * Создаёт пустой список.
   */
  list() : head_(new ListNode), size_(0) {}

  /**
   * Конструктор с параметрами.
   * Создаёт список размером n.
   */
  explicit list(size_type n) : list() {
    while (n > 0) {
      push_front(value_type{});
      --n;
    }
  }

  /**
   * Списки инициализаторов конструкторов.
   * Создаёт список, инициализированный с помощью std::initializer_list
   */
  list(std::initializer_list<value_type> const& items) : list() {
    for (auto& elem : items) push_back(elem);
  }

  /**
   * Конструктор копирования
   */
  list(const list& other) : list() { *this = other; }

  /**
   * Конструктор перемещения
   */
  list(list&& other) : list() { *this = std::move(other); }

  /**
   * Десктруктор
   */
  ~list() {
    clear();
    delete head_;
  }

  /**
   * Перегрузка оператора присваивания для объекта копирования
   */
  list& operator=(const list& other) {
    if (this != &other) {
      size_type count = other.size_ < size_ ? other.size_ : size_;
      iterator iter = begin();
      const_iterator iter_other = other.begin();
      while (count > 0) {
        *iter++ = *iter_other++;
        --count;
      }
      while (other.size_ < size_) {
        pop_back();
      }
      while (other.size_ > size_) {
        push_back(*iter_other++);
      }
    }
    size_ = other.size_;
    return *this;
  }

  /**
   * Перегрузка оператора присваивания для объекта перемещения
   */
  list& operator=(list&& other) noexcept {
    if (this != &other) {
      clear();
      swap(other);
    }
    return *this;
  }

  /**
   * Метод для доступа к первому элементу
   */
  reference front() noexcept { return *begin(); }

  /**
   * Метод для доступа к первому элементу без изменений
   */
  const_reference front() const noexcept { return *begin(); }

  /**
   * Метод для доступа к последнему элементу
   */
  reference back() noexcept { return *--end(); }

  /**
   * Метод для доступа к последнему элементу без изменений
   */
  const_reference back() const noexcept { return *--end(); }

  /**
   * Метод возвращает итератор на первый элемент
   */
  iterator begin() noexcept { return iterator(head_->next_); }

  /**
   * Метод возвращает итератор на первый элемент без изменений
   */
  const_iterator begin() const noexcept { return iterator(head_->next_); }

  /**
   * Метод возвращает итератор на последний элемент
   */
  iterator end() noexcept { return iterator(head_); }

  /**
   * Метод возвращает итератор на последний элемент без изменений
   */
  const_iterator end() const noexcept { return iterator(head_); }

  /**
   * Метод проверяет пустой ли список
   */
  bool empty() const noexcept { return size_ == 0; }

  /**
   * Метод проверяет размер списка
   */
  size_type size() const noexcept { return size_; }

  /**
   * Метод возвращает максимальный номер элемента
   */
  size_type max_size() const {
    return ((std::numeric_limits<size_type>::max() / 2) / sizeof(ListNode));
  }

  /**
   * Метод очищает список
   */
  void clear() {
    while (size_ > 0) pop_back();
  }

  /**
   * Метод вставляет элемент в список на указанную позицию и возвращает итератор
   * на новый элемент
   */
  iterator insert(iterator pos, const_reference value) {
    ListNode* new_Node = new ListNode;
    new_Node->data_ = value;
    new_Node->next_ = pos.currentNode_;
    new_Node->prev_ = pos.currentNode_->prev_;

    pos.currentNode_->prev_->next_ = new_Node;
    pos.currentNode_->prev_ = new_Node;
    ++size_;
    return iterator(new_Node);
  }

  /**
   * Метод удаляет элемент по итератору
   */
  void erase(iterator pos) {
    if (pos.currentNode_ != head_) {
      pos.currentNode_->prev_->next_ = pos.currentNode_->next_;
      pos.currentNode_->next_->prev_ = pos.currentNode_->prev_;

      delete pos.currentNode_;
      --size_;
    }
  }

  /**
   * Метод добавляет элемент в конец списка
   */
  void push_back(const_reference value) { insert(end(), value); }

  /**
   * Метод добавляет элемент в начало списка
   */
  void push_front(const_reference value) { insert(begin(), value); }

  /**
   * Метод удаляет последний элемент
   */
  void pop_back() { erase(--end()); }

  /**
   * Метод удаляет первый элемент
   */
  void pop_front() { erase(begin()); }

  /**
   * Метод меняет элементы в двух списках между собой
   */
  void swap(list& other) {
    if (this != &other) {
      std::swap(size_, other.size_);
      std::swap(head_, other.head_);
    }
  }

  /**
   * Объединяет два отсортированных списка
   */
  void merge(list& other) {
    if (this != &other) {
      iterator iter = begin();
      iterator iter_other = other.begin();
      iterator iter_end = end();
      iterator iter_end_other = other.end();
      while (iter != iter_end && iter_other != iter_end_other) {
        if (*iter > *iter_other) {
          iterator tmp = iter_other;
          ++iter_other;

          // изменение адресов в предидущей ноде и вставляемой
          iter.currentNode_->prev_->next_ = tmp.currentNode_;
          tmp.currentNode_->prev_ = iter.currentNode_->prev_;

          // изменениме адресов в следующей ноде и вставляемой
          iter.currentNode_->prev_ = tmp.currentNode_;
          tmp.currentNode_->next_ = iter.currentNode_;

          ++size_;
          --other.size_;

          // удаление адресов ноды из списка other
          other.head_->next_ = iter_other.currentNode_;
          iter_other.currentNode_->prev_ = other.head_;
        } else {
          ++iter;
        }
      }
      splice(iter, other);
    }
  }

  /**
   * Метод перемещает элементы из списка other на позицию итератора pos
   */
  void splice(const_iterator pos, list& other) noexcept {
    if (!other.empty()) {
      iterator iter_other_first = other.begin();
      iterator iter_other_last = --other.end();

      // Начало диапазона для вставки
      pos.currentNode_->prev_->next_ = iter_other_first.currentNode_;
      iter_other_first.currentNode_->prev_ = pos.currentNode_->prev_;

      // Конец диапазона для вставки
      pos.currentNode_->prev_ = iter_other_last.currentNode_;
      iter_other_last.currentNode_->next_ = pos.currentNode_;

      size_ += other.size_;
      other.size_ = 0;
      other.head_->next_ = other.head_;
      other.head_->prev_ = other.head_;
    }
  }

  /**
   * Метод расставляет элементы списка в обратном порядке
   */
  void reverse() {
    iterator iter = begin();
    iterator iter_final = iter;
    do {
      std::swap(iter.currentNode_->next_, iter.currentNode_->prev_);
      ++iter;
    } while (iter != iter_final);
  }

  /**
   * Метод удаляет повторяющиеся элементы
   */
  void unique() {
    iterator iter_prev = begin();
    iterator iter = iter_prev;
    ++iter;
    iterator iter_end = end();
    while (iter != iter_end) {
      if (*iter == *iter_prev) {
        erase(iter);
        iter = iter_prev;
      } else {
        ++iter_prev;
      }
      ++iter;
    }
  }

  /**
   * Метод сортирует элементы списка
   */
  void sort() {
    if (size_ > 1) qsort(begin(), size_);
  }

  /**
   * Метод вставляет новые элементы в контейнер перед элементом pos
   */
  template <typename... Args>
  iterator insert_many(const_iterator pos, Args&&... args) {
    iterator iter(pos.currentNode_);
    for (auto elem : {std::forward<Args>(args)...}) {
      insert(iter, elem);
    }
    return iter;
  }

  /**
   * Метод добавляет новый элемент в конец контейнера
   */
  template <typename... Args>
  void insert_many_back(Args&&... args) {
    insert_many(end(), std::forward<Args>(args)...);
  }

  /**
   * Метод добавляет новый элемент в начало контейнера
   */
  template <typename... Args>
  void insert_many_front(Args&&... args) {
    insert_many(begin(), std::forward<Args>(args)...);
  }

 private:
  /**
   * Класс нода списка
   */
  class ListNode {
   public:
    /**
     * Конструктор по умолчанию.
     * Создаёт пустую ноду.
     */
    ListNode() : next_(this), prev_(this), data_(value_type{}) {}

    ListNode* next_;
    ListNode* prev_;
    value_type data_;
  };

  /**
   * Внутренний класс ListIterator<T>.
   * Определяет тип для итератора
   */
  class ListIterator {
    friend list;

   public:
    /**
     * Конструктор по умолчанию.
     * Создаёт пустой итератор.
     */
    ListIterator() : currentNode_(nullptr) {}

    /**
     * Конструктор копирования
     */
    ListIterator(const iterator& other) : currentNode_(other.currentNode_) {}

    /**
     * Перегрузка оператора присваивания
     */
    iterator& operator=(const iterator& other) {
      if (this != &other) currentNode_ = other.currentNode_;
      return *this;
    }

    /**
     * Перегрузка оператора ссылки
     */
    reference operator*() { return currentNode_->data_; }

    /**
     * Перегрузка оператора проверки на равенство
     */
    bool operator==(const iterator& other) {
      return currentNode_ == other.currentNode_;
    }

    /**
     * Перегрузка оператора проверки на неравенство
     */
    bool operator!=(const iterator& other) {
      return currentNode_ != other.currentNode_;
    }

    /**
     * Перегрузка оператора сложения
     */
    iterator& operator++() {
      currentNode_ = currentNode_->next_;
      return *this;
    }

    /**
     * Перегрузка оператора вычитания
     */
    iterator& operator--() {
      currentNode_ = currentNode_->prev_;
      return *this;
    }

    /**
     * Перегрузка оператора сложения
     */
    iterator operator++(int) {
      iterator temp = *this;
      ++(*this);
      return temp;
    }

    /**
     * Перегрузка оператора вычитания
     */
    iterator operator--(int) {
      iterator temp = *this;
      --(*this);
      return temp;
    }

   private:
    /**
     * Параметризованнный конструктор,.
     * Создаёт элемент списка
     */
    ListIterator(ListNode* Node) : currentNode_(Node) {}

    ListNode* currentNode_;
  };

  /**
   * Внутренний класс ListConstIterator<T>.
   * Определяет тип константы для итерации по контейнеру.
   */
  class ListConstIterator {
    friend list;
    friend bool operator==(const const_iterator& iter1,
                           const const_iterator& iter2) {
      return iter1.currentNode_ == iter2.currentNode_;
    }
    friend bool operator!=(const const_iterator& iter1,
                           const const_iterator& iter2) {
      return iter1.currentNode_ != iter2.currentNode_;
    }

   public:
    /**
     * Конструктор по умолчанию.
     * Создаёт пустой итератор
     */
    ListConstIterator() : currentNode_(nullptr) {}

    /**
     * Конструктор копирования
     */
    ListConstIterator(const iterator& other)
        : currentNode_(other.currentNode_) {}

    /**
     * Конструктор копирования для константного итератора
     */
    ListConstIterator(const const_iterator& other)
        : currentNode_(other.currentNode_) {}

    /**
     * Перегрузка оператора присваивания для константного итератора
     */
    const_iterator& operator=(const const_iterator& other) {
      if (this != &other) currentNode_ = other.currentNode_;
      return *this;
    }

    /**
     * Перегрузка оператора разименования указателя
     */
    const_reference operator*() { return currentNode_->data_; }

    /**
     * Перегрузка префиксного оператора инкремента
     */
    const_iterator& operator++() {
      currentNode_ = currentNode_->next_;
      return *this;
    }

    /**
     * Перегрузка префиксного оператора декремента
     */
    const_iterator& operator--() {
      currentNode_ = currentNode_->prev_;
      return *this;
    }

    /**
     * Перегрузка постфиксного оператора инкремента
     */
    const_iterator operator++(int) {
      const_iterator temp = *this;
      ++(*this);
      return temp;
    }

    /**
     * Перегрузка постфиксного оператора декремента
     */
    const_iterator operator--(int) {
      const_iterator temp = *this;
      --(*this);
      return temp;
    }

   private:
    ListNode* currentNode_;
  };

  /**
   * Метод быстрой сортировки
   */
  void qsort(iterator iter_begin, size_type size) {
    if (size > 1) {
      long long i = 0;
      long long j = size - 1;  // Номер последного эл-та
      iterator iter_i = iter_begin;
      iterator iter_j = iter_begin;  // Итератор на последнем эл-те
      for (long long k = 0; k < j; k++, ++iter_j) {
      }

      iterator iter_pivot = iter_begin;
      for (size_type k = 0; k < size / 2; ++k, ++iter_pivot) {
      }
      value_type pivot = *iter_pivot;

      do {
        while (*iter_i < pivot) {
          i++;
          ++iter_i;
        }
        while (*iter_j > pivot) {
          j--;
          --iter_j;
        }
        if (i <= j) {
          std::swap(*iter_i, *iter_j);
          i++;
          ++iter_i;
          j--;
          --iter_j;
        }
      } while (i <= j);

      if (j > 0) {
        qsort(iter_begin, j + 1);
      }
      if (i < (long long)size) {
        qsort(iter_i, size - i);
      }
    }
  }

  ListNode* head_;  // Головной узел
  size_type size_;  // Количество элементов
};
}  // namespace s21

#endif  // SRC_S21_LIST_H_
