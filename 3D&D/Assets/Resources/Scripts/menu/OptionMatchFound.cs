using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMatchFound : MenuOption
{
    private void StartPlaying(){
        ChangeMusic();
        GoToChair();
    }
    private void GoToChair()
    {
        GameObject orb = GameObject.FindGameObjectWithTag("Orbit");
        orb.GetComponent<AutoRotate>().deactivate();

        GameObject.FindGameObjectWithTag("GvrReticle").gameObject.GetComponent<MeshRenderer>().enabled = true;
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
        StartPlaying();
    }
}
