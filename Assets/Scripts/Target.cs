using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float radius = 20f;
    //[SerializeField] private float maxRadius = 10.0f;
    //[SerializeField] private float minRadius = 80.0f;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Gaze");
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //Vector3 gazePos = playerCamera.transform.position + playerCamera.transform.TransformDirection(Vector3.forward) * scale(-1, 1, minRadius, maxRadius, Mathf.Sin(Time.time));
        Vector3 gazePos = playerCamera.transform.position + playerCamera.transform.TransformDirection(Vector3.forward) * radius;
        target.transform.position = gazePos;

    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

}
