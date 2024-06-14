using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class VRSceneTransition : MonoBehaviour
{
    public Image fadeImage; // Reference to the UI Image used for fading
    public float fadeDuration = 2.0f; // Duration of the fade in seconds

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("Fade image is not assigned!");
        }
        else
        {
            // Ensure the image is initially transparent
            Color color = fadeImage.color;
            color.a = 1f;
            fadeImage.color = color;
        }
        StartCoroutine(FadIn());
    }

    // Method to start the scene transition
    public void TransitionToScene(string sceneName)
    {
        StartCoroutine(FadeOutIn(sceneName));
    }

    // Coroutine to handle the fading out, scene change, and fading in process
    private IEnumerator FadeOutIn(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(Fade(0f, 1f));

        // Load the new scene
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is loaded
        while (!asyncLoad.isDone)
        {
            // Check if the load has finished
            if (asyncLoad.progress >= 0.9f)//
            {
                // Allow the scene to be activated
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
        // Optionally, wait for the scene to load (add delay if needed)
        yield return new WaitForSeconds(0.5f);
        

        // Fade in
        //yield return StartCoroutine(Fade(1f, 0f));
    }

    // Coroutine to handle the fading process
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0.0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            color.a = alpha;
            fadeImage.color = color;
            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;
    }

    private IEnumerator FadIn()
    {
        // Fade out
        //yield return StartCoroutine(Fade(0f, 1f));

        // Load the new scene
        
        // Optionally, wait for the scene to load (add delay if needed)
        //yield return new WaitForSeconds(0.5f);
        

        // Fade in
        yield return StartCoroutine(Fade(1f, 0f));
    }
}