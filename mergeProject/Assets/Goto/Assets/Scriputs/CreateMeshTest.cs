using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goto
{
    public class CreateMeshTest : MonoBehaviour
    {
        public Vector3[] _verticArray;          // 頂点配列
        public Vector2[] _uvArray;              // UV配列
        public int[] _triangleArray;            // 三角形の順番配列
        private Mesh _mesh;                     // メッシュ
        private MeshRenderer _meshRenderer;     // メッシュ表示コンポーネント
        public Material _material;              // メッシュに設定するマテリアル
                                                // Start is called before the first frame update
        void Start()
        {
            _verticArray = new Vector3[]
            {
                new Vector3(0f, 0f, 0f),
                new Vector3(0f, -1f, 0f),
                new Vector3(-1f, -1f, 0f),
                new Vector3(-1f, 0f, 0f),
            };

            _uvArray = new Vector2[]
            {
                new Vector2(0f, 0f),
                new Vector2(0f, 1f),
                new Vector2(1f, 1f),
                new Vector2(1f, 0f),
            };

            _triangleArray = new int[]
            {
                0, 1, 2,
                0, 2, 3,
            };

            gameObject.AddComponent<MeshFilter>();
            _meshRenderer = gameObject.AddComponent<MeshRenderer>();
            _mesh = GetComponent<MeshFilter>().mesh;
            _meshRenderer.material = _material;
        }

        // Update is called once per frame
        void Update()
        {
            CreateMesh(_mesh, _verticArray, _uvArray, _triangleArray);
        }

        bool CreateMesh(Mesh mesh, Vector3[] vertices, Vector2[] uv, int[] triangles)
        {
            // 初めにメッシュをクリア
            mesh.Clear();
            // 頂点の設定
            mesh.vertices = vertices;
            // テクスチャのUV座標設定
            mesh.uv = uv;
            // 三角形メッシュの設定
            mesh.triangles = triangles;
            // Boundsの再計算
            mesh.RecalculateBounds();
            // NormalMapの再計算
            mesh.RecalculateNormals();

            return true;
        }
    }
}
