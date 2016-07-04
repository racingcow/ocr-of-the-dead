using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.command.impl;

namespace Racingcow.OcrOfTheDead.Commands
{
    public class GameInitializeCommand : Command
    {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject contextView { get; set; }

        public override void Execute()
        {
        }
    }
}