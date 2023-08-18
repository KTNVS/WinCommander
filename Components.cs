﻿using System;

namespace WinCommander
{
    public class Components
    {
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        public struct RECT
        {
            public RECT(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
            public bool Contains(POINT pt)
            {
                return Left < pt.X && pt.X < Right && Top < pt.Y && pt.Y < Bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public static bool operator ==(RECT c1, RECT c2)
            {
                return c1.Equals(c2);
            }

            public static bool operator !=(RECT c1, RECT c2)
            {
                return !c1.Equals(c2);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is RECT))
                {
                    return false;
                }
                RECT rct = (RECT)obj;

                return Left == rct.Left && Right == rct.Right && Top == rct.Top && Bottom == rct.Bottom;
            }
            public override int GetHashCode()
            {
                int hash = 29;

                hash = (hash * 31) + Left.GetHashCode();
                hash = (hash * 31) + Right.GetHashCode();
                hash = (hash * 31) + Top.GetHashCode();
                hash = (hash * 31) + Bottom.GetHashCode();

                return hash;
            }
        }
        public struct POINT
        {
            public static POINT Empty = new POINT(0, 0);
            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X;
            public int Y;


            public static bool operator ==(POINT c1, POINT c2)
            {
                return c1.Equals(c2);
            }

            public static bool operator !=(POINT c1, POINT c2)
            {
                return !c1.Equals(c2);
            }

            public override bool Equals(object obj)
            {
                if (!(obj is POINT))
                {
                    return false;
                }
                POINT pt = (POINT)obj;

                return X == pt.X && Y == pt.Y;
            }


            public override int GetHashCode()
            {
                int hash = 29;

                hash = (hash * 31) + X.GetHashCode();
                hash = (hash * 31) + Y.GetHashCode();

                return hash;
            }
        }

        public struct MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        public enum MouseButtons
        {
            None = 0,
            Left = 1048576,
            Right = 2097152,
            Middle = 4194304,
            XButton1 = 8388608,
            XButton2 = 16777216
        }
        public enum GetWindow_Cmd : uint
        {
            GW_HWNDFIRST = 0,
            GW_HWNDLAST = 1,
            GW_HWNDNEXT = 2,
            GW_HWNDPREV = 3,
            GW_OWNER = 4,
            GW_CHILD = 5,
            GW_ENABLEDPOPUP = 6
        }
        public enum WindowStyles : uint
        {
            WS_OVERLAPPED = 0x00000000,
            WS_POPUP = 0x80000000,
            WS_CHILD = 0x40000000,
            WS_MINIMIZE = 0x20000000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
            WS_CLIPSIBLINGS = 0x04000000,
            WS_CLIPCHILDREN = 0x02000000,
            WS_MAXIMIZE = 0x01000000,
            WS_BORDER = 0x00800000,
            WS_DLGFRAME = 0x00400000,
            WS_VSCROLL = 0x00200000,
            WS_HSCROLL = 0x00100000,
            WS_SYSMENU = 0x00080000,
            WS_THICKFRAME = 0x00040000,
            WS_GROUP = 0x00020000,
            WS_TABSTOP = 0x00010000,

            WS_MINIMIZEBOX = 0x00020000,
            WS_MAXIMIZEBOX = 0x00010000,

            WS_CAPTION = WS_BORDER | WS_DLGFRAME,
            WS_TILED = WS_OVERLAPPED,
            WS_ICONIC = WS_MINIMIZE,
            WS_SIZEBOX = WS_THICKFRAME,
            WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

            WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
            WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
            WS_CHILDWINDOW = WS_CHILD,

            WS_EX_DLGMODALFRAME = 0x00000001,
            WS_EX_NOPARENTNOTIFY = 0x00000004,
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_ACCEPTFILES = 0x00000010,
            WS_EX_TRANSPARENT = 0x00000020
        }

        public class MouseEventArgs : EventArgs
        {
            public MouseEventArgs(MouseButtons button, int x, int y)
            {
                Button = button;
                X = x;
                Y = y;
                Location = new POINT(x, y);
            }

            public MouseButtons Button { get; }
            public int X { get; }
            public int Y { get; }
            public POINT Location { get; }
        }

        public class KeyEventArgs : EventArgs
        {
            public KeyEventArgs(Keys keyCode)
            {
                KeyCode = keyCode;
            }

