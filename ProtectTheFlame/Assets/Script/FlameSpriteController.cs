using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameSpriteController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite fire1;
    public Sprite fire2;
    public Sprite fire3;
    void Start()
    {
        
    }
    
    void Update()
    {
        if (FlameSliderController.FlameGage >= 300f && spriteRenderer.sprite != fire1)
        {
            spriteRenderer.sprite = fire1;
        }
        else if (FlameSliderController.FlameGage < 300f &&
            FlameSliderController.FlameGage > 0 && spriteRenderer.sprite != fire2)
        {
            spriteRenderer.sprite = fire2;
        }
        else if (FlameSliderController.FlameGage <= 0 && spriteRenderer.sprite != fire3)
        {
            spriteRenderer.sprite = fire3;
        }
    }
}
