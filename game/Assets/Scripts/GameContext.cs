using System.Linq;
using Assets.Scripts.WordSources;
using UnityEngine;

using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

using Racingcow.OcrOfTheDead.Commands;
using Racingcow.OcrOfTheDead.Models;
using Racingcow.OcrOfTheDead.Signals;
using Racingcow.OcrOfTheDead.Views;
using Object = UnityEngine.Object;

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

            injectionBinder.GetInstance<GameStarted>().Dispatch();

            var playerModel = injectionBinder.GetInstance<IPlayer>();
            playerModel.Name = "Player1";
            playerModel.Health = 100; // todo: to config

            var waypointsModel = injectionBinder.GetInstance<IWaypointList>();
            var waypointViews = Object.FindObjectsOfType<WaypointView>();
            var count = 0;
            foreach (var wayPointView in waypointViews.OrderBy(x => x.name))
            {
                var wayPoint = injectionBinder.GetInstance<IWaypoint>();
                wayPoint.Sequence = count++;
                wayPoint.Name = wayPointView.name;

                foreach (var enemyView in wayPointView.GetComponentsInChildren<EnemyView>())
                {
                    var enemy = injectionBinder.GetInstance<IEnemy>();
                    enemy.DamageAmount = 10;
                    enemy.Health = 1; // todo: to config
                    enemy.Name = enemyView.name;
                    enemy.Waypoint = wayPoint;

                    wayPoint.Enemies.Add(enemy);
                }
                waypointsModel.Add(wayPoint);
            }

            return this;
        }

        protected override void mapBindings()
        {
            base.mapBindings();

            // These signals that aren't bound to a command are typically listened to by mediators
            injectionBinder.Bind<PlayerHealthChanged>().ToSingleton();
            injectionBinder.Bind<PlayerHolstered>().ToSingleton();
            injectionBinder.Bind<EnemyTargetedChanged>().ToSingleton();
            injectionBinder.Bind<WaypointEngaged>().ToSingleton();
            injectionBinder.Bind<WaypointChanged>().ToSingleton();
            injectionBinder.Bind<WordsHighCountReached>().ToSingleton(); // may need to bind to a command later
            injectionBinder.Bind<EnemyHealthChanged>().ToSingleton();
            injectionBinder.Bind<EnemyWaiting>().ToSingleton();
            injectionBinder.Bind<EnemyWaited>().ToSingleton();
            injectionBinder.Bind<EnemyDying>().ToSingleton();
            injectionBinder.Bind<EnemyDied>().ToSingleton();
            injectionBinder.Bind<EnemyPersuing>().ToSingleton();
            injectionBinder.Bind<EnemyPersued>().ToSingleton();
            injectionBinder.Bind<EnemyAttacking>().ToSingleton();

            // Only one of some model objects
            injectionBinder.Bind<IPlayer>().To<Player>().ToSingleton();
            injectionBinder.Bind<IWaypointList>().To<WaypointList>().ToSingleton();
            injectionBinder.Bind<IWordList>().To<WordList>().ToSingleton();

            // These require construction at some place in code. 
            // Strange likes service locator pattern in these cases.
            // Maybe we could find a better way...
            injectionBinder.Bind<IEnemyList>().To<EnemyList>();
            injectionBinder.Bind<IWaypoint>().To<Waypoint>();
            injectionBinder.Bind<IEnemy>().To<Enemy>();
            injectionBinder.Bind<IWordsSource>().To<FileWordSource>();

            // What logic should happen in response to signals coming from views / mediators?
            // Command names are chosen from perspective of player (i.e. ReceiveDamage is player receiving damage from something else)
            commandBinder.Bind<GameStarted>().To<InitializeGame>().Once();
            commandBinder.Bind<EnemyAttacked>().To<ReceiveDamage>();
            commandBinder.Bind<PlayerAttacked>().To<InflictDamage>();
            commandBinder.Bind<PlayerAimed>().To<TargetEnemy>();
            commandBinder.Bind<PlayerArrivedAtWaypoint>().To<ChangeWaypoint>();
            commandBinder.Bind<WordsLowCountReached>().To<ReplenishWords>();

            // Mediators try and abstract away the chaos of Views (GameObjects)
            // I think of it as the Unity equivalent of shadow-dom/virtual-dom from web dev
            mediationBinder.Bind<EnemyView>().To<EnemyMediator>();
            mediationBinder.Bind<PlayerHealthView>().To<PlayerHealthMediator>();
            mediationBinder.Bind<BloodView>().To<BloodMediator>();
            mediationBinder.Bind<ReticleView>().To<ReticleMediator>();
            mediationBinder.Bind<WaypointView>().To<WaypointMediator>();
            mediationBinder.Bind<CameraView>().To<CameraMediator>();
        }
    }
}