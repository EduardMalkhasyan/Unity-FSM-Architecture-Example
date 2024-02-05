# Unity Architecture Example

This example employs key plugins and design patterns to establish a robust architecture:

## Core Plugins:

1. **Zenject:** Manages dependency injection and facilitates link passing through its DI containers. [Link](https://github.com/Mathijs-Bakker/Extenject)
2. **Addressables:** Empowers a versatile content loading system, supporting both remote and local resources. [Link](https://docs.unity3d.com/Packages/com.unity.addressables@0.8/manual/AddressableAssetsGettingStarted.html)
3. **UniTask:** Enhances asynchronous method handling, promoting efficient task execution. [Link](https://github.com/Cysharp/UniTask)

### Helper Plugins:

1. **UniRx:** Streamlines event handling for improved code structure. [Link](https://github.com/neuecc/UniRx)
2. **DOTween:** Drives smooth animations through a powerful tweening engine. [Link](https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676)
3. **Cinemachine:** Provides a flexible and adaptive camera system. [Link](https://unity.com/unity/features/editor/art-and-design/cinemachine)
4. **Odin Inspector:** Unity extension for improved Editor UI. [Link](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041)
4. **Newtonsoft Json:** high-performance JSON framework for .NET. [Link](https://github.com/applejag/Newtonsoft.Json-for-Unity)

## Design Patterns:

1. **Core System (State Machine):** Organizes the core architecture using a State Machine for clarity and control.
2. **Helper (Object Pool):** Implements an Object Pool pattern to efficiently manage and reuse objects [Git Page](https://github.com/EduardMalkhasyan/Unity-Architecture-Example/blob/main/Example%20Scripts/Tools/ObjectPool.cs).

## Realization Examples:

State Machine realization in project, where it can be used as core game states where state will use some game logic and UI space his inside, also can be realized as in game components like - AI state machine 

1. Initial Machine
```csharp
public interface IGameState
    {
        void Enter();
        void Exit();
        void Tick();
    }

  public class StateMachine
    {
        public Type CurrentStateClass => (currentState != null) ? currentState.GetType() : null;
        private IGameState currentState;

        public void Tick()
        {
            currentState?.Tick();
        }

        public void SetState(IGameState state)
        {
            currentState?.Exit();
            currentState = state;
            currentState.Enter();
        }
    }
```

2. Initalization
```csharp
 public class SceneContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
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

 public class FindAssemblyTypes
    {
        public static IEnumerable<Type> FindDerivedTypesFromAssembly(Assembly assembly, Type baseType, bool classOnly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly", "Assembly must be defined");

            if (baseType == null)
                throw new ArgumentNullException("baseType", "Parent Type must be defined");

            var types = assembly.GetTypes();

            foreach (var type in types)
            {
                if (classOnly && !type.IsClass)
                    continue;

                if (baseType.IsInterface)
                {
                    var it = type.GetInterface(baseType.FullName);

                    if (it != null)
                        yield return type;
                }
                else if (type.IsSubclassOf(baseType))
                {
                    yield return type;
                }
            }
        }
    }
```

3. Core Machine
```csharp
 public abstract class MainAbstractGameState : IGameState
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Tick() { }
    }

  public class MainGameStates : MonoBehaviour
    {
        [Inject] private DiContainer container;
        private StateMachine stateMachine;
        public System.Type CurrentStateType => stateMachine.CurrentStateClass;

        private void Awake()
        {
            stateMachine = new StateMachine();
        }

        private void FixedUpdate()
        {
            stateMachine.Tick();
        }

        public void EnterState<T>() where T : MainAbstractGameState
        {
            if (stateMachine.CurrentStateClass == typeof(T)) return;
            DebugColor.LogBlue($"Entering state: {typeof(T)}");
            var state = container.Resolve<T>();
            stateMachine.SetState(state);
        }
    }

// Example realization
   public class PlayGameState : MainAbstractGameState
    {
        [Inject] private UIScreensController screensController;
        [Inject] private VirtualCamera virtualCamera;
        [Inject] private MainGameStates mainGameStates;
        [Inject] private TutorialState tutorialState;
        [Inject] private Kitchen kitchen;

        public override void Enter()
        {
            screensController.ShowInstantUIScreen(UIScreenEnum.Gameplay);
            virtualCamera.SwitchCamera(VirtualCameraType.Close);

            kitchen.ActivateKitchen();
            TutorialObserver.OnTutorial += EnterTutorialDemonstration;
        }

        public override void Exit()
        {
            TutorialObserver.OnTutorial -= EnterTutorialDemonstration;
        }

        private void EnterTutorialDemonstration(TutorialEnum tutorialEnum)
        {
            var preset = TutorialProps.Value.GetTutorialPresetKVP(tutorialEnum);
            tutorialState.Setup(preset.tutorialObject, tutorialEnum);
            mainGameStates.EnterState<TutorialState>();
        }
    }
```
4. AI Machine
```csharp
 public abstract class AIAbstractGameState : IGameState
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Tick() { }
    }

 [InfoBox("This component is bound 'FromNewComponentOnNewGameObject' and 'AsTransient' for multiple uses in AI space")]
    public class AIGameStates : MonoBehaviour
    {
        [Inject] private DiContainer container;
        private StateMachine stateMachine;
        public System.Type CurrentStateType => stateMachine.CurrentStateClass;

        private void Awake()
        {
            stateMachine = new StateMachine();
        }

        private void FixedUpdate()
        {
            stateMachine.Tick();
        }

        public void EnterState<T>(bool canShowDebug = true) where T : AIAbstractGameState
        {
            if (stateMachine.CurrentStateClass == typeof(T)) return;

            if (canShowDebug)
            {
                DebugColor.LogBlue($"AI entering state: {typeof(T)}", bold: true);
            }

            var state = container.Resolve<T>();
            stateMachine.SetState(state);
        }
    }

// Example realization
 public class AIWorkerSorterGoToCookingPlaceState : AIAbstractGameState
    {
        [Inject] private AIWorkerSorter workerSorter;

        public override void Enter()
        {
            workerSorter.PlaySimpleWalkAnimation();
            workerSorter.GoToCookingPlace(OnComplete: () => { EnterPickupPlaceState(); });
        }

        public override void Exit()
        {

        }

        public async void EnterPickupPlaceState()
        {
            try
            {
                workerSorter.PlayIdleWithItemAnimation();
                await UniTask.WaitUntil(() => workerSorter.FoodStack.AvailableStackIsEmpty() == false);

                workerSorter.EnterState<AIWorkerSorterGoToPickupPlaceState>();
            }
            catch (Exception error)
            {
                Debug.LogWarning(error);
            }
        }
    }
```
### Final Result:
1. Having mutiple UI screens with no confusion
2. States can used not only for UI and some logic, but can travel also in game settings like AI realization or some other
3. Can be easly refactored, no Monobehaviur addiction, No Coroutine addiction
4. Optimazed for low mobile devices (Until 2GB Ram 2016 year release mobile - with 50-60 fps benchmark and 60+ minutes gameplay with no warming up phone and droping fps) because of Addressables, Low use of Update thanks to UniTask, and no scene reload system thanks to Zenject and SelfReset Interface and more. 

Games with this archictecture can be find in this [Link](https://bugigames.com/)

<img width="758" alt="Screenshot_1" src="https://github.com/EduardMalkhasyan/Unity-Architecture-Example/assets/78969017/101d3895-2d6e-4ec1-a560-6919f14962c1">

