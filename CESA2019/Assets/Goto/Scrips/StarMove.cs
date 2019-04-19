//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__
/// <file>		StarMove.cs
/// 
/// <brief>		☆関連のC++
/// 
/// <date>		2019/4/14
/// 
/// <author>	後藤　駿介
//__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__|__

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goto
{
    public class StarMove : MonoBehaviour
    {
        [SerializeField]
        private GameObject _starPrefab;                             // 星のプレハブ

        private GameObject[] _starObject = new GameObject[5];       // 星のゲームオブジェクト

        private float _starAngle;                                   // 角度

        private GameObject _parent;


        // Start is called before the first frame update
        void Start()
        {
            _starAngle = 0f;

            for (int i = 0; i < _starObject.Length; i++)
            {
                _starObject[i] = Instantiate(_starPrefab) as GameObject;
            }
            _parent = transform.root.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            _starAngle++;
            _starAngle = _starAngle > 360 ? 0 : _starAngle;

            for (int i = 0; i < _starObject.Length; i++)
            {
                int angle = i * (360 / 5);
                float x = Mathf.Cos((angle + _starAngle) * Mathf.Deg2Rad);
                float z = Mathf.Sin((angle + _starAngle) * Mathf.Deg2Rad); ;
                _starObject[i].transform.position = new Vector3(_parent.transform.position.x + x, transform.position.y, _parent.transform.position.z + z);
            }
        }
    }
}