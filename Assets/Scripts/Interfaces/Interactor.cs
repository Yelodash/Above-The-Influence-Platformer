using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interfaces
{
    /// <summary>
    /// Manages interactions between the player and interactable objects.
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        /// <summary>
        /// The transform representing the point from which interactions occur.
        /// </summary>
        public Transform InteractionPoint;

        /// <summary>
        /// The layer mask used for identifying interactable objects.
        /// </summary>
        public LayerMask InteractionLayer;

        /// <summary>
        /// The radius around the interaction point within which objects are considered for interaction.
        /// </summary>
        public float InteractionPointRadius = 1f;

        /// <summary>
        /// Indicates whether the player is currently interacting with an object.
        /// </summary>
        public bool IsInteracting { get; private set; }

        private void Update()
        {
            // Detect interactable objects within the interaction radius
            var colliders = Physics.OverlapSphere(InteractionPoint.position, InteractionPointRadius, InteractionLayer);

            // Check for interaction input
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Iterate through detected colliders
                for (int i = 0; i < colliders.Length; i++)
                {
                    var interactable = colliders[i].GetComponent<IInteractable>();

                    // Call the Interact method on the interactable object
                    if (interactable != null)
                    {
                        StartInteraction(interactable);
                    }
                }
            }
        }

        /// <summary>
        /// Initiates an interaction with the specified interactable object.
        /// </summary>
        /// <param name="interactable">The interactable object to interact with.</param>
        void StartInteraction(IInteractable interactable)
        {
            interactable.Interact(this, out bool interactSuccessful);
            IsInteracting = true;
        }

        /// <summary>
        /// Ends the current interaction.
        /// </summary>
        void EndInteraction()
        {
            IsInteracting = false;
        }
    }
}