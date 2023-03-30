using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopImage : MonoBehaviour
{
    SpriteRenderer thisSprite;

    private void Awake()
    {
        thisSprite = GetComponent<SpriteRenderer>();
    }

    private void OnBecameInvisible()
    {
        transform.position += Vector3.up * 2 * thisSprite.bounds.size.y;
    }
}
