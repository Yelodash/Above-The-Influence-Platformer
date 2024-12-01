using UnityEngine;

/// <summary>
/// Displays a text message when the player reaches a checkpoint.
/// </summary>
public class CheckpointTextDisplay : MonoBehaviour
{
    /// <summary>
    ///The object that contains the checkpoint text.
    /// </summary>
    public GameObject checkpointTextObject; 
    
    /// <summary>
    /// The duration the checkpoint text is displayed.
    /// </summary>
    public float displayDuration = 2f; 

    /// <summary>
    /// Initializes the checkpoint text display.
    /// </summary>
    private void Start()
    {
        checkpointTextObject.SetActive(false); 
    }

    /// <summary>
    /// Displays the checkpoint text.
    /// </summary>
    public void DisplayText()
    {
        checkpointTextObject.SetActive(true); 
        Invoke("HideText", displayDuration); 
    }

    /// <summary>
    /// Hides the checkpoint text.
    /// </summary>
    private void HideText()
    {
        checkpointTextObject.SetActive(false); 
    }
}