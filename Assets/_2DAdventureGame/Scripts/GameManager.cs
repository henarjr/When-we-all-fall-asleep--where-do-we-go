using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public PlayerController player;
    EnemyController[] enemies;
    public UIHandler uiHandler;

    int enemiesFixed = 0;
    public string returnToNPCMessage = "You fixed them all! Go back.";
    void Start()
    {
        player.OnTalkedToNPC += HandlePlayerTalkedToNPC;
        enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        foreach (var enemy in enemies)
        {
            enemy.OnFixed += HandleEnemyFixed;
        }
        uiHandler.SetCounter(0, enemies.Length);
    }

    void Update()
    {
        // Lose condition
        if (player.health <= 0)
        {
            uiHandler.DisplayLoseScreen();
            Invoke(nameof(ReloadScene), 3f);
        }

        // Win condition
        //if (AllEnemiesFixed())
        //{
          //  uiHandler.DisplayWinScreen();
          //  Invoke(nameof(ReloadScene), 3f);
       // }
    }

    void HandlePlayerTalkedToNPC(string[] lines)
    {
        

        if (AllEnemiesFixed())
        {
            
            uiHandler.DisplayWinScreen();
            Invoke(nameof(LoadStartScreen), 3f);
        }
        else
        {
            
            UIHandler.instance.StartDialogue(lines);
        }
    }

    bool AllEnemiesFixed()
    {
        foreach (EnemyController enemy in enemies)
        {
            if (enemy.isBroken) return false;
        }
        return true;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void HandleEnemyFixed()
    {
        enemiesFixed++;
        uiHandler.SetCounter(enemiesFixed, enemies.Length);
        if (enemiesFixed == enemies.Length)
        {
            UIHandler.instance.ShowGreeting(returnToNPCMessage);
        }
    }
    void LoadStartScreen()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
