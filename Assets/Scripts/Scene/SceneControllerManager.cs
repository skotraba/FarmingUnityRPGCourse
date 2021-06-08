using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneControllerManager : SingletonMonoBehavior<SceneControllerManager>
{
    private bool isFading;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private CanvasGroup faderCanvasGroup = null;
    [SerializeField] private Image faderImage = null;
    public SceneName startingSceneName;


    private IEnumerator Fade(float finalAlpha)
    {
        // set the fading flag to be true so that fadeandswitchscenes coroutine wont be called again
        isFading = true;

        // Make sure the canvasgroup blocks raycasts into the scene so no mor einput can be accepted
        faderCanvasGroup.blocksRaycasts = true;

        //Calculate how fast the canvas group should fade based on it's current alpha, it's final alpha and how long it has to change between the two.
        float fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        // while the canvas group hasn't reached the final alpha yet...
        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            // move the alpha towards it's target alpha
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha, fadeSpeed * Time.deltaTime);

            yield return null;

        }

        // set the flag to false since the fade has finished.
        isFading = false;

        // stop the vancas group from blocking raycasts so input is no longer ignored.
        faderCanvasGroup.blocksRaycasts = false;
    }

    // This is the coroutine where the building blocks of the script are put together.
    private IEnumerator FadeAndSwitchScenes(string sceneName, Vector3 spawnPosition)
    {
        // call before scene unload fade out event
        EventHandler.CallBeforeSceneUnloadFadeOutEvent();

        // start fading to black and wait for it to finish before continuing
        yield return StartCoroutine(Fade(1f));

        // Set player position
        Player.Instance.gameObject.transform.position = spawnPosition;

        // call before scene unload event
        EventHandler.CallBeforeSceneUnloadEvent();

        // Unload the current active scene
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // Start loading the given scene and wait for it to finish
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));

        //Call after scene load event
        EventHandler.CallAfterSceneloadEvent();

        // Start fading back in and wait for it to finish before exiting the function.
        yield return StartCoroutine(Fade(0f));

        //Call after scene load fade in event
        EventHandler.CallAfterSceneloadFadeInEvent();
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        //Allow the given scene to load over several frames and add it to the already loaded scenes
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // find scene that was most recently loaded
        Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Set the newly loaded scene as the active scene
        SceneManager.SetActiveScene(newlyLoadedScene);
    }

    private IEnumerator Start()
    {
        // Set the initial alpha to start off with a black screen
        faderImage.color = new Color(0f, 0f, 0f, 1f);
        faderCanvasGroup.alpha = 1f;

        //Start the first scene loading and wait for it to finish.
        yield return StartCoroutine(LoadSceneAndSetActive(startingSceneName.ToString()));

        // if this event has any subscribers, call it
        EventHandler.CallAfterSceneloadEvent();

        // once the scene is finished loading, start fading in
        StartCoroutine(Fade(0f));
    }

    //This is the main external point of contact and influence from the rest of the project.
    // This will be called when the player wants to switch scenes.
    public void FadeAndLoadScene(string sceneName, Vector3 spawnPosition)
    {
        // if a fade isn't happening then start fading and switching scenes
        if(!isFading)
        {
            StartCoroutine(FadeAndSwitchScenes(sceneName, spawnPosition));
        }
    }
}
