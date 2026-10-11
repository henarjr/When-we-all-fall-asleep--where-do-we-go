using UnityEngine;

public class HealingZone : MonoBehaviour
{
    public float healingCooldown = 2.0f; 
    float timer;

    void Update()
    {
        
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            
            if (timer <= 0)
            {
                controller.ChangeHealth(1); 
                timer = healingCooldown;    
            }
        }
    }
}