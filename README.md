# Unity Architecture Example

This project employs key plugins and design patterns to establish a robust architecture:

## Core Plugins:

1. **Zenject:** Manages dependency injection and facilitates link passing through its DI containers. [Link](https://github.com/Mathijs-Bakker/Extenject)
2. **Addressables:** Empowers a versatile content loading system, supporting both remote and local resources.
3. **UniTask:** Enhances asynchronous method handling, promoting efficient task execution.

### Helper Plugins:

1. **UniRx:** Streamlines event handling for improved code structure.
2. **DOTween:** Drives smooth animations through a powerful tweening engine.
3. **Cinemachine:** Provides a flexible and adaptive camera system.
4. **Odin Inspector:** Unity extension for improved Editor UI

## Design Patterns:

1. **Core System (State Machine):** Organizes the core architecture using a State Machine for clarity and control.
2. **Helper (Object Pool):** Implements an Object Pool pattern to efficiently manage and reuse objects.

```csharp
```
