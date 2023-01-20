using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager{

    public static AudioClip ping;


    public static void PlayPingSound()
    {
        GameObject soundSource = new GameObject("Sound");
        AudioSource source = soundSource.AddComponent<AudioSource>();
        ping = Resources.Load<AudioClip>("Sounds/ping");
        source.PlayOneShot(ping);
    }

    

}
