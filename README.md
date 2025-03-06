# MVC Framework
MVC Framework for Unity

### How to use

![MVCFramework drawio](https://github.com/Nebulate-me/WindowsSystem/assets/6073507/d34276c0-1818-4ee9-89fa-5c3049951d7b)

- Inherit `ControllerBase` to create controller of your screen.
- Inherit `ScreenView` to create view of your screen. Views are regular MonoBehaviours.
- Inherit `ViewStoreBase` to create ViewStore of your screen. ViewStore is a store for a Model. You should modify model only from a ViewStore.
- Controls are just sub-controllers. They don't have their own ViewStore. There is only `Control` and `ControlView`. Use control as a reusable widget.
- `CompositeControl` is useful when you want to use Control as a container for other Controls.
- Use `ReactiveProperty<T>` and `ReactiveTrigger` in your Model to update View when data changed.
- There is an example of implemented screen with controls in Examples folder.

### How to install

In Package Manager click `+` -> `Add package from git URL...` and paste `https://github.com/Nebulate-me/MVCFramework.git`
