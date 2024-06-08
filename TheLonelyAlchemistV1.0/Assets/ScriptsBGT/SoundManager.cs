using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //Sound Effects
    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickupItemSound;
    public AudioSource grassWalkSound;
    public AudioSource quickSlotSound;

    public AudioSource bossAttacksPlayer;
    public AudioSource bossDies;
    public AudioSource bossGetsDamage;

    public AudioSource campfireMusic;
    public AudioSource foodReady;

    public AudioSource openBox;
    public AudioSource closeBox;

    public AudioSource eatingSound;

    public AudioSource upgradeSound;

    public AudioSource placementSound;

    public AudioSource treeDead;

    public AudioSource animalGetsDamage;
    public AudioSource eatingFood;
    public AudioSource horseDeath;

    //Music
    public AudioSource startingZoneMusic;
    public AudioSource boss1BattleMusic;
    public AudioSource boss2BattleMusic;
    public AudioSource boss3BattleMusic;
    public AudioSource boss1DeathMusic;

    public AudioSource hittingOre;
    public AudioSource oreBreaking;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if (!soundToPlay.isPlaying)
        {
            soundToPlay.Play(); 
        }
    }

    public void MuteSound(AudioSource soundToPlay)
    {
        if (soundToPlay.isPlaying)
        {
            soundToPlay.Pause();
        }
    }

    public void PlayOneShotMusic(AudioSource soundToPlay)
    {
        soundToPlay.PlayOneShot(soundToPlay.clip);
    }


}
