using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInOut : MonoBehaviour
{
    private float _fadeSpeed = 0.005f;        //透明度が変わるスピードを管理
    private float _red, _green, _blue, _alfa;   //パネルの色、不透明度を管理

    public  bool _isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
    public  bool _isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

    private Image _fadeImage;                //透明度を変更するパネルのイメージ

    void Start()
    {
        _fadeImage = GetComponent<Image>();
        _red = _fadeImage.color.r;
       _green = _fadeImage.color.g;
        _blue = _fadeImage.color.b;
        _alfa = _fadeImage.color.a;
    }

    void Update()
    {

    }

    public void StartFadeIn()
    {
        _alfa -= _fadeSpeed;                //a)不透明度を徐々に下げる
        SetAlpha();                      //b)変更した不透明度パネルに反映する
        if (_alfa <= 0)
        {                    //c)完全に透明になったら処理を抜ける
            _isFadeIn = false;
            _fadeImage.enabled = false;    //d)パネルの表示をオフにする
        }
    }

    public void StartFadeOut()
    {
        _fadeImage.enabled = true;  // a)パネルの表示をオンにする
        _alfa += _fadeSpeed;         // b)不透明度を徐々にあげる
        SetAlpha();               // c)変更した透明度をパネルに反映する
        if (_alfa >= 1)
        {             // d)完全に不透明になったら処理を抜ける
            _isFadeOut = false;
        }
    }

    void SetAlpha()
    {
        _fadeImage.color = new Color(_red, _green, _blue, _alfa);
    }
}
