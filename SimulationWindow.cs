using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinCommander
{
    public class SimulationWindow
    {
        public IntPtr HWND { get; private set; }

        private LinkedList<IAction> Recordings;

        public bool Simulating { get; private set; } = false;

        public SimulationWindow(IntPtr hwnd)
        {
            HWND = hwnd;
        }

        public bool HasRecording()
        {
            return !(Recordings == null || Recordings.Count == 0);
        }
        public void SetRecordings(LinkedList<IAction> recordings)
        {
            Recordings = recordings;
        }
        public void RemoveRecordings()
        {
            StopSimulation();
            Recordings = null;
        }
        public void StartSimulation()
        {
            if(!HasRecording())
            {
                throw new Exception("There is no recording to simulate.");
            }
            if (Simulating)
            {
                return;
            }
            Simulating = true;

            Task loopSimulation = Task.Run(async () => {

                LinkedListNode<IAction> currentAction = Recordings.First;
                while (Simulating)
                {
                    await currentAction.Value.Execute(HWND);
                    currentAction = currentAction.Next ?? currentAction.List.First;
                }
            });
        }
        public void StopSimulation()
        {
            Simulating = false;
        }
    }
}
