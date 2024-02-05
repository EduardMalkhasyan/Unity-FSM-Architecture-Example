# Unity Architecture Example

This project employs key plugins and design patterns to establish a robust architecture:

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
2. **Helper (Object Pool):** Implements an Object Pool pattern to efficiently manage and reuse objects.

## Realization Examples:

State Machine realization in project, where it can be used as core game states where state will bind some game logic and UI space his inside, also can be realized as in game components like - AI state machine 

1.
```csharp
public interface IGameState
    {
        void Enter();
        void Exit();
        void Tick();
    }
```
2.
```csharp
 public abstract class MainAbstractGameState : IGameState
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Tick() { }
    }
```

