using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 newPos = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * 5f);
        }
    }
}
