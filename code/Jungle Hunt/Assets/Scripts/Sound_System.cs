using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sound_System : MonoBehaviour {

    [SerializeField] private GameObject sfxPrefab;
    private GameObject bgm;
    private AudioSource bgmAS;
    public List<GameObject> sfxList = new List<GameObject>();
    private float startFade;
    private bool fadingOut = false;
    private bool fadingIn = false;

    public List<AudioClip> tracks = new List<AudioClip>();
    public List<AudioClip> sounds = new List<AudioClip>();

    // Use this for initialization
    void Start ()
    {
        bgm = GameObject.Find("BG_Music").gameObject;
        
        bgmAS = bgm.GetComponent<AudioSource>();
        InstantiateTrackList();
        ChangeBG(0);
    }

    public void ChangeBG(int trackid)
    {
        StartCoroutine(WaitForFadeOut(trackid));
    }

    public void FadeOut()
    {
        startFade = bgmAS.volume;
        fadingOut = true;
    }

    public void PlaySFX(int sfxid)
    {
        if (sfxid < 0 || sfxid >= sounds.Count)
        {
            Debug.Log(string.Format("SOUND INDEX OVERFLOW, Trying to Access unexisting clip #{0,2}", sfxid));
        }
        else
        {
            GameObject sfx = Instantiate(sfxPrefab, this.transform);
            sfx.GetComponent<AudioSource>().clip = sounds[sfxid];
            sfxList.Add(sfx);
            sfx.GetComponent<AudioSource>().Play();
        }
    }

	// Update is called once per frame
	void Update ()
    {
        if (fadingOut)
        {
            if (bgmAS.volume == 0)
            {
                fadingOut = false;
            }
            else
            {
                bgmAS.volume -= (startFade / 80);
            }
        }
        else if (fadingIn)
        {
            if (bgmAS.volume >= startFade)
            {
                fadingIn = false;
            }
            else
            {
                bgmAS.volume += (startFade / 40);
            }
        }
        for (int a = 0; a < sfxList.Count; a++)
        {
            if (sfxList[a].GetComponent<AudioSource>().isPlaying == false)
            {
                Destroy(sfxList[a]);
                sfxList.RemoveAt(a);
            }
        }
    }

    IEnumerator WaitForFadeOut(int trackid)
    {
        while (fadingOut == true)
        {
            yield return null;
        }
        fadingIn = true;
        if (trackid < 0 || trackid >= tracks.Count)
        {
            Debug.Log(string.Format("TRACK INDEX OVERFLOW, Trying to Access unexisting clip #{0,2}", trackid));
        }
        else if (tracks[trackid] != bgmAS.clip)
        {
            bgmAS.clip = tracks[trackid];
            bgmAS.Play();
        }
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
}
