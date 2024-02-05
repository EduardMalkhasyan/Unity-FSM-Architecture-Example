# Unity Architecture Example

This project employs key plugins and design patterns to establish a robust architecture:

## Plugins:

1. **Zenject:** Manages dependency injection and facilitates link passing through its DI containers.
2. **Addressables:** Empowers a versatile content loading system, supporting both remote and local resources.
3. **UniTask:** Enhances asynchronous method handling, promoting efficient task execution.

### Extra Helper Plugins:

1. **UniRx:** Streamlines event handling for improved code structure.
2. **DOTween:** Drives smooth animations through a powerful tweening engine.
3. **Cinemachine:** Provides a flexible and adaptive camera system.

## Design Patterns:

1. **Core System (State Machine):** Organizes the core architecture using a State Machine for clarity and control.
2. **Helper (Object Pool):** Implements an Object Pool pattern to efficiently manage and reuse objects.

```csharp
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
        }```
