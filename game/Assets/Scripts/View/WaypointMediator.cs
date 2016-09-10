using strange.extensions.mediation.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Views
{
    public class WaypointMediator : Mediator
    {
        [Inject]
        public WaypointView View { get; set; }

        [Inject]
        public PlayerArrivedAtWaypoint PlayerArrivedAtWaypoint { get; set; }

        public override void OnRegister()
        {
            View.triggerSignal.AddListener(OnArrived);
        }

        public override void OnRemove()
        {
            View.triggerSignal.RemoveListener(OnArrived);
        }

        private void OnArrived()
        {
            PlayerArrivedAtWaypoint.Dispatch();
        }
    }
}