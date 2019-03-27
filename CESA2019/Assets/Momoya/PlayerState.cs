using UnityEngine;
using System.Collections;


//プレイヤーのステート
namespace PlayerState
{

    //ステートの実行を管理するクラス
    public class StateProcessor
    {
        //ステート本体
        private PlayerState _state;
        // ステートを取得、セットをするプロパティ
        public PlayerState State
        {
            set { _state = value; }
            get { return _state; }
        }

        // 実行関数
        public void Execute()
        {
            State.Execute();
        }

    }

    //ステートのクラス
    public abstract class PlayerState
    {
        //委譲
        public delegate void ExecuteState();
        public ExecuteState execDelegate;

        //実行関数
        public virtual void Execute()
        {
            if (execDelegate != null)
            {
                execDelegate();
            }
        }

        //現在のステートをストリング型で返す(c++で純粋仮想関数みたいなやつ)
        public abstract string GetStateName();
    }

    // 以下状態クラス

    //  デフォルト状態
    public class StateDefault : PlayerState
    {
        public override string GetStateName()
        {
            return "Player is Default";
        }
    }

    //  歩き状態
    public class StateWalk : PlayerState
    {
        public override string GetStateName()
        {
            return "Plaer Is Walk";
        }
    }

}