using UnityEngine;
using System.Collections;

namespace Goto
{
    /// <summary>
    /// ぷにぷにコントローラー
    /// </summary>
    public class PunipuniController : MonoBehaviour
    {
        public Camera _targetCamera;                    // 描画対象のカメラ
        public Material _material;                      // 描画対象のマテリアル
        public Flag _puniFlug;
        
        [SerializeField]
        private float __radiusPixel = 30f;              // 半径
        private float _radius;                          // 半径
        [SerializeField]
        private int _mesh;                              // メッシュの数
        private PunipuniMesh _puniMesh;                 // ぷにメッシュ        
        private MeshRenderer _renderer;                 // メッシュ描画
        
        private Bezier _bezierCenter = new Bezier();    // ベジェ曲線パラメータ　中央
        private Bezier _bezierLeft = new Bezier();      // ベジェ曲線パラメータ　左
        private Bezier _bezierRight = new Bezier();     // ベジェ曲線パラメータ　右
        
        private Vector3 _beginMousePosition;            // MousePos
        
        private float ArrivalTime = 10;                 // frame count(本来はtimeがいい)

        /// <summary>
        /// ぷにぷにコントローラーの表示設定
        /// </summary>
        private bool VisiblePunipuniController
        {
            get
            {
                if (this._renderer != null)
                {
                    return this._renderer.enabled;
                }
                return false;
            }
            set
            {
                if (this._renderer != null)
                {
                    this._renderer.enabled = value;
                }
            }
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        void Start()
        {
            Vector3 p1 = _targetCamera.ScreenToWorldPoint(new Vector3(this.__radiusPixel, this.__radiusPixel, 0f));
            Vector3 p2 = _targetCamera.ScreenToWorldPoint(new Vector3(-this.__radiusPixel, -this.__radiusPixel, 0f));
            this._radius = System.Math.Abs(p1.x - p2.x);

            // メッシュを作成しMeshRendererを追加
            
            _puniMesh = new PunipuniMesh(_mesh, this._radius);
            AddMeshRenderer(gameObject, this._material);

            // MeshRendererを保持
            this._renderer = GetComponent<MeshRenderer>();

            // 非表示
            VisiblePunipuniController = false;

            _puniFlug = new Flag();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        void Update()
        {
            if (Input.GetMouseButtonDown(0)) BeginPunipuni();
            if (Input.GetMouseButtonUp(0)) EndPunipuni();
            if (Input.GetMouseButton(0)) TrackingPunipuni();
        }

        /// <summary>
        /// 指定GameObjectにMeshRendererを追加
        /// </summary>
        /// <param name="target"></param>
        /// <param name="material"></param>
        void AddMeshRenderer(GameObject target, Material material)
        {
            // メッシュ設定
            MeshFilter meshFilter = target.AddComponent<MeshFilter>();
            meshFilter.mesh =  _puniMesh.Mesh;
            // meshFilter.sharedMesh = this.Mesh;

            // gameObject.AddComponent<MeshCollider>();
            //gameObject.GetComponent<MeshFilter>().sharedMesh.name = name;
            //gameObject.GetComponent<MeshCollider>().sharedMesh = PuniMesh.Mesh;

            { // マテリアル設定
                MeshRenderer renderer = target.AddComponent<MeshRenderer>();
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
                renderer.material = material;
            }
        }

        /// <summary>
        /// ぷにぷにコントローラーの開始
        /// </summary>
        void BeginPunipuni()
        {
            // 初期位置
            this._beginMousePosition = _targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

            // 位置
            transform.position = _targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

            // 表示
            VisiblePunipuniController = true;

            // ベジェ曲線パラメータ
            int x = 0;
            int y = 0;
            _bezierCenter.P1 = new Vector2(x, y);     // 中心
            _bezierCenter.P2 = new Vector2(x, y);     // 制御点1
            _bezierCenter.P3 = new Vector2(x, y);     // 制御点2
            _bezierCenter.P4 = new Vector2(x, y);     // 終点
            _bezierLeft.P1 = new Vector2(x, y);     // 中心
            _bezierLeft.P2 = new Vector2(x, y);     // 制御点1
            _bezierLeft.P3 = new Vector2(x, y);     // 制御点2
            _bezierLeft.P4 = new Vector2(x, y);     // 終点
            _bezierRight.P1 = new Vector2(x, y);     // 中心
            _bezierRight.P2 = new Vector2(x, y);     // 制御点1
            _bezierRight.P3 = new Vector2(x, y);     // 制御点2
            _bezierRight.P4 = new Vector2(x, y);     // 終点
        }

        /// <summary>
        /// ぷにぷにコントローラーの終了
        /// </summary>
        void EndPunipuni()
        {
            // 表示
            VisiblePunipuniController = false;
        }

        /// <summary>
        /// ぷにぷにコントローラーの追跡処理
        /// </summary>
        void TrackingPunipuni()
        {
            // ベジェ曲線パラメータの更新
            Vector3 pos = _targetCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            float x = pos.x - this._beginMousePosition.x;
            float y = pos.y - this._beginMousePosition.y;
            UpdateBezierParameter(x, y);
            // Debug.LogFormat("Mouse X = {0}, y = {1}", Input.mousePosition.x, Input.mousePosition.y);

            // メッシュの更新
            _puniMesh.Vertexes = TransformFromBezier(new Vector3());
        }

        /// <summary>
        /// ベジェ曲線のパラメータを更新
        /// </summary>
        /// <param name="x">中心座標からドラッグした差分X</param>
        /// <param name="y">中心座標からドラッグした差分Y</param>
        private void UpdateBezierParameter(float x, float y)
        {
            AnimateBezierParameter(this._bezierCenter, this._bezierCenter, x, y);

            { // 他の2個のベジェの開始位置を更新
                Vector2 dir = this._bezierCenter.P2 - this._bezierCenter.P1;
                Vector2 dirL = new Vector2(dir.y, -dir.x);
                Vector2 dirR = new Vector2(-dir.y, dir.x);
                dirL = dirL.normalized;
                dirR = dirR.normalized;
                dirL.x = dirL.x * this._radius + this._bezierCenter.P1.x;
                dirL.y = dirL.y * this._radius + this._bezierCenter.P1.y;
                dirR.x = dirR.x * this._radius + this._bezierCenter.P1.x;
                dirR.y = dirR.y * this._radius + this._bezierCenter.P1.y;
                this._bezierLeft.P1 = dirL;
                this._bezierRight.P1 = dirR;
            }

            AnimateBezierParameter(this._bezierLeft, this._bezierCenter, x, y);
            AnimateBezierParameter(this._bezierRight, this._bezierCenter, x, y);
        }



        /// <summary>
        /// ベジェ曲線パラメータを更新
        /// </summary>
        /// <param name="bezier">ベジェ曲線</param>
        /// <param name="baseBezierier">元のベジェ曲線</param>
        /// <param name="x">中心座標からドラッグした差分X</param>
        /// <param name="y">中心座標からドラッグした差分Y</param>
        private void AnimateBezierParameter(Bezier bezier, Bezier baseBezier, float x, float y)
        {
            // 先端点
            bezier.P4 = new Vector2(x, y);

            { // 先端制御点
                if (bezier.P3 != null)
                {
                    Vector2 vec = baseBezier.P1 - bezier.P1;
                    vec = vec.normalized * (this._radius / 4);

                    Vector2 pos = bezier.P3 - bezier.P4;
                    pos += vec;
                    pos /= this.ArrivalTime;
                    bezier.P3 -= pos;
                }
                else
                {
                    bezier.P3 = new Vector2(x, y);
                }
            }

            { // 中心制御点
              // 最終的な位置
                Vector2 ev = baseBezier.P4 - baseBezier.P1;
                float len = ev.magnitude;
                ev = ev.normalized;
                ev *= (len / 4);
                ev += bezier.P1;

                if (bezier.P2 != null)
                {
                    Vector2 v = ev - bezier.P2;
                    v /= 3; // this.ArrivalTime;
                    bezier.P2 += v;
                }
                else
                {
                    bezier.P2 = new Vector2(bezier.P1.x, bezier.P1.y);
                }
            }
        }

        /// <summary>
        /// 操作対象の頂点インデックスを取得
        /// </summary>
        /// <param name="center">中心座標</param>
        /// <param name="startIndex">最初の指数</param>
        /// <param name="endIndex">最後の指数</param>
        void GetMoveFixedVertexIndex(Vector3 center, out int startIndex, out int endIndex)
        {
            Vector3[] points = _puniMesh.OriginalVertexes;

            int sidx = -1;
            int eidx = -1;
            int idx = 0;
            bool recheckStart = true;
            bool recheckEnd = true;
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 point = points[i];

                if (_bezierLeft.IsValid)
                {
                    Vector3 PT = point - center;
                    Vector2 AB = _bezierCenter.P1 - _bezierLeft.P1;
                    float c1 = AB.x * PT.y - AB.y * PT.x;
                    if (c1 < 0)
                    {
                        // move
                        if (recheckStart)
                        {
                            sidx = idx;
                            recheckStart = false;
                            recheckEnd = true;
                        }
                    }
                    else
                    {
                        // fixed
                        if (recheckEnd)
                        {
                            eidx = idx - 1;
                            recheckStart = true;
                            recheckEnd = false;
                        }
                    }
                }
                ++idx;
            }
            startIndex = sidx;
            endIndex = eidx;
        }

        /// <summary>
        /// ベジェ曲線パラメータに応じてメッシュを変形
        /// </summary>
        /// <param name="center">中心座標</param>
        Vector3[] TransformFromBezier(Vector3 center)
        {
            Vector3[] points = _puniMesh.Vertexes;
            Vector3[] orgPoints = _puniMesh.OriginalVertexes;

            // 操作対象の頂点インデックスを取得
            int si;
            int ei;

            GetMoveFixedVertexIndex(center, out si, out ei);

            if (ei == -1) ei = points.Length - 1;

            if (si == -1 || ei == -1)
            {
                return orgPoints;
            }

            if (si > ei) ei += points.Length;

            int useCount = ei - si;

            if (useCount <= 0)
            {
                return orgPoints;
            }

            int centerIdx = (int)(useCount / 2) + si;
            int count1 = centerIdx - si;
            int count2 = ei - centerIdx;
            // Debug.LogFormat("{0}Num, {1}, {2}Num", count1, centerIdx, count2);

            for (int i = 0; i < points.Length;i++)
            {
                points[i] = orgPoints[i];
            }

            for (int i = 0; i < count1; i++)
            {
                float t = (float)(i + 1) / (float)(count1 + 1);
                Vector2 point = _bezierLeft.GetPosition(t);

                // 半径内にある場合はオリジナルを使用
                Vector3 dist = new Vector3(point.x, point.y, center.z) - center;
                int index = (i + si) % points.Length;
                if (dist.magnitude > this._radius)
                {
                    points[index].x = point.x;
                    points[index].y = point.y;
                }
            }
            for (int i = 0; i < count2; i++)
            {
                float t = (float)(i + 1) / (float)(count2 + 1);
                Vector2 point = _bezierRight.GetPosition(t);

                // 半径内にある場合はオリジナルを使用
                Vector3 dist = new Vector3(point.x, point.y, center.z) - center;
                int index = (ei - i) % points.Length;
                if (dist.magnitude > this._radius)
                {
                    points[index].x = point.x;
                    points[index].y = point.y;
                }
            }
            {
                // 半径内にある場合はオリジナルを使用
                Vector3 dist = new Vector3(_bezierCenter.P4.x, _bezierCenter.P4.y, center.z) - center;
                int index = (centerIdx) % points.Length;
                if (dist.magnitude > this._radius)
                {
                    // 中心
                    points[index].x = _bezierCenter.P4.x;
                    points[index].y = _bezierCenter.P4.y;
                }
            }
            return points;
        }
    }
}
