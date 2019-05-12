using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LogoFadeInOut : MonoBehaviour
{
    [SerializeField]
    private GameObject _fadeInOutScript;
    [SerializeField]
    private float _InStartCount = 2f;
    [SerializeField]
    private float _OutStartCount = 6f;
    [SerializeField]
    private float _tempTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _tempTime += Time.deltaTime;
        //フェイドインスタートフラグ
        if(_tempTime >= _InStartCount && _tempTime <= _OutStartCount)
            _fadeInOutScript.GetComponent<FadeInOut>()._isFadeIn = true;

        //フェイドアウトスタートフラグ
        if (_tempTime >= _OutStartCount)
            _fadeInOutScript.GetComponent<FadeInOut>()._isFadeOut = true;

        //フェードイン処理
        if (_fadeInOutScript.GetComponent<FadeInOut>()._isFadeIn == true)
            _fadeInOutScript.GetComponent<FadeInOut>().StartFadeIn();

        //フェードアウト処理
        if (_fadeInOutScript.GetComponent<FadeInOut>()._isFadeOut == true)
            _fadeInOutScript.GetComponent<FadeInOut>().StartFadeOut();
       
        if (_tempTime >= _OutStartCount && _fadeInOutScript.GetComponent<FadeInOut>()._isFadeOut == false)
            SceneManager.LoadScene("StartScene");
    }
}
