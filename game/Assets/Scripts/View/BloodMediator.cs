using strange.extensions.mediation.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Views
{
    public class BloodMediator : Mediator
    {
        [Inject]
        public BloodView View { get; set; }

        [Inject]
        public HealthChangedSignal HealthChangedSignal { get; set; }

        public override void OnRegister()
        {
            HealthChangedSignal.AddListener(OnHealthChanged);
        }

        private void OnHealthChanged(ValueChangeInfo changeInfo)
        {
            View.UpdateHealth(changeInfo.NewValue);
        }
    }
}