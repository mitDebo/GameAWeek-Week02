using UnityEngine;
using System.Collections;

public class ScreenOverlay : MonoBehaviour {
    public UILabel label;
    public float LabelFadeOutTime;
    public float ScreenFadeOutTime;

    UISprite sprite;

	// Use this for initialization
	void Start () {
        sprite = GetComponent<UISprite>();
	}

    public void FadeOut()
    {
        StartCoroutine("fadeOutSequence");
    }

    public void FadeIn()
    {
        StartCoroutine("fadeUpSequence");
    }

    public void UpdateWeek(int w)
    {
        label.text = "Week " + w;
    }

    IEnumerator fadeOutSequence()
    {
        float timer = LabelFadeOutTime;
        while (timer > 0)
        {
            timer -= LabelFadeOutTime * Time.deltaTime;
            Color c = new Color(1f, 1f, 1f, (timer / LabelFadeOutTime));
            label.color = c;
            yield return new WaitForEndOfFrame();
        }
        timer = ScreenFadeOutTime;
        while (timer > 0)
        {
            timer -= ScreenFadeOutTime * Time.deltaTime;
            Color c = new Color(0f, 0f, 0f, (timer / ScreenFadeOutTime));
            sprite.color = c;
            yield return new WaitForEndOfFrame();
        }
        GameState.AcceptInput = true;
    }

    IEnumerator fadeUpSequence()
    {
        GameState.AcceptInput = false;
        float timer = 0;
        while (timer < ScreenFadeOutTime)
        {
            timer += ScreenFadeOutTime * Time.deltaTime;
            Color c = new Color(0f, 0f, 0f, (timer / ScreenFadeOutTime));
            sprite.color = c;
            yield return new WaitForEndOfFrame();
        }

        timer = 0;
        while (timer < LabelFadeOutTime)
        {
            timer += LabelFadeOutTime * Time.deltaTime;
            Color c = new Color(1f, 1f, 1f, (timer / LabelFadeOutTime));
            label.color = c;
            yield return new WaitForEndOfFrame();
        }
    }
}
