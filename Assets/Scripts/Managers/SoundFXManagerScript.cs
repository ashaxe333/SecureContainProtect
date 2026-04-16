using System.Security.Cryptography;
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

    /// <summary>
    /// Pøehraje customizovaný sound
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="transform"></param>
    /// <param name="volume"></param>
    /// <param name="speed"></param>
    /// <param name="clipLength"></param>
    /// <param name="start"></param>
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

    /// <summary>
    /// Pøehraje náhodný sound z pøedaného listu
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="transform"></param>
    /// <param name="volume"></param>
    /// <param name="forScp939"></param>
    public void PlaySoundFX(AudioClip[] clip, Transform transform, float volume, bool forScp939)    // Prozatím vyøeším zvuky scp939 takhle žebrácky
    {
        int index = Random.Range(0, clip.Length);
        AudioSource audioSource = Instantiate(soundFXobject, transform.position, Quaternion.identity);
        audioSource.clip = clip[index];
        audioSource.volume = volume;

        // chat
        if (forScp939)
        {
            audioSource.rolloffMode = AudioRolloffMode.Custom;
            AnimationCurve curve = new AnimationCurve();

            audioSource.minDistance = 3.0f;  // plná hlasitost do 2 metrù
            audioSource.maxDistance = 60f; // slyšitelný do 20 metrù

            curve.AddKey(0f, 1f);
            curve.AddKey(0.3f, 0.9f);  // pomalejší pokles na zaèátku
            curve.AddKey(0.6f, 0.4f);
            curve.AddKey(0.85f, 0.1f);
            curve.AddKey(1f, 0f);

            audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
        }
        // chat

        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
