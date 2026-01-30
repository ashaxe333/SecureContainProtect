using NUnit.Framework;
using UnityEngine;

public class GasTrigger : MonoBehaviour
{
    public bool isSafe = true;

    private void OnTriggerEnter(Collider other)
    {
        //if (other == player) {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isSafe)
            {
                if (!PlayerDamageManager.instance.isTakingGas)
                    PlayerDamageManager.instance.isTakingGas = true;
            }
            else {
                PlayerDamageManager.instance.isTakingGas = false;
            }
        }
    }
}
