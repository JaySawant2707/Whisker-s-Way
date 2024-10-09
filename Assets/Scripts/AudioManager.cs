using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("-----Audio Sources-----")]
    public AudioSource Music;
    public AudioSource SFX;
    [Header("-----Audio Clips-----")]
    public AudioClip MMbackground;
    public AudioClip Lvbackground;
    public AudioClip Button;
    public AudioClip DoorOpen;
    public AudioClip Shoot;
    public AudioClip Death;
    public AudioClip laserShoot;
    public AudioClip Slash;
    public AudioClip CoinCollect;
    public AudioClip Blast;

    private void Start()
    {

    }
    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

    public void ButtonClick()
    {
        PlaySFX(Button);
    }
}
/************How to use in other scripts**********
 *  
 *  
    AudioManager audioManager;          ...Declare this on top of script
    
   
    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();          ...Put this in start function
   

    audioManager.PlaySFX(audioManager.Death);     ......put this where you wants sound to play and
                                                            change name of clip as your requirements e.g. Death
*/