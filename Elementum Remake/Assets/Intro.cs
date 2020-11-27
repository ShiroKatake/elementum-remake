using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class Intro : MonoBehaviour
{
    public PlayableDirector dir;

    void Start()
    {
        Bind(dir, "Animation Track", GameObject.Find("Player"));
        Bind(dir, "Signal Track", GameObject.Find("SceneController"));
    }

    public static void Bind(PlayableDirector director, string trackName, GameObject animator)
    {
        var timeline = director.playableAsset as TimelineAsset;
        foreach (var track in timeline.GetOutputTracks())
        {
            if (track.name == trackName)
            {
                director.SetGenericBinding(track, animator);
                break;
            }
        }
    }
}
