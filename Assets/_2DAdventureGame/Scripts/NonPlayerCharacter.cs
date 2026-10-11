using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    public string[] dialogueLines;
    public string greetingLine = "Oh... You're finally here."; 

    private bool hasGreeted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player") && !hasGreeted)
        {
            hasGreeted = true;

            
            UIHandler.instance.ShowGreeting(greetingLine);
        }
    }
}