using System;
using System.Text;

namespace OneAsset
{
    /// <summary>
    /// Applies loading rules for <see cref="OneAssetLoader"/>
    /// Can be used on <see cref="UnityEngine.ScriptableObject"/> and <see cref="UnityEngine.MonoBehaviour"/>
    /// Use multiple <see cref="LoadFromAssetAttribute"/> to look for the asset in multiple different paths.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class LoadFromAssetAttribute : Attribute
    {
        /// <summary>
        /// If Path starts with "Resources/" it will be loaded from resources and be available in runtime
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Enables a system that will create scriptable object file if it cannot be loaded from <see cref="LoadFromAssetAttribute.Path"/>
        /// </summary>        
        public bool CreateAssetIfMissing { get; set; }

        /// <summary>
        /// <para>If set to true a <see cref="AssetIsMissingException"/> will be thrown when <see cref="OneAssetLoader"/> will not find asset at any of the paths.</para>
        /// <para>By default: true</para>
        /// </summary>
        public bool AssetIsMandatory { get; set; } = true;

        /// <summary>
        /// Relevant for types with multiple <see cref="LoadFromAssetAttribute"/>.
        /// Optional field to specify the order in which asset is searched for. Paths with higher priority are searched first
        /// </summary>
        public int Priority { get; set; } = 1;

        /// <summary>
        /// In Editor, use the <see cref="UnityEditorInternal.InternalEditorUtility.LoadSerializedFileAndForget"/> as a fallback load option. Use with caution!
        /// </summary>
        public bool LoadAndForget { get; set; }

        /// <summary>
        /// Defines a path at which asset can be found for <see cref="OneAssetLoader"/> and <see cref="OneGameObject{T}"/>.
        /// Valid on types derived from <see cref="UnityEngine.ScriptableObject"/> or <see cref="OneGameObject{T}"/>
        /// </summary>
        /// <param name="path">
        /// Path at which asset should be found. Should be relative to unity project directory and contain file extensions.
        /// Under certain conditions path can be less specific.
        /// <para>If path is absolute and contains a file extension, it will work with all of the options.</para>
        /// <para>If <see cref="CreateAssetIfMissing"/> is enabled, the path must be absolute</para>
        /// <para>If path </para>
        /// Doesn't have to contain file name if <see cref="UseTypeNameAsFileName"/> is set to true.
        /// </param>
        public LoadFromAssetAttribute(string path)
        {
            Path = path;
        }

        internal static string NicifyClassName(string input)
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
    }
}