using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.CanvasElements
{
    /// <summary>
    /// A collection of gameobjects
    /// where a lifetime of the gameobject is bound to the collection
    /// if its removed from the collection it should no longer be active in the scene
    /// </summary>
    public interface IGameObjectCollection<T> : ICollection<T> where T : Component
    {
        T AddNew();
    }
}