using UnityEngine;

public class RestartRigActivator : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Activating Restart Rig");
        gameObject.SetActive(true);
    }
}