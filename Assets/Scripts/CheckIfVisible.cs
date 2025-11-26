using UnityEngine;

public class CheckIfVisible : MonoBehaviour
{
    // Objekt, který chceme testovat (napø. kostka)
    public Transform target;

    private Camera cam;

    void Start()
    {
        cam = Camera.main; // vezmeme hlavní kameru
    }

    void Update()
    {
        if (target == null) return;

        // 1) Pøevod pozice objektu do Viewport prostoru (0–1)
        Vector3 viewportPos = cam.WorldToViewportPoint(target.position);

        // 2) Objekt musí být pøed kamerou (z > 0)
        bool inFront = viewportPos.z > 0;

        // 3) Musí být uvnitø viewportu (x a y mezi 0 a 1)
        bool insideViewport = viewportPos.x > 0 && viewportPos.x < 1 &&
                              viewportPos.y > 0 && viewportPos.y < 1;

        // 4) Finální podmínka
        bool isVisible = inFront && insideViewport;

        if (isVisible)
        {
            Debug.Log("Kamera vidí kostku!");
        }
        else
        {
            Debug.Log("Kamera nevidí kostku.");
        }
    }
}
