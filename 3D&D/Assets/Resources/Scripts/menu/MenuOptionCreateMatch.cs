using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionCreateMatch : MonoBehaviour, SelectableMenuOption
{
    public void Execute()
    {
        waitForPlayer();
        startPlaying();
    }
    private void waitForPlayer()
    {

    }

    
    private void startPlaying(){
        ChangeMusic();
        goToChair();
    }
    
    private void goToChair()
    {
        GameObject orb = GameObject.FindGameObjectWithTag("Orbit");
        orb.GetComponent<AutoRotate>().deactivate();
    }
    private IEnumerator ChangeMusic()
    {

        yield return new WaitForSeconds(0.1f);

        GameObject music = GameObject.FindGameObjectWithTag("MusicPlayer");
        music.GetComponent<AudioSource>().Stop();
        music = GameObject.FindGameObjectWithTag("MusicPlayerGame");
        music.GetComponent<AudioSource>().Play();

    }
}
