using strange.extensions.mediation.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Views
{
    public class ReticleMediator : Mediator
    {
        private string _targetedEnemyName;

        [Inject]
        public ReticleView View { get; set; }

        [Inject]
        public PlayerAimed PlayerAimed { get; set; }

        [Inject]
        public EnemyTargetedChanged EnemyTargetedChanged { get; set; }

        [Inject]
        public PlayerAttacked PlayerAttacked { get; set; }

        [Inject]
        public PlayerHolstered PlayerHolstered { get; set; }

        public override void OnRegister()
        {
            View.tabKeySignal.AddListener(OnTabKeyPressed);
            View.enterKeySignal.AddListener(OnEnterKeyPressed);
            EnemyTargetedChanged.AddListener(OnEnemyTargetedChanged);
            PlayerHolstered.AddListener(OnEnemyHolstered);
        }

        public override void OnRemove()
        {
            PlayerHolstered.RemoveListener(OnEnemyHolstered);
            EnemyTargetedChanged.RemoveListener(OnEnemyTargetedChanged);
            View.enterKeySignal.RemoveListener(OnEnterKeyPressed);
            View.tabKeySignal.RemoveListener(OnTabKeyPressed);
        }

        private void OnEnemyHolstered()
        {
            _targetedEnemyName = null;
            View.ClearTextbox();
            View.MoveToEnemy(null);
            View.SetImage(null, 0, 0);
            View.Hide();
        }

        private void OnEnemyTargetedChanged(EnemyTargetInfo info)
        {
            if (!info.Targeted) return;
            _targetedEnemyName = info.Name;
            View.Show();
            View.MoveToEnemy(info.Name);
            View.ClearTextbox();
            View.SetImage(info.Image, info.ImageWidth, info.ImageHeight);
        }

        private void OnEnterKeyPressed()
        {
            PlayerAttacked.Dispatch(new AttackInfo
            {
                EnemyName = _targetedEnemyName
            });
        }

        private void OnTabKeyPressed(bool shift)
        {
            var direction = shift ? AimInfo.AimDirection.Previous : AimInfo.AimDirection.Next;
            PlayerAimed.Dispatch(new AimInfo(direction));
        }
    }
}