using Audio;
using Managers;
using UnityEngine;

namespace Powerups
{
    /// <summary>
    /// The Alcohol class is a script that is used to detect when the player picks up an alcohol bottle.
    /// </summary>
    public class Alcohol : MonoBehaviour
    {
        public static event AlcoholEventHandler AlcoholObtained;
    
        /// <summary>
        /// Invokes an event when colliding with the player.
        /// </summary>
        /// <param name="other">The object collided with - Should be the player</param>
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 7)
            {
                //Event
                AudioManager.Instance.PlaySound(20);
                AlcoholObtained?.Invoke();
                Debug.Log("Alcohol bottle picked up!");
                Debug.LogWarning("Yeehaw you got the alcohol!");
                Destroy(gameObject);
            }
        }
    }
}
