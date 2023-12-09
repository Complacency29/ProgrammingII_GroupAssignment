using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHints : MonoBehaviour
{
    [SerializeField] TMP_Text hintTextObject;
    [SerializeField] float fadeSpeed = 1f;
    [SerializeField] float hintDisplayDuration = 5f;

    private Queue<KeyValuePair<string, int>> hintQueue = new Queue<KeyValuePair<string, int>>();
    private bool isDisplayingHint = false;

    private void Start()
    {
        AddHint("stuff to say", 100);
    }

    // Call this method to add a hint to the queue
    public void AddHint(string _hintText, int _priority = 0)
    {
        hintQueue.Enqueue(new KeyValuePair<string, int>(_hintText, _priority));

        // If not currently displaying a hint, start displaying the next one
        if (!isDisplayingHint)
        {
            StartCoroutine(DisplayNextHint());
        }
    }

    private IEnumerator DisplayNextHint()
    {
        isDisplayingHint = true;

        while (hintQueue.Count > 0)
        {
            // Get the next hint from the queue
            var nextHint = hintQueue.Dequeue();

            // Display the hint
            StartCoroutine(ShowHintCO(nextHint.Key));

            // Wait for the display duration
            yield return new WaitForSeconds(hintDisplayDuration);

            // Fade out the hint
            StartCoroutine(FadeOutHint());

            // Wait for the fade out to complete
            yield return new WaitForSeconds(fadeSpeed);

            // Clear the text
            hintTextObject.text = "";
        }

        isDisplayingHint = false;
    }

    private IEnumerator ShowHintCO(string _hintText)
    {
        // Set the hint text
        hintTextObject.text = _hintText;

        // Fade in the alpha
        while (hintTextObject.alpha < 255)
        {
            hintTextObject.alpha += fadeSpeed * Time.deltaTime;
            hintTextObject.SetAllDirty();
            yield return null;
        }
    }

    private IEnumerator FadeOutHint()
    {
        // Fade out the alpha
        while (hintTextObject.alpha > 0)
        {
            hintTextObject.alpha -= fadeSpeed * Time.deltaTime;
            hintTextObject.SetAllDirty();
            yield return null;
        }
    }
}