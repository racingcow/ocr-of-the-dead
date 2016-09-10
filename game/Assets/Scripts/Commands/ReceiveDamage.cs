using System.Linq;
using strange.extensions.command.impl;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class ReceiveDamage : Command
    {
        [Inject]
        public IPlayer Player { get; set; }

        [Inject]
        public AttackInfo AttackInfo { get; set; }

        [Inject]
        public IWaypointList WaypointList { get; set; }

        [Inject]
        public PlayerHealthChanged PlayerHealthChanged { get; set; }

        public override void Execute()
        {
            // Store old value
            var oldHealth = Player.Health;

            // Damage player. Maybe later take into account armor for damage reduction, etc.
            var attackingEnemy = WaypointList.Current.Enemies.Single(e => e.Name == AttackInfo.EnemyName);
            Player.Health -= attackingEnemy.DamageAmount;
            
            // Signal that value has changed
            PlayerHealthChanged.Dispatch(new HealthInfo
            {
                OldHealth = oldHealth,
                NewHealth = Player.Health
            });
        }
    }
}