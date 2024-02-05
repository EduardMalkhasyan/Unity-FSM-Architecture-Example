using BugiGames.Tools;
using Zenject;

namespace BugiGames.Installer
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AddressableLoader>().AsSingle();
        }
    }
}
