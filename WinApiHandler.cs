using System;
using System.Runtime.InteropServices;
using System.Text;

using static WinCommander.Components;
using static WinCommander.WinAPI.DllImports;

namespace WinCommander
{
    public static class WinApiHandler
    {
        private static int CreateLParamFromPosition(int LoWord, int HiWord)
        {
            return (HiWord << 16) | (LoWord & 0xffff);
        }


        public static IntPtr SendKey(Keys key, uint WM_Message, IntPtr hwnd)
        {
            return PostMessage(hwnd, WM_Message, (IntPtr)key, IntPtr.Zero);
        }
        public static IntPtr SendMouse(int x, int y, uint WM_Message, IntPtr handle)
        {
            IntPtr lparam = (IntPtr)CreateLParamFromPosition(x, y);
            return PostMessage(handle, WM_Message, IntPtr.Zero, lparam);
        }
        public static POINT GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }
        public static IntPtr GetHWNDFromPoint(POINT point)
        {
            return WindowFromPoint(point);
        }
        public static string GetWindowTitle(IntPtr hWnd)
        {
            int length = GetWindowTextLength(hWnd) + 1;
            StringBuilder title = new StringBuilder(length);

            GetWindowText(hWnd, title, length);
            return title.ToString();
        }

    }
    public static class WinAPI
    {
        public static partial class DllImports
        {
            [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
            public static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

            [DllImport("user32.dll")]
            public static extern int SetWindowText(IntPtr hWnd, string text);

            [DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

            [DllImport("user32.dll")]
            public static extern IntPtr WindowFromPoint(POINT Point);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

            [DllImport("user32.dll")]
            public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);

            [DllImport("user32.dll")]
            public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);

            [DllImport("user32", ExactSpelling = true, SetLastError = true)]
            public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] ref RECT rect, [MarshalAs(UnmanagedType.U4)] int cPoints);

            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr GetWindow(IntPtr hWnd, GetWindow_Cmd uCmd);

            [DllImport("gdi32.dll")]
            public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        } //Window
        public static partial class DllImports
        {
            [DllImport("ntdll.dll")]
            // NtQuerySystemInformation gives us data about all the handlers in the system
            public static extern uint NtQuerySystemInformation(uint SystemInformationClass, IntPtr SystemInformation,
            int SystemInformationLength, ref int nLength);

            [DllImport("ntdll.dll")]
            // Retrieves information about an object
            // Handle is the object's handle we're getting information from
            // ObjectInformationClass is the type of information we want; ObjectBasicInformation/ObjectTypeInformation, undocumented ObjectNameInformation?
            // ObjectInformation is the buffer where the data is returned to, ObjectInformationLength is the size of that buffer
            // returnLength is a variable where NtQueryObject writes the size of the information returned to us
            public static extern int NtQueryObject(IntPtr Handle, int ObjectInformationClass, IntPtr ObjectInformation,
            int ObjectInformationLength, ref int returnLength);

            [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern bool QueryFullProcessImageName(IntPtr hProcess, uint dwFlags, [Out, MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpExeName, ref uint lpdwSize);

            [DllImport("kernel32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            // DuplicateHandle duplicates a handle from an external process to ours
            // hSourceProcessHandle is the process we duplicate from, hSourceHandle is the handle we duplicate
            // hTargetProcessHandle is the process we duplicate to, lpTargetHandle is a pointer to a var that receives the new handler
            public static extern bool DuplicateHandle(IntPtr hSourceProcessHandle, ushort hSourceHandle, IntPtr hTargetProcessHandle,
            out IntPtr lpTargetHandle, uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, uint dwOptions);

            [DllImport("kernel32.dll")]
            // Closes a handle
            public static extern int CloseHandle(IntPtr hObject);

            [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr GetModuleHandle(string lpModuleName);
        } //Handle
        public static partial class DllImports
        {
            [DllImport("kernel32.dll")]
            // dwDesiredAccess sets the process access rights (docs.microsoft.com/en-us/windows/desktop/ProcThread/process-security-and-access-rights)
            // if bInheritHandle is true, processes created by this process will inherit the handle (we don't need this, maybe just set it as a bool)
            // dwProcessId is the PID of the process we want to open with those access rights
            public static extern IntPtr OpenProcess(uint dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

            [DllImport("kernel32.dll")]
            // Returns us a handle to the current process
            public static extern IntPtr GetCurrentProcess();

            [DllImport("kernel32.dll")]
            // Access rights, inheritance bool, ID of thread
            public static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

            [DllImport("kernel32.dll")]
            // Thread to suspend
            public static extern uint SuspendThread(IntPtr hThread);

            [DllImport("kernel32.dll")]
            // Thread to resume
            public static extern int ResumeThread(IntPtr hThread);

            [DllImport("user32.dll")]
            public static extern int GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);
        } //ProcessAndThreads
        public static partial class DllImports
        {
            [DllImport("user32.dll")]
            public static extern IntPtr PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern bool UnhookWindowsHookEx(IntPtr idHook);

            [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
            public static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);
        } //Messages
        public static partial class DllImports
        {
            [DllImport("user32.dll")]
            public static extern bool GetCursorPos(out POINT lpPoint);

            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern int GetWindowTextLength(IntPtr hWnd);

            [DllImport("user32.dll")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
        }


    }
}
