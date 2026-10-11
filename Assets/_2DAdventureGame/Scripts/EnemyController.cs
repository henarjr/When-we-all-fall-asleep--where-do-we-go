using UnityEngine;
using System;

public class EnemyController : MonoBehaviour
{
    // Public variables
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public int hitsToFix = 2;

    // Private variables
    Rigidbody2D rigidbody2d;
    Animator animator;
    float timer;
    int direction = 1;

    int currentHits = 0;

    bool broken = true;

    public bool isBroken { get { return broken; } }

    AudioSource audioSource;
    public AudioClip fixSound;

    public ParticleSystem smokeParticleEffect;

    public event Action OnFixed;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 position = rigidbody2d.position;

        if (vertical)
        {
            position.y = position.y + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + speed * direction * Time.deltaTime;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        rigidbody2d.MovePosition(position);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        currentHits++;

        if (currentHits >= hitsToFix)
        {
            smokeParticleEffect.Stop();
            broken = false;
            rigidbody2d.simulated = false;
            animator.SetTrigger("Fixed");
            audioSource.Stop();
            if (audioSource != null && fixSound != null)
            {
                audioSource.PlayOneShot(fixSound);
            }
            OnFixed?.Invoke();
        }
    }
}