using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDialogueScript : MonoBehaviour
{

    [Header("Dialogue Audio")]
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private List<AudioClip> dialogueAudioClips = new List<AudioClip>();

    private int nextAudioIndex = 0;
    [System.Serializable]
    public struct TimedDialogueEntry
    {
        public float triggerTimeSeconds;   // When dialogue appears
        public string messageText;         // Text to display
        public float visibleDuration;      // Duration shown
    }

    [Header("Dialogue Timeline")]
    [SerializeField]
    private List<TimedDialogueEntry> dialogueTimeline = new List<TimedDialogueEntry>();

    [Header("UI References")]
    [SerializeField]
    private GameObject textContainer;

    [SerializeField]
    private TMP_Text textUI;

    private float elapsedTime;
    private int nextDialogueIndex;

    private Coroutine activeDialogueRoutine;

    void Start()
    {
        if (textContainer != null)
            textContainer.SetActive(false);

        // Ensure timeline is sorted by trigger time
        dialogueTimeline.Sort((a, b) => a.triggerTimeSeconds.CompareTo(b.triggerTimeSeconds));
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (nextDialogueIndex >= dialogueTimeline.Count)
            return;

        TimedDialogueEntry nextEntry = dialogueTimeline[nextDialogueIndex];

        if (elapsedTime < nextEntry.triggerTimeSeconds)
            return;

        TriggerDialogue(nextEntry);
        nextDialogueIndex++;
    }

    void TriggerDialogue(TimedDialogueEntry entry)
    {
        if (activeDialogueRoutine != null)
            StopCoroutine(activeDialogueRoutine);

        activeDialogueRoutine = StartCoroutine(ShowDialogueRoutine(entry));
    }

    IEnumerator ShowDialogueRoutine(TimedDialogueEntry entry)
    {
        textContainer.SetActive(true);
        textUI.text = entry.messageText;
        PlayNextAudio();

        yield return new WaitForSeconds(entry.visibleDuration);

        textContainer.SetActive(false);
    }

    public void ShowExternalDialogue(string messageText, float visibleDuration)
    {
        if (activeDialogueRoutine != null)
            StopCoroutine(activeDialogueRoutine);

        TimedDialogueEntry entry = new TimedDialogueEntry
        {
            triggerTimeSeconds = 0f,
            messageText = messageText,
            visibleDuration = visibleDuration
        };

        activeDialogueRoutine = StartCoroutine(ShowDialogueRoutine(entry));
    }
    IEnumerator DelayedDialogueRoutine(string messageText, float preDelaySeconds, float visibleDuration)
    {
        if (preDelaySeconds > 0f)
            yield return new WaitForSeconds(preDelaySeconds);

        textContainer.SetActive(true);
        textUI.text = messageText;

        PlayNextAudio();

        yield return new WaitForSeconds(visibleDuration);

        textContainer.SetActive(false);
    }
    public void ShowDialogueDelayed(string messageText, float preDelaySeconds, float visibleDuration)
    {
        if (activeDialogueRoutine != null)
            StopCoroutine(activeDialogueRoutine);

        activeDialogueRoutine = StartCoroutine(DelayedDialogueRoutine(messageText, preDelaySeconds, visibleDuration));
    }

    void PlayNextAudio()
    {
        if (audioSource == null)
            return;

        if (nextAudioIndex >= dialogueAudioClips.Count)
            return;

        Debug.Log("Playing audio index: " + nextAudioIndex);
        AudioClip clipToPlay = dialogueAudioClips[nextAudioIndex];
        nextAudioIndex++;

        if (clipToPlay == null)
            return;

        audioSource.clip = clipToPlay;
        audioSource.Play();
    }
}
