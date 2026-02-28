using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class SoundFXManagerScript : MonoBehaviour
{
    public static SoundFXManagerScript instance;

    [SerializeField] private AudioSource soundFXobject;
    [SerializeField] private AudioClip sirensSoundFX;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void PlaySoundFX(AudioClip clip, Transform transform, float volume, float speed, float clipLength, float start)
    {
        AudioSource audioSource = Instantiate(soundFXobject, transform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.time = start;
        if (clipLength <= 0)
            clipLength = audioSource.clip.length;
        audioSource.pitch = speed;
        audioSource.Play();
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySoundFX(AudioClip[] clip, Transform transform, float volume)
    {
        int index = Random.Range(0, clip.Length);
        AudioSource audioSource = Instantiate(soundFXobject, transform.position, Quaternion.identity);
        audioSource.clip = clip[index];
        audioSource.volume = volume;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
