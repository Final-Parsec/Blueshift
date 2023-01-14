using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameDialogue : MonoBehaviour
{
    public AudioClip[] dialogueAudioClips;
    
    private AudioSource audioSource;
    private Image background;
    private int currentLine;
    private Text dialogue;
    private Image portrait;
    private AudioClip previousAudioClip;
    private Text speaker;
    public static InGameDialogue Instance { get; private set; }
    public static IEnumerator DialogueCoroutine { get; set; }

    private void Start()
    {
        this.audioSource = this.GetComponent<AudioSource>();
        this.background = GameObject.Find("Dialogue Background").GetComponent<Image>();
        this.background.color = new Color(0, 0, 0, 0);
        this.dialogue = GameObject.Find("Dialogue").GetComponent<Text>();
        this.portrait = GameObject.Find("Dialogue Portrait").GetComponent<Image>();
        this.portrait.enabled = false;
        this.speaker = GameObject.Find("Dialogue Speaker").GetComponent<Text>();
        InGameDialogue.Instance = this;
    }

    public void HideForPause()
    {
        this.dialogue.text = string.Empty;
        this.portrait.enabled = false;
        this.speaker.text = string.Empty;
        var current = this.background.color;
        this.background.color = new Color(current.r, current.g, current.b, 0f);
    }

    /// <summary>
    ///     Starts the dialogue and scrolls it ever .o2 sec using a waitforseconds
    /// </summary>
    public IEnumerator SayDialogue(string speakerName, string toSay)
    {
        var coroutineCharacterCount = 0;

        // Fade In
        this.dialogue.text = string.Empty;
        var portraitImage = (Texture2D) Resources.Load("Portraits/" + speakerName);
        this.portrait.enabled = true;
        this.portrait.sprite = Sprite.Create(portraitImage, new Rect(0, 0, portraitImage.width, portraitImage.height), new Vector2(0, 0));
        this.speaker.text = speakerName;
        while (this.background.color.a <= .39f)
        {
            var current = this.background.color;
            this.background.color = new Color(current.r, current.g, current.b, current.a + .02f);
            yield return new WaitForEndOfFrame();
        }

        var randomAudioClip = this.dialogueAudioClips[Random.Range(0, this.dialogueAudioClips.Length)];
        while (randomAudioClip == this.previousAudioClip)
        {
            // Our random clip was the same as last time. Pick a new one so we don't duplicate.
            randomAudioClip = this.dialogueAudioClips[Random.Range(0, this.dialogueAudioClips.Length)];
        }
        this.previousAudioClip = randomAudioClip;
        this.audioSource.clip = randomAudioClip;
        this.audioSource.Play();

        // Display text character by character.
        while (coroutineCharacterCount <= toSay.Length)
        {
            this.dialogue.text = toSay.Substring(0, coroutineCharacterCount);

            if ((coroutineCharacterCount > 0) &&
                (toSay[coroutineCharacterCount - 1] == '.' || toSay[coroutineCharacterCount - 1] == '?' ||
                 toSay[coroutineCharacterCount - 1] == '!'))
            {
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                yield return new WaitForSeconds(0.02f);
            }

            coroutineCharacterCount++;
        }

        yield return new WaitForSeconds(3f);

        // Fade Out
        this.dialogue.text = string.Empty;
        this.portrait.enabled = false;
        this.speaker.text = string.Empty;
        while (this.background.color.a >= 0f)
        {
            var current = this.background.color;
            this.background.color = new Color(current.r, current.g, current.b, current.a - .02f);
            yield return new WaitForEndOfFrame();
        }
    }
}