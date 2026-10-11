using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    private Label storyLabel;

    [Header("Story Settings")]
    [TextArea(2, 4)]
    public string[] storyLines;

    public float typingSpeed = 0.08f;
    public float delayBetweenLines = 1.5f;
    public float delayAfterStory = 2f;
    public string nextSceneName = "StartScreen";

    [Header("Audio Settings")]
    public AudioClip typingSound; 
    private AudioSource audioSource;

    void Start()
    {
        UIDocument uiDocument = GetComponent<UIDocument>();
        storyLabel = uiDocument.rootVisualElement.Q<Label>("StoryText");
        storyLabel.text = "";

        
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        foreach (string line in storyLines)
        {
            storyLabel.text = "";

            
            if (audioSource != null && typingSound != null)
            {
                audioSource.clip = typingSound;
                audioSource.loop = true;
                audioSource.Play();
            }

            foreach (char letter in line.ToCharArray())
            {
                storyLabel.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }

            
            if (audioSource != null)
            {
                audioSource.Stop();
            }

            yield return new WaitForSeconds(delayBetweenLines);
        }

        yield return new WaitForSeconds(delayAfterStory);
        SceneManager.LoadScene(nextSceneName);
    }
}