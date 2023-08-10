using System.Collections.Generic;
using System.Linq;

namespace OneAsset
{
    public class AssetLoadOptions
    {
        /// <summary>
        /// Paths from which <see cref="OneAssetLoader"/> will try to load an asset.
        /// Asset will be loaded from the first path that contains loadable asset.
        /// If path starts with "Resources/" it will be loaded from resources and be available in runtime.
        /// </summary>
        public string[] Paths { get; }

        internal AssetPath[] AssetPaths { get; private set; }

        /// <summary>
        /// <para>If set to true a <see cref="AssetIsMissingException"/> will be thrown when <see cref="OneAssetLoader"/> will not find asset at any of the paths.</para>
        /// <para>By default: true</para>
        /// </summary>
        public bool AssetIsMandatory { get; set; }

        /// <summary>
        /// In Editor, enables a system that will create scriptable object file if it cannot be loaded from <see cref="AssetLoadOptions.Paths"/>. It will always create asset at the first path from the <see cref="Paths"/> property.
        /// </summary>
        public bool CreateAssetIfMissing { get; set; }

        /// <summary>
        /// In Editor, use the <see cref="UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget"/> as a fallback load option. Use with caution!
        /// </summary>
        public bool LoadAndForget { get; set; }

        /// <param name="path">
        /// Path from which <see cref="OneAssetLoader"/> will try to load an asset.
        /// If path starts with "Resources/" it will be loaded from resources and be available in runtime.
        /// File extension is required.
        /// </param>
        public AssetLoadOptions(string path) : this(new[] { path })
        {
        }

        /// <param name="paths">
        /// Paths from which <see cref="OneAssetLoader"/> will try to load an asset.
        /// Asset will be loaded from the first path that contains loadable asset.
        /// If path starts with "Resources/" it will be loaded from resources and be available in runtime.
        /// File extension is required.
        /// </param>
        public AssetLoadOptions(IEnumerable<string> paths)
        {
            Paths = paths.Select(CleanPath).ToArray();
            AssetPaths = Paths.Select(p => new AssetPath(p)).ToArray();
        }

        // TODO: Do more cleaning to remove trailing slashes and so on. It will be best if the Path is as predictable as possible
        private static string CleanPath(string path)
        {
            return path.TrimStart('/');
        }
    }
}