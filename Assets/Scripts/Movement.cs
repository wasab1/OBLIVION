using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float preferenceMaxVel = 5f;
    [SerializeField] private float autoMaxVel = 0.5f;
    [SerializeField] private float maxVel = 1f;
    [SerializeField] private float turnspeed = 1f;
    private GameObject preferredEnemy;
    // Start is called before the first frame update

    //VARIABILI LEGATE ALLO SGUARDO
    [SerializeField] bool CONTROLLO_SGUARDO = false;
    private GameObject Target_Gaze;
    private bool targetIsSet = false;
    private bool lastTarget = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (CONTROLLO_SGUARDO)
        {
            Target_Gaze = GameObject.FindGameObjectWithTag("Gaze");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if(CONTROLLO_SGUARDO){
            if (enemies.Length != 0)
            {
                GameObject e;
                if (preferredEnemy != null)
                {
                    e = preferredEnemy;
                    maxVel = preferenceMaxVel;
                    targetIsSet = false;
                }
                else
                {
                    if (!targetIsSet)
                    {
                        SetBoidsTarget(Target_Gaze);
                        targetIsSet = true;
                    }
                    e = enemies[0];
                    maxVel = autoMaxVel;
                }
                Vector3 toEnemy = e.transform.position - this.transform.position;
                toEnemy.Normalize();
                toEnemy *= turnspeed;
                rb.AddForce(toEnemy);
                if (rb.velocity.magnitude > maxVel)
                {
                    Vector3 v = rb.velocity;
                    v.Normalize();
                    v *= maxVel;
                    rb.velocity = v;
                }
            }
            else if (!lastTarget)
            {
                SetBoidsTarget(this.gameObject);
                lastTarget = true;
            }
        }
        //SCIE NON CONTROLLATE DALLO SGUARDO    
        else
        {
            if (enemies.Length != 0)
            {
                GameObject e;
                if (preferredEnemy != null)
                {
                    e = preferredEnemy;
                    maxVel = preferenceMaxVel;
                }
                else
                {
                    e = enemies[0];
                    maxVel = autoMaxVel;
                }
                Vector3 toEnemy = e.transform.position - this.transform.position;
                toEnemy.Normalize();
                toEnemy *= turnspeed;
                rb.AddForce(toEnemy);
                if (rb.velocity.magnitude > maxVel)
                {
                    Vector3 v = rb.velocity;
                    v.Normalize();
                    v *= maxVel;
                    rb.velocity = v;
                }
            }
        }
    }

    public void SetPreferredEnemy(GameObject go)
    {
        preferredEnemy = go;
        SetBoidsTarget(go);
    }

    public void SetBoidsTarget(GameObject go)
    {
        Boid[] boids = FindObjectsOfType<Boid>();
        if (boids.Length > 0)
        {
            foreach (Boid b in boids)
            {
                b.SetTarget(go);
            }
        }
    }
}
