using UnityEngine;
using strange.extensions.command.impl;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class AttackCommand : Command
    {
        [Inject]
        public IPlayerModel Player { get; set; }

        [Inject]
        public AttackInfo AttackInfo { get; set; }

        [Inject]
        public HealthChangedSignal HealthChangedSignal { get; set; }

        public override void Execute()
        {
            // Store old value
            var healthInfo = new ValueChangeInfo { OldValue = Player.Health };

            // Damage player. Maybe later take into account armor for damage reduction, etc.
            Player.Health -= AttackInfo.DamageAmount;
            healthInfo.NewValue = Player.Health;

            // Signal that value has changed
            HealthChangedSignal.Dispatch(healthInfo);
            Debug.Log(string.Format("Invoked AttackCommand for {0} points of damage", healthInfo.OldValue - healthInfo.NewValue));
        }
    }
}