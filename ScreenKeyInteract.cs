using UnityEngine;
using UnityEngine.Video;

public class ScreenKeyInteract : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private bool playerNear;

    //玩家走进感应范围
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerNear = true;
    }
    //玩家离开感应范围
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerNear = false;
    }

    private void Update()
    {
        //站在范围内按E切换播放/暂停
        if(playerNear && Input.GetKeyDown(KeyCode.E))
        {
            if(videoPlayer.isPlaying) videoPlayer.Pause();
            else videoPlayer.Play();
        }
    }
}