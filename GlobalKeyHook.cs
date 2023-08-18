
using System;
using System.Runtime.InteropServices;

using static WinCommander.Components;
using static WinCommander.WinAPI.DllImports;

namespace WinCommander
{
    class GlobalKeyHook
    {
        private const int WH_KEYBOARD_LL = 13;

        private const uint WM_KEYDOWN = (uint)WM.KEYDOWN;
        private const uint WM_KEYUP = (uint)WM.KEYUP;
        private const uint WM_SYSKEYDOWN = (uint)WM.SYSKEYDOWN;
        private const uint WM_SYSKEYUP = (uint)WM.SYSKEYUP;

        private IntPtr HookID = IntPtr.Zero;
        private HookProc hProc;

        public GlobalKeyHook()
        {
            hProc = KeyHookProc;
        }

        public IntPtr SetHook()
        {
            HookID = SetWindowsHookEx(WH_KEYBOARD_LL, hProc, IntPtr.Zero, 0);

            return HookID;
        }
        public void UnHook()
        {
            UnhookWindowsHookEx(HookID);
        }

        private int KeyHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                OnKeyDown(this, new KeyEventArgs((Keys)vkCode));
            }
            else if (nCode >= 0 && wParam == (IntPtr)WM_KEYUP || wParam == (IntPtr)WM_SYSKEYUP)
            {
                int vkCode = Marshal.ReadInt32(lParam);

                OnKeyUp(this, new KeyEventArgs((Keys)vkCode));
            }

            return CallNextHookEx(HookID, nCode, wParam, lParam);
        }

        public delegate void KeyDownHandler(object sender, KeyEventArgs e);
        public event KeyDownHandler OnKeyDown;

        public delegate void KeyUpHandler(object sender, KeyEventArgs e);
        public event KeyUpHandler OnKeyUp;
    }
}
