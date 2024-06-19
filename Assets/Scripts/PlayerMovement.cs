using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    public float maxSpeed = 10f; // 최대 속도
    public float forceMultiplier = 10f; //힘의 배수(플레이어의 움직임에 적용되는 힘의 크기), 플레이어가 얼마나 빠르게 가속하는 지를 결정

    public float JumpPower;
    SpriteRenderer sprite;

    private Rigidbody2D rb;
    private float moveInput;

    Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // 점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("IsJump"))
        {
            rb.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            anim.SetBool("IsJump", true);
        }
       
        moveInput = Input.GetAxisRaw("Horizontal");

        // 방향 전환
        sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;

        // 달리는 애니메이션
        if (rb.velocity.x == 0)
        {
            anim.SetBool("IsRun", false);
        }
        else 
        {
            anim.SetBool("IsRun", true);
        }
    }

    void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) < maxSpeed)
        {
            // 물리적 이동 처리
            rb.AddForce(new Vector2(moveInput * forceMultiplier, 0));
        }
        else
        {
            // 최대 속도를 초과한 경우 현재 속도를 유지
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        // Landing Platform //착지 모션 RayCastHit : Ray에 닿은 오브젝트
        if (rb.velocity.y < 0)      // y축의 속도를 보고 내려가는 속도일 때만 Ray를 사용해라.
        {
          Debug.DrawRay(rb.position, Vector2.down, new Color(0,1,0));

        RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector2.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider != null)
        {
            if (rayHit.distance < 1.0f) // distance : Ray에 닿았을 때의 거리
            {                
                anim.SetBool("IsJump", false);
            }
        }  
        }
        
    }
}
