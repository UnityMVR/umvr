# Unity Model-View-Reactor

Tl;dr Unity architectural framework built on [Zenject](https://github.com/Mathijs-Bakker/Extenject) and [UniRx](https://github.com/neuecc/UniRx) with the purpose of speeding up development while encouraging well laid out architecture. It heavily relies on code generation. You could compare it to Ruby on Rails, in that it prefers convention over configuration and wants you to automate the repetitive stuff (but then, my experience with RoR is _very_ limited)

## Reactor
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

There's also experimental implementation of automatic view binding, for the time being though (20/02/24), I discourage using it yet; it's still subject to change.

## Repository
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

## Conventions and assumptions

1. Zenject usage is assumed and made easier by automatic generation of installers
2. `Views` are written manually, Zenject will deliver them to `Reactors` automatically
3. Rest of the stuff is generated: in Unity, `[Tools]->[UMVR]->[Generator]`
4. You **should not** modify `Model` in `Reactors` to avoid circular dependencies. Write own `Controllers` to drive the logic and modify `Model`.
5. `Views` should never touch `Model` directly - there's `ICommand` interface for that.
6. Since UniRx uses `IDisposable` for subscription cleanup, it trickles down to UMVR as well. Most types are `IDisposable` so you can dispose your subscriptions easily. Bind to `IDisposable` in Zenject to automate that.

## Credits

Made possible, aside from [Zenject](https://github.com/Mathijs-Bakker/Extenject) and [UniRx](https://github.com/neuecc/UniRx), by [Mono.T4](https://github.com/mono/t4).
