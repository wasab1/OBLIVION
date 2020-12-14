using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

	//This static List holds all Boid instances & is SHARED amongs them
	static public List<Boid> boids;

	public Vector3 velocity;
	public Vector3 newVelocity;
	public Vector3 newPosition;

	public List<Boid> neighbors; //All nearby Boids
	public List<Boid> collisionRisks; //All Boids that are too close
	public Boid closest; //The single closest Boid

    private GameObject Target;
    private GameObject Pallino;

    void Awake(){
		if (boids == null) {
			boids = new List<Boid> ();
		}

		boids.Add (this);

		

		Vector3 randPos = Random.insideUnitSphere * BoidSpawner.S.spawnRadius;
		randPos.y = 0;
		//this.transform.position = randPos;
		velocity = Random.onUnitSphere;
		velocity *= BoidSpawner.S.spawnVelocity;

		neighbors = new List<Boid> ();
		collisionRisks = new List<Boid> ();

		//Make this.transform a child of the Boids GameObject
		this.transform.parent = GameObject.Find("Boids").transform;
		
		Color randColor = Color.black;
		while (randColor.r + randColor.g + randColor.b < 1.0f) {
			randColor = new Color (Random.value, Random.value, Random.value);
		}
		//Renderer[] rends = gameObject.GetComponentInChildren<Renderer>();
		Renderer rends = gameObject.GetComponentInChildren<Renderer>();
		//foreach (Renderer r in rends) {
		//	r.material.color = randColor;
		//}
		rends.material.color = randColor; 

		Pallino = GameObject.FindGameObjectWithTag("Pallino");
        Target = Pallino;
    }

	
	// Update is called once per frame
	void Update () {
		List<Boid> neighbors = GetNeighbors (this);

        if (Target == null) {
            Target = Pallino;
        }

        newVelocity = velocity;
		newPosition = this.transform.position;
        if (boids.Count > 1)
        {
			//Velociy Matching: this sets the velocity of the bold to be similar to that of its neighbots
            Vector3 neighborVel = GetAverageVelocity(neighbors);
            //Utilizes the fields st on the BoidSpawner singleton
            newVelocity += neighborVel * BoidSpawner.S.velocityMatchingAmt;

            //Flock centering
            Vector3 neighborCenterOffset = GetAveragePosition(neighbors) - this.transform.position;
            newVelocity += neighborCenterOffset * BoidSpawner.S.flockCenteringAmt;

            //Collision Avoidance
            Vector3 dist;
            if (collisionRisks.Count > 0)
            {
                Vector3 collisionAveragePos = GetAveragePosition(collisionRisks);
                dist = collisionAveragePos - this.transform.position;
                newVelocity += dist * BoidSpawner.S.collisionAvoidanceAmt;
            }


            //Mouse Attraction

            //*******************************************
            //PALLINO EDIT
            Vector3 targetPos = Target.transform.position;
            //dist = BoidSpawner.S.mousePos - this.transform.position;
            dist = targetPos - this.transform.position;
            if (dist.magnitude > BoidSpawner.S.mouseAvoidanceDist)
            {
                newVelocity += dist * BoidSpawner.S.mouseAttracionAmt;
            }
            else
            {
                //if the mouse is too close, move away quickly
                newVelocity -= dist.normalized * BoidSpawner.S.mouseAvoidanceDist * BoidSpawner.S.mouseAvoidanceAmt;
            }
            //newVelocity & newPosition are ready, but wait until LateUpdate() to
            //set them so that this Boid doesn't move before others have had a
            //chance to calculate their new value 
        }
        else
        {
            Vector3 targetPos = Target.transform.position;
            //dist = BoidSpawner.S.mousePos - this.transform.position;
            Vector3 dist = targetPos - this.transform.position;
            if (dist.magnitude > BoidSpawner.S.mouseAvoidanceDist)
            {
                newVelocity += dist * BoidSpawner.S.mouseAttracionAmt;
            }
            else
            {
                //if the mouse is too close, move away quickly
                newVelocity -= dist.normalized * BoidSpawner.S.mouseAvoidanceDist * BoidSpawner.S.mouseAvoidanceAmt;
            }
            //newVelocity & newPosition are ready, but wait until LateUpdate() to
            //set them so that this Boid doesn't move before others have had a
            //chance to calculate their new value 
        }
    }

	//we let the boids update before make any move
	void LateUpdate(){
		//Adjust the current velocity based on newVelocity using a linear interpolation
		velocity = (1- BoidSpawner.S.velocityLerpAmt)*velocity + BoidSpawner.S.velocityLerpAmt*newVelocity;

		//make sure velocity stay between min and max
		if(velocity.magnitude > BoidSpawner.S.maxVelocity){
			velocity = velocity.normalized*BoidSpawner.S.maxVelocity;
		}
		if (velocity.magnitude < BoidSpawner.S.minVelocity) {
			velocity = velocity.normalized*BoidSpawner.S.minVelocity;
		}

		//decide on the new position
		newPosition = this.transform.position + velocity * Time.deltaTime;
		//keep everything in the xz plane
		//EDIT WE DONT WANT OUR CRYSTALS TO BE LOCKED IN 2D
		//newPosition.y = 0; 
		//look from the old position to the new position to orient the model
		this.transform.LookAt(newPosition);
		//actually move to the new position
		this.transform.position = newPosition;
    }

	// find the boids close enough to be considered neighbors
	//boi = boid of interest
	public List<Boid> GetNeighbors(Boid boi){
		float closestDist = float.MaxValue; //max value a float can hold
		Vector3 delta;
		float dist;
		neighbors.Clear ();
		collisionRisks.Clear ();

        foreach (Boid b in boids)
        {
            if (b == boi) {
           		continue;
        	}
       		delta = b.transform.position - boi.transform.position;
			dist = delta.magnitude;
			if (dist < closestDist) {
				closestDist = dist;
				closest = b;
			}

			if (dist < BoidSpawner.S.nearDist) {
				neighbors.Add (b);
			}

			if (dist < BoidSpawner.S.collisionDist) {
				collisionRisks.Add (b);
			}
		}

		if (neighbors.Count == 0 && closest!=null) {
			neighbors.Add (this);
		}else if(neighbors.Count == 0){
            neighbors.Add(closest);
		}

        return (neighbors);
	}

	//get the average position of the boids

	public Vector3 GetAveragePosition(List<Boid> someBoids){
		Vector3 sum = Vector3.zero;
		foreach (Boid b in someBoids) {
			sum += b.transform.position;
		}
		Vector3 centre = sum / someBoids.Count;
		return (centre);
	}

	//get the average velocity of the boids

	public Vector3 GetAverageVelocity(List<Boid> someBoids){
		Vector3 sum = Vector3.zero;
		foreach (Boid b in someBoids) {
			sum += b.velocity;
		}
		Vector3 avg = sum / someBoids.Count;
		return (avg);
			
	}

    public void SetTarget(GameObject go) {
        Target = go;
    }
}
