using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseFlug : MonoBehaviour
{
    // Start is called before the first frame update

    /*
     フラグは2進数によって管理されています。
     下のenumでstateを設定しておきます
     (1 << 0)
     左側は変える数
     右側は変える所をどれだけシフトさせるか
     を表しています
    */
    public enum SampleFlug
    {
        SPACE_FLUG      = (1 << 0),     // スペースキーが押された時のフラグ(0001)
        LEFT_SHIFT_FLUG = (1 << 1),     // 左シフトキーが押された時のフラグ(0010)
    }

     

　  private Goto.Flag _flug;    // フラグ管理（これ一つでフラグ管理ができる）

    void Start()
    {
        _flug = new Goto.Flag();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            // _flug.OnFlug(ここに立てたいstateを書く);
            _flug.OnFlag((uint)SampleFlug.SPACE_FLUG);

            // _flug.IsFlug(ここに立っているかみたいstaeを書く)
            Debug.Log("SPACE_FLUG:" + _flug.IsFlag((uint)SampleFlug.SPACE_FLUG));
        }
        else
        {
            // _flug.OffFlug(ここに伏せたいstateを書く)
            _flug.OffFlag((uint)SampleFlug.SPACE_FLUG);

            // _flug.IsFlug(ここに立っているかみたいstaeを書く)
            Debug.Log("SPACE_FLUG:" + _flug.IsFlag((uint)SampleFlug.SPACE_FLUG));
        }

        /*
            以上が使い方となっています。
            このフラグは同時に二つ以上のフラグを立てることも可能です
            例
            _flug.OnFlug((uint)SampleFlug.SPACE_FLUG + (uint)SampleFlug.LEFT_SHIFT_FLUG);
            このように二つ以上立てたい場合は+をすれば可能になります。
            これは、伏せるとき　見るときも同様のやり方です。
            LeftShiftは使っていないので試したい人はやってみるといいかもしれません

            以上で分からない人や疑問点などがあれば答えるので聞いてみてください
         
        */
    }
}
