using UnityEngine;

namespace Gun_System
{
    /// <summary>
    /// Makes sure the player cannot fire at themselves.
    /// </summary>
    public class FriendlyFireCheck : MonoBehaviour
    {
        [SerializeField] private GameObject cannotFireCrosshair;
        public bool canFire = true;
        private Ray ray;
        
        /// <summary>
        /// Constantly call the Check method
        /// </summary>
        private void Update()
        {
            Check();
        }
        
        /// <summary>
        /// Checks if the player is in the crosshair.
        /// </summary>
        private void Check()
        {
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;
            var range = 100f;
            if (Physics.Raycast(ray, out hit, range))
            {
                if (hit.collider.gameObject.layer == 7)
                {
                    Debug.Log("Hit player");
                    cannotFireCrosshair.SetActive(true);
                    canFire = false;
                }
                else
                {
                    cannotFireCrosshair.SetActive(false);
                    canFire = true;
                }
            }
        }

        /// <summary>
        /// Draws the ray in the scene view.
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(ray.origin, ray.direction * 20f);
        }
    }
}
