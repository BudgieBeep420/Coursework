using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Serialization;

public class FaultyLightScript : MonoBehaviour
{
    [SerializeField] private GameObject pointLight;
    [SerializeField] private float smallestTime;
    [SerializeField] private float largestTime;
    [SerializeField] private Material onMaterial;
    [SerializeField] private Material offMaterial;

    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        StartCoroutine(LightFlicker());
    }

    private IEnumerator LightFlicker()
    {
        while (true) {
            pointLight.SetActive(true);
            meshRenderer.material = onMaterial;
            yield return new WaitForSeconds(Random.Range(smallestTime, largestTime));
            pointLight.SetActive(false);
            meshRenderer.material = offMaterial;
            yield return new WaitForSeconds(Random.Range(smallestTime, largestTime));
        }
    }
}
