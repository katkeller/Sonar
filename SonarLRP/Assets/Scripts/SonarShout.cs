using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class SonarShout : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] yellClip = new AudioClip[6];

    [SerializeField]
    private int[] numberOfRingsToSpawn = new int[6];

    [SerializeField]
    private GameObject sonarObjectPrefab;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float delayBetweenRings = 0.5f, sonarCooldown = 8.0f;

    public bool isTalking = false;

    private bool isYelling = false;
    private AudioSource audioSource;
    private AudioClip clipToPlay;
    private Vector3 playerPosition;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (SteamVR_Actions._default.SonarShout.GetStateDown(SteamVR_Input_Sources.Any) && !isYelling && !isTalking)
        {
            StartCoroutine(SpawnSonar());
        }
    }

    IEnumerator SpawnSonar()
    {
        isYelling = true;
        int randomIndex = Random.Range(0, yellClip.Length);
        audioSource.PlayOneShot(yellClip[randomIndex]);

        for (int i = 0; i < numberOfRingsToSpawn[randomIndex]; i++)
        {
            GameObject a = Instantiate(sonarObjectPrefab) as GameObject;
            playerPosition = player.transform.position;
            a.transform.position = new Vector3(playerPosition.x, playerPosition.y + 1.5f, playerPosition.z);
            yield return new WaitForSeconds(delayBetweenRings);
        }

        yield return new WaitForSeconds(sonarCooldown);
        isYelling = false;
    }
}
