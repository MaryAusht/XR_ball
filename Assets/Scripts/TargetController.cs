using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetController : MonoBehaviour, ITargetInterface


{
    public void TargetShot()
    {
        Destroy(gameObject);
    }

    public void PlayAudio()
    {

    }

    public void PlayAnimation()
    {

    }
}
