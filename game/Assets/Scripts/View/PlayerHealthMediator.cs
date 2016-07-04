using strange.extensions.mediation.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Views
{
    public class PlayerHealthMediator : Mediator
    {
        [Inject]
        public PlayerHealthView View { get; set; }

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