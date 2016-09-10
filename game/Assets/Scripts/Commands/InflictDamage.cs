using System.Linq;
using Racingcow.OcrOfTheDead.Enums;
using strange.extensions.command.impl;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class InflictDamage : Command
    {
        [Inject]
        public IWaypointList Waypoints { get; set; }

        [Inject]
        public AttackInfo AttackInfo { get; set; }

        [Inject]
        public IPlayer Player { get; set; }

        [Inject]
        public EnemyHealthChanged EnemyHealthChanged { get; set; }

        [Inject]
        public EnemyDying EnemyDying { get; set; }

        [Inject]
        public PlayerAimed PlayerAimed { get; set; }

        [Inject]
        public PlayerArrivedAtWaypoint PlayerArrivedAtWaypoint { get; set; }

        public override void Execute()
        {
            var enemy = Waypoints.Current.Enemies.SingleOrDefault(e => e.Targeted);
            if (enemy == null) return;

            // Store old value
            var oldHealth = enemy.Health;

            // Damage enemy
            enemy.Health -= Player.DamageAmount;

            // Signal that value has changed
            EnemyHealthChanged.Dispatch(new HealthInfo
            {
                OldHealth = oldHealth,
                NewHealth = enemy.Health
            });

            // Check for dead
            if (enemy.Health <= 0)
            {
                enemy.State = EnemyStates.Die;
                EnemyDying.Dispatch(new EnemyInfo(enemy.Name));
                PlayerAimed.Dispatch(new AimInfo(AimInfo.AimDirection.Next));

                if (Waypoints.Current.Enemies.All(e => e.State == EnemyStates.Die))
                {
                    PlayerArrivedAtWaypoint.Dispatch();
                }
            }
        }
    }
}