
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    //qui gruppiamo tutti i suoni in una lista

    public static AudioManager instance; // lo uso per evitare che un suono nizia quando è gia in funzione un altro

    // Start is called before the first frame update (ho cambiato il nome ma è uguale) 
    //questi suoni si sentiranno quando metti play, e anche tutte le caratteristiche scritte dentro nell'inspector si vedono quando metti play

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        } //se vogliamo distruggee il suono iniziato prima (DOPRATTUTTO QUANDO SI DEVE CAMBIARE SCENA)

        DontDestroyOnLoad(gameObject); //per non interrompere dopo che ha iniziato

        foreach (Sound s in sounds) //per aggiungere il suono alla lista di suoni
        {
            s.source= gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            //per visualizzarlo in ogni audio source
        }
    }

   void Start()
   {
        foreach (Sound s in sounds)
        {
            Play(s.name);
        }
   } 
   //SERVE PER FAR PARTIRE L'AUDIO QUANDO INIZIA L'ESPERIENZA


    // Update is called once per frame
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + "not found!");
            return; //evitiamo errori
        }
        s.source.Play();
        //da errore se non ha il giusto nome
    }
}
