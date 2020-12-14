using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPreference : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    Movement movement;
    [SerializeField] private float selectionTime = 1f;
    public float timeTracker = 0;

    private void Start() {
        movement = FindObjectOfType<Movement>();
    }

    private void FixedUpdate()
    {
        //I used the layer ignore raycast prebuilt in unity to ignore player and ground
        //int layerMask = 1 << 8;
        //layerMask = ~layerMask;
        
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
            Debug.Log(hit.transform.tag);
            //Enemy target = hit.transform.GetComponent<Enemy>();
            timeTracker += Time.deltaTime;
            if (!hit.transform.gameObject.GetComponent<Enemy>()){
                timeTracker = 0;
            }
            else if (timeTracker > selectionTime)
            {
                movement.SetPreferredEnemy(hit.transform.gameObject);
                hit.transform.gameObject.GetComponent<Enemy>().PlaySound();
                timeTracker = 0;
            }
        }else {
            timeTracker = 0;
        }
    }
}
