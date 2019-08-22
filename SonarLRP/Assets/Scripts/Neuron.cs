using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron : MonoBehaviour
{
    [SerializeField]
    private AudioClip activationSFX;

    [SerializeField]
    private GameObject neuronSonarPrefab;

    [SerializeField]
    private float delayBetweenRings = 0.5f, sonarCooldown = 5.0f;

    [SerializeField]
    private int numberOfRingsToSpawn = 3;

    private bool isActivated = false;
    private Vector3 neuronPosition;

    private MeshCollider triggerCollider;
    private AudioSource audioSource;

    void Awake()
    {
        triggerCollider = GetComponent<MeshCollider>();
        audioSource = GetComponent<AudioSource>();
        neuronPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerSonar" && !isActivated)
        {
            isActivated = true;
            StartCoroutine(SpawnSonarRings());
        }
    }

    IEnumerator SpawnSonarRings()
    {
        audioSource.PlayOneShot(activationSFX);
        
        for (int i = 0; i > numberOfRingsToSpawn; i++)
        {
            GameObject a = Instantiate(neuronSonarPrefab) as GameObject;
            a.transform.position = new Vector3(neuronPosition.x, neuronPosition.y + 1.5f, neuronPosition.z);
            yield return new WaitForSeconds(delayBetweenRings);
        }

        yield return new WaitForSeconds(sonarCooldown);
        isActivated = false;
    }
}
