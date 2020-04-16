using UnityEngine;

namespace QuickEye.CanvasElements
{
    public abstract class CanvasElement<TContext> : MonoBehaviour, ICanvasElement<TContext>
    {
        protected TContext Context { get; private set; }

        public virtual void Initialize(TContext context)
        {
            Context = context;
        }
    }
}
