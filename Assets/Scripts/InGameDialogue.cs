using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InGameDialogue : MonoBehaviour
{
    private AudioSource audiosource;
    private Image background;
    private int currentLine;
    private Text dialogue;
    public static InGameDialogue Instance { get; private set; }

    private void Start()
    {
        this.background = GameObject.Find("Dialogue Background").GetComponent<Image>();
        this.background.color = new Color(0, 0, 0, 0);
        this.dialogue = GameObject.Find("Dialogue").GetComponent<Text>();
        InGameDialogue.Instance = this;
    }

    /// <summary>
    ///     Starts the dialogue and scrolls it ever .o2 sec using a waitforseconds
    /// </summary>
    public IEnumerator SayDialogue(string toSay)
    {
        var coroutineCharacterCount = 0;

        // Fade In
        this.dialogue.text = string.Empty;
        while (this.background.color.a <= .39f)
        {
            var current = this.background.color;
            this.background.color = new Color(current.r, current.g, current.b, current.a + .02f);
            yield return new WaitForEndOfFrame();
        }
        
        // Display text character by character.
        while (coroutineCharacterCount <= toSay.Length)
        {
            this.dialogue.text = toSay.Substring(0, coroutineCharacterCount);

            // Play audio for characters that aren't whitespace.
            if (coroutineCharacterCount > 0 &&
                (toSay[coroutineCharacterCount - 1] != ' ' || toSay[coroutineCharacterCount - 1] != '\n'))
            {
                this.GetComponent<AudioSource>().Play();
            }

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
        while (this.background.color.a >= 0f)
        {
            var current = this.background.color;
            this.background.color = new Color(current.r, current.g, current.b, current.a - .02f);
            yield return new WaitForEndOfFrame();
        }
    }
}