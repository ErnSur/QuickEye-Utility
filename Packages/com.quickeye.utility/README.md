<img src="Documentation~/eyeLogo.png" align="right" width="11%" />

# QuickEye Utility

### `UnityDictionary` Serialized and editable

New type and property drawer to serialize and edit dictionaries in inspector.

![](Documentation~/SerializedDictionary.jpg)

### Time Serialization Types

Data types, GUI Controls and Property Drawers related to time.

![](Documentation~/TimeInspector.png)

#### `UnityTimeSpan`

`System.TimeSpan` equivalent. If `TimeOfDayAttribute` is used on serialized field the property drawer will restrain input between `0` and `23:59:59.999`. 

####  `UnityDateTime` & `UnityDateOnly`

`System.DateTime` and .NET 6s `System.DateOnly` equivalents.

## One-Asset

One Asset became a standalone package that can be found at [ErnSur/One-Asset](https://github.com/ErnSur/One-Asset).

## Editor

### Custom Editor Window Title Settings
> 1. Go to: Edit > Preferences (Windows) or Unity > Preferences (macOS)
> 2. Select **Window Title** from sidebar

Modify Unity Editor main window title to include information relevant for you!

<img src="Documentation~/CustomWindowTitle.jpeg" width="100%">

### Duplicate Window Action
> - Open from menu bar: _Window/Duplicate Window_
> - Shortcut: Shift+Ctrl+D (Windows) or Shift+Cmd+D (macOS)

Handy tool when you need to open another inspector to compare or drag and drop data.
It will duplicate focused editor window, lock it (in case of inspector or project browser) and position it directly next to the original one.

<img src="Documentation~/DuplicateWindow.gif" width="100%">

### `EditorColorPalette` and Editor Color Palette Window
> - Open from menu bar: _Window/Editor Color Palette_
> - Use `QuickEye.Utility.Editor.EditorColorPalette` class to access colors from code.

Browser for color values used by Unity Editor. 

<img  src="Documentation~/EditorColorPalette.png" width="50%">

## Other

### `Container` and `PoolContainer`

A serializable object that holds a prefab and a `Transform` reference, implements `IList<T>`.
`AddNew` method creates a new instance of prefab inside the transform. Because of `IList<T>`, it functions as a
collection with additional behavior of easy object instantiation.
`PoolContainer` has the same interface but uses object pooling. Cuts much of the boilerplate code with UI scripting.

### `GameObjectPool`

Object pool pattern implementation with Unity specific API.

* Serializable, can be configured from editor.
* Prototype as prefab.
* Configurable transform parent.