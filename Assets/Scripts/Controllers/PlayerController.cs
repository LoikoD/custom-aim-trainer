using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] Transform eyes;
    [SerializeField] Transform arm;
    [SerializeField] Camera cam;
    [SerializeField] GameObject bulletPrefab;

    [SerializeField] Transform bulletStartPos;
    [SerializeField] Transform bulletEndPoint;

    GameManager gm;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void Start()
    {
        gm = GameManager.Instance;

    }

    public void DefaultState()
    {
        transform.rotation = Quaternion.identity;
    }



    public void Shoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletStartPos.position, bulletPrefab.transform.rotation);
                bullet.GetComponent<Bullet>().endPosition = hit.point;
                Destroy(hit.transform.gameObject);
                gm.ShotHit();

            }
            else
            {
                GameObject bullet = Instantiate(bulletPrefab, bulletStartPos.position, bulletPrefab.transform.rotation);
                bullet.GetComponent<Bullet>().endPosition = bulletEndPoint.position;

                gm.ShotMiss();
            }
        }
    }

    public void LookAround(InputAction.CallbackContext context)
    {

        if (context.performed)
        {

            Vector2 inputVector = context.ReadValue<Vector2>() * gm.Sensitivity;

            // This is from forum. With these multipliers sens is more standard.
            // "Account for scaling applied directly in Windows code by old input system."
            inputVector *= 0.5f;
            // "Account for sensitivity setting on old Mouse X and Y axes."
            inputVector *= 0.1f;
            // ---------------------------------------------------------------------------

            float verticalRotation = -inputVector.y;
            verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

            Vector3 localEulerNew = transform.localEulerAngles + Vector3.right * verticalRotation;
            
            // Check euler angles constraints
            if (localEulerNew.x > 90 && localEulerNew.x < 180)
            {
                localEulerNew.x = 90;
            } else if (localEulerNew.x < -90 || localEulerNew.x < 270 && localEulerNew.x > 180)
            {
                localEulerNew.x = -90;
            }

            transform.localEulerAngles = localEulerNew;
            transform.Rotate(Vector3.up * inputVector.x, Space.World);
        }

    }
}
