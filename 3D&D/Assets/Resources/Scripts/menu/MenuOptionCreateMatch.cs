using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionCreateMatch : MenuOption
{
    private void WaitForPlayer()
    {

    }
    private void StartPlaying(){
        ChangeMusic();
        GoToChair();
    }
    
    private void GoToChair()
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

    public override void Execute()
    {
        WaitForPlayer();
        StartPlaying();
    }
}
