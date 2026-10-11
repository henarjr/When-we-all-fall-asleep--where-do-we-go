using UnityEngine;

public class AmmoCollectible : MonoBehaviour
{
    public int ammoAmount = 4;
    public AudioClip collectSound;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.AddAmmo(ammoAmount);
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}