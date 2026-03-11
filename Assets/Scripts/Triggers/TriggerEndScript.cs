using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerEndScript : MonoBehaviour
{
    //NÁPADY:
    // 1) V budoucnu spustí cutscenu útìku

    public static TriggerEndScript instance;

    private bool isEnabled;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isEnabled)
        {
            SceneManager.LoadScene(3);
        }
    }

    /// <summary>
    /// Povolí spuštìní konce (v budoucnu cutscény útìku)
    /// </summary>
    /// <param name="enabled"> bool </param>
    public void SetEnabled(bool enabled)
    {
        isEnabled = enabled;
    }
}
