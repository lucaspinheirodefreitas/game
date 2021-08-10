using UnityEngine;
using UnityEngine.UI;
//using DG.Tweening;
using UnityEngine.SceneManagement;
// Instalar a lib DG.Tweening; para conseguir gerar o efeito de transparencia.
public class Fader : MonoBehaviour
{
    Image image;
    public float duration = 0.5f;

    void Start()
    {
        image = GetComponent<Image>();
        //image.color = new Color(0, 0, 0, 1);
        //image.enabled = true;

        //image.DOFade(0, duration)
        //    .OnComplete(() => { image.enabled = false; });
    }

    public void FadeToGame()
    {
        //image.enabled = true;
        //image.color = new Color(0, 0, 0, 0);
        //image.DOFade(1, duration)
        //    .OnComplete(() => { SceneManager.LoadScene("GameScene"); });
        SceneManager.LoadScene("GameScene");
    }

    public void FadeToMenu()
    {
        //image.enabled = true;
        //image.color = new Color(0, 0, 0, 0);
        //image.DOFade(1, duration)
        //    .OnComplete(() => { GameManager.Instance.GameOver = false; SceneManager.LoadScene("Menu"); });
        SceneManager.LoadScene("Menu");
    }

    public void Flash()
    {
        //image.enabled = true;
        //image.color = new Color(1, 1, 1, 0);
        //image.DOFade(1, 0.1f)
        //    .SetLoops(2, LoopType.Yoyo);
    }
}