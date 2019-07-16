using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarRing : MonoBehaviour
{
    [SerializeField]
    private float rotationsPerMinute = 8.0f;

    [SerializeField]
    private Vector3 maxScale;

    [SerializeField]
    private float scaleSpeed = 2f, scaleDuration = 5.0f;

    private Vector3 startingScale;
    private bool shouldScale;

    private void Awake()
    {
        startingScale = transform.localScale;
    }

    void Start()
    {
        StartCoroutine(IncreaseScale());
    }

    void Update()
    {
        transform.Rotate(Vector3.one * rotationsPerMinute * Time.deltaTime);
    }

    IEnumerator IncreaseScale()
    {
        float i = 0.0f;
        float rate = (1.0f / scaleDuration) * scaleSpeed;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(startingScale, maxScale, i);
            yield return null;
        }

        //float progress = 0;

        //while (progress <= 1)
        //{
        //    transform.localScale = Vector3.Lerp(startingScale, finalScale, progress);
        //    progress += Time.deltaTime * timeScale;
        //    yield return null;
        //}

        //transform.localScale = finalScale;
    }
}
