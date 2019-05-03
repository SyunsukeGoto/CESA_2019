using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageChange : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectUi;

    [SerializeField]
    private GameObject _StageSelectionScript;

    [SerializeField]
    List<Sprite> _spriteList;

    private Image _selectUiImage;

    

    // Start is called before the first frame update
    void Start()
    {
        _selectUiImage = _selectUi.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_StageSelectionScript.GetComponent<StageSelectionController>()._stagSelectFlag == true)
        {
            _StageSelectionScript.GetComponent<StageSelectionController>()._stagSelectFlag = false;
            int stageNum = _StageSelectionScript.GetComponent<StageSelectionController>().GetStageName();
            Debug.Log(stageNum);
           _selectUiImage.sprite = _spriteList[stageNum];
        }
    }
}
