using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;

public class UIHandler : MonoBehaviour
{
    private VisualElement m_Healthbar;
    public static UIHandler instance { get; private set; }

    
    private VisualElement m_NonPlayerDialogue;
    private Label m_DialogueText;
    public bool isDialogueActive = false;
    private string[] currentLines;
    private int currentLineIndex = 0;

    
    private VisualElement m_GreetingPopUp;
    private Label m_GreetingText;
    private float m_GreetingTimer = -1.0f;

    private VisualElement m_WinScreen;
    private VisualElement m_LoseScreen;

    [Header("Typewriter Effect")]
    public float typingSpeed = 0.05f; 
    public AudioClip dialogueSound; 
    private AudioSource uiAudioSource; 
    private Coroutine typingCoroutine; 
    private bool isTyping = false;

    private Label m_RobotCounter;

    private VisualElement m_CounterContainer;
    private int storedCurrent = 0;
    private int storedTotal = 0;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        uiAudioSource = GetComponent<AudioSource>();

        UIDocument uiDocument = GetComponent<UIDocument>();
        m_Healthbar = uiDocument.rootVisualElement.Q<VisualElement>("HealthBar");
        SetHealthValue(1.0f);

        m_NonPlayerDialogue = uiDocument.rootVisualElement.Q<VisualElement>("NPCDialogue");
        m_DialogueText = uiDocument.rootVisualElement.Q<Label>("DialogueText");
        if (m_NonPlayerDialogue != null) m_NonPlayerDialogue.style.display = DisplayStyle.None;

        m_GreetingPopUp = uiDocument.rootVisualElement.Q<VisualElement>("POPupScreen");
        m_GreetingText = uiDocument.rootVisualElement.Q<Label>("GreetingText");
        if (m_GreetingPopUp != null) m_GreetingPopUp.style.display = DisplayStyle.None;

        m_LoseScreen = uiDocument.rootVisualElement.Q<VisualElement>("LoseScreenContainer");
        m_WinScreen = uiDocument.rootVisualElement.Q<VisualElement>("WinScreenContainer");
        m_RobotCounter = uiDocument.rootVisualElement.Q<Label>("CounterLabel");

        m_CounterContainer = uiDocument.rootVisualElement.Q<VisualElement>("RobotCounter");
        if (m_CounterContainer != null) m_CounterContainer.style.display = DisplayStyle.None;
    }


    private void Update()
    {
        if (m_GreetingTimer > 0)
        {
            m_GreetingTimer -= Time.deltaTime;
            if (m_GreetingTimer <= 0)
            {
                if (m_GreetingPopUp != null) m_GreetingPopUp.style.display = DisplayStyle.None;
            }
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        if (m_DialogueText != null) m_DialogueText.text = ""; 

        
        if (uiAudioSource != null && dialogueSound != null)
        {
            uiAudioSource.clip = dialogueSound;
            uiAudioSource.loop = true;
            uiAudioSource.Play();
        }

        
        foreach (char letter in line.ToCharArray())
        {
            if (m_DialogueText != null) m_DialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed); 
        }

        
        if (uiAudioSource != null) uiAudioSource.Stop();
        isTyping = false;
    }

    public void ShowGreeting(string text)
    {
        if (m_GreetingText != null) m_GreetingText.text = text;
        if (m_GreetingPopUp != null) m_GreetingPopUp.style.display = DisplayStyle.Flex;
        m_GreetingTimer = 4.0f;
    }


    public void StartDialogue(string[] lines)
    {
        currentLines = lines;
        currentLineIndex = 0;
        isDialogueActive = true;

        m_NonPlayerDialogue.style.display = DisplayStyle.Flex;

        if (m_GreetingPopUp != null)
        {
            m_GreetingPopUp.style.display = DisplayStyle.None;
            m_GreetingTimer = -1.0f; 
        }

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(currentLines[currentLineIndex]));
    }

    public void AdvanceDialogue()
    {
        
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            if (m_DialogueText != null) m_DialogueText.text = currentLines[currentLineIndex];
            if (uiAudioSource != null) uiAudioSource.Stop();
            isTyping = false;
        }
        else 
        {
            currentLineIndex++;
            if (currentLineIndex < currentLines.Length)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(TypeLine(currentLines[currentLineIndex]));
            }
            else
            {
                EndDialogue();
            }
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        m_NonPlayerDialogue.style.display = DisplayStyle.None;

        if (m_CounterContainer != null)
        {
            m_CounterContainer.style.display = DisplayStyle.Flex;
        }

        if (m_RobotCounter != null)
        {
            m_RobotCounter.text = $"{storedCurrent} / {storedTotal}";
        }
    }

    public void SetHealthValue(float percentage)
    {
        if (m_Healthbar != null) m_Healthbar.style.width = Length.Percent(100 * percentage);
    }

    public void DisplayWinScreen()
    {
        m_WinScreen.style.opacity = 1.0f;
    }

    public void DisplayLoseScreen()
    {
        m_LoseScreen.style.opacity = 1.0f;
    }

    public void SetCounter(int current, int enemies)
    {
        storedCurrent = current;
        storedTotal = enemies;
        if (m_RobotCounter != null)
        {
            m_RobotCounter.text = $"{current} / {enemies}";
        }
    }
}