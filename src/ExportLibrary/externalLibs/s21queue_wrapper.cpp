#include "s21queue_wrapper.h"

#include "s21_queue.h"

extern "C" {
void* CreateQueue() { return new s21::queue<int>(); }

void DeleteQueue(void* queue) {
  static_cast<s21::queue<int>*>(queue)->~queue();
}

void QueuePush(void* queue, int value) {
  static_cast<s21::queue<int>*>(queue)->push(value);
}

int QueueFront(void* queue) {
  return static_cast<s21::queue<int>*>(queue)->front();
}

int QueueBack(void* queue) {
  return static_cast<s21::queue<int>*>(queue)->back();
}

int QueueSize(void* queue) {
  return static_cast<s21::queue<int>*>(queue)->size();
}

void QueuePop(void* queue) { static_cast<s21::queue<int>*>(queue)->pop(); }
}
