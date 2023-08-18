using System;
using System.Runtime.InteropServices;

using static WinCommander.Components;
using static WinCommander.WinAPI.DllImports;

namespace WinCommander
{
    class GlobalMouseHook
    {
        private POINT point;
        private POINT Point
        {
            get { return point; }
            set
            {
                if (point != value)
                {
                    point = value;
                    if (OnMouseMove != null)
                    {
                        var e = new MouseEventArgs(MouseButtons.None, point.X, point.Y);
                        OnMouseMove(this, e);
                    }
                }
            }
        }

        private IntPtr HookID;
        public HookProc hProc;

        public const int WH_MOUSE_LL = 14;

        private const uint WM_LBUTTONDOWN = (uint)WM.LBUTTONDOWN;
        private const uint WM_RBUTTONDOWN = (uint)WM.RBUTTONDOWN;
        private const uint WM_LBUTTONUP = (uint)WM.LBUTTONUP;
        private const uint WM_RBUTTONUP = (uint)WM.RBUTTONUP;
        private const uint WM_MBUTTONDOWN = (uint)WM.MBUTTONDOWN;
        private const uint WM_MBUTTONUP = (uint)WM.MBUTTONUP;

        public GlobalMouseHook()
        {
            Point = new POINT();
            hProc = new HookProc(MouseHookProc);
        }
        public IntPtr SetHook()
        {
            HookID = SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);

            return HookID;
        }

        public void UnHook()
        {
            UnhookWindowsHookEx(HookID);
        }
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            MouseHookStruct MyMouseHookStruct = (MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseHookStruct));

            if (nCode < 0)
            {
                return CallNextHookEx(HookID, nCode, wParam, lParam);
            }
            else
            {
                if (OnMouseClick != null)
                {
                    MouseButtons button = MouseButtons.None;
                    switch ((uint)wParam)
                    {
                        case WM_LBUTTONDOWN:
                            button = MouseButtons.Left;
                            OnMouseDown(this, new MouseEventArgs(button, point.X, point.Y));
                            break;
                        case WM_LBUTTONUP:
                            button = MouseButtons.Left;
                            OnMouseUp(this, new MouseEventArgs(button, point.X, point.Y));
                            break;
                        case WM_RBUTTONDOWN:
                            button = MouseButtons.Right;
                            OnMouseDown(this, new MouseEventArgs(button, point.X, point.Y));
                            break;
                        case WM_RBUTTONUP:
                            button = MouseButtons.Right;
                            OnMouseUp(this, new MouseEventArgs(button, point.X, point.Y));
                            break;
                        case WM_MBUTTONDOWN:
                            button = MouseButtons.Middle;
                            OnMouseDown(this, new MouseEventArgs(button, point.X, point.Y));
                            break;
                        case WM_MBUTTONUP:
                            button = MouseButtons.Middle;
                            OnMouseUp(this, new MouseEventArgs(button, point.X, point.Y));
                            break;
                    }

                    var e = new MouseEventArgs(button, point.X, point.Y);
                    OnMouseClick(this, e);
                }
                Point = new POINT(MyMouseHookStruct.pt.X, MyMouseHookStruct.pt.Y);
                return CallNextHookEx(HookID, nCode, wParam, lParam);
            }
        }

        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler OnMouseMove;

        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
        public event MouseClickHandler OnMouseClick;

        public delegate void MouseDownHandler(object sender, MouseEventArgs e);
        public event MouseDownHandler OnMouseDown;

        public delegate void MouseUpHandler(object sender, MouseEventArgs e);
        public event MouseUpHandler OnMouseUp;

    }
}
