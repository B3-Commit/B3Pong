using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnSpawn : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    void Start()
    {
        AudioManager.Instance.PlayEffect(clip);
    }
}
