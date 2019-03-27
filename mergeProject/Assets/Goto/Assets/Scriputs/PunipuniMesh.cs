using UnityEngine;
using System.Collections;

namespace Goto
{
    /// <summary>
    /// ぷにぷにコントローラー用メッシュを保持します。
    /// </summary>
    public class PunipuniMesh : MonoBehaviour
    {
        private Mesh _mesh;                         // メッシュ
        
        private Vector3[] _vertexesArray;           // 頂点座標配列

        /// <summary>
        /// メッシュのgetter
        /// </summary>
        public Mesh Mesh
        {
            get { return _mesh; }
        }

        /// <summary>
        /// Vertexesのgetter&setter
        /// </summary>
        public Vector3[] Vertexes
        {
            get
            {
                Vector3[] vtx = new Vector3[_mesh.vertices.Length - 1];
                for (int i = 0; i < vtx.Length; i++)
                {
                    vtx[i] = _mesh.vertices[i];
                }
                return vtx;
            }
            set
            {
                var vtx = _mesh.vertices;
                for (int i = 0;i < value.Length; i++)
                {
                    vtx[i] = value[i];
                }
                _mesh.vertices = vtx;
                _mesh.RecalculateBounds();
            }
        }

        /// <summary>
        /// 中心座標のgetter&setter
        /// </summary>
        public Vector3 CenterPoint
        {
            get
            {
                return _mesh.vertices[_mesh.vertices.Length - 1];
            }
            set
            {
                Vector3[] vtx = _mesh.vertices;
                vtx[vtx.Length - 1] = value;
                _mesh.vertices = vtx;
                _mesh.RecalculateBounds();
            }
        }

        /// <summary>
        /// OriginalVertexesのgetter
        /// </summary>
        public Vector3[] OriginalVertexes
        {
            get { return _vertexesArray; }
        }
        

        /// <summary>
        /// メッシュを作成
        /// </summary>
        /// <param name="vertexCount">頂点数</param>
        /// <param name="radius">半径</param>
        public PunipuniMesh(int vertexCount, float radius)
        {
            CreateMesh(vertexCount, radius);
        }

        /// <summary>
        /// ぷにぷにコントローラー用メッシュを生成
        /// </summary>
        /// <param name="vertexCount">頂点数</param>
        /// <param name="radius">半径</param>
        /// <returns>true or false</returns>
        bool CreateMesh(int vertexCount, float radius)
        {
            _mesh = new Mesh();

            // 頂点の生成
            Vector3[] points = new Vector3[vertexCount + 1];
            float angle = Mathf.PI * 2.0f;
            for (int i = 0; i < vertexCount; i++)
            {
                float r = angle / vertexCount * i;
                float x = Mathf.Cos(r) * radius;
                float y = Mathf.Sin(r) * radius;
                points[i] = new Vector3((float)x, (float)y, 0.0f);
            }
            points[vertexCount] = new Vector3(0, 0, 0);   // 中心
            _mesh.vertices = points;

            _vertexesArray = new Vector3[points.Length - 1];
            for (int i = 0; i < points.Length - 1; i++)
            {
                _vertexesArray[i] = new Vector3(points[i].x, points[i].y, points[i].z);
            }

            // 頂点インデックス生成
            int[] indexes = new int[vertexCount * 3];
            for (int i = 0; i < vertexCount; i++)
            {
                indexes[i * 3 + 0] = i;
                indexes[i * 3 + 1] = (i + 1) % vertexCount;
                indexes[i * 3 + 2] = vertexCount;
            }
            _mesh.triangles = indexes;

            // 頂点色の生成
            Color[] colors = new Color[vertexCount + 1];
            for (int i = 0; i < vertexCount; i++)
            {
                colors[i] = new Color(1f, 1f, 1f, 1f);
            }
            colors[vertexCount] = new Color(0f, 0f, 0f, 0f);
            _mesh.colors = colors;

            //_mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            
            return true;
        }
    }
}
