using UnityEngine.Events;

namespace Interfaces
{
    /// <summary>
    /// Interface for interactable objects.
    /// </summary>
    public interface IInteractable
    {
        public UnityAction<IInteractable> OnInteractionComplete { get; set; }
        public void Interact(Interactor interactor, out bool interactSuccessful);
        public void EndInteraction();
    }
}