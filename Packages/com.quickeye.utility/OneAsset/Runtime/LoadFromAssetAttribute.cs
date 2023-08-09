using System;
using System.Text;

namespace OneAsset
{
    /// <summary>
    /// Applies loading rules to <see cref="OneAssetLoader"/> and <see cref="OneGameObject{T}"/>.
    /// Can be used on <see cref="UnityEngine.ScriptableObject"/> and <see cref="UnityEngine.MonoBehaviour"/>
    /// Use multiple <see cref="LoadFromAssetAttribute"/> to look for the asset in multiple different paths.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class LoadFromAssetAttribute : Attribute
    {
        public string Path { get; internal set; }

        /// <summary>
        /// <para>If set to true a <see cref="AssetIsMissingException"/> will be thrown when trying to load missing asset.</para>
        /// <para>By default: true</para>
        /// </summary>
        public bool Mandatory { get; set; } = true;

        /// <summary>
        /// When enabled, the path file name will be based on type name  
        /// </summary>
        public bool UseTypeNameAsFileName { get; set; }

        /// <summary>
        /// Relevant for types with multiple <see cref="LoadFromAssetAttribute"/>.
        /// Optional field to specify the order in which asset is searched for. Paths with higher priority are searched first
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// 
        /// </summary>
        public bool UnsafeLoad { get; set; }

        /// <summary>
        /// Defines a path at which asset can be found for <see cref="OneAssetLoader"/> and <see cref="OneGameObject{T}"/>.
        /// Valid on types derived from <see cref="UnityEngine.ScriptableObject"/> or <see cref="OneGameObject{T}"/>
        /// </summary>
        /// <param name="path">
        /// Path at which asset should be found. Relative to the Resources folder.
        /// Doesn't have to contain file name if <see cref="UseTypeNameAsFileName"/> is set to true.
        /// </param>
        // TODO: What if I would put additional "absolutePath" here?
        // change argument to "path" it can lead to resources but it doesnt have to
        public LoadFromAssetAttribute(string path)
        {
            Path = PathUtility.EnsurePathStartsWith("Assets",path);
            if (!path.EndsWith(".asset"))
                Path = $"{Path}.asset";
        }

        // internal string TryGetResourcesPath()
        // {
        //     if (!PathUtility.ContainsFolder("Resources", Path))
        //         return false;
        //     return UseTypeNameAsFileName
        //         ? $"{Path}/{NicifyClassName(owner.Name)}".TrimStart('/')
        //         : Path;
        // }

        private static string TrimPath(string path, string startDir)
        {
            path = path.TrimStart('/');
            startDir = startDir.TrimStart('/');
            if (path.StartsWith(startDir))
            {
                path = path.Substring(startDir.Length).TrimStart('/');
            }

            return path;
        }

        public static string NicifyClassName(string input)
        {
            var result = new StringBuilder(input.Length * 2);

            var prevIsLetter = false;
            var prevIsLetterUpper = false;
            var prevIsDigit = false;
            var prevIsStartOfWord = false;
            var prevIsNumberWord = false;

            var firstCharIndex = 0;
            if (input.StartsWith("_"))
                firstCharIndex = 1;
            else if (input.StartsWith("m_"))
                firstCharIndex = 2;

            for (var i = input.Length - 1; i >= firstCharIndex; i--)
            {
                var currentChar = input[i];
                var currIsLetter = char.IsLetter(currentChar);
                if (i == firstCharIndex && currIsLetter)
                    currentChar = char.ToUpper(currentChar);
                var currIsLetterUpper = char.IsUpper(currentChar);
                var currIsDigit = char.IsDigit(currentChar);
                var currIsSpacer = currentChar == ' ' || currentChar == '_';

                var addSpace = (currIsLetter && !currIsLetterUpper && prevIsLetterUpper) ||
                               (currIsLetter && prevIsLetterUpper && prevIsStartOfWord) ||
                               (currIsDigit && prevIsStartOfWord) ||
                               (!currIsDigit && prevIsNumberWord) ||
                               (currIsLetter && !currIsLetterUpper && prevIsDigit);

                if (!currIsSpacer && addSpace)
                {
                    result.Insert(0, ' ');
                }

                result.Insert(0, currentChar);
                prevIsStartOfWord = currIsLetter && currIsLetterUpper && prevIsLetter && !prevIsLetterUpper;
                prevIsNumberWord = currIsDigit && prevIsLetter && !prevIsLetterUpper;
                prevIsLetterUpper = currIsLetter && currIsLetterUpper;
                prevIsLetter = currIsLetter;
                prevIsDigit = currIsDigit;
            }

            return result.ToString();
        }

        public string TryGetResourcesPath()
        {
            var path= PathUtility.GetPathRelativeTo("Resources",Path);
            path = PathUtility.GetPathWithoutExtension(path);
            return path;
        }
    }
}