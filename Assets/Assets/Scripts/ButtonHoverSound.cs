using UnityEngine;

public class ButtonHoverSound : MonoBehaviour
{
    public AudioSource sfxAudioSource;    
    
    public AudioClip clickSound;    
    public AudioClip hoverSound;    
    

    public void HoverSound()
    {
        if (hoverSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(hoverSound);
        }
    }

    public void ClickSound()
    {
        if (clickSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(clickSound);
        }
    }
 
}
