using UniRx.Toolkit;
using UnityEngine;
using Zenject;

namespace UnityBasicLib
{
    public class PoolUsingFactory<T> : ObjectPool<T>
        where T : Component
    {
        readonly IFactory<T> factory;
        readonly Transform parent;

        public PoolUsingFactory(IFactory<T> factory, Transform parent)
        {
            this.factory = factory;
            this.parent = parent;
        }

        protected override T CreateInstance()
        {
            var instance = factory.Create();
            instance.transform.SetParent(parent);
            return instance;
        }
    }
}