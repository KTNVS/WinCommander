using System;
using System.Collections.Generic;
using static WinCommander.Components;

namespace WinCommander
{
    public class ShortcutCatcher
    {
        public delegate void RecordHandler(object sender, IntPtr hwnd);
        public event RecordHandler OnRecordRequest;

        public delegate void SimulateHandler(object sender, IntPtr hwnd);
        public event SimulateHandler OnSimulateRequest;

        private readonly GlobalMouseHook MouseHook;
        private readonly GlobalKeyHook KeyHook;

        public ShortcutCatcher()
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

        public void StartCatching()
        {
            MouseHook.SetHook();
            KeyHook.SetHook();
        }
        public void StopCatching()
        {
            MouseHook.UnHook();
            KeyHook.UnHook();
        }

        readonly HashSet<Keys> PressedKeys = new HashSet<Keys>();
        private void KeyHook_OnKeyDown(object sender, KeyEventArgs e)
        {
            PressedKeys.Add(e.KeyCode);
        }
        private void KeyHook_OnKeyUp(object sender, KeyEventArgs e)
        {
            PressedKeys.Remove(e.KeyCode);
        }


        private void MouseHook_OnMouseClick(object sender, MouseEventArgs e)
        {
            
        }
        private void MouseHook_OnMouseMove(object sender, MouseEventArgs e)
        {

        }
        private void MouseHook_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (PressedKeys.Contains(Keys.LControlKey) && PressedKeys.Contains(Keys.LShiftKey))
            {
                if (PressedKeys.Contains(Keys.R))
                {
                    IntPtr hwnd = WinApiHandler.GetHWNDFromPoint(WinApiHandler.GetCursorPosition());
                    if (hwnd != IntPtr.Zero)
                    {
                        OnRecordRequest.Invoke(this, hwnd);
                    }
                }
                else if (PressedKeys.Contains(Keys.S))
                {
                    IntPtr hwnd = WinApiHandler.GetHWNDFromPoint(WinApiHandler.GetCursorPosition());
                    if (hwnd != IntPtr.Zero)
                    {
                        OnSimulateRequest.Invoke(this, hwnd);
                    }
                }
            }
        }
        private void MouseHook_OnMouseUp(object sender, MouseEventArgs e)
        {

        }

        
    }
}
