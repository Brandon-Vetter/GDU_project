using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public GameObject trackPlayer; //Track Player Prefab
    public AudioClip[] tracks; //audio tracks
    private int trackIndex = 0;
    public GameObject[] trackPlayers; //not to be confused with the component called "TrackPlayer"

    // Start is called before the first frame update
    void Start()
    {
        foreach (var track in tracks) //a gameobject with an audio source is created for each track placed in the inspector
        {
            GameObject tp = Instantiate(trackPlayer, transform.position, Quaternion.identity);
            tp.transform.parent = this.transform;
            tp.GetComponent<AudioSource>().clip = track;
            tp.GetComponent<AudioSource>().Play();
            tp.GetComponent<TrackPlayer>().state = "off";
            trackPlayers[trackIndex] = tp;
            trackIndex++;
        }
    }

    void StartTrack(int trackNumber)
    {
        trackPlayers[trackNumber].GetComponent<TrackPlayer>().IncreaseVolume();
    }

    void EndTrack(int trackNumber)
    {
        trackPlayers[trackNumber].GetComponent<TrackPlayer>().DecreaseVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //temporary code for testing
        {
            StartTrack(0);
            StartTrack(1);
            StartTrack(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartTrack(3);
            StartTrack(4);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            StartTrack(5);
            StartTrack(6);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            EndTrack(0);
            EndTrack(1);
            EndTrack(2);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            EndTrack(3);
            EndTrack(4);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EndTrack(5);
            EndTrack(6);
        }
    }



}
