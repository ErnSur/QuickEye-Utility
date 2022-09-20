using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace QuickEye.DevTools
{
    internal class ScreenShotUtility
    {
        public static Color kToolbarBorderColor = new Color(0.54f, 0.54f, 0.54f, 1f);
        public static Color kWindowBorderColor = new Color(0.51f, 0.51f, 0.51f, 1f);
        public static bool s_TakeComponentScreenshot = false;
        
       
        public static void ScreenshotFocusedWindow()
        {
            var wnd = GetFocusedWindow();
            if (wnd != null)
            {
                var name = wnd.titleContent.text;
                //GUIUtility.GUIToScreenRect()
                Rect r = wnd.position;
                SaveScreenShot(r, name);
            }
        }

        public static void ScreenShotComponent()
        {
            s_TakeComponentScreenshot = true;
        }

        public static void ScreenShotComponent(Rect contentRect, Object target)
        {
            s_TakeComponentScreenshot = false;

            contentRect.yMax += 2;
            contentRect.xMin += 1;
            SaveScreenShotWithBorder(contentRect, kWindowBorderColor, target.GetType().Name + "Inspector");
        }

        public static void ScreenGameViewContent()
        {
            string path = GetUniquePathForName("ContentExample");
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log(string.Format("Saved screenshot at {0}", path));
        }

        static EditorWindow GetFocusedWindow()
        {
            EditorWindow v = EditorWindow.focusedWindow;
            if (v == null)
            {
                EditorApplication.Beep();
                Debug.LogWarning("Could not take screenshot.");
            }
            return v;
        }

        public static void SaveScreenShot(Rect r, string name)
        {
            SaveScreenShot((int)r.width, (int)r.height, InternalEditorUtility.ReadScreenPixel(new Vector2(r.x, r.y), (int)r.width, (int)r.height), name);
        }

        // Adds a gray border around the screenshot
        // Useful for e.g. toolbars because they don't have a nice border all the way round due to the tabs
        public static string SaveScreenShotWithBorder(Rect r, Color borderColor, string name)
        {
            int w = (int)r.width;
            int h = (int)r.height;
            Color[] colors1 = InternalEditorUtility.ReadScreenPixel(new Vector2(r.x, r.y), w, h);
            Color[] colors2 = new Color[(w + 2) * (h + 2)];
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    colors2[x + 1 + (w + 2) * (y + 1)] = colors1[x + w * y];
                }
            }
            for (int x = 0; x < w + 2; x++)
            {
                colors2[x] = borderColor;
                colors2[x + (w + 2) * (h + 1)] = borderColor;
            }
            for (int y = 0; y < h + 2; y++)
            {
                colors2[y * (w + 2)] = borderColor;
                colors2[y * (w + 2) + (w + 1)] = borderColor;
            }

            return SaveScreenShot((int)(r.width + 2), (int)(r.height + 2), colors2, name);
        }

        static string SaveScreenShot(int width, int height, Color[] pixels, string name)
        {
            Texture2D t = new Texture2D(width, height);
            t.SetPixels(pixels, 0);
            t.Apply(true);

            byte[] bytes = t.EncodeToPNG();
            Object.DestroyImmediate(t, true);

            string path = GetUniquePathForName(name);
            System.IO.File.WriteAllBytes(path, bytes);
            AssetDatabase.ImportAsset(path);
            Debug.Log(string.Format("Saved screenshot at {0}", path));
            return path;
        }

        static string GetUniquePathForName(string name)
        {
            string path = $"Assets/{name}.png";
            return path;
        }
    }
}