using System.Runtime.InteropServices;

namespace ExportLibrary;

public static class ExportQueue {
#if WINDOWS
  [DllImport("s21_queue.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_queue.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern IntPtr CreateQueue();

#if WINDOWS
  [DllImport("s21_queue.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_queue.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern void DeleteQueue(IntPtr queue);

#if WINDOWS
  [DllImport("s21_queue.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_queue.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern void QueuePush(IntPtr queue, int value);

#if WINDOWS
  [DllImport("s21_queue.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_queue.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern int QueueFront(IntPtr queue);

#if WINDOWS
  [DllImport("s21_queue.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_queue.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern int QueueBack(IntPtr queue);

#if WINDOWS
  [DllImport("s21_queue.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_queue.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern int QueueSize(IntPtr queue);

#if WINDOWS
  [DllImport("s21_queue.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_queue.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern void QueuePop(IntPtr queue);
}
