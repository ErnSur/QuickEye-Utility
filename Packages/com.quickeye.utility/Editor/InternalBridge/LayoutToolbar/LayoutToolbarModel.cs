using System;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    [Serializable]
    internal class LayoutToolbarModel
    {
        public List<string> tabNames = new List<string>();

        public bool Add(string tabName)
        {
            if (tabNames.Contains(tabName))
                return false;
            tabNames.Add(tabName);
            return true;
        }
        
        public string ToJson() => JsonUtility.ToJson(this, true);
        public static LayoutToolbarModel FromJson(string json) => JsonUtility.FromJson<LayoutToolbarModel>(json);
    }

    [Serializable]
    public class LayoutTabModel
    {
        public string name;
    }
}