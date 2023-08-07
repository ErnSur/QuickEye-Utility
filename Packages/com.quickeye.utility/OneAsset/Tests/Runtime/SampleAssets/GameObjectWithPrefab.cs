namespace OneAsset.Tests.SampleAssets
{
    [LoadFromAsset(ResourcesPath)]
    internal class GameObjectWithPrefab : SingletonMonoBehaviour<GameObjectWithPrefab>
    {
        public const string ResourcesPath = "one-asset-tests/GameObjectWithPrefab";
    }
}