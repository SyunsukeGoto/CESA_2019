﻿using UnityEngine;
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
        //構造隊の宣言
        

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

        const float Speedlimit = 3.0f;
        const float DropdownPoint = -5.0f; //落下ポイント
        //変数の宣言
        private Vector3 _startPos;    //初期位置
        [SerializeField]
        private float _speedMagnification = 1.5f; //速度の倍率
        private float _dashSpeed;     //ダッシュスピード
        private float _nowSpeed;      //現在のスピード
        private Vector3 _vec;         //速度
        private float _nowJumpPower;  //現在のジャンプパワー
        [SerializeField]
        private float _normalJumpPower; //ノーマルジャンプ

        private Rigidbody _rg;        //リジットボディ

        [SerializeField]
        private KeyCode[] _moveKey = new KeyCode[(int)MoveDirection.NUM];//移動キー
        [SerializeField]
        private KeyCode _dashKey;                                        //ダッシュキー    
        [SerializeField]
        private KeyCode _jumpKey;                                        //ジャンプキー
        [SerializeField]
        private KeyCode _strikeKey;                                      //ハンマーで殴るキー

        private string _beforeStateName;                                 //変更前のステート名
        public StateProcessor _stateProcessor = new StateProcessor();    //プロセッサー
        //ハンマーに必要な変数
        [SerializeField]
        private int _hammerLevelLimit;                                  //ハンマーリミットレベル
        private int  _hammerLevel;                                        //ハンマーレベル
        [SerializeField]
        private float _hammerPowerLimit;                                 //ハンマーパワーリミット
        private float _hammerPower;                                      //ハンマーパワー
        [SerializeField]
        private float _hammerChargSpeed = 1.0f;                          //ハンマーチャージスピード
                                                                         //ハンマーリミット ÷ ハンマーレベル のあたい
        int _importantPoint;
        //ステートの宣言
        public StateDefault _stateDefault = new StateDefault();                 //デフォルト状態
        public StateWalk _stateWalk = new StateWalk();                          //歩き状態
        public StateJump _stateJump = new StateJump();                          //ジャンプ状態
        public StateDash _stateDash = new StateDash();                          //ダッシュ状態
        public StateStrike _stateStrike = new StateStrike();                    //叩く状態

        //////////デバッグ用
        public Text _chargeText;     //現在のパワーを表示するデバッグ用変数
        public Text _levelText;      //現在のレベルを表示するデバッグ用変数

        // Use this for initialization
        void Start()
        {
            //プレイヤーの初期設定
            _rg = GetComponent<Rigidbody>(); //リジットボディの取得
            _startPos = _rg.position;        //初期位置の設定
            _dashSpeed = MoveSpeed * _speedMagnification;
            _nowSpeed = MoveSpeed;
            _nowJumpPower = _normalJumpPower;
            
            if(_hammerLevelLimit <= 0)
            {
                _hammerLevelLimit = 1; //0にはしない
            }

            _hammerLevel = 0;                //ハンマーのレベル
            _hammerPower = 0.0f;             //ハンマーのパワー
            _importantPoint = (int)_hammerPowerLimit / _hammerLevelLimit;

            //初期ステートをdefaultにする
            _stateProcessor.State = _stateDefault;
            //委譲の設定
            _stateDefault.execDelegate = Default;
            _stateWalk.execDelegate = Walk;
            _stateJump.execDelegate = Jump;
            _stateDash.execDelegate = Dash;
            _stateStrike.execDelegate = Strike;
        }

        // Update is called once per frame
        void Update()
        {
            //Vector3 axis = new Vector3(Input.GetAxis("Horizontal") , 0, Input.GetAxis("Vertical") );
            //Debug.Log(axis);
            PlayerCtrl();
            DebugCtrl(); //デバッグ用

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
            //HorizontalとVerticalの取得
            float hor = Input.GetAxis("Horizontal");
            float ver = Input.GetAxis("Vertical");

            //絶対値0.3より低いなら動かないようにする
            if(hor <= 0.3f && hor >= -0.3f)
            {
                hor = 0.0f;
            }
            if(ver <= 0.3f && ver >= -0.3f)
            {
                ver = 0.0f;
            }

            //スピードの代入
           _vec = new Vector3(hor * _nowSpeed,0, ver * _nowSpeed);

            //    if (Input.GetKey(_moveKey[(int)MoveDirection.UP]))
            //    {

            //        _vec.z = _nowSpeed;
            //    }


            //    if (Input.GetKey(_moveKey[(int)MoveDirection.DOWN]))
            //    {
            //        _vec.z = -_nowSpeed;
            //    }

            //    if (Input.GetKey(_moveKey[(int)MoveDirection.RIGHT]))
            //    {
            //        _vec.x = _nowSpeed;
            //    }

            //    if (Input.GetKey(_moveKey[(int)MoveDirection.LEFT]))
            //    {
            //        _vec.x = -_nowSpeed;
            //    }
            //    /////////ここより下は停止用処理

            //    if (Input.GetKeyUp(_moveKey[(int)MoveDirection.UP]))
            //    {
            //        _vec.z = 0.0f;
            //    }

            //    if (Input.GetKeyUp(_moveKey[(int)MoveDirection.DOWN]))
            //    {
            //        _vec.z = 0.0f;
            //    }

            //    if (Input.GetKeyUp(_moveKey[(int)MoveDirection.RIGHT]))
            //    {
            //        _vec.x = 0.0f;
            //    }

            //    if (Input.GetKeyUp(_moveKey[(int)MoveDirection.LEFT]))
            //    {
            //        _vec.x = 0.0f;
            //    }
        }

        //ハンマーパワーをchargeする関数
        void ChargeHammerPower()
        {
            //ハンアーキーを押されたら
            if (Input.GetKey(_strikeKey))
            {
                _hammerPower += Time.deltaTime * _hammerChargSpeed;
            }

            //ハンマーパワーを上限を越させない
            if(_hammerPower > _hammerPowerLimit)
            {
                _hammerPower = _hammerPowerLimit;
            }

           
            

        }
        //ハンマーレベルをチェックする関数
        int LevelCheck(int importantPoint, int power)
        {
            int rLevel = 0;
            
            while(true)
            {
                if (power >= importantPoint)
                {
                    rLevel++;
                    power -= importantPoint;
                }
                else
                {
                    //もし0なら1を返す
                    if(rLevel == 0)
                    {
                        rLevel = 1;
                    }
                  break;
                }
           }
            return rLevel; 
        }

        public void PlayerCtrl()
        {
            //速度を足す
            _rg.velocity = new Vector3(_vec.x, this._rg.velocity.y, _vec.z);

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
            //速度を0にする
            _vec = Vector3.zero;

            if(Input.GetKeyDown(_dashKey))
            {
                _stateProcessor.State = _stateDash;
            }

            //ジャンプキーを押されたらジャンプ状態へ
            if (Input.GetKeyDown(_jumpKey))
            {
                _stateProcessor.State = _stateJump;
            }


            //ジャンプキーを押されたらハンマー状態へ
            if (Input.GetKeyDown(_strikeKey))
            {
                _stateProcessor.State = _stateStrike;
            }


            //移動量があればウォークに
            if(Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal")!= 0.0f)
            {
                _stateProcessor.State = _stateWalk;
            }

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
            //速度をムーブスピードにする
            _nowSpeed = MoveSpeed;
            //移動する
            Move();
            //ダッシュキーを押されたら走るステートに切り替え
            if(Input.GetKey(_dashKey))
            {
                _stateProcessor.State = _stateDash;
            }

            //ジャンプキーを押されたらジャンプ状態へ
            if(Input.GetKeyDown(_jumpKey))
            {
                _stateProcessor.State = _stateJump;
            }

            //ジャンプキーを押されたらハンマー状態へ
            if (Input.GetKeyDown(_strikeKey))
            {
                _stateProcessor.State = _stateStrike;
            }

            //止まっていたらステートを通常状態にする
            if (CheckStop() == true)
            {
                _stateProcessor.State = _stateDefault;
            }
        }
        
        //ジャンプ状態
        public void Jump()
        {
            _rg.AddForce(Vector3.up * _nowJumpPower);
            //ジャンプ後defaultに戻す
            _stateProcessor.State = _stateDefault;
        }

        //ダッシュ状態
        public void Dash()
        {
            _nowSpeed = _dashSpeed;
            //移動する
            Move();
            
            ////ダッシュキーを離されたら走るステートに切り替え
            if (Input.GetKeyUp(_dashKey))
            {
                _stateProcessor.State = _stateWalk;
            }

            //ジャンプキーを押されたらジャンプ状態へ
            if (Input.GetKeyDown(_jumpKey))
            {
                _stateProcessor.State = _stateJump;
            }

            //ジャンプキーを押されたらハンマー状態へ
            if (Input.GetKeyDown(_strikeKey))
            {
                _stateProcessor.State = _stateStrike;
            }

            //止まっていたらステートを通常状態にする
            if (CheckStop() == true)
            {
                _stateProcessor.State = _stateDefault;
            }

        }

        //叩き状態
        public void Strike()
        {
            _dashSpeed = Speedlimit;
            Move();//歩く

            //ハンマーパワーをチャージ
            ChargeHammerPower();

            //ハンマーキーを離したら
            if (Input.GetKeyUp(_strikeKey))
            {
                _hammerLevel = LevelCheck( _importantPoint, (int)_hammerPower);
                //パワーを0にする
                _hammerPower = 0.0f;
                //ステートをデフォルトに
                _stateProcessor.State = _stateDefault;
            }
        }
         

        //デバッグ用関数
        public void DebugCtrl()
        {
            _chargeText.text = _hammerPower.ToString(); //現在のハンマーパワー
            _levelText.text = _hammerLevel.ToString();  //現在のレベル
        }

    }
}