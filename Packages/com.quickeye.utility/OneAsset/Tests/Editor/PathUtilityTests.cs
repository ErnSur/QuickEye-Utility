using NUnit.Framework;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(PathUtility))]
    public class PathUtilityTests
    {
        [TestCase("Assets/Resources/folder-x/some.asset", "Resources", ExpectedResult = "folder-x/some.asset")]
        public string Should_GetPathRelativeTo(string fullPath, string relativeFolder)
        {
            return PathUtility.GetPathRelativeTo(relativeFolder, fullPath);
        }
    }
}