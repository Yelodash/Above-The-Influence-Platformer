using System;
using UnityEngine;

/// <summary>
/// Provides a unique identifier for GameObjects.
/// </summary>
[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    /// <summary>
    /// The unique identifier associated with this GameObject.
    /// </summary>
    [ReadOnly, SerializeField] private string _id;

    /// <summary>
    /// A dictionary storing the mapping of unique identifiers to GameObjects.
    /// </summary>
    [SerializeField] private static SerializableDictionary<string, GameObject> idDatabase = new SerializableDictionary<string, GameObject>();

    /// <summary>
    /// Gets the unique identifier associated with this GameObject.
    /// </summary>
    public string ID => _id;

    private void Awake()
    {
        if (idDatabase == null) idDatabase = new SerializableDictionary<string, GameObject>();

        if (idDatabase.ContainsKey(_id)) Generate();
        else idDatabase.Add(_id, this.gameObject);
    }

    private void OnDestroy()
    {
        if (idDatabase.ContainsKey(_id)) idDatabase.Remove(_id);
    }

    /// <summary>
    /// Generates a new unique identifier for this GameObject.
    /// </summary>
    [ContextMenu("Generate ID")]
    private void Generate()
    {
        _id = Guid.NewGuid().ToString();
        idDatabase.Add(_id, this.gameObject);
        Debug.Log(idDatabase.Count);
    }
}