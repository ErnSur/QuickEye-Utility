# TODO

- Add event logs
- add event invoke counter
- add preview for runtime subscription list
  - remove `UnityEditor.Events.UnityEventTools.AddPersistentListener`
  - https://github.com/mob-sakai/UnityEventDrawerEx

Use EditorPreview to:
- show invoke count
- objects subscribed
- event logs (time : invoker : payload)

Investigate:
  - assetdatabase paths are case insensitive (Windows)
    - SingletonGUI does not know about this
  - adding "/" at the end of SingletonAsset and CreateAssetAutomatically breaks system