using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPlayer : MonoBehaviour
{
    public string state = "on";
    private float t = 0f;
    private AudioSource aus;

    // Start is called before the first frame update
    void Start()
    {
        aus = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            default:
                break;
            case "on":
                aus.volume = 1f;
                break;
            case "off":
                aus.volume = 0f;
                break;
            case "increasing": //increases volume until it's at max, then changes state
                aus.volume = Mathf.Lerp(0f, 1f, t);
                t += 0.6f * Time.deltaTime;
                if (aus.volume == 1f)
                {
                    t = 0f;
                    state = "on";
                }
                break;
            case "decreasing": //same thing as increasing
                aus.volume = Mathf.Lerp(1f, 0f, t);
                t += 0.6f * Time.deltaTime;
                if (aus.volume == 0f)
                {
                    t = 0f;
                    state = "off";
                }
                break;
        }
    }


    public void IncreaseVolume()
    {
        if (state == "decreasing" || state == "off") //fixes a bug where you can make the volume increase while its already increasing, restarting the process
        {
            state = "increasing";
        }
        else
        {
            return;
        }
    }


    public void DecreaseVolume() //fixes a similar bug
    {
        if (state == "increasing" || state == "on")
        {
            state = "decreasing";
        }
        else
        {
            return;
        }
    }
}
