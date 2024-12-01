using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Editor window that allows you to replace selected objects with a prefab.
    /// </summary>
    public class ObjectToPrefab : EditorWindow
    {
        private GameObject _prefab;
        private bool _applyPos, _applyRot = true, _applyScale, _applyParent;

        /// <summary>
        /// Shows the window in the editor.
        /// </summary>
        [MenuItem("Window/Object To Prefab")]
        public static void ShowWindow()
        {
            // Show existing window instance. If one doesn't exist, create one.
            GetWindow(typeof(ObjectToPrefab), false, "ObjectToPrefab");
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("Replace selected objects with a prefab:", EditorStyles.boldLabel);

            _prefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", _prefab, typeof(GameObject), true);

            GUILayout.Label("Options:", EditorStyles.boldLabel);
            _applyPos = EditorGUILayout.Toggle("Apply Position", _applyPos);
            _applyRot = EditorGUILayout.Toggle("Apply Rotation", _applyRot);
            _applyScale = EditorGUILayout.Toggle("Apply Scale", _applyScale);
            _applyParent = EditorGUILayout.Toggle("Apply Parent", _applyParent);
        
        
            if (GUILayout.Button("Replace Selected with Prefab"))
            {
                ReplaceSelectedObjectsWithPrefab();
            }
        }

        /// <summary>
        /// Replaces the selected objects with the prefab.
        /// </summary>
        private void ReplaceSelectedObjectsWithPrefab()
        {
            if (_prefab != null)
            {
                if (!_applyPos && !_applyRot && !_applyScale && !_applyParent)
                {
                    Debug.LogError("You must select at least one option!");
                    return;
                }

                foreach (GameObject objToBeReplaced in Selection.gameObjects)
                {
                    // Instantiate the prefab at the same position and rotation as the original GameObject
                    GameObject newObject = PrefabUtility.InstantiatePrefab(_prefab) as GameObject;

                    if (newObject == null)
                    {
                        return;
                    }
                    
                    if (_applyPos)
                    {
                        newObject.transform.position = objToBeReplaced.transform.position;
                    }
                    if (_applyRot)
                    {
                        newObject.transform.rotation = objToBeReplaced.transform.rotation;
                    }
                    if (_applyScale)
                    {
                        newObject.transform.localScale = objToBeReplaced.transform.localScale;
                    }
                    if (_applyParent)
                    {
                        newObject.transform.parent = objToBeReplaced.transform.parent;
                    }

                    // Destroy the original GameObject
                    DestroyImmediate(objToBeReplaced);
                }
            }
            else
            {
                Debug.LogError("Prefab is not assigned!");
            }
        }
    }
}
