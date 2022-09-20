using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace Slot
{
    public enum RollState
    {
        Spnning = 0,
        Stopped = 1,
    }
    public enum RollIconState
    {
        J = 0,
        GreenFish = 1,
        final = 2,
        Wild = 3,
        Star = 4,
        Bonus = 5,
        A = 6,
        Q = 7,
        K = 8,

    }
    public class Roll : MonoBehaviour
    {
        private RollIconState CurrentSymbol = RollIconState.J;
        private RollIconState LastSymbol = RollIconState.K;
        private RollState rollState = RollState.Stopped;
        [SerializeField] float rollSpeed;
        [SerializeField] Transform[] RollSegments;
        [SerializeField] double totoalRollSpined = 0;
        [SerializeField] double spinningError = 0;
        RollIconState stopIcon;

        public static UnityAction ReelStopped;

        bool isChecking = false;

        public void SetStop(RollIconState symbol, float time)
        {
            rollState = RollState.Spnning;
            StartCoroutine(StopState(time));
            stopIcon = symbol;
        }
        public void ForceStop()
        {
            isChecking = true;
        }
        IEnumerator StopState(float time)
        {
            yield return new WaitForSeconds(time);
            isChecking = true;

        }
        private void Update()
        {
            if (!isChecking) return;
            StopRolling(stopIcon);
            ReelStopped?.Invoke();
        }
        private void FixedUpdate()
        {
            if (rollState != RollState.Spnning) return;
            ResetPosition();
            Spin();
        }

        void ResetPosition()
        {
            for (int i = 0; i < RollSegments.Length; i++)
            {
                int r = (i < RollSegments.Length - 1) ? (i + 1) : 0;
                if (RollSegments[i].position.y < -7.5)
                {
                    var afterRoll = RollSegments[r].position;
                    RollSegments[i].position = new Vector3(afterRoll.x, afterRoll.y + 22.5f, afterRoll.z);
                }
            }
        }
        void Spin()
        {
            foreach (Transform roll in RollSegments)
            {
                var pos = roll.position;
                var displacement = rollSpeed * Time.fixedDeltaTime;
                roll.position = new Vector3(pos.x, pos.y - displacement, pos.z);
                totoalRollSpined += displacement;
                if (totoalRollSpined >= 7.5)
                {
                    spinningError = totoalRollSpined - 7.5f;
                    totoalRollSpined = spinningError;
                    CheckCurrentSymbol();
                }
            }
        }

        private void StopRolling(RollIconState currentSymbol)
        {
            if (CurrentSymbol == currentSymbol)
            {
                rollState = RollState.Stopped;
                isChecking = false;
            }
        }

        private void CheckCurrentSymbol()
        {
            if (CurrentSymbol == LastSymbol)
            {
                CurrentSymbol = 0;
                return;
            }
            CurrentSymbol++;
        }
    }
}
