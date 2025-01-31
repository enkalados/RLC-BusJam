using UnityEngine;
namespace Base.Collectable
{
    public class Collector: MonoBehaviour
    {
        #region MonoBehaviour Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICollectable collectable))
            {
                collectable.Collect(this);
            }
        }
        #endregion
    }
}