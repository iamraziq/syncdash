using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float despawnZ = -15f;
    public Material targetMaterial; 
    private Coroutine shaderCoroutine;
    private bool isHit = false;
    private void Start()
    {
        targetMaterial = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        if(!isHit)
        {
            float speed = GameManager.Instance.CurrentSpeed;
            transform.Translate(Vector3.back * speed * Time.deltaTime);

            if (transform.position.z < despawnZ)
            {
                gameObject.SetActive(false);
            }
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
            isHit = true;
            GameManager.Instance.GameOver();
            StartDissolve(targetMaterial);
            Destroy(other.gameObject);
        }
    }

    public void StartDissolve(Material dissolveMaterial)
    {
        if (shaderCoroutine != null)
            StopCoroutine(shaderCoroutine);

        shaderCoroutine = StartCoroutine(LerpShader(dissolveMaterial));
    }
    private IEnumerator LerpShader(Material lerpMaterial)
    {
        float duration = 1f;
        float elapsed = 0f;

        float startTimeValue = lerpMaterial.GetFloat("_time");

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newTime = Mathf.Lerp(startTimeValue, 1f, elapsed / duration);
            targetMaterial.SetFloat("_time", newTime);
            yield return null;
        }

        targetMaterial.SetFloat("_time", 1f); 
    }
}


