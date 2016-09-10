using Racingcow.OcrOfTheDead.Models;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using Racingcow.OcrOfTheDead.Signals;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class InitializeGame : Command
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        [Inject]
        public WaypointChanged WaypointChanged { get; set; }

        [Inject]
        public IPlayer Player { get; set; }

        [Inject]
        public IWordList WordList { get; set; }

        [Inject]
        public WordsLowCountReached WordsLowCountReached { get; set; }

        public override void Execute()
        {
            //todo: use config/settings here
            WordList.LowCountThreshold = 10;
            WordList.HighCountThreshold = 50;

            Player.DamageAmount = 1;

            WaypointChanged.Dispatch(new WaypointInfo { NextWaypointName = "Waypoint (0)" });
            WordsLowCountReached.Dispatch();
        }
    }
}