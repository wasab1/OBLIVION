using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    [SerializeField] bool box = false;
    private MeshRenderer meshRenderer;
    [SerializeField] public Renderer rend;
    [SerializeField] Material myMaterial;
    public bool isHit = false;

    [SerializeField] GameObject boidPrefab;
    [SerializeField] float disintegrationTime = 2f;

    //Audio
    public AudioSource bisbiglio;
    public bool soundIsPlaying = false;
    [SerializeField] public List<AudioClip> audio_clips;
    [SerializeField] public AudioClip bisbClip;
    [Range(0.0f, 1.0f)][SerializeField] private float bisbVol = 0.1f;
    float bisbCounter = 0f;

    //VFX
    [SerializeField] VisualEffect nebulosa;
    [SerializeField] float attractionAmt = 1f;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        rend.material = myMaterial;
        rend.material.SetFloat("Percentage", -0.01f);
        bisbiglio = GetComponent<AudioSource>();
        foreach (AudioClip ac in audio_clips)
        { 
            if (ac == null) audio_clips.Remove(ac);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // //ci sarà un modo migliore spero ma per ora ci si accontenta
        // bisbiglio.PlayOneShot(bisbClip, bisbVol);
        // bisbCounter += Time.deltaTime;
        // if (bisbCounter >= bisbClip.length)
        // {
        //     bisbCounter = 0f;
        //     bisbiglio.PlayOneShot(bisbClip, bisbVol);
        // }

        if (isHit)
        {
            StartCoroutine("Dissolve");
            isHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("hit by: "+ other.transform.tag);
        if (other.transform.tag == "Pallino")
        {
            isHit = true;
            GetComponent<Collider>().enabled = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    IEnumerator Dissolve()
    {
        if (nebulosa != null)
        {
            float k = 0f;
            nebulosa.Play();
            while (k < 1f)
            {
                k += Time.deltaTime / disintegrationTime;
                rend.material.SetFloat("Percentage", k);
                yield return null;
            }
            rend.material.SetFloat("Percentage", 1f);
            nebulosa.SetInt("numparticles", 0);
            while (k < 3f)
            {
                k += Time.deltaTime / disintegrationTime;
                nebulosa.SetFloat("AttractiveStrength", k * attractionAmt);
                yield return null;
            }
            //cosa brutta da fixare
            this.nebulosa.Stop();
            nebulosa.SetInt("lifetimeMax", 0);
            //
            Instantiate(boidPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else
        {
            float k = 0f;
            while (k < 1f)
            {
                k += Time.deltaTime / disintegrationTime;
                rend.material.SetFloat("Percentage", k);
                yield return null;
            }
            rend.material.SetFloat("Percentage", 1f);
            while (k < 3f)
            {
                k += Time.deltaTime / disintegrationTime;
                yield return null;
            }
            Instantiate(boidPrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public void PlaySound()
    {
        if (audio_clips != null)
        {
            if (!soundIsPlaying)
            {
                int rand_nm = (int)Random.Range(0, audio_clips.Count - 1);
                bisbiglio.volume = 0;
                bisbiglio.clip = audio_clips[rand_nm];
                bisbiglio.Play();
                soundIsPlaying = true;
                StartCoroutine("IncreaseVolume");
                
            }
        }
    }

    IEnumerator IncreaseVolume()
    {
        //possibili problemi con suoni che durano
        //meno di un secondo
        float k = 0f;
        bisbiglio.volume = k;
        while (k < 1f)
        {
            k += Time.deltaTime;
            bisbiglio.volume = k;
            yield return null;
        }
        bisbiglio.volume = 1f;
        while (k < bisbiglio.clip.length) {
            k += Time.deltaTime;
            yield return null;
        }
        soundIsPlaying = false;
    }

}
