using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

// Modified version of https://github.com/karl-/unity-symlink-utility
namespace QuickEye.Utility.Editor
{
    [InitializeOnLoad]
    internal static class SymlinkUtility
    {
        private const string JunctionMenuName = "Assets/Symlink/Junction";
        private const string AbsSymlinkMenuName = "Assets/Symlink/Absolute Symlink";
        private const string RelSymlinkMenuName = "Assets/Symlink/Relative Symlink";

        // FileAttributes that match a junction folder.
        private const FileAttributes FolderSymlinkAttributes = FileAttributes.Directory | FileAttributes.ReparsePoint;

        private const string SymlinkLabelText = "⇔";

        private static GUIStyle SymlinkMarkerStyle => new GUIStyle(EditorStyles.label)
        {
            normal =
            {
                textColor = EditorColorPalette.Current.DefaultText
            },
            fontStyle = FontStyle.Bold,
            fontSize = 13
        };

        static SymlinkUtility()
        {
            EditorApplication.projectWindowItemOnGUI += OnProjectWindowItemGUI;
        }

        private static void OnProjectWindowItemGUI(string guid, Rect r)
        {
            try
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

                if (string.IsNullOrEmpty(path) || !File.Exists(path))
                    return;
                var attributes = File.GetAttributes(path);
                var nr = IMGUIUtility.CalculateRectAfterLabelText(r, path, true);
                if ((attributes & FolderSymlinkAttributes) == FolderSymlinkAttributes)
                    GUI.Label(nr, SymlinkLabelText, SymlinkMarkerStyle);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

#if UNITY_EDITOR_WIN
        [MenuItem(JunctionMenuName, false, 20)]
        private static void Junction()
        {
            Symlink(SymlinkType.Junction);
        }
#endif

        [MenuItem(AbsSymlinkMenuName, false, 21)]
        private static void SymlinkAbsolute()
        {
            Symlink(SymlinkType.Absolute);
        }

        [MenuItem(RelSymlinkMenuName, false, 22)]
        private static void SymlinkRelative()
        {
            Symlink(SymlinkType.Relative);
        }

        private static void Symlink(SymlinkType linkType)
        {
            var sourceFolderPath = EditorUtility.OpenFolderPanel("Select Folder Source", "", "");

            // Cancelled dialog
            if (string.IsNullOrEmpty(sourceFolderPath))
                return;

            if (sourceFolderPath.Contains(Application.dataPath))
            {
                Debug.LogWarning("Cannot create a symlink to folder in your project!");
                return;
            }

            var sourceFolderName = sourceFolderPath.Split(new char[] { '/', '\\' }).LastOrDefault();

            if (string.IsNullOrEmpty(sourceFolderName))
            {
                Debug.LogWarning("Couldn't deduce the folder name?");
                return;
            }

            var uobject = Selection.activeObject;

            var targetPath = uobject != null ? AssetDatabase.GetAssetPath(uobject) : null;

            if (string.IsNullOrEmpty(targetPath))
                targetPath = "Assets";

            if (targetPath.StartsWith("Packages/"))
                targetPath = "Packages";

            var attributes = File.GetAttributes(targetPath);

            if ((attributes & FileAttributes.Directory) != FileAttributes.Directory)
                targetPath = Path.GetDirectoryName(targetPath);

            // Get path to project.
            var pathToProject = Application.dataPath.Split(new[] { "/Assets" }, StringSplitOptions.None)[0];

            targetPath = $"{pathToProject}/{targetPath}/{sourceFolderName}";

            if (Directory.Exists(targetPath))
            {
                Debug.LogWarning(
                    $"A folder already exists at this location, aborting link.\n{sourceFolderPath} -> {targetPath}");
                return;
            }

            // Use absolute path or relative path?
            var sourcePath = linkType == SymlinkType.Relative
                ? GetRelativePath(sourceFolderPath, targetPath)
                : sourceFolderPath;
#if UNITY_EDITOR_WIN
            var linkOption = linkType == SymlinkType.Junction ? "/J" : "/D";
            var command = $"mklink {linkOption} \"{targetPath}\" \"{sourcePath}\"";
            ExecuteCmdCommand(command,
                linkType != SymlinkType.Junction); // Symlinks require admin privilege on windows, junctions do not.
#elif UNITY_EDITOR_OSX
            // For some reason, OSX doesn't want to create a symlink with quotes around the paths, so escape the spaces instead.
            sourcePath = sourcePath.Replace(" ", "\\ ");
            targetPath = targetPath.Replace(" ", "\\ ");
            string command = string.Format("ln -s {0} {1}", sourcePath, targetPath);
            ExecuteBashCommand(command);
#elif UNITY_EDITOR_LINUX
            // Is Linux the same as OSX?
#endif

            //UnityEngine.Debug.Log(string.Format("Created symlink: {0} <=> {1}", targetPath, sourceFolderPath));

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        private static string GetRelativePath(string sourcePath, string outputPath)
        {
            if (string.IsNullOrEmpty(outputPath))
            {
                return sourcePath;
            }

            if (sourcePath == null)
            {
                sourcePath = string.Empty;
            }

            var splitOutput = outputPath.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var splitSource = sourcePath.Split(new char[1] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            var max = Mathf.Min(splitOutput.Length, splitSource.Length);
            var i = 0;
            while (i < max)
            {
                if (splitOutput[i] != splitSource[i])
                {
                    break;
                }

                ++i;
            }

            var hopUpCount = splitOutput.Length - i - 1;
            var newSplitCount = hopUpCount + splitSource.Length - i;
            var newSplitTarget = new string[newSplitCount];
            var j = 0;
            for (; j < hopUpCount; ++j)
            {
                newSplitTarget[j] = "..";
            }

            for (max = newSplitTarget.Length; j < max; ++j, ++i)
            {
                newSplitTarget[j] = splitSource[i];
            }

            return string.Join(Path.DirectorySeparatorChar.ToString(), newSplitTarget);
        }

        private static void ExecuteCmdCommand(string command, bool asAdmin)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "CMD.exe",
                Arguments = "/C " + command,
                UseShellExecute = asAdmin,
                RedirectStandardError = !asAdmin,
                CreateNoWindow = true,
            };
            if (asAdmin)
            {
                startInfo.Verb =
                    "runas"; // Runs process in admin mode. See https://stackoverflow.com/questions/2532769/how-to-start-a-process-as-administrator-mode-in-c-sharp
            }

            var proc = new Process()
            {
                StartInfo = startInfo
            };

            using (proc)
            {
                proc.Start();
                proc.WaitForExit();

                if (!asAdmin && !proc.StandardError.EndOfStream)
                {
                    Debug.LogError(proc.StandardError.ReadToEnd());
                }
            }
        }

        private static void ExecuteBashCommand(string command)
        {
            command = command.Replace("\"", "\"\"");

            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                }
            };

            using (proc)
            {
                proc.Start();
                proc.WaitForExit();

                if (!proc.StandardError.EndOfStream)
                {
                    Debug.LogError(proc.StandardError.ReadToEnd());
                }
            }
        }

        private enum SymlinkType
        {
            Junction,
            Absolute,
            Relative
        }
    }
}