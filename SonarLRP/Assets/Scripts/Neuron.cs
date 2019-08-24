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

    [SerializeField]
    private float timeBetweenLightBandIllumination = 0.1f;

    [SerializeField]
    private GameObject neuronTopper;

    [SerializeField]
    private GameObject[] lightBands = new GameObject[10];

    //[SerializeField]
    //private Color emissionColor;

    [SerializeField]
    private Shader topperStartingShader, startingShader, glowShader;

    private bool isActivated = false;
    private Vector3 neuronPosition;
    private Material topperMaterial;
    private Color startColor;

    private MeshCollider triggerCollider;
    private AudioSource audioSource;

    void Awake()
    {
        triggerCollider = GetComponent<MeshCollider>();
        audioSource = GetComponent<AudioSource>();
        neuronPosition = transform.position;
        topperMaterial = neuronTopper.GetComponent<Material>();
        //startColor = topperMaterial.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerSonar" && !isActivated)
        {
            Debug.Log("PlayerSonar has activated the Neuron trigger.");
            isActivated = true;
            StartCoroutine(SpawnSonarRings());
            StartCoroutine(LightUpBands());
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
        topperMaterial.shader = topperStartingShader;

        for (int j = 0; j > lightBands.Length; j++)
        {
            Material material = lightBands[j].GetComponent<Material>();
            material.shader = startingShader;
        }

        isActivated = false;
    }

    IEnumerator LightUpBands()
    {
        //topperMaterial.SetColor("_EmissionColor", emissionColor);
        topperMaterial.shader = glowShader;
        yield return new WaitForSeconds(timeBetweenLightBandIllumination);

        for (int e = 0; e > lightBands.Length; e++)
        {
            Material material = lightBands[e].GetComponent<Material>();
            material.shader = glowShader;
            //material.SetColor("_EmissionColor", emissionColor);
            yield return new WaitForSeconds(timeBetweenLightBandIllumination);
        }
    }
}
