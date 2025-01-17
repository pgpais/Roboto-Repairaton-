﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

/// <summary>
/// Handles the Global State of the game, and is used for scene transition with fades and others.
/// </summary>
public class GlobalManager : SingletonBehaviour<GlobalManager>
{
    [Header("Reference")]
    public Reference Reference;

    [Header("Game State")]
    public GameMode GameMode;

    private Canvas fadeCanvas;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        // Instatiates the Reference for safe-keeping.
        Reference = Instantiate(Reference);
    }

    /// <summary>
    /// Start is called just before any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        fadeCanvas = transform.Find("Fade Canvas").GetComponent<Canvas>();
    }

    /// <summary>
    /// Used for making transition fades between scenes and menus.
    /// </summary>
    public void ProcessFade(bool fadeIn, int sortOrder)
    {
        // Changes the sorting order of the canvas.
        fadeCanvas.sortingOrder = sortOrder;

        // Does a Fade In or Out depending on the bool parsed in.
        Animator fadeAnimator = fadeCanvas.GetComponentInChildren<Animator>();

        if (fadeIn)
        {
            fadeAnimator.SetTrigger("Fade In");

        }
        else
        {
            fadeAnimator.SetTrigger("Fade Out");
        }
    }

    /// <summary>
    /// Used for changing scenes. Calls the Coroutine that applies the delay to fade the scene out.
    /// </summary>
    public void ChangeScene(string targetScene, int fadeSortOrder)
    {
        // Calls the coroutine that actually handles the scene changing.
        StartCoroutine(ChangeSceneIE(targetScene, fadeSortOrder));
    }

    /// <summary>
    /// Changes the scene after a delay for fading out.
    /// </summary>
    private IEnumerator ChangeSceneIE(string targetScene, int fadeSortOrder)
    {
        // Does the fade animation, and does a WaitForSeconds before applying it.
        ProcessFade(false, fadeSortOrder);
        yield return new WaitForSecondsRealtime(1.5f);

        // Changes the scene to the target scene.
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(targetScene);
        while (!loadScene.isDone)
        {
            yield return null;
        }

        // Returns gameplay control.
        Time.timeScale = 1;
    }

    /// <summary>
    /// Opens a dialog, showing it on top of other dialogs.
    /// </summary>
    public void OpenDialog(GameObject dialog)
    {
        dialog.SetActive(true);
    }

    /// <summary>
    /// Closes a dialog.
    /// </summary>
    public void CloseDialog(GameObject dialog)
    {
        dialog.SetActive(false);
    }

    /// <summary>
    /// Statically open a link.
    /// </summary>
    public void OpenURL(string url)
    {
        // Depending on the current platform use a different share method.
#if !UNITY_WEBGL
        Application.OpenURL(url);
#else
        OpenWindow(url);
#endif
    }
}
