using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///This class is used to give a GameObject a unique ID that can be used to reference it from anywhere.
/// </summary>
public class GameObjectId : MonoBehaviour
{
    /// <summary>
    /// A dictionary that stores all instances of this script.
    /// </summary>
    private static Dictionary<string, GameObject> _instances = new Dictionary<string, GameObject>();
    
    /// <summary>
    /// The ID of the GameObject.
    /// </summary>
    public string ID; // This ID can be pretty much anything, as long as you can set it from the inspector

    void Awake()
    {
        if(_instances.ContainsKey(ID))
        {
            var existing = _instances[ID];

            // A null result indicates the other object was destroyed for some reason
            if(existing != null)
            {
                if(ReferenceEquals(gameObject, existing))
                    return;

                Destroy(gameObject);

                // Return to skip the following registration code
                return;
            }
        }

        // The following code registers this GameObject regardless of whether it's new or replacing
        _instances[ID] = gameObject;

        DontDestroyOnLoad(gameObject);
    }
}