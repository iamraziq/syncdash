using UnityEngine;
using System.Collections.Generic;

public class GhostObjectSync : MonoBehaviour
{
    [Header("Ghost Sync Settings")]
    public float delay = 0.3f;
    public float interpolationSpeed = 10f;
    public bool enableRotation = false;

    private struct ObjectState
    {
        public Vector3 position;
        public Quaternion rotation;
        public float timestamp;

        public ObjectState(Vector3 pos, Quaternion rot, float time)
        {
            position = pos;
            rotation = rot;
            timestamp = time;
        }
    }

    private Queue<ObjectState> stateBuffer = new Queue<ObjectState>();

    private void LateUpdate()
    {
        float currentTime = Time.time;

        // Record the object's current state (for ghost sync)
        stateBuffer.Enqueue(new ObjectState(transform.position, transform.rotation, currentTime));

        // Process delay
        while (stateBuffer.Count > 0 && stateBuffer.Peek().timestamp <= currentTime - delay)
        {
            ObjectState ghostState = stateBuffer.Dequeue();
            ApplyGhostSync(ghostState);
        }
    }

    void ApplyGhostSync(ObjectState state)
    {
        // Only sync rendering position (for ghost camera)
        transform.position = Vector3.Lerp(transform.position, state.position, Time.deltaTime * interpolationSpeed);

        if (enableRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, state.rotation, Time.deltaTime * interpolationSpeed);
        }
    }
}
