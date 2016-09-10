using System.Linq;
using Racingcow.OcrOfTheDead.Enums;
using strange.extensions.command.impl;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class TargetEnemy : Command
    {
        [Inject]
        public IEnemy Enemies { get; set; }

        [Inject]
        public AimInfo AimInfo { get; set; }

        [Inject]
        public EnemyTargetedChanged EnemyTargetedChanged { get; set; }

        [Inject]
        public IWaypointList Waypoints { get; set; }

        [Inject]
        public PlayerHolstered PlayerHolstered { get; set; }

        public override void Execute()
        {
            // Only target one enemy at a time
            Waypoints.Current.Enemies.ForEach(e =>
            {
                e.Targeted = false;
                EnemyTargetedChanged.Dispatch(EnemyTargetFromEnemy(e));
            });

            var nextTarget = FindNextEnemy();
            if (nextTarget == null)
            {
                PlayerHolstered.Dispatch();
                return;
            }
            nextTarget.Targeted = true;
            EnemyTargetedChanged.Dispatch(EnemyTargetFromEnemy(nextTarget));
        }

        private static EnemyTargetInfo EnemyTargetFromEnemy(IEnemy enemy)
        {
            return new EnemyTargetInfo
            {
                Name = enemy.Name,
                Targeted = enemy.Targeted,
                Word = enemy.Word.Value,
                Image = enemy.Word.Image,
                ImageWidth = enemy.Word.ImageWidth,
                ImageHeight = enemy.Word.ImageWidth
            };
        }

        private IEnemy FindNextEnemy()
        {
            var enemies = Waypoints.Current.Enemies;
            if (!enemies.Any()) return null;

            //var curTargeted = AimInfo.EnemyName == null ? null : enemies.FirstOrDefault(e => e.Name == AimInfo.EnemyName);
            var curTargeted = enemies.FirstOrDefault(e => e.Targeted);
            var idx = curTargeted == null ? 0 : enemies.IndexOf(curTargeted);
            var rotCt = 0;
            while (rotCt < enemies.Count) //todo: put direction in here
            {
                idx = (idx + 1) % enemies.Count;
                var curEnemy = enemies[idx];
                if (curEnemy.State != EnemyStates.Die && curEnemy.State != EnemyStates.Dead)
                {
                    return curEnemy;
                }
                rotCt++;
            }
            return null;
        }
    }
}