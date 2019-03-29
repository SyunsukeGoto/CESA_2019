using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
/////////////
using Momoya;
using Momoya.PlayerState;

namespace Momoya
{
    public class PlayerController : MonoBehaviour
    {
        //列挙型の宣言
        enum MoveDirection
        {
            UP,         //上
            DOWN,       //下
            LEFT,       //左
            RIGHT,      //右

            NUM,
        }
        //定数の定義


        [SerializeField]
        const float MoveSpeed = 5.0f; //動くスピード
        const float DashSpeed = MoveSpeed * 1.5f; //ダッシュスピード
        const float Speedlimit = 3.0f;
        const float DropdownPoint = -5.0f; //落下ポイント
        //変数の宣言
        private Vector3 _startPos;    //初期位置
        private int _pos;
        private Vector3 _vec;         //速度
        private Rigidbody _rg;        //リジットボディ
        [SerializeField]
        private KeyCode[] _moveKey = new KeyCode[(int)MoveDirection.NUM];//移動キー
                                                                         //変更前のステート名
        private string _beforeStateName;


        public StateProcessor _stateProcessor = new StateProcessor();           //プロセッサー

        //ステートの宣言
        public StateDefault _stateDefault = new StateDefault();                 //デフォルト状態
        public StateWalk _stateWalk = new StateWalk();                          //歩き状態
        public StateJump _stateJump = new StateJump();                          //ジャンプ状態
        public StateDash _stateDash = new StateDash();                          //ダッシュ状態
        public StateStrike _stateStrike = new StateStrike();                    //叩く状態

        // Use this for initialization
        void Start()
        {

            _rg = GetComponent<Rigidbody>(); //リジットボディの取得
            _startPos = _rg.position;        //初期位置の設定

            //初期ステートをdefaultにする
            _stateProcessor.State = _stateDefault;
            //委譲処理
            _stateDefault.execDelegate = Default;
            _stateWalk.execDelegate = Walk;
            _stateJump.execDelegate = Jump;
            _stateDash.execDelegate = Dash;
            _stateDash.execDelegate = Strike;
        }

        // Update is called once per frame
        void Update()
        {
            PlayerCtrl();
            
            //ステートの値が変更されたら実行処理を行う
            if (_stateProcessor.State == null)
            {
                return;
            }

            //現在どのステートか確認するためのデバッグ処理
            if (_stateProcessor.State.GetStateName() != _beforeStateName)
            {
                Debug.Log(" Now State:" + _stateProcessor.State.GetStateName());
                _beforeStateName = _stateProcessor.State.GetStateName();

            }

            _stateProcessor.Execute();//実行関数
        }


        //移動関数
        public void Move()
        {

            if (Input.GetKey(_moveKey[(int)MoveDirection.UP]))
            {

                _vec.z = MoveSpeed;
            }


            if (Input.GetKey(_moveKey[(int)MoveDirection.DOWN]))
            {
                _vec.z = -MoveSpeed;
            }

            if (Input.GetKey(_moveKey[(int)MoveDirection.RIGHT]))
            {
                _vec.x = MoveSpeed;
            }

            if (Input.GetKey(_moveKey[(int)MoveDirection.LEFT]))
            {
                _vec.x = -MoveSpeed;
            }
            /////////ここより下は停止用処理

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.UP]))
            {
                _vec.z = 0.0f;
            }

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.DOWN]))
            {
                _vec.z = 0.0f;
            }

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.RIGHT]))
            {
                _vec.x = 0.0f;
            }

            if (Input.GetKeyUp(_moveKey[(int)MoveDirection.LEFT]))
            {
                _vec.x = 0.0f;
            }
        }

        public void PlayerCtrl()
        {
            //速度を足す
            _rg.velocity = _vec;

            //落下ポイントよりポジションが低ければ初期位置に戻す
            if (_rg.position.y < DropdownPoint)
            {
                _rg.position = _startPos;
            }
        }

        //止まっているかチェックする関数
        public bool CheckStop()
        {
            //止まっていたらtrueを返す
            if (_vec == Vector3.zero)
            {
                return true;
            }
            //止まっていないことを伝える
            return false;
        }
        /////////ステートの関数をここに記入

        //通常状態
        public void Default()
        {
            //移動キーのどれかが押されたら移動状態に切り替える
            for (int i = 0; i < (int)MoveDirection.NUM; i++)
            {
                if(Input.GetKey(_moveKey[i]))
                {
                    _stateProcessor.State = _stateWalk;
                }
                           
            }
        }

        //歩き状態の関数
        public void Walk()
        {
            //移動する
            Move();
            //止まっていたらステートを通常状態にする
            if (CheckStop() == true)
            {
                _stateProcessor.State = _stateDefault;
            }
        }
        
        //ジャンプ状態
        public void Jump()
        {

        }

        //ダッシュ状態
        public void Dash()
        {
            
        }

        //叩き状態
        public void Strike()
        {

        }
         




    }
}