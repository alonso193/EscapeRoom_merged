using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVInteractive : MonoBehaviour, IInteractive {
    public string interactiveName;
    public GameObject remoteControl;
    public int[] channels;
    public int[] videoIds;
    public VideoClip[] videoClips;
    public int currentChannel;

    private VideoPlayer videoPlayer;
    private bool onState;

	void Start () {
        videoPlayer = transform.Find("VideoPlayer").GetComponent<VideoPlayer>();
	}

    public string GetInteractiveName()
    {
        if (!onState) {
            return interactiveName;
        } else {
            return interactiveName + " (" + channels[currentChannel].ToString() + ")";
        }
    }

    public Dictionary<string, string> GetHitActions(GameObject interactor, GameObject other)
    {
        var hitActions = new Dictionary<string, string>();

        if (other == remoteControl) {
            if (!onState) {
                hitActions["Button_X"] = "Power On";
            } else {
                hitActions["Button_X"] = "Power Off";
                hitActions["Button_Square"] = "Channel Up";
                hitActions["Button_Circle"] = "Channel Down";
            }
        }

        return hitActions;
    }

    public void ExecuteHitAction(string actionName, GameObject interactor, GameObject other)
    {
        switch (actionName)
        {
            case "Power On":
                PowerAction(true);
                break;
            case "Power Off":
                PowerAction(false);
                break;
            case "Channel Up":
                ChannelAction(true);
                break;
            case "Channel Down":
                ChannelAction(false);
                break;
            default:
                Debug.Log("Invalid HitAction");
                break;
        }
    }

    void PowerAction(bool powerState)
    {
        onState = powerState;
        if (onState) {
            SetChannel(currentChannel);
        }
        else {
            videoPlayer.Stop();
        }
    }

    void ChannelAction(bool channelUp)
    {
        currentChannel += channelUp ? 1 : -1;
        if (currentChannel >= channels.Length) {
            currentChannel = 0;
        } else if (currentChannel < 0) {
            currentChannel = channels.Length-1;
        }

        SetChannel(currentChannel);
    }

    void SetChannel(int channel)
    {
        videoPlayer.clip = videoClips[videoIds[currentChannel]];
        //videoPlayer.Stop();
        videoPlayer.Play();
    }

    public Dictionary<string, string> GetCarryActions(GameObject interactor)
    {
        return null;
    }

    public void ExecuteCarryAction(string actionName, GameObject interactor)
    {
    }
	
}
