using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

using Racingcow.OcrOfTheDead.Commands;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;
using Racingcow.OcrOfTheDead.Views;

namespace Racingcow.OcrOfTheDead
{
    public class GameContext : MVCSContext
    {
        public GameContext(MonoBehaviour view) : base(view)
        {
        }

        public GameContext(MonoBehaviour view, ContextStartupFlags flags) : base(view, flags)
        {
        }

        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        public override IContext Start()
        {
            Debug.Log("Context Starting");

            base.Start();

            injectionBinder.GetInstance<GameStartedSignal>().Dispatch();

            //TODO: FIGURE OUT A BETTER WAY TO INITIALIZE THIS MODEL
            var playerModel = injectionBinder.GetInstance<IPlayerModel>();
            playerModel.Health = 100;

            return this;
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            injectionBinder.Bind<IPlayerModel>().To<PlayerModel>().ToSingleton();
            injectionBinder.Bind<HealthChangedSignal>().ToSingleton();

            commandBinder.Bind<GameStartedSignal>().To<GameInitializeCommand>().Once();
            commandBinder.Bind<AttackedSignal>().To<AttackCommand>();

            mediationBinder.Bind<EnemyView>().To<EnemyMediator>();
            mediationBinder.Bind<PlayerHealthView>().To<PlayerHealthMediator>();
        }
    }
}