using UnityEngine;
using System.Collections.Generic;

public class GhostPlayerController : MonoBehaviour
{
    [Header("Sync Settings")]
    public Transform playerTransform;
    public float delay = 0.3f; 
    public float interpolationSpeed = 10f;

    private struct PlayerState
    {
        public Vector3 position;
        public Quaternion rotation;
        public float timestamp;

        public PlayerState(Vector3 pos, Quaternion rot, float time)
        {
            position = pos;
            rotation = rot;
            timestamp = time;
        }
    }

    private Queue<PlayerState> stateBuffer = new Queue<PlayerState>();

    void Update()
    {
        if (playerTransform == null)
        {
            Destroy(gameObject, delay); 
            enabled = false; 
            return;
        }
        float currentTime = Time.time;

        stateBuffer.Enqueue(new PlayerState(playerTransform.position, playerTransform.rotation, currentTime));

        while (stateBuffer.Count > 0 && stateBuffer.Peek().timestamp <= currentTime - delay)
        {
            PlayerState targetState = stateBuffer.Dequeue();
            ApplyState(targetState);
        }
    }

    void ApplyState(PlayerState state)
    {
        transform.position = Vector3.Lerp(transform.position, state.position, Time.deltaTime * interpolationSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, state.rotation, Time.deltaTime * interpolationSpeed);
    }
}
