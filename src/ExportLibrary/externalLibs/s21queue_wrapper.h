extern "C" {
void* CreateQueue();
void DeleteQueue(void* queue);
void QueuePush(void* queue, int value);
int QueueFront(void* queue);
int QueueBack(void* queue);
int QueueSize(void* stack);
void QueuePop(void* queue);
}
