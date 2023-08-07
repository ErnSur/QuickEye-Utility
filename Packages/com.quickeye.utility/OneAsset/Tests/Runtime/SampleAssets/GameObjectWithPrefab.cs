namespace OneAsset.Tests.SampleAssets
{
    [LoadFromAsset(ResourcesPath)]
    internal class GameObjectWithPrefab : OneGameObject<GameObjectWithPrefab>
    {
        public const string ResourcesPath = "one-asset-tests/GameObjectWithPrefab";
    }
}