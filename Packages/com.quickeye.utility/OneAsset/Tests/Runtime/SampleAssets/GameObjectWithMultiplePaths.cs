namespace OneAsset.Tests.SampleAssets
{
    [LoadFromAsset(FirstResourcesPathNotValid, Priority = 2)]
    [LoadFromAsset("Resources/"+SecondaryResourcesPath, Priority = 1)]
    internal class GameObjectWithMultiplePaths : OneGameObject<GameObjectWithMultiplePaths>
    {
        public const string FirstResourcesPathNotValid = "missing-path";
        public const string SecondaryResourcesPath = "one-asset-tests/GameObjectWithMultiplePaths";
    }
}