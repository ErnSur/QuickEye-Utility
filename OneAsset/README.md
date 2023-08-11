# One-Asset
> Extended singleton solution

A set of classes and editor UI improvements aimed to improve workflows that expect single instances of assets.

**Package contains**:
- Abstract class that can be inherited to get a singleton behaviour
  - `SingletonMonoBehaviour<T>`
  - `SingletonScriptableObject<T>`
- Customize a singleton implementation with options like:
  - Load instance from a prefab or `ScriptableObject` asset
  - Create an asset automatically if it doesn't exist already
- Extended editor UI for singleton assets:
  - Custom icons and tooltips in the project browser 
  - Singleton Asset path field in the inspector header
<img  src="../Documentation~/SingletonUI.png" align="center" width="70%">


## `SingletonMonoBehaviour<T>`

`MonoBehaviour` Singleton implementation.
Takes into account some common problems of many singleton implementations that are out there.

Options:
- Load instance from a prefab asset by adding a `SingletonAssetAttribute` attribute

Example:
```c#
[SingletonAsset("Popup View")]
public class PopupView : SingletonMonoBehaviour<PopupView> { }
void UseExample()
{
    // Calling `PopupView.Instance` will load a prefab from */Resources/Popup View.prefab
    var obj = PopupView.Instance;
}
```


## `SingletonScriptableObject<T>`

`ScriptableObject` Singleton implementation.

Options:
- Automatically create scriptable object asset (if it doesn't exists already) when used with `SingletonAssetAttribute` and `CreateAssetAutomatically`
- Create a [SettingsProvider](https://docs.unity3d.com/ScriptReference/SettingsProvider.html) just by adding `SettingsProviderAttribute` and `SingletonAssetAttribute`


Example:
```c#
    // SingletonAssetAttribute will make the `SuperSdkSettings.Instance` load the scriptable object from
    // resources path: "*Resources/Super SDK Settings"
    // `Mandatory = true` option will make sure to show error messages if the asset is missing at this path.
    [SingletonAsset("Super SDK Settings", Mandatory = true)]
    // `CreateAssetAutomatically` Attribute turns on a system that will create scriptable object file at specific path
    // if it cannot be loaded from path specified in `SingletonAsset` attribute
    // in this example it will create asset with path: "Assets/Settings/Super SDK Settings"
    [CreateAssetAutomatically("Assets/Settings/")]
    // The `SettingsProviderAsset` will create a new UI settings tab with name "Super SDK" in the Project Settings window
    // where users can edit this asset
    [SettingsProviderAsset("Project/Super SDK")]
    public class SuperSdkSettings : SingletonScriptableObject<SuperSdkSettings>
    {
        public string AppKey;
    }
```

## Samples
Import the "Usage of singleton classes" from [Package Manager Window](https://docs.unity3d.com/Manual/upm-ui-details.html)

## Embedding this code in a package
To avoid collisions with other packages with this code:
- Make sure you delete the .meta files- use different guids for the scripts.
- Encapsulate it with a assembly definition files or change the namespaces
- Update absolute paths to icons in _OneAsset/Editor/UI/SingletonGUI.cs_