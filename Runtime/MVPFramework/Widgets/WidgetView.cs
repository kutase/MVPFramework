using UnityEngine;

namespace MVPFramework.Widgets
{
    public abstract class WidgetView<TData> : MonoBehaviour, IWidgetView<TData>
    {
        private bool isAlive = true;
        public Transform Transform => transform;

        public virtual void Activate(TData data)
        {
        }

        public virtual void Deactivate()
        {
        }

        public bool IsAlive()
        {
            return isAlive;
        }

        private void OnDestroy()
        {
            isAlive = false;
        }
    }
}