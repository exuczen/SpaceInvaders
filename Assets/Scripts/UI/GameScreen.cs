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
            GameManager.Instance.SetGameActive(false);
            Canvas.AlertPopup.ShowWithConfirmButton("Press OK to resume the game", () => {
                _pauseButton.enabled = true;
                GameManager.Instance.SetGameActive(true);
            });
        });
    }

    public void SetAnchors(Camera camera, Vector2 viewSize)
    {
        Vector2 cameraSize = new Vector2(2f * camera.orthographicSize * camera.aspect, 2f * camera.orthographicSize);
        Vector2 normalizedViewSize = viewSize / cameraSize;
        RectTransform rectTransform = transform as RectTransform;
        Vector2 anchorOffset = (Vector2.one - normalizedViewSize) / 2f;
        rectTransform.anchorMin = anchorOffset;
        rectTransform.anchorMax = Vector2.one - anchorOffset;
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
