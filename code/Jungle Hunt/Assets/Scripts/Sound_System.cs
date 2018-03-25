using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sound_System : MonoBehaviour {

    private GameObject sfx;
    private GameObject bgm;
    private AudioSource bgmAS;
    private AudioSource sfxAS;
    private float startfade;
    private bool f_out = false;
    private bool f_in = false;

    public List<AudioClip> tracks = new List<AudioClip>();
    public List<AudioClip> sounds = new List<AudioClip>();

    private int loadedSFX = -1;

    // Use this for initialization
    void Start ()
    {
        sfx = GameObject.Find("SoundFx").gameObject;
        bgm = GameObject.Find("BG_Music").gameObject;
        sfxAS = sfx.GetComponent<AudioSource>();
        bgmAS = bgm.GetComponent<AudioSource>();
        InstantiateTrackList();
        ChangeBG(0);
    }

    private void InstantiateTrackList()
    {
        var loadedBGMs = Resources.LoadAll("Sounds/BG/", typeof(AudioClip)).Cast<AudioClip>();
        foreach (var track in loadedBGMs)
        {
            tracks.Add(track);
        }
        var loadedSFXs = Resources.LoadAll("Sounds/SFX/", typeof(AudioClip)).Cast<AudioClip>();
        foreach (var sound in loadedSFXs)
        {
            sounds.Add(sound);
        }
    }

    public void ChangeBG(int trackid)
    {
        f_in = true;
        if (trackid > tracks.Count)
        {
            Debug.Log(string.Format("TRACK INDEX OVERFLOW, Trying to Access unexisting clip #{0,2}", trackid));
        }
        else if (tracks[trackid] != bgmAS.clip)
        {
            bgmAS.clip = tracks[trackid];
            bgmAS.Play();
        }
    }

    public void FadeOut()
    {
        startfade = bgmAS.volume;
        f_out = true;
    }

    public void PlaySFX(int sfxid)
    {
        if (sfxid == loadedSFX)
        {
            sfxAS.Play();
        }
        else if (sfxid > sounds.Count)
        {
            Debug.Log(string.Format("SOUND INDEX OVERFLOW, Trying to Access unexisting clip #{0,2}", sfxid));
        }
        else
        {
            sfxAS.clip = sounds[sfxid];
            sfxAS.Play();
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (f_out)
        {
            if (bgmAS.volume == 0)
            {
                f_out = false;
            }
            else
            {
                bgmAS.volume -= (startfade / 80);
            }
        }
        else if (f_in)
        {
            if (bgmAS.volume >= startfade)
            {
                f_in = false;
            }
            else
            {
                bgmAS.volume += (startfade / 40);
            }
        }
    }
}
