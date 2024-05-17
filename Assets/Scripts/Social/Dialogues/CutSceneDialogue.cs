using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using Visuals;

public class CutSceneDialogue : MonoBehaviour
{
    [SerializeField] TMP_Text targetText;

    public CutSceneDialogueContainer currentDialogue;
    public ScreenTint screenTint;

    public GameObject selection;
    int currentTextLine;

    [Range(0f, 1f)]
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;

    string lineToShow;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (currentDialogue != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PushText();
            }

            TypeOutText();
        }
    }

    private void PushText()
    {
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        if (currentTextLine >= currentDialogue.lines.Count)
        {
            Show(true);
        }
        else
        {
            CycleLine();
        }
    }

    void CycleLine()
    {
        lineToShow = currentDialogue.lines[currentTextLine];
        totalTimeToType = lineToShow.Length * timePerLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;
        targetText.text = "";
        currentTextLine += 1;
    }

    private void TypeOutText()
    {
        if (visibleTextPercent >= 1f) { return; }

        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0, 1f);
        UpdateText();
    }

    void UpdateText()
    {
        int letterCount = (int)(lineToShow.Length * visibleTextPercent);
        targetText.text = lineToShow.Substring(0, letterCount);
    }

    private void Show(bool s)
    {
        selection.SetActive(s);
    }

    public void Conclude()
    {
        currentDialogue = null;

        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        Show(false);
        screenTint.Tint();

        yield return new WaitForSeconds(1f / screenTint.speed + .1f);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Loop", LoadSceneMode.Single);
        var load = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Persistents", LoadSceneMode.Additive);

        while (!load.isDone) { yield return new WaitForSeconds(0.1f); }

        screenTint.UnTint();

        Destroy(gameObject);
    }

    #region Button Selection

    public void SelectYes()
    {
        Conclude();
    }

    public void SelectNo()
    {
        screenTint.Tint();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        screenTint.UnTint();

        Destroy(gameObject);
    }

    public void SelectMaybe()
    {
        targetText.text = "Good enough.";
        Conclude();
    }

    #endregion
}
