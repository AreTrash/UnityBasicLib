using UniRx.Toolkit;
using UnityEngine;

namespace UnityBasicLib
{
    public class PoolForPrefab<T> : ObjectPool<T>
        where T : Component
    {
        readonly T prefab;
        readonly Transform parent;

        public PoolForPrefab(T prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        protected override T CreateInstance()
        {
            return Object.Instantiate(prefab, parent);
        }
    }
}