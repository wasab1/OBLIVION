using System.Collections;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {

	// This is a Singleton of the BoidSpawner. There is only one instance of BoidSpawner, so we can store it in a static variable named S

	static public BoidSpawner S;
	[SerializeField] public int numBoids = 100;
	public GameObject boidPrefab;
	public float spawnRadius = 100f;
	public float spawnVelocity = 10f;
	public float minVelocity = 0f;
	public float maxVelocity = 30f;
	public float nearDist = 30f;
	public float collisionDist = 5f;
	public float velocityMatchingAmt = 0.01f;
	public float flockCenteringAmt = 0.15f;
	public float collisionAvoidanceAmt = -0.5f;
	public float mouseAttracionAmt = 0.01f;
	public float mouseAvoidanceAmt = 0.75f;
	public float mouseAvoidanceDist = 15f;
	public float velocityLerpAmt = 0.25f;
	public bool _____________;

	public Vector3 mousePos;
    [SerializeField] Camera myCamera;

    void Start () {
		//Set singleton S to this instance 
		S = this;
        if (Boid.boids != null)
        {
            Boid.boids.Clear();
        }
        for (int i = 0; i < numBoids; i++) {
			Instantiate (boidPrefab);
		}
	}
	/*
	void LateUpdate () {
		//Track mouse position 
		Vector3 mousePos2d = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.transform.position.y);
        myCamera = FindObjectOfType<Camera>();
        mousePos = myCamera.ScreenToWorldPoint (mousePos2d); // different wrt the book
	}
	*/
}
