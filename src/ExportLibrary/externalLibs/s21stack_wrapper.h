extern "C" {
void* CreateStack();
void StackPush(void* stack, int value);
void StackPop(void* stack);
int StackTop(void* stack);
int StackSize(void* stack);
void DeleteStack(void* stack);
}
