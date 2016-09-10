using strange.extensions.mediation.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Views
{
    public class BloodMediator : Mediator
    {
        [Inject]
        public BloodView View { get; set; }

        [Inject]
        public PlayerHealthChanged PlayerHealthChanged { get; set; }

        public override void OnRegister()
        {
            PlayerHealthChanged.AddListener(OnHealthChanged);
        }

        public override void OnRemove()
        {
            PlayerHealthChanged.RemoveListener(OnHealthChanged);
        }

        private void OnHealthChanged(HealthInfo health)
        {
            View.UpdateHealth(health.NewHealth);
        }
    }
}