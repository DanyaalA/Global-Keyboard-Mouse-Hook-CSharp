using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GlobalMouseHook
{
    public class KeyboardHook : IDisposable
    {
        public event KeyEventHandler KeyDown, KeyUp;

        private HookProc keyboardHookProc;
        private IntPtr keyboardHookHandle = IntPtr.Zero;

        public const int WH_KEYBOARD_LL = 13;

        public KeyboardHook()
        {
            keyboardHookProc = KeyboardHookProc;
            keyboardHookHandle = SetHook(WH_KEYBOARD_LL, keyboardHookProc);
        }

        ~KeyboardHook()
        {
            Dispose();
        }

        private static IntPtr SetHook(int hookType, HookProc hookProc)
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            using (ProcessModule currentModule = currentProcess.MainModule)
            {
                return NativeMethods.SetWindowsHookEx(hookType, hookProc, NativeMethods.GetModuleHandle(currentModule.ModuleName), 0);
            }
        }

        public enum KeyEvent
        {
            WM_KEYDOWN = 0x100,
            WM_KEYUP = 0x101,
            WM_SYSKEYUP = 0x104,
            WM_SYSKEYDOWN = 0x105
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                bool handled = false;

                switch ((KeyEvent)wParam)
                {
                    case KeyEvent.WM_KEYDOWN:
                        Console.WriteLine("Test");
                    case KeyEvent.WM_SYSKEYDOWN:
                        handled = OnKeyDown(lParam);
                        
                        break;
                    case KeyEvent.WM_KEYUP:
                    case KeyEvent.WM_SYSKEYUP:
                        handled = OnKeyUp(lParam);
                        break;
                }

                if (handled)
                {
                    return keyboardHookHandle;
                }
            }

            return NativeMethods.CallNextHookEx(keyboardHookHandle, nCode, wParam, lParam);
        }

        private bool OnKeyDown(IntPtr key)
        {
            if (KeyDown != null)
            {
                KeyEventArgs keyEventArgs = GetKeyEventArgs(key);
                KeyDown(this, keyEventArgs);
                return keyEventArgs.Handled || keyEventArgs.SuppressKeyPress;
            }

            return false;
        }

        private bool OnKeyUp(IntPtr key)
        {
            if (KeyUp != null)
            {
                KeyEventArgs keyEventArgs = GetKeyEventArgs(key);
                KeyUp(this, keyEventArgs);
                return keyEventArgs.Handled || keyEventArgs.SuppressKeyPress;
            }

            return false;
        }

        private KeyEventArgs GetKeyEventArgs(IntPtr key)
        {
            Keys keyData = (Keys)Marshal.ReadInt32(key) | Control.ModifierKeys;
            return new KeyEventArgs(keyData);
        }

        public void Dispose()
        {
            NativeMethods.UnhookWindowsHookEx(keyboardHookHandle);
        }
    }
