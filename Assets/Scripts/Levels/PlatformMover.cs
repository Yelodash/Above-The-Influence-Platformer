using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Platform mover class that inherits from MonoBehaviour.
    /// </summary>
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] private GameObject[] waypointGameObjects;
        [SerializeField] private float platformSpeed = 1f;
        [SerializeField] private float platformMovementDelay = 1f; 
        
        private int currentTargetIndex = 0; // Start at the first point
        private float timer = 0f;
        private bool isDelaying = false;
        private bool movingForward = true; // Flag to indicate if the platform is moving forward or backward

        /// <summary>
        /// Handle the movement of the platform.
        /// </summary>
        private void Update()
        {
            if (waypointGameObjects.Length == 0)
            {
                return;
            }

            if (!isDelaying)
            {
                // Move the platform towards the current target
                Vector3 currentTargetPosition = waypointGameObjects[currentTargetIndex].transform.position;
                transform.position = Vector3.MoveTowards(transform.position, currentTargetPosition, platformSpeed * Time.deltaTime);

                if (transform.position == currentTargetPosition)
                {
                    timer = 0f;
                    isDelaying = true;
                }
            }
            else
            {
                timer += Time.deltaTime;

                if (timer >= platformMovementDelay)
                {
                    isDelaying = false;
                
                    // Check if the platform reached the last or first waypoint
                    if (currentTargetIndex == waypointGameObjects.Length - 1 && movingForward)
                    {
                        movingForward = false; // Start moving backward
                    }
                    else if (currentTargetIndex == 0 && !movingForward)
                    {
                        movingForward = true; // Start moving forward
                    }
                
                    // Update the target index based on movement direction
                    currentTargetIndex += movingForward ? 1 : -1;
                }
            }
        }
    }
}