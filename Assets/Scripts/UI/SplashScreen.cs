using MustHave.Utilities;
using MustHave.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : ScreenScript
{
    public void ShowGameScreen()
    {
        GameManager.Instance.InitGame();
        this.StartCoroutineActionAfterFrames(() => {
            Canvas.ShowScreen<GameScreen>();
        }, 1);
    }
}
