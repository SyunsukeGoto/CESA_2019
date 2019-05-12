using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectionController : MonoBehaviour
{
    [SerializeField]
    public GameObject _stageSelectCollsion;

    [SerializeField]
    public GameObject _stageConfirmationCollsion;
    [SerializeField]
    public static string _stageName;
   
    private string _tempStageName;
   
    private bool _collisionFlag;
    
    private bool _decisionFlag;

    private bool _startFlag;
    [SerializeField]
    public bool _stagSelectFlag;
   
    private Vector3 _hummerAngle;
    
    private float _startAngle = -90.0f;
    
    private float _tempAngle = -45.0f;
    [SerializeField]
    public float _angleSpeed;
   
    // Start is called before the first frame update
    void Start()
    {
        _collisionFlag = false;
        _stagSelectFlag = false;
        _decisionFlag = false;
       
    }

    // Update is called once per frame
    void Update()
    {
        //start以外のステージ番号を記録
        if(_tempStageName != "Start")
        {
            _stageName = _tempStageName;
        }

      
        
        if (Input.GetKeyDown("joystick button 0") && _collisionFlag == true && _tempStageName != "0" && _tempStageName != "Start")
        {
            _decisionFlag = true;
            foreach (Transform child in _stageSelectCollsion.transform)
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
            _decisionFlag = true;

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
            _decisionFlag = true;
            _startFlag = true;
        }

        if(_decisionFlag == true)
        {
            AngleMove();
        }

        if (_startFlag == true)
        {
            Debug.Log("a");
            if (_tempAngle >= 80.0f)
            {
                SceneManager.LoadScene("PlayScene");
            }
        }

        if (_tempAngle >= 80.0f)
        {
            ResetAngle();
            _stagSelectFlag = true;
            _decisionFlag = false;
           
        }

        

    }

    public void AngleMove()
    {
        
        float angleZ = 0.0f;
        float maxAngle = 45.0f;
        angleZ += _angleSpeed;
        _tempAngle += _angleSpeed;

        if (_tempAngle <= maxAngle)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f, angleZ));
        }
    }

    public void ResetAngle()
    {
       
        transform.Rotate(new Vector3(0.0f, 0.0f, _startAngle));
        _tempAngle = -45.0f;
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
