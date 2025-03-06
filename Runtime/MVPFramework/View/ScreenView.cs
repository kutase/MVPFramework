using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVPFramework.View
{
    public class ScreenView : View, IScreenView
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GraphicRaycaster raycaster;

        protected virtual void OnValidate()
        {
            canvas ??= GetComponent<Canvas>();
            raycaster ??= GetComponent<GraphicRaycaster>();
        }

        public virtual void OnActivate()
        {
        }
        
        public virtual void OnDeactivate()
        {
        }

        public virtual void Hide()
        {
            SetActive(false);
            OnDeactivate();
        }

        public virtual void Show()
        {
            SetActive(true);
            OnActivate();
        }
    }
}