/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Comment for git test commit v2

public class LookAround : MonoBehaviour
{
    [SerializeField] private Transform playerCam;
    [SerializeField] private Vector2 sens;

    private Vector2 _rot;

    void Start()
    {
        _rot.x = transform.eulerAngles.y;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        Vector2 mouseInput = new Vector2
        {
            x = Input.GetAxis("Mouse X"),
            y = Input.GetAxis("Mouse Y")
        };

        _rot.y -= mouseInput.y * sens.y;
        _rot.x += mouseInput.x * sens.x;

        _rot.y = Mathf.Clamp(_rot.y, -90f, 90f);

        transform.eulerAngles = new Vector3(0f, _rot.x, 0f);
        Debug.Log(_rot.y);
        //playerCam.localEulerAngles = new Vector3(_rot.y, 0f, 0f);
        
    }
    
    
}*/