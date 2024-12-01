using UnityEngine;

namespace Levels
{
    /// <summary>
    /// Do not destroy this GameObject when a new scene is loaded.
    /// </summary>
    public class DoNotDestroyMe : MonoBehaviour
    {
        private void Start()
        {
            // DontDestroyOnLoad(this);
        }
    }
}

