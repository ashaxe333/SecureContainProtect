using UnityEngine;

public class SoundFXManagerScript : MonoBehaviour
{
    public static SoundFXManagerScript instance;

    [SerializeField] private AudioSource doorMovement;
    [SerializeField] private AudioSource doorLockInteraction;
    [SerializeField] private AudioSource lightSwitch;

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void PlayDoorMovement(AudioSource source)
    { 
        
    }
}
