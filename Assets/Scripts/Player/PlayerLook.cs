using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;

    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    RaycastHit _hitInfo;
    Ray _testRay;

    [SerializeField] private LayerMask _interactionMask, _placementMask;

    [SerializeField] private float _grabRange = 50f;

    PlayerInventory _inventory;

    public void FireInteractRay()
    {
        Debug.Log("Firing interact ray");
        _testRay = new Ray(cam.transform.position, cam.transform.forward);
        Debug.DrawRay(_testRay.origin,_testRay.direction * 100f, Color.yellow, 3f);

        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, _grabRange, _interactionMask))
        {
            if (hitInfo.collider.CompareTag("Distraction"))
            {
                Debug.Log("Grabbing " + hitInfo.collider.name);
                _inventory.AddtoInventory(hitInfo.collider.gameObject);
                UIManager.updateDistractible(hitInfo.collider.gameObject);
            }
            else if (hitInfo.collider.CompareTag("Interactable"))
            {
                PlayerCollection _playerCollect = hitInfo.collider.GetComponent<PlayerCollection>();
                if (_playerCollect != null)
                    _playerCollect.Interact();
            }
        }
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _inventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hitInfo;
        UIManager.HoverInteractable(Physics.Raycast
            (cam.transform.position, cam.transform.forward, out hitInfo, _grabRange, _interactionMask));
        
    }
}
