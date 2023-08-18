using System;
using System.Threading.Tasks;

namespace WinCommander
{
    public interface IAction
    {
        Task Execute(IntPtr hwnd);
    }

    public static class Actions
    {
        public struct Wait : IAction
        {
            private TimeSpan WaitingTime;
            public Wait(TimeSpan waitingTime)
            {
                WaitingTime = waitingTime;
            }
            public async Task Execute(IntPtr hwnd)
            {
                await Task.Delay(WaitingTime);
            }
            public override string ToString()
            {
                return string.Format("Passed Time = {0}\n", WaitingTime.ToString());
            }
        }
        public struct MouseAction : IAction
        {
            private int X, Y;
            private MouseEvent MouseEvent;

            public MouseAction(int x, int y, MouseEvent mouseEvent)
            {
                X = x;
                Y = y;
                MouseEvent = mouseEvent;
            }
            public Task Execute(IntPtr hwnd)
            {
                uint WM_MESSAGE = (uint)WM.NULL;
                switch (MouseEvent)
                {
                    case MouseEvent.LeftButtonDown:
                        WM_MESSAGE = (uint)WM.LBUTTONDOWN;
                        break;
                    case MouseEvent.LeftButtonUp:
                        WM_MESSAGE = (uint)WM.LBUTTONUP;
                        break;
                    case MouseEvent.RightButtonDown:
                        WM_MESSAGE = (uint)WM.RBUTTONDOWN;
                        break;
                    case MouseEvent.RightButtonUp:
                        WM_MESSAGE = (uint)WM.RBUTTONUP;
                        break;
                    case MouseEvent.MiddleButtonDown:
                        WM_MESSAGE = (uint)WM.MBUTTONDOWN;
                        break;
                    case MouseEvent.MiddleButtonUp:
                        WM_MESSAGE = (uint)WM.MBUTTONUP;
                        break;
                    case MouseEvent.Move:
                        WM_MESSAGE = (uint)WM.MOUSEMOVE;
                        break;
                }

                WinApiHandler.SendMouse(X, Y, WM_MESSAGE, hwnd);
                return Task.CompletedTask;
            }
            public override string ToString()
            {
                return string.Format("Mouse Position = ({0}, {1}), Mouse Event = {2}\n", new string[] { X.ToString(), Y.ToString(), MouseEvent.ToString() });
            }
        }
        public struct KeyAction : IAction
        {
            private Keys Key;
            private KeyEvent KeyEvent;

            public KeyAction(Keys key, KeyEvent keyEvent)
            {
                Key = key;
                KeyEvent = keyEvent;
            }
            public Task Execute(IntPtr hwnd)
            {
                uint WM_MESSAGE = (uint)WM.NULL;
                switch (KeyEvent)
                {
                    case KeyEvent.Down:
                        WM_MESSAGE = (uint)WM.KEYDOWN;
                        break;
                    case KeyEvent.Up:
                        WM_MESSAGE = (uint)WM.KEYUP;
                        break;
                }

                WinApiHandler.SendKey(Key, WM_MESSAGE, hwnd);
                return Task.CompletedTask;
            }
            public override string ToString()
            {
                return string.Format("Pressed Key = {0}, Mouse Event = {1}\n", new string[] { Key.ToString(), KeyEvent.ToString() });
            }
        }


        public enum KeyEvent : byte
        {
            Down,
            Up
        }
        public enum MouseEvent : byte
        {
            LeftButtonDown,
            LeftButtonUp,
            RightButtonDown,
            RightButtonUp,
            MiddleButtonDown,
            MiddleButtonUp,
            Move
        }
    }
}
