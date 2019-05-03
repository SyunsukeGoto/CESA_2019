using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject _stageSelectCollsion;

    [SerializeField]
    private GameObject _stageConfirmationCollsion;

    public static string _stageName;
    private string _tempStageName;
    private bool _collisionFlag;

    public bool _stagSelectFlag;
   
    // Start is called before the first frame update
    void Start()
    {
        _collisionFlag = false;
        _stagSelectFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_tempStageName != "Start")
        {
            _stageName = _tempStageName;
        }
        

        if (Input.GetKeyDown("joystick button 0") && _collisionFlag == true && _tempStageName != "0")
        {
            _stagSelectFlag = true;
            Debug.Log("stage");
            foreach(Transform child in _stageSelectCollsion.transform)
            {
                child.gameObject.SetActive(false);
            }
            foreach (Transform child in _stageConfirmationCollsion.transform)
            {
                child.gameObject.SetActive(true);
            }
            
        }


        if (Input.GetKeyDown("joystick button 0") && _collisionFlag == true && _tempStageName == "0")
        {
            _stagSelectFlag = true;
            Debug.Log("a");
            foreach (Transform child in _stageSelectCollsion.transform)
            {
                child.gameObject.SetActive(true);
            }
            foreach (Transform child in _stageConfirmationCollsion.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown("joystick button 0") && _collisionFlag == true && _tempStageName == "Start")
        {
            SceneManager.LoadScene("PlayScene");
        }

    }

    public int GetStageName()
    {
        int temp = int.Parse(_stageName);
        return temp;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        _tempStageName = col.gameObject.name;
        _collisionFlag = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _collisionFlag = false;
    }

}
