using BugiGames.Ads;
using BugiGames.AI.State;
using BugiGames.Analytics;
using BugiGames.IAP;
using BugiGames.Main;
using BugiGames.StateMachine;
using BugiGames.Tools;
using Sirenix.Utilities;
using System.Reflection;
using Zenject;

namespace BugiGames.Installer
{
    public class SceneContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GameReset>().AsSingle();
            Container.Bind<FoodStackSorter>().AsTransient();
            Container.Bind<AIGameStates>().FromNewComponentOnNewGameObject().AsTransient();
            InstallGameStates<MainAbstractGameState>();
            InstallGameStates<AIAbstractGameState>();
        }

        public void InstallGameStates<T>()
        {
            var assembly = Assembly.GetAssembly(typeof(T));

            FindAssemblyTypes.FindDerivedTypesFromAssembly(assembly, typeof(T), true).ForEach(
                 (type) =>
                 {
                     if (type.IsAbstract == false)
                     {
                         Container.UnbindInterfacesTo(type);
                         Container.BindInterfacesAndSelfTo(type).AsSingle();
                     }
                 });
        }
    }
}

