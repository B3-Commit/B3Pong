using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnDeath : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    void OnDestroy()
    {
        if (gameObject.scene.isLoaded) 
        {
            // Was Deleted
            AudioManager.Instance.PlayEffect(clip);
        }
    }
}
