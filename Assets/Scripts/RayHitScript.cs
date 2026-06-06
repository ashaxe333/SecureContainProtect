using UnityEngine;

public class RayHitScript : MonoBehaviour
{
    public static RayHitScript Instance;

    public static bool HitTargertFromTo(GameObject broadcast, GameObject target, float distance, LayerMask raycastLayerMask, string color)
    {
        float distanceToPLayer = Vector3.Distance(broadcast.transform.position, target.transform.position);
        Vector3 directionToPlayer = (target.transform.position - broadcast.transform.position).normalized;

        Ray ray = new Ray(broadcast.transform.position, directionToPlayer);
        RaycastHit hit;

        switch (color)
        {
            case "red":
                Debug.DrawRay(broadcast.transform.position, directionToPlayer * 100.0f, Color.red);
                break;
            case "blue":
                Debug.DrawRay(broadcast.transform.position, directionToPlayer * 100.0f, Color.blue);
                break;
            case "green":
                Debug.DrawRay(broadcast.transform.position, directionToPlayer * 100.0f, Color.green);
                break;
            case "yellow":
                Debug.DrawRay(broadcast.transform.position, directionToPlayer * 100.0f, Color.yellow);
                break;
            default:
                Debug.DrawRay(broadcast.transform.position, directionToPlayer * 100.0f, Color.white);
                break;
        }

        if (Physics.Raycast(ray, out hit, distance, raycastLayerMask/*Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore*/))
        {
            //Debug.Log("Trefil jsem: " + hit.collider.gameObject.name);
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject == target.gameObject)
                return true;
            else 
                return false;
        }
        else 
            return false;
    }

    private Color PickColor(string color)
    {
        switch (color)
        {
            case "red":
                return Color.red;
            case "blue":
                return Color.blue;
            case "green":
                return Color.green;
            case "yellow":
                return Color.yellow;
            default:
                return Color.white;
        }
    }
}
