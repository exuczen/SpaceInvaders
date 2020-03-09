using MustHave.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : ScreenScript
{
    public void ShowGameScreen()
    {
        Canvas.ShowScreen<GameScreen>();
    }
}
