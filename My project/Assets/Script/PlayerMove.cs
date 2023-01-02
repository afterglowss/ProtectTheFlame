using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;
    public float moveSpeed=3.0f;
    Vector2 move = new Vector2();
    Animator anim;
    AudioSource audioSource;
    public bool stopMove;
    

    void Start()
    {
        audioSource = GameObject.Find("SoundManager").GetComponentInChildren<AudioSource>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        stopMove = false;
    }
    void Update()
    {
        UpdateState();
    }
    private void FixedUpdate()
    {
        if (!stopMove)
            MoveCharacter();
    }
    private void MoveCharacter()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        move.Normalize();
        rigid.velocity = move * moveSpeed;
    }
    private void UpdateState()
    {
        if (Mathf.Approximately(move.x, 0) && Mathf.Approximately(move.y, 0))
        {
            audioSource.Stop();
            anim.SetBool("isMove", false);
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                audioSource.pitch = 1.7f;
            }
            anim.SetBool("isMove", true);
        }
        anim.SetFloat("inputx", move.x);
        anim.SetFloat("inputy", move.y);
    }

    
}
