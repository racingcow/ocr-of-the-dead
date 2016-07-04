using UnityEngine;
using strange.extensions.mediation.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Views
{
    public class EnemyMediator : Mediator
    {
        [Inject]
        public EnemyView View { get; set; }

        [Inject]
        public AttackedSignal AttackedSignal { get; set; }

        public override void OnRegister()
        {
            View.clawSignal.AddListener(OnClaw);
            Debug.Log("Enemy mediator started listening to claw signal");
        }

        public override void OnRemove()
        {
            View.clawSignal.RemoveListener(OnClaw);
            Debug.Log("Enemy mediator stopped listening to claw signal");
        }

        private void OnClaw()
        {
            Debug.Log("Enemy mediator raising attack signal");
            AttackedSignal.Dispatch(new AttackInfo { DamageAmount = View.damage });
        }
    }
}