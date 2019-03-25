using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isogai
{
    public class BlockController : MonoBehaviour
    {
        public enum BlockCSFlag
        {
            CHENGE_FLAG = (1 << 0),      // 入れ替え可能かどうかのフラグ(0001)
        }

        private GameObject _mouse;          // マウスオブジェクト
        private GameObject _firstObject;    // 1つ目のオブジェクト
        private GameObject _secondObject;   // 2つ目のオブジェクト
        private Vector3 _firstPos;          // 1つ目のオブジェクトの移動前の位置
        private Vector3 _secondPos;         // 2つ目のオブジェクトの移動前の位置
        private Goto.Flag _blockCSFlag;     // フラグ管理

        // Start is called before the first frame update
        void Start()
        {
            _mouse = GameObject.Find("Mouse");
            _firstObject = null;
            _secondObject = null;
            _blockCSFlag = new Goto.Flag();
        }

        // Update is called once per frame
        void Update()
        {
            // 2つ目のオブジェクトがクリックされたら
            if (_mouse.GetComponent<MouseController>().GetMouseCSFlag().IsFlag((uint)Isogai.MouseController.MouseCSFlag.SECOND_HIT_FLAG) == true)
            {
                // 入れ替えしていなかったら
                if (_blockCSFlag.IsFlag((uint)BlockCSFlag.CHENGE_FLAG) == false)
                {
                    // オブジェクトを代入
                    _firstObject = _mouse.GetComponent<MouseController>().GetFirstObject();
                    _secondObject = _mouse.GetComponent<MouseController>().GetSecondObject();
                    // 位置を代入
                    _firstPos = _firstObject.transform.position;
                    _secondPos = _secondObject.transform.position;
                    // フラグを立てる(入れ替え可能にする)
                    _blockCSFlag.OnFlag((uint)BlockCSFlag.CHENGE_FLAG);
                }
            }
            // 入れ替え可能だったら
            if (_blockCSFlag.IsFlag((uint)BlockCSFlag.CHENGE_FLAG) == true)
            {
                // 1つ目のオブジェクトと同じ名前だったら
                if (_firstObject.name == this.name)
                {
                    // 2つ目のオブジェクトのあった位置に移動
                    transform.position = Vector2.MoveTowards(transform.position, _secondPos, 0.1f);
                }
                // 2つ目のオブジェクトと同じ名前だったら
                if (_secondObject.name == this.name)
                {
                    // 1つ目のオブジェクトのあった位置に移動
                    transform.position = Vector2.MoveTowards(transform.position, _firstPos, 0.1f);
                }

                // 移動し終わったら
                if (_firstObject.transform.position == _secondPos || _secondObject.transform.position == _firstPos)
                {
                    // フラグを伏せる
                    _blockCSFlag.OffFlag((uint)BlockCSFlag.CHENGE_FLAG);
                    _mouse.GetComponent<MouseController>().GetMouseCSFlag().OffFlag((uint)Isogai.MouseController.MouseCSFlag.SECOND_HIT_FLAG);
                }
            }
        }
    }
}