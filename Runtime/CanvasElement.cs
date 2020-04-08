using System.Collections.Generic;
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

    public abstract class CanvasElement<TEventHub, TModel> : CanvasElement<(TEventHub, TModel)>
        where TEventHub : IEventHub
    {
        protected TEventHub EventHub => Context.Item1;
        protected TModel Model => Context.Item2;
    }
}
