using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using static WinCommander.Components;

namespace WinCommander
{
    
    public class SelectWindow
    {
        private readonly GlobalMouseHook MouseHook;
        private readonly GlobalKeyHook KeyHook;

        public SelectWindow()
        {
            MouseHook = new GlobalMouseHook();
            KeyHook = new GlobalKeyHook();

            MouseHook.OnMouseClick += MouseHook_OnMouseClick;
            MouseHook.OnMouseDown += MouseHook_OnMouseDown;
            MouseHook.OnMouseMove += MouseHook_OnMouseMove;
            MouseHook.OnMouseUp += MouseHook_OnMouseUp;

            KeyHook.OnKeyDown += KeyHook_OnKeyDown;
            KeyHook.OnKeyUp += KeyHook_OnKeyUp;
        }

        private TaskCompletionSource<bool> tcs = null;
        private bool AlreadyRecording = false;
        private IntPtr? SelectedHWND = null;

        public async Task<IntPtr?> StartSelecting()
        {
            if (AlreadyRecording) { return null; }
            AlreadyRecording = true;

            SelectedHWND = null;

            MouseHook.SetHook();
            KeyHook.SetHook();


            tcs = new TaskCompletionSource<bool>();
            await tcs.Task;


            MouseHook.UnHook();
            KeyHook.UnHook();


            AlreadyRecording = false;

            return SelectedHWND;
        }
        public void StopSelecting()
        {
            tcs?.SetResult(true);
        }

        private readonly Keys SelectKey = Keys.LControlKey;
        private readonly Keys ExitKey = Keys.Escape;

        private bool SelectKeyPressed = false;
        private void KeyHook_OnKeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == ExitKey) { StopSelecting(); }
            if(e.KeyCode == SelectKey) { SelectKeyPressed = true; }
        }
        private void KeyHook_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == SelectKey) { SelectKeyPressed = false; }
        }


        private void MouseHook_OnMouseClick(object sender, MouseEventArgs e)
        {
            if (SelectKeyPressed)
            {
                IntPtr hwnd = WinApiHandler.GetHWNDFromPoint(WinApiHandler.GetCursorPosition());
                if(hwnd != IntPtr.Zero)
                {
                    SelectedHWND = hwnd;
                    StopSelecting();
                }
            }
        }
        private void MouseHook_OnMouseMove(object sender, MouseEventArgs e)
        {

        }
        private void MouseHook_OnMouseDown(object sender, MouseEventArgs e)
        {

        }
        private void MouseHook_OnMouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
