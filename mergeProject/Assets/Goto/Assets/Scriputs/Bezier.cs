using UnityEngine;
using System.Collections;

namespace Goto
{
    public class Bezier
    {
        /// <summary>
        /// 始点
        /// </summary>
        public Vector2 P1
        {
            get;
            set;
        }

        /// <summary>
        /// 始点制御点
        /// </summary>
        public Vector2 P2
        {
            get;
            set;
        }

        /// <summary>
        /// 終点制御点
        /// </summary>
        public Vector2 P3
        {
            get;
            set;
        }

        /// <summary>
        /// 終点
        /// </summary>
        public Vector2 P4
        {
            get;
            set;
        }

        /// <summary>
        /// パラメータが有効か
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (P1 == null) return false;
                if (P2 == null) return false;
                if (P3 == null) return false;
                if (P4 == null) return false;
                return true;
            }
        }
        /// <summary>
        /// パラメータt(0～1)を指定して位置を取得します。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public Vector2 GetPosition(float t)
        {
            if (t <= 0) return new Vector2(P1.x, P1.y);
            if (t >= 1) return new Vector2(P4.x, P4.y);

            float x1 = P1.x;    // 始点のX座標を設定
            float x2 = P2.x;    // 始点制御点のX座標を設定
            float x3 = P3.x;    // 終点制御点のX座標を設定
            float x4 = P4.x;    // 終点のX座標を設定
            float y1 = P1.y;    // 始点のY座標を設定
            float y2 = P2.y;    // 始点制御点のY座標を設定
            float y3 = P3.y;    // 終点制御点のY座標を設定
            float y4 = P4.y;    // 終点のY座標を設定
            float tp = 1 - t;
            float x = t * t * t * x4 + 3 * t * t * tp * x3 + 3 * t * tp * tp * x2 + tp * tp * tp * x1;
            float y = t * t * t * y4 + 3 * t * t * tp * y3 + 3 * t * tp * tp * y2 + tp * tp * tp * y1;
            return new Vector2(x, y);
        }
    }
}
