using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityBasicLib
{
    public interface IInitialable<in TArg>
    {
        void Init(TArg arg);
    }

    public abstract class Presenter<TArg> : UIBehaviour, IInitialable<TArg>
    {
        RectTransform m_rectTransform;
        public RectTransform rectTransform => m_rectTransform ?? (m_rectTransform = transform as RectTransform);

        readonly CompositeDisposable disps = new CompositeDisposable();

        protected abstract IEnumerable<IDisposable> InitCore(TArg arg);

        public void Init(TArg arg)
        {
            disps.Clear();
            foreach (var disp in InitCore(arg)) disps.Add(disp);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            disps.Dispose();
        }
    }
}