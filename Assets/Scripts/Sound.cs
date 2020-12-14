using UnityEngine.Audio;
using UnityEngine;


[System.Serializable] 
//questo per farlo vedere nell'inspector 

public class Sound{

    public string name;

    public AudioClip clip;
    //qui aggingiamo le varie caratteristiche del suono
    [Range(0f,1f)] public float volume;

    [Range(.1f, 3f)] public float pitch;

    [HideInInspector] public AudioSource source;

    public bool loop;

    //per non farlo vedere nell'inspector perchè tanto lo vediamo nel metodo awake di audio maanager

}
