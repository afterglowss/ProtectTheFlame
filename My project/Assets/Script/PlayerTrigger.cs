using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerTrigger : MonoBehaviour
{
    Rigidbody2D rigid;
    FlameSliderController fsc;
    PileSliderController psc;
    TemparatureSliderController tsc;
    Animator anim;
    PlayerMove pm;

    public void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        tsc = FindObjectOfType<TemparatureSliderController>();
        anim = GetComponent<Animator>();
        pm = FindObjectOfType<PlayerMove>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            tsc.stopSlider = true;
        }
        else if (other.gameObject.CompareTag("Pile"))
        {

        }
        else if (other.gameObject.CompareTag("Junk"))
        {
            
        }
        else if (other.gameObject.CompareTag("Table"))
        {
            
        }
        else if (other.gameObject.CompareTag("Tent"))
        {
            
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetBool("isFanning", true);
                pm.stopMove = true;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                anim.SetBool("isFanning", false);
                Debug.Log("Stay함수 입력중");
                pm.stopMove = false;
            }
        }
        else if (other.gameObject.CompareTag("Pile"))
        {

        }
        else if (other.gameObject.CompareTag("Junk"))
        {

        }
        else if (other.gameObject.CompareTag("Table"))
        {

        }
        else if (other.gameObject.CompareTag("Tent"))
        {

        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Flame"))
        {
            tsc.stopSlider = false;
            anim.SetBool("isFanning", false);
        }
        else if (other.gameObject.CompareTag("Pile"))
        {

        }
        else if (other.gameObject.CompareTag("Junk"))
        {

        }
        else if (other.gameObject.CompareTag("Table"))
        {

        }
        else if (other.gameObject.CompareTag("Tent"))
        {

        }
    }
    
}