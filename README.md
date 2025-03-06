# MVP Framework
MVP Framework for Unity

### How to use

![MVPFramework drawio](https://github.com/Nebulate-me/WindowsSystem/assets/6073507/d34276c0-1818-4ee9-89fa-5c3049951d7b)

- Inherit `PresenterBase` to create a presenter of your screen.
- Inherit `ScreenView` to create a view of your screen. Views are regular MonoBehaviours.
- Inherit `ModelBase` to create a Model of your screen. Model stores and modifies data of your screen.
- Widgets are just sub-presenters. They don't have their own Models. There is only `Widget` and `WidgetView`. Widgets are convinient to use re-use.
- `CompositeWidget` is useful when you want to use Widget as a container for other Widgets.
- Use `ReactiveProperty<T>` and `ReactiveTrigger` in your Model to update View when data changed.
- There is an example of implemented screen with controls in Examples folder.

### How to install

In Package Manager click `+` -> `Add package from git URL...` and paste `https://github.com/kutase/MVPFramework.git`
