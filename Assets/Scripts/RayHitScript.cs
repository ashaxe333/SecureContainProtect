using UnityEngine;

public class RayHitScript : MonoBehaviour
{
    public static RayHitScript instance;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }
    //public Transform broadcast;

    public bool HitTargertFromTo(GameObject broadcast, GameObject target, float distance, LayerMask raycastLayerMask)
    {
        float distanceToPLayer = Vector3.Distance(broadcast.transform.position, target.transform.position);
        Vector3 directionToPlayer = (target.transform.position - broadcast.transform.position).normalized;

        Ray ray = new Ray(broadcast.transform.position, directionToPlayer);
        RaycastHit hit;

        Debug.DrawRay(broadcast.transform.position, directionToPlayer * 100.0f, Color.red);

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
}
