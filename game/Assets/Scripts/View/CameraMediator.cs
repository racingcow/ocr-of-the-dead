using Racingcow.OcrOfTheDead.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Racingcow.OcrOfTheDead.Views
{
    public class CameraMediator : Mediator
    {
        [Inject]
        public CameraView CameraView { get; set; }

        [Inject]
        public WaypointEngaged WaypointEngaged { get; set; }

        [Inject]
        public WaypointChanged WaypointChanged { get; set; }

        public override void OnRegister()
        {
            WaypointEngaged.AddListener(OnWaypointEngaged);
            WaypointChanged.AddListener(OnWaypointChanged);
        }

        public override void OnRemove()
        {
            WaypointChanged.RemoveListener(OnWaypointChanged);
            WaypointEngaged.RemoveListener(OnWaypointEngaged);
        }

        private void OnWaypointChanged(WaypointInfo info)
        {
            CameraView.SetNextWaypoint(info.NextWaypointName);
            CameraView.Start();
        }

        private void OnWaypointEngaged()
        {
            CameraView.Stop();
        }
    }
}