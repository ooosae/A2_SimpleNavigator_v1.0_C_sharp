#include "s21stack_wrapper.h"

#include "s21_stack.h"

extern "C" {
void* CreateStack() { return new s21::stack<int>(); }

void DeleteStack(void* stack) {
  static_cast<s21::stack<int>*>(stack)->~stack();
}

void StackPush(void* stack, int value) {
  static_cast<s21::stack<int>*>(stack)->push(value);
}

int StackTop(void* stack) {
  return static_cast<s21::stack<int>*>(stack)->top();
}

int StackSize(void* stack) {
  return static_cast<s21::stack<int>*>(stack)->size();
}

void StackPop(void* stack) { static_cast<s21::stack<int>*>(stack)->pop(); }
}
