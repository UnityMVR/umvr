# Unity Model-View-Reactor

Tl;dr Unity architectural framework built on [Zenject](https://github.com/Mathijs-Bakker/Extenject) and [UniRx](https://github.com/neuecc/UniRx) with the purpose of speeding up development while encouraging well laid out architecture. It heavily relies on code generation. You could compare it to Ruby on Rails, in that it prefers convention over configuration and wants you to automate the repetitive stuff (but then, my experience with RoR is _very_ limited)

## The 3 Rs
The `R` in UMVR comes from 3 pillars: Reactors, Repositories and Rails.

### Reactor
`Reactor` is just the name for MVPs `Presenter` with all the data exposed in form of UniRx data streams. In its current form, one presenter gets created for every model and you bind data manually, more or less like this:

```
using UniRx;
using UnityEngine;

public partial class FooReactor
{
	protected override void BindDataSourceImpl(FooModel model)
	{
		Subscriptions.Add(model.GetProperty<string>(nameof(IFoo.Text)).Subscribe(Debug.Log));
	}
}
```

### Repository
This is the second meaning of R in UMVR - `Repository` lays out the models in a predictable way. Think of it like of the way to make your model a little more like a database, with each Repository being a table.
You can get by primary key:
```
Id totallyValidId = default;
repository.Get(totallyValidId);
```

You can register secondary key and get by it:
```
public FooRepository(FooReactorFactory fooReactorFactory, FooRepository repository) : base(fooReactorFactory)
{
	AddIndex(nameof(IFoo.Text), new SecondaryIndex<string,IFoo>(nameof(IFoo.Text), repository));
}
...
repository.GetBy(nameof(IFoo.Text), "Hello world");
```

And then there's the obvious stuff: `Added/Removed` events, iterators.

### Rails
So, the inspiration with Ruby on Rails is a thing here. What UMVR tries to provide is a way to focus on design of your game/app and NOT on wiring it up together. You design the data layout and implement the game logic, UMVR puts it all together for you. The standard use case would be:
1. Write your interfaces, provide `IModel` as a base. You can use a set of attributes to steer the behavior of generated code.
2. `[Tools]->[UMVR]->[Generator]` -> Generate essentials.
3. You now have a set of tools to be used with your model (lets call it `IFoo`)
	- `FooModel` + `FooFactory` - implementation of provided interface, packed with UniRx `IObservable` properties.
	- `FooReactor` - every `FooModel` causes one to be created automatically and is passed as a parameter to it's `Reactor`. Use it to _react_ to stuff that happens to your model. The most obvious use would be feeding changes to the view. Note, that since the `Model` properties are observable, you can perform any stream shenanigans you want.
	- `FooRepository` - sometimes one single `Model` doesn't cut it when you want to describe something collection based. You could put an `IList<IFoo>` into another type, but that's not necessary - you can instead inject `IRepository<IFoo>` to get an database-like access to an entire model type.
	- `FooInstallerBase` - last but not least, all the essentials are already ready to be called with `FooInstallerBase.Install(Container)` in your own code.

## Conventions, assumptions and good practices

1. Zenject usage is assumed and made easier by automatic generation of installers
2. `Views` are written manually, Zenject will deliver them to `Reactors`
3. Rest of the stuff is generated: in Unity, `[Tools]->[UMVR]->[Generator]`
4. You should not, or at least be careful, modify `Model` in `Reactors` to avoid circular dependencies. It's probably better to write separate `Controllers` to drive the logic and modify `Model`.
5. `Views` don't have to touch `Model` directly - there's `ICommand` interface for that.
6. Since UniRx uses `IDisposable` for subscription cleanup, it trickles down to UMVR as well. Most types are `IDisposable` so you can dispose your subscriptions easily. Bind to `IDisposable` in Zenject to automate that.

## Credits

Made possible, aside from [Zenject](https://github.com/Mathijs-Bakker/Extenject) and [UniRx](https://github.com/neuecc/UniRx), by [Mono.T4](https://github.com/mono/t4).
