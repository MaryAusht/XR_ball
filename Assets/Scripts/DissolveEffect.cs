using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{

    public float dissolveDuration = 2;
    public float dissolveStrength;

    public void StartDissolver()
    {
        StartCoroutine(dissolver());
    }

    public IEnumerator dissolver()
    {
        float elapsedTime = 0;

        Material dissolvematerial = GetComponent<Renderer>().material;

        while (elapsedTime < dissolveDuration)

        {
            elapsedTime += Time.deltaTime;

            dissolveStrength = Mathf.Lerp(0, 1, elapsedTime / dissolveDuration);
            dissolvematerial.SetFloat("_DissolveStrength", dissolveStrength);

            yield return null; 
        }

    }

    private void Update()
    {
       if (Input.GetKeyDown(KeyCode.Space))
        {
            StartDissolver();
        }        
    }

}
