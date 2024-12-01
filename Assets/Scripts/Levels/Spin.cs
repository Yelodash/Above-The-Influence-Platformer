using UnityEngine;

namespace Levels
{
    /// <summary>
    /// This class is used to make a GameObject spin.
    /// </summary>
    public class Spin : MonoBehaviour
    {
        [SerializeField] private float turnSpeed = 50f;
    
        /// <summary>
        /// Spins the GameObject.
        /// </summary>
        private void Update()
        {
            transform.Rotate(Vector3.up * (turnSpeed * Time.deltaTime));
        }
    }
}
