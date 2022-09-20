using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Slot
{
    enum MachineState
    {
        Spinning = 0,
        Stopped = 1,
    }
    public class Machine : MonoBehaviour
    {
        public Roll[] rolls;
        [SerializeField] private Image SpinAndStopButton;
        [SerializeField] private Sprite StopSprite;
        [SerializeField] private Sprite StartSprite;
        private MachineState CurrentState;
        private int stopCount = 0;

        private void OnEnable()
        {
            Roll.ReelStopped += RollStopped;

        }
        private void Awake()
        {
            Application.targetFrameRate = 30;
        }
        private void RollStopped()
        {
            stopCount++;
            if(stopCount == 5)
            {
                SpinAndStopButton.sprite = StartSprite;
                CurrentState = MachineState.Stopped;
                stopCount = 0;
            }
        }

        public void SymbolButtonPressed(int Symbol)
        {
            foreach (Roll R in rolls)
            {
                float spinTime = Random.Range(3, 5);
                R.SetStop((RollIconState)Symbol, spinTime);
            }
            CurrentState = MachineState.Spinning;
            SpinAndStopButton.sprite = StopSprite;
        }
        public void RandomSpin()
        {
            if (CurrentState == MachineState.Spinning)
            {
                foreach (Roll R in rolls)
                {
                    R.ForceStop();
                }
                return;
            }

            foreach (Roll R in rolls)
            {
                float spinTime = Random.Range(3, 5);
                int randomSymbol = Random.Range(0, 8);
                R.SetStop((RollIconState)randomSymbol, spinTime);
            }
            CurrentState = MachineState.Spinning;
            SpinAndStopButton.sprite = StopSprite;
        }
    }
}
