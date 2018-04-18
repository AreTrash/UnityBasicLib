using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx.Toolkit;
using UnityEngine;
using Zenject;

namespace UnityBasicLib
{
    public abstract class ListPresenter<TArg, TItem> : Presenter<IEnumerable<TArg>>, IReadOnlyDictionary<TArg, TItem>
        where TItem : Component, IInitialable<TArg>
    {
        [Inject] IInstantiator instantiator;
        [SerializeField] TItem itemPrefab;

        ObjectPool<TItem> pool;
        protected ObjectPool<TItem> Pool => pool ?? (pool = new PoolUsingInstantiator<TItem>(instantiator, itemPrefab, transform));

        readonly Dictionary<TArg, TItem> itemDic = new Dictionary<TArg, TItem>();

        public TItem this[TArg key] => itemDic[key];
        public int Count => itemDic.Count;
        public IEnumerable<TArg> Keys => itemDic.Keys;
        public IEnumerable<TItem> Values => itemDic.Values;

        public new void Init(IEnumerable<TArg> args)
        {
            var children = itemDic.Values.Union(GetComponentsInChildren<TItem>());
            itemDic.Clear();
            foreach (var child in children) Pool.Return(child);
            foreach (var arg in args) Add(arg);
            base.Init(args);
        }

        public void Add(TArg arg)
        {
            var instance = Pool.Rent();
            instance.Init(arg);
            itemDic.Add(arg, instance);
        }

        public void Remove(TArg arg)
        {
            Pool.Return(itemDic[arg]);
            itemDic.Remove(arg);
        }

        public IEnumerator<KeyValuePair<TArg, TItem>> GetEnumerator()
        {
            return itemDic.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool ContainsKey(TArg key)
        {
            return itemDic.ContainsKey(key);
        }

        public bool TryGetValue(TArg key, out TItem value)
        {
            return itemDic.TryGetValue(key, out value);
        }
    }
}