using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sound_System : MonoBehaviour {

    private GameObject sfx;
    private GameObject bgm;

    public List<AudioClip> tracks = new List<AudioClip>();
    public List<AudioClip> sounds = new List<AudioClip>();

    // Use this for initialization
    void Start ()
    {
        sfx = GameObject.Find("SoundFx").gameObject;
        bgm = GameObject.Find("BG_Music").gameObject;
        var loadedObjects = Resources.LoadAll("Sounds/BG/", typeof(AudioClip)).Cast<AudioClip>();
        foreach (var track in loadedObjects)
        {
            tracks.Add(track);
        }
        ChangeBG(0);
    }

    public void ChangeBG(int trackid)
    {
        if (trackid > tracks.Count)
        {
            Debug.Log(string.Format("TRACK INDEX OVERFLOW, Trying to Access clip #{0,2}", trackid));
        }
        else
        {
            bgm.GetComponent<AudioSource>().clip = tracks[trackid];
            bgm.GetComponent<AudioSource>().Play();
        }
    }

	// Update is called once per frame
	void Update ()
    {
		
	}
}
