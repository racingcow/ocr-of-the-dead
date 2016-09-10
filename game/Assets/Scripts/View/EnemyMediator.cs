using UnityEngine;
using strange.extensions.mediation.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Views
{
    public class EnemyMediator : Mediator
    {
        [Inject]
        public EnemyView EnemyView { get; set; }

        [Inject]
        public EnemyAttacked EnemyAttacked { get; set; }

        [Inject]
        public EnemyPersuing EnemyPersuing { get; set; }

        [Inject]
        public EnemyDying EnemyDying { get; set; }

        public override void OnRegister()
        {
            EnemyView.clawSignal.AddListener(OnClaw);
            EnemyPersuing.AddListener(OnEnemyPersuing);
            EnemyDying.AddListener(OnEnemyDying);
        }

        public override void OnRemove()
        {
            EnemyDying.RemoveListener(OnEnemyDying);
            EnemyPersuing.RemoveListener(OnEnemyPersuing);
            EnemyView.clawSignal.RemoveListener(OnClaw);
        }

        private void OnEnemyDying(EnemyInfo info)
        {
            if (info.Name != EnemyView.name) return;
            EnemyView.Die();
        }

        private void OnEnemyPersuing(EnemyInfo info)
        {
            if (info.Name != EnemyView.name) return;
            EnemyView.Persue();
        }

        private void OnClaw()
        {
            Debug.Log("EnemyMediator raising EnemyAttacked");
            EnemyAttacked.Dispatch(new AttackInfo
            {
                EnemyName = EnemyView.name
            });
        }
    }
}