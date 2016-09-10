using System.Linq;
using Racingcow.OcrOfTheDead.Enums;
using strange.extensions.command.impl;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;
using UnityEngine;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class ChangeWaypoint : Command
    {
        [Inject]
        public IWaypointList Waypoints { get; set; }

        [Inject]
        public IWordList WordList { get; set; }

        [Inject]
        public EnemyWaited EnemyWaited { get; set; }

        [Inject]
        public EnemyPersuing EnemyPersuing { get; set; }

        [Inject]
        public WaypointEngaged WaypointEngaged { get; set; }

        [Inject]
        public WaypointChanged WaypointChanged { get; set; }

        [Inject]
        public PlayerAimed PlayerAimed { get; set; }

        [Inject]
        public WordsLowCountReached WordsLowCountReached { get; set; }

        public override void Execute()
        {
            Waypoints.MoveNext();

            // Only stop and fight at waypoints that have enemies. Otherwise move along.
            if (Waypoints.Current.Enemies.Any())
            {
                Debug.Log("Engaging at waypoint " + Waypoints.Current.Name);
                AggroEnemies();
                WaypointEngaged.Dispatch();
                PlayerAimed.Dispatch(new AimInfo(AimInfo.AimDirection.Next));
            }
            else
            {
                Debug.Log("Waypoint " + Waypoints.Current.Name + " has no enemies. Not engaging.");
                if (Waypoints.Next != null)
                {
                    WaypointChanged.Dispatch(new WaypointInfo { NextWaypointName = Waypoints.Next.Name });
                }
            }
            
        }

        private void AggroEnemies()
        {
            foreach (var enemy in Waypoints.Current.Enemies)
            {
                enemy.Word = WordList[0];
                WordList.RemoveAt(0);
                if (WordList.Count <= WordList.LowCountThreshold)
                {
                    WordsLowCountReached.Dispatch();
                }

                EnemyWaited.Dispatch(new EnemyInfo(enemy.Name));
                enemy.State = EnemyStates.Walk;
                EnemyPersuing.Dispatch(new EnemyInfo(enemy.Name));
            }
        }
    }
}