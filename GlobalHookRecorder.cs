using System;
using System.Collections.Generic;
using System.Diagnostics;

using static WinCommander.Components;
using static WinCommander.Actions;
using System.Threading.Tasks;

namespace WinCommander
{
    public class GlobalHookRecorder
    {
        public static readonly Keys FINISH_KEY = Keys.Q;
        readonly GlobalMouseHook mouseHook;
        readonly GlobalKeyHook keyHook;


        private IntPtr AllowedHWND;
        private readonly LinkedList<IAction> Actions;
        private readonly Stopwatch WaitTime;

        public GlobalHookRecorder()
        {
            mouseHook = new GlobalMouseHook();
            keyHook = new GlobalKeyHook();

            mouseHook.OnMouseClick += MouseHook_OnMouseClick;
            mouseHook.OnMouseDown += MouseHook_OnMouseDown;
            mouseHook.OnMouseMove += MouseHook_OnMouseMove;
            mouseHook.OnMouseUp += MouseHook_OnMouseUp;

            keyHook.OnKeyDown += KeyHook_OnKeyDown;
            keyHook.OnKeyUp += KeyHook_OnKeyUp;


            Actions = new LinkedList<IAction>();
            WaitTime = new Stopwatch();
        }


        private TaskCompletionSource<bool> tcs = null;
        private bool AlreadyRecording = false;
        public async Task<LinkedList<IAction>> StartRecording(IntPtr allowedHWND)
        {
            if (AlreadyRecording) { return null; }
            AlreadyRecording = true;

            AllowedHWND = allowedHWND;

            /*
            uint flags = (uint)(WindowStyles.WS_OVERLAPPED | WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU | WindowStyles.WS_MINIMIZEBOX | WindowStyles.WS_MAXIMIZEBOX);
            WinAPI.DllImports.SetWindowLong(allowedHWND, 0, flags);*/

            Actions.Clear();
            WaitTime.Start();

            mouseHook.SetHook();
            keyHook.SetHook();


            tcs = new TaskCompletionSource<bool>();
            await tcs.Task;


            mouseHook.UnHook();
            keyHook.UnHook();

            Actions.AddLast(new Wait(WaitTime.Elapsed));
            WaitTime.Stop();

            Actions.RemoveFirst();

            AlreadyRecording = false;
            return Actions;
        }
        public void StopRecording()
        {
            tcs?.SetResult(true);
        }


        private void KeyHook_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == FINISH_KEY)
            {
                StopRecording();
                return;
            }

            NewKeyAction(e, KeyEvent.Down);
        }
        private void KeyHook_OnKeyUp(object sender, KeyEventArgs e)
        {
            NewKeyAction(e, KeyEvent.Up);
        }
        private void NewKeyAction(KeyEventArgs e, KeyEvent keyEvent)
        {

            Actions.AddLast(new Wait(WaitTime.Elapsed));
            WaitTime.Restart();

            Actions.AddLast(new KeyAction(e.KeyCode, keyEvent));
        }


        private void MouseHook_OnMouseClick(object sender, MouseEventArgs e)
        {
            // useless as we are measuring the time between mouse down and up
        }
        private void MouseHook_OnMouseMove(object sender, MouseEventArgs e)
        {
            // only usable in global mouse simulation, also inefficient
        }
        private void MouseHook_OnMouseDown(object sender, MouseEventArgs e)
        {
            MouseEvent mouseEvent;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseEvent = MouseEvent.LeftButtonDown;
                    break;
                case MouseButtons.Right:
                    mouseEvent = MouseEvent.RightButtonDown;
                    break;
                case MouseButtons.Middle:
                    mouseEvent = MouseEvent.MiddleButtonDown;
                    break;

                default:
                    return;
            }
            NewMouseAction(e, mouseEvent);
        }
        private void MouseHook_OnMouseUp(object sender, MouseEventArgs e)
        {
            MouseEvent mouseEvent;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    mouseEvent = MouseEvent.LeftButtonUp;
                    break;
                case MouseButtons.Right:
                    mouseEvent = MouseEvent.RightButtonUp;
                    break;
                case MouseButtons.Middle:
                    mouseEvent = MouseEvent.MiddleButtonUp;
                    break;

                default:
                    return;
            }
            NewMouseAction(e, mouseEvent);
        }

        private void NewMouseAction(MouseEventArgs e, MouseEvent mouseEvent)
        {
            // somehow check the hwnd

            Actions.AddLast(new Wait(WaitTime.Elapsed));
            WaitTime.Restart();


            POINT cursorPosition = new POINT(e.X, e.Y);
            WinAPI.DllImports.ScreenToClient(AllowedHWND, ref cursorPosition);

            Actions.AddLast(new MouseAction(cursorPosition.X, cursorPosition.Y, mouseEvent));
        }
    }
}
