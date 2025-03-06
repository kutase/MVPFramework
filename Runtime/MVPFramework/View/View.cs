using UnityEngine;

namespace MVPFramework.View
{
    public class View : MonoBehaviour, IView
    {
        private Transform transformInternal;

        public Transform Transform => transformInternal ??= transform;

        public void SetTransform(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            if (rotation != Quaternion.identity)
                transform.rotation = rotation;
        }

        public virtual void Destroy()
        {
            if (!gameObject)
                return;

            SetActive(false);
            Destroy(gameObject);
        }

        public virtual void SetActive(bool active)
        {
            if (active != gameObject.activeInHierarchy)
                gameObject.SetActive(active);
        }
    }
}