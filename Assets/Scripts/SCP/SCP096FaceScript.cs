using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SCP096FaceScript : MonoBehaviour
{
    public GameObject face;
    public bool isSeen = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool FaceIsSeen()
    {
        if (face.GetComponent<Renderer>().isVisible)
        {
            Debug.Log("bacha");
            return true;
        }
        else
        {
            Debug.Log("dobrý");
            return false;
        }
    }
}