            public Keys KeyCode { get; }
        }
    }
    public enum Keys : uint
    {
        KeyCode = 0x0000FFFF,
        Modifiers = 0xFFFF0000,
        None = 0x00,
        LButton = 0x01,
        RButton = 0x02,
        Cancel = 0x03,
        MButton = 0x04,
        XButton1 = 0x05,
        XButton2 = 0x06,
        Back = 0x08,
        Tab = 0x09,
        LineFeed = 0x0A,
        Clear = 0x0C,
        Return = 0x0D,
        Enter = Return,
        ShiftKey = 0x10,
        ControlKey = 0x11,
        Menu = 0x12,
        Pause = 0x13,
        Capital = 0x14,
        CapsLock = 0x14,
        KanaMode = 0x15,
        HanguelMode = 0x15,
        HangulMode = 0x15,
        JunjaMode = 0x17,
        FinalMode = 0x18,
        HanjaMode = 0x19,
        KanjiMode = 0x19,
        Escape = 0x1B,
        IMEConvert = 0x1C,
        IMENonconvert = 0x1D,
        IMEAccept = 0x1E,
        IMEAceept = IMEAccept,
        IMEModeChange = 0x1F,
        Space = 0x20,
        Prior = 0x21,
        PageUp = Prior,
        Next = 0x22,
        PageDown = Next,
        End = 0x23,
        Home = 0x24,
        Left = 0x25,
        Up = 0x26,
        Right = 0x27,
        Down = 0x28,
        Select = 0x29,
        Print = 0x2A,
        Execute = 0x2B,
        Snapshot = 0x2C,
        PrintScreen = Snapshot,
        Insert = 0x2D,
        Delete = 0x2E,
        Help = 0x2F,
        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        LWin = 0x5B,
        RWin = 0x5C,
        Apps = 0x5D,
        Sleep = 0x5F,
        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 0x64,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69,
        Multiply = 0x6A,
        Add = 0x6B,
        Separator = 0x6C,
        Subtract = 0x6D,
        Decimal = 0x6E,
        Divide = 0x6F,
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        NumLock = 0x90,
        Scroll = 0x91,
        LShiftKey = 0xA0,
        RShiftKey = 0xA1,
        LControlKey = 0xA2,
        RControlKey = 0xA3,
        LMenu = 0xA4,
        RMenu = 0xA5,
        BrowserBack = 0xA6,
        BrowserForward = 0xA7,
        BrowserRefresh = 0xA8,
        BrowserStop = 0xA9,
        BrowserSearch = 0xAA,
        BrowserFavorites = 0xAB,
        BrowserHome = 0xAC,
        VolumeMute = 0xAD,
        VolumeDown = 0xAE,
        VolumeUp = 0xAF,
        MediaNextTrack = 0xB0,
        MediaPreviousTrack = 0xB1,
        MediaStop = 0xB2,
        MediaPlayPause = 0xB3,
        LaunchMail = 0xB4,
        SelectMedia = 0xB5,
        LaunchApplication1 = 0xB6,
        LaunchApplication2 = 0xB7,
        OemSemicolon = 0xBA,
        Oem1 = OemSemicolon,
        Oemplus = 0xBB,
        Oemcomma = 0xBC,
        OemMinus = 0xBD,
        OemPeriod = 0xBE,
        OemQuestion = 0xBF,
        Oem2 = OemQuestion,
        Oemtilde = 0xC0,
        Oem3 = Oemtilde,
        OemOpenBrackets = 0xDB,
        Oem4 = OemOpenBrackets,
        OemPipe = 0xDC,
        Oem5 = OemPipe,
        OemCloseBrackets = 0xDD,
        Oem6 = OemCloseBrackets,
        OemQuotes = 0xDE,
        Oem7 = OemQuotes,
        Oem8 = 0xDF,
        OemBackslash = 0xE2,
        Oem102 = OemBackslash,
        ProcessKey = 0xE5,
        Packet = 0xE7,
        Attn = 0xF6,
        Crsel = 0xF7,
        Exsel = 0xF8,
        EraseEof = 0xF9,
        Play = 0xFA,
        Zoom = 0xFB,
        NoName = 0xFC,
        Pa1 = 0xFD,
        OemClear = 0xFE,
        Shift = 0x00010000,
        Control = 0x00020000,
        Alt = 0x00040000
    };
}
