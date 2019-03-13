using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    [SerializeField]
    private Vector2 _pos; //ポジション
    [SerializeField]
    private Vector2 _vec; //ベクター

    private float _angle;//角度

    [SerializeField]
    private float _rotationAngle = 1.0f; //回転パワー

    private Rigidbody2D _rigid2D; //リジットボディ

    public Goto.Flag _goalFlag;  //ゴール系統に使うフラグ

    enum GoalFlag
    {
        GOAL = (1 << 0),//ゴールフラグ

        NUMSTATE,
    }
        

    // Start is called before the first frame update
    void Start()
    {
        _goalFlag = new Goto.Flag();
        _angle = 0.0f;
        _rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //posにvecを足す
        _pos += _vec;
        _angle += _rotationAngle;


        transform.position = new Vector2(_pos.x,transform.position.y);
        //角度を渡して回転
        _rigid2D.MoveRotation(_angle);
        //ゴールフラグがtrueなら
        if(_goalFlag.IsFlag((uint)GoalFlag.GOAL))
        {
            Debug.Log("ゴール!");
            _goalFlag.OffFlag((uint)GoalFlag.GOAL);
        }
               
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ゴールのタグに触れていたら
        if (collision.transform.tag == "Goal")
        {
            //ゴールフラグをtrue
            _goalFlag.OnFlag((uint)GoalFlag.GOAL);
        }
    }
}
