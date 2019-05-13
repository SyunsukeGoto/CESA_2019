using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kazu
{
    public class CarContoller : MonoBehaviour
    {
        //定数の定義
        const int DoNotMove = 180;         //停止時間
        const float DropdownPoint = -3.0f; //落下ポイント
        //変数の定義
        [SerializeField]
        private Vector3 _carSpeed;         //スピード
                                           
        private Rigidbody _rg;             //リジットボディ
        private Vector3 _startPos;         //初期位置

        // Start is called before the first frame update
        void Start()
        {
            _rg = GetComponent<Rigidbody>(); //リジットボディの取得
            _startPos = _rg.position;        //初期位置の設定
        }

        // Update is called once per frame
        void Update()
        {
            //スピードの加速
            _rg.velocity = new Vector3(_carSpeed.x,this._rg.velocity.y,_carSpeed.z);

            //デバッグ用
            //落下ポイントよりポジションが低ければ初期位置に戻す
            if (_rg.position.y < DropdownPoint)
            {
                _rg.position = _startPos;
            }
        }

        public int GetTime()
        {
            return DoNotMove;
        }
    }
}

