using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Isogai
{
    public class MouseController : MonoBehaviour
    {
        public enum MouseCSFlag
        {
            FIRST_HIT_FLAG = (1 << 0),      // 1つ目のブロックを取った時のフラグ(0001)
            SECOND_HIT_FLAG = (1 << 1),     // 2つ目のブロックを取った時のフラグ(0010)
            CLICK_FLAG = (1 << 2),          // クリックされた時のフラグ(0100)
        }

        private GameObject _firstObject;   // 1つ目のオブジェクト
        private GameObject _secondObject;  // 2つ目のオブジェクト
        private Goto.Flag _mouseCSFlag;    // フラグ管理

        // Start is called before the first frame update
        void Start()
        {
            _firstObject = null;
            _secondObject = null;
            _mouseCSFlag = new Goto.Flag();
        }

        // Update is called once per frame
        void Update()
        {
            // フラグを伏せる
            _mouseCSFlag.OffFlag((uint)MouseCSFlag.CLICK_FLAG);

            // クリックしたら
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
                // 1つ目のオブジェクトが無かったら
                if (_mouseCSFlag.IsFlag((uint)MouseCSFlag.FIRST_HIT_FLAG) == false)
                {
                    // レイが当たっていたら
                    if (hit.collider)
                    {
                        // フラグを立てる
                        _mouseCSFlag.OnFlag((uint)MouseCSFlag.FIRST_HIT_FLAG);
                        _mouseCSFlag.OnFlag((uint)MouseCSFlag.CLICK_FLAG);
                        // 当たったオブジェクトを代入する
                        _firstObject = hit.collider.gameObject;
                        Debug.Log("持った");
                    }
                }
                // 1つ目のオブジェクトがあったら
                if (_mouseCSFlag.IsFlag((uint)MouseCSFlag.FIRST_HIT_FLAG) == true)
                {
                    // このフレームにクリックされていなかったら
                    if (_mouseCSFlag.IsFlag((uint)MouseCSFlag.CLICK_FLAG) == false)
                    {
                        // レイが当たっていたら
                        if (hit.collider)
                        {
                            // レイが当たったオブジェクトが1つ目と同じだったら
                            if (hit.collider.gameObject.name == _firstObject.name)
                            {
                                // 1つ目のオブジェクトをnullにする
                                _firstObject = null;
                                // フラグを伏せる
                                _mouseCSFlag.OffFlag((uint)MouseCSFlag.FIRST_HIT_FLAG);
                                Debug.Log("置いた");
                            }
                            // レイが当たったオブジェクトが1つ目と違ったら
                            else
                            {
                                // 当たったオブジェクトを代入する
                                _secondObject = hit.collider.gameObject;
                                // フラグを伏せる
                                _mouseCSFlag.OffFlag((uint)MouseCSFlag.FIRST_HIT_FLAG);
                                // フラグを立てる
                                _mouseCSFlag.OnFlag((uint)MouseCSFlag.SECOND_HIT_FLAG);
                                Debug.Log("チェンジ");
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 1つ目のオブジェクトのゲッター
        /// </summary>
        /// <returns>1つ目のオブジェクト</returns>
        public GameObject GetFirstObject()
        {
            return _firstObject;
        }

        /// <summary>
        /// 2つ目のオブジェクトのゲッター
        /// </summary>
        /// <returns>2つ目のオブジェクト</returns>
        public GameObject GetSecondObject()
        {
            return _secondObject;
        }

        /// <summary>
        /// フラグのゲッター
        /// </summary>
        /// <returns>フラグ</returns>
        public Goto.Flag GetMouseCSFlag()
        {
            return _mouseCSFlag;
        }
    }
}