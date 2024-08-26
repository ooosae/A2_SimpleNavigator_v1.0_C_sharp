using System.Runtime.InteropServices;

namespace ExportLibrary;

public static class ExportStack {
#if WINDOWS
  [DllImport("s21_stack.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_stack.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern IntPtr CreateStack();

#if WINDOWS
  [DllImport("s21_stack.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_stack.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern void StackPush(IntPtr stack, int value);

#if WINDOWS
  [DllImport("s21_stack.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_stack.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern void StackPop(IntPtr stack);

#if WINDOWS
  [DllImport("s21_stack.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_stack.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern int StackTop(IntPtr stack);

#if WINDOWS
  [DllImport("s21_stack.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_stack.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern int StackSize(IntPtr stack);

#if WINDOWS
  [DllImport("s21_stack.dll", CallingConvention = CallingConvention.Cdecl)]
#elif LINUX
  [DllImport("libs21_stack.so", CallingConvention = CallingConvention.Cdecl)]
#endif
  public static extern void DeleteStack(IntPtr stack);
}
