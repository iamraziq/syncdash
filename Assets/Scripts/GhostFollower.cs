using UnityEngine;
using System.Collections.Generic;

public class GhostFollower : MonoBehaviour
{
    private Transform target;
    private float delay;
    private Queue<Vector3> positionBuffer = new Queue<Vector3>();
    private Queue<float> timeBuffer = new Queue<float>();

    private bool originalDisabled = false;
    private float disableTime;

    public void Init(Transform targetTransform, float delayTime)
    {
        target = targetTransform;
        delay = delayTime;
    }

    private void Update()
    {
        if (!target) return;

        if (!originalDisabled && !target.gameObject.activeInHierarchy)
        {
            originalDisabled = true;
            disableTime = Time.time + delay;
        }

        if (originalDisabled && Time.time >= disableTime)
        {
            gameObject.SetActive(false);
            return;
        }

        positionBuffer.Enqueue(target.position);
        timeBuffer.Enqueue(Time.time);

        while (timeBuffer.Count > 0 && Time.time - timeBuffer.Peek() >= delay)
        {
            transform.position = positionBuffer.Dequeue();
            timeBuffer.Dequeue();
        }
    }
}

