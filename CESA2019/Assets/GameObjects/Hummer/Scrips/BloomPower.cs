using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
/// <file>		BloomPower.cs
/// 
/// <brief>		ハンマー関連のC++
/// 
/// <date>		2019/5/15
/// 
/// <author>	後藤　駿介
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/


///-----------------------------------------------------------------------------
/// 
/// </brief> Intensityクラス
///
///-----------------------------------------------------------------------------

public class BloomPower : MonoBehaviour
{

    private Material _bloom;
    private float _intensityAmount;
    [SerializeField]
    private GameObject _playerControllerObject;
    private Momoya.PlayerController _playerController;

    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        _intensityAmount = 0f;
        _playerController = _playerControllerObject.GetComponent<Momoya.PlayerController>();
        _bloom = gameObject.GetComponent<Renderer>().material;

    }
    /// <summary>
    /// 更新処理
    /// </summary>
    void Update()
    {
        if(_playerController.GetPlayerMoveStaet().IsFlag((uint)Momoya.PlayerController.PlayerMoveStaet.HAMMER_MOVE_STATE))
        {
            
            switch (_playerController.GetNowHammerState())
            {
                case (int)Momoya.PlayerController.HammerState.WEAK:
                    _bloom.SetFloat("Intensity", 3f);
                    break;

                case (int)Momoya.PlayerController.HammerState.NOMAL:
                    _bloom.SetFloat("Intensity", 5f);
                    break;

                case (int)Momoya.PlayerController.HammerState.STRENGTH:
                    _bloom.SetFloat("Intensity", 8f);
                    break;

            }

        }

        //_bloom.SetFloat("Intensity", )
    }
}
