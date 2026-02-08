using UnityEngine;

public class InteractionUIManager : MonoBehaviour
{
    public static InteractionUIManager Instance { get; private set; }
    public GameObject hand;
    public bool lockInteractState;
    public bool objectInteractState;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        lockInteractState = false;
        objectInteractState = false;
    }

    public void SetLockInteractState(bool state)
    {
        lockInteractState = state;
        UpdateIcon();
    }

    public void SetObjectInteractState(bool state)
    {
        objectInteractState = state;
        UpdateIcon();
    }

    public void UpdateIcon()
    {
        hand.SetActive(lockInteractState || objectInteractState);
    }
}
