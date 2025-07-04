using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public AudioClip deathClip;
    public float jumpForce = 700f;
    public int jumpcount = 0;
    public bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody2D playerRigidbody;
    private Animator animator;
    private AudioSource playerAudio;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        if (playerRigidbody == null)
            Debug.LogError($"PlayerController: Rigidbody2D가 할당되지 않았습니다. (오브젝트명: {gameObject.name})");
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogError($"PlayerController: Animator가 할당되지 않았습니다. (오브젝트명: {gameObject.name})");
        playerAudio = GetComponent<AudioSource>();
        if (playerAudio == null)
            Debug.LogError($"PlayerController: AudioSource가 할당되지 않았습니다. (오브젝트명: {gameObject.name})");
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame && jumpcount < 2)
        {
            jumpcount++;
            playerRigidbody.linearVelocity = Vector2.zero; // Reset velocity before applying jump force
            playerRigidbody.AddForce(new Vector2(0, jumpForce));
            playerAudio.Play();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasReleasedThisFrame && playerRigidbody.linearVelocity.y > 0)
        {
            playerRigidbody.linearVelocity = playerRigidbody.linearVelocity * 0.5f; // Reduce upward velocity when mouse button is released
        }
        animator.SetBool("Grounded", isGrounded);
    }

    private void Die()
    {
        if (animator != null)
            animator.SetTrigger("Die");
        if (playerAudio != null)
        {
            playerAudio.clip = deathClip;
            playerAudio.Play();
        }
        if (playerRigidbody != null)
            playerRigidbody.linearVelocity = Vector2.zero; // Reset velocity to prevent further movement
        isDead = true;
        // 게임 매니저에 플레이어 사망 알림
        if (GameManager.instance != null)
            GameManager.instance.OnPlayerDead();
        else
            Debug.LogWarning("PlayerController: GameManager.instance가 null입니다. OnPlayerDead()를 호출할 수 없습니다.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Dead" && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.7f)
        {
            isGrounded = true;
            jumpcount = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
