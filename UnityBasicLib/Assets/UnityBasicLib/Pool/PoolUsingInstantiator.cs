using UniRx.Toolkit;
using UnityEngine;
using Zenject;

namespace UnityBasicLib
{
    public class PoolUsingInstantiator<T> : ObjectPool<T>
        where T : Component
    {
        readonly IInstantiator instantiator;
        readonly T prefab;
        readonly Transform parent;

        public PoolUsingInstantiator(IInstantiator instantiator, T prefab, Transform parent)
        {
            this.instantiator = instantiator;
            this.prefab = prefab;
            this.parent = parent;
        }

        protected override T CreateInstance()
        {
            return instantiator.InstantiatePrefabForComponent<T>(prefab, parent);
        }
    }
}