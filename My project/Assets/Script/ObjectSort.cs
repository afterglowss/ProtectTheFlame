using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSort : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void Update()
    {
        spriteRenderer.sortingOrder=(int)(transform.position.y*-100);

    }
}
