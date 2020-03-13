using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MustHave.Utilities;
using UnityEditor;
using MustHave.UI;
using UnityEngine.UI;

public class GameScreen : ScreenScript
{
    [SerializeField]
    private Button _pauseButton = default;
    [SerializeField]
    private Text _playerHealthText = default;
    [SerializeField]
    private Text _levelText = default;

    protected override void OnAwake()
    {
        _pauseButton.onClick.AddListener(() => {
            _pauseButton.enabled = false;
            GameManager.Instance.PauseGame(() => {
                _pauseButton.enabled = true;
            });
        });
    }

    public void SetPlayerHealthText(int hp)
    {
        _playerHealthText.text = hp.ToString();
    }

    public void SetLevelText(int level)
    {
        _levelText.text = "Level: " + level;
    }

    protected override void OnShow()
    {
        SetLevelText(GameManager.Instance.Level);
        Canvas.AlertPopup.ShowWithConfirmButton("Start the game!", () => {
            GameManager.Instance.SetGameActive(true);
        });
    }
}
