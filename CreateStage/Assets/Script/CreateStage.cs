using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using System;


namespace Momoya
{
public class CreateStage : MonoBehaviour
{
        //列挙型の宣言
       public enum ObjectType
        {
            FloorNone = -1,     //何にもなし
            FloorWhite001, //白床001
            FloorBlue001,  //青床001
            FloorRed001,   //赤床001
            FloorGreen001, //緑床001
            FloorBlack001, //黒床001

            Num
        }

        public GameObject floor;

        //定数の定義
        [SerializeField]
        private  int _width = 10;
        //変数の定義
        private Vector3 startPos;
        private string _fileName; //ファイルの名前
        private string _filePath; //ファイルパス        
        [SerializeField]
        private GameObject[] _gameObj = new GameObject[(int)ObjectType.Num]; //オブジェクトを入れる

        private List<int> _objectDataLest; //オブジェクトデータリスト
    // Start is called before the first frame update
    void Start()
    {
            startPos = this.transform.position;

            _fileName = "StageData001.csv";
            _filePath = Application.dataPath + @"\Data\StageData\" + _fileName;

            _objectDataLest = new List<int>(); //データリスト作成  
            ReadFile();//ファイルを読み込む
            BuildFloor(); //床を作る
            BuildStage(); //ステージを作る
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        //ファイル読み込み
        public void ReadFile()
        {
            // _csvData.Clear();

            int objData;
            //　一括で取得
            string[] texts = File.ReadAllText(_filePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in texts)
            {
                int tmp = 0;
                Int32.TryParse(text,out tmp);

                _objectDataLest.Add(tmp);
            }
            objData = _objectDataLest.Count;

            //デバッグ
            for(int i = 0; i < objData; i++)
            {
                Debug.Log(_objectDataLest[i]);
            }

        }
        //作成関数
        public void BuildStage()
        {
            for(int i = _objectDataLest.Count -1; i >= 0;i--)
            {
                if(_objectDataLest[i] != -1)
                {
                    GameObject go = Instantiate(_gameObj[_objectDataLest[i]]);
                    go.transform.position = this.transform.position;
                }
                if((i) % _width != 0)
                {
                    transform.position = new Vector3(this.transform.position.x + 1.0f, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(startPos.x, this.transform.position.y, this.transform.position.z - 1);
                }
                
            }
        }

        //床を作る関数
        public void BuildFloor()
        {
            for (int i = _objectDataLest.Count - 1; i >= 0; i--)
            {
                GameObject go = Instantiate(floor);
                go.transform.position = this.transform.position;
                if ((i) % _width != 0)
                {
                    transform.position = new Vector3(this.transform.position.x + 1.0f, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(startPos.x, this.transform.position.y, this.transform.position.z - 1.0f);
                }

            }

            transform.position = new Vector3(startPos.x, this.transform.position.y + 1.0f, startPos.z);
        }
    }

}
