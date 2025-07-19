using UnityEngine;

public class Orb : MonoBehaviour
{
    public float despawnZ = -15f;

    private void Update()
    {
        float speed = GameManager.Instance.CurrentSpeed;
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.z < despawnZ)
        {
            gameObject.SetActive(false);
        }
    }

    public void Activate(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddScore(1);
            GameManager.Instance.PlayBurstEffect();
            gameObject.SetActive(false);
        }
    }
}


