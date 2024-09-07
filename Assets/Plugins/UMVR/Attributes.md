# Code generation

Creating `IModel` descendant interface allows generation of `Model`. E.x., `interface IFoo` will generate `FooModel`. All properties will be generated according to following rules:

1. If property is a primitive type, it will be generated as a `SingleProperty`. If it's a `IModel` type, it will be generated as a `SingleModelProperty`. Usage of other reference types is strongly discouraged.
2. If property is an `IList<non-IModel>`, it will be generated as a `CollectionProperty`. If it's an `IList<IModel>`, it will be generated as a `CollectionModelProperty`. Usage of other generic types is not supported.
3. Property having a `set` or not provides basic control over the property being read-only or read-write. Collections are always read-write. Note, that having a read-only property causes it to not generate a `Property` data stream. 
    > Note, that even read-only properties will *always* have a `private set` accessor. This is required for the model to be able to set the property value during deserialization. 
4. You can fine-tune automatic generation using a set of attributes.

## `[Initialization]`, `[CustomImplementation]`

These attributes can be applied to a property to provide custom initialization or implementation. They are not required, but can be useful in some cases:
* `[Initialization(InitializationLevel.Explicit)]` will force adding initial value to the constructor. This is a default value for get-only properties.
* `[Initialization(InitializationLevel.Skip)]` will skip adding the property to the constructor. This sometimes useful in a tandem with a get-only `[CustomImplementation]` property. Trying to add a `[Initialization(InitializationLevel.Skip)]` to a property without either a `set` accessor or `CascadeDispose` will emit a warning and be ignored.
* `[CustomImplementation]` will cause generator to not generate property implementation in the auto-generated model. It will instead be generated in a separate partial class, defaulting to auto-property. Initialization attributes will still be respected.

### Generation output cheat-sheet - single, no custom implementation

| Type     | `set` | `[Initialization]`    | Underlying field                | Constructor parameter        |
|--------  |------ |-----------------------|---------------------------------|------------------------------|
| `int`    | [x]   | `Default`, `Skip`     | `SingleProperty<int>`           | No                           |
| `int`    | [x]   | `Explicit`            | `SingleProperty<int>`           | Yes                          |
| `int`    | [ ]   | `Default`, `Explicit` | `int`                           | Yes                          |
| `int`    | [ ]   | `Skip`                | `int`                           | Yes - ignored with a warning |
| `IModel` | [x]   | `Default` , `Skip`    | `SingleModelProperty<IModel>`   | No                           |
| `IModel` | [x]   | `Explicit`            | `SingleModelProperty<IModel>`   | Yes                          |
| `IModel` | [ ]   | `Default`, `Explicit` | `Model`                         | Yes                          |
| `IModel` | [ ]   | `Skip`                | `Model`                         | Yes - ignored with a warning |

### Generation output cheat-sheet - collection, no custom implementation

| Type            | `[Initialization]`    | Underlying field                  | Constructor parameter |
|-----------------|-----------------------|-----------------------------------|-----------------------|
| `IList<int>`    | `Default`, `Skip`     | `CollectionProperty<int>`         | No                    |
| `IList<int>`    | `Explicit`            | `CollectionProperty<int>`         | Yes                   |
| `IList<IModel>` | `Default`, `Skip`     | `ModelCollectionProperty<IModel>` | No                    |
| `IList<IModel>` | `Explicit`            | `ModelCollectionProperty<IModel>` | Yes                   |
