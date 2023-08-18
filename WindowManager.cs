using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCommander
{
    public class WindowManager
    {
        private readonly List<SimulationWindow> SimulationWindows;

        private readonly GlobalHookRecorder GlobalHookRecorder;
        private readonly SelectWindow SelectWindow;

        public WindowManager()
        {
            SimulationWindows = new List<SimulationWindow>();

            GlobalHookRecorder = new GlobalHookRecorder();
            SelectWindow = new SelectWindow();
        }
        public void ReleaseAll()
        {
            foreach (SimulationWindow simWin in SimulationWindows)
            {
                simWin.StopSimulation();
            }
            SimulationWindows.Clear();
        }

        public async Task Record(IntPtr hwnd)
        {
            SimulationWindow simWin;

            bool existingWindow = WindowExists(hwnd);

            if (existingWindow)
            {
                simWin = GetWindow(hwnd);
                simWin.RemoveRecordings();
            }
            else
            {
                simWin = new SimulationWindow(hwnd);
            }

            LinkedList<IAction> recording = await GlobalHookRecorder.StartRecording(hwnd);

            if(recording == null || recording.Count == 0) { return; }

            simWin.SetRecordings(recording);

            if (!existingWindow)
            {
                SimulationWindows.Add(simWin);
            }
            

            return;
        }
        public void Simulate(IntPtr hwnd)
        {
            if (!WindowExists(hwnd)) { return; }

            SimulationWindow simWin = GetWindow(hwnd);
            if (!simWin.HasRecording()) { return; }

            if (simWin.Simulating) { simWin.StopSimulation(); }
            else { simWin.StartSimulation(); }

            return;
        }
        public async Task Record()
        {
            IntPtr? hwnd = await SelectWindow.StartSelecting();
            if (hwnd == null) { return; }

            await Record(hwnd.Value);
        }
        public async void Simulate()
        {
            IntPtr? hwnd = await SelectWindow.StartSelecting();
            if(hwnd == null) { return; }

            Simulate(hwnd.Value);
        }



        public bool WindowExists(IntPtr hwnd)
        {
            return SimulationWindows.Exists(x => x.HWND.Equals(hwnd));
        }
        public SimulationWindow GetWindow(IntPtr hwnd)
        {
            return SimulationWindows.Find(x => x.HWND.Equals(hwnd));
        }
    }
}
