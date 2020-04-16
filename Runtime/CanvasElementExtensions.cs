namespace QuickEye.CanvasElements
{
    public static partial class CanvasElementExtensions
    {
        public static T AddNewInitialized<T, C>(this Container<T> g, C c) where T : CanvasElement<C>
        {
            var e = UnityEngine.Object.Instantiate(g.itemPrefab, g.transform);
            e.Initialize(c);
            g.Add(e);
            return e;
        }

        public static T AddNewInitialized<T, C>(this ElementPool<T> g, C c) where T : CanvasElement<C>
        {
            var e = g.pool.GetFromPool();
            e.Initialize(c);
            e.gameObject.SetActive(true);
            g.items.Add(e);

            return e;
        }
    }
}
