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

        public enum GroundType
        {
            GroundNone = -1, //何もなし
            GroundWhite001,  //白床
            GroundBlue001,  //青床001
            GroundRed001,   //赤床001
            GroundGreen001, //緑床001
            GroundBlack001, //黒床001

            Num
        }


        

        //定数の定義
        [SerializeField]
        private  int _width = 1;
        //変数の定義
        private Vector3 startPos;
        private int _searchWidth;
    
        [SerializeField]
        private string _objFileName = "ObjectData"; //ファイルの名前(オブジェクトよう)
        [SerializeField]
        private string _stageFileName = "StageData"; //ファイルの名前(スーテジ用)
        [SerializeField]
        private int _stageNumber = 1; //ステージ番号
        private string _openFilenameExtension;
        private string _objFilePath; //オブジェクト用ファイルパス
        private string _stageFilePath; //オブジェクト用ファイルパス         
        [SerializeField]
        private GameObject[] _gameObj = new GameObject[(int)ObjectType.Num]; //オブジェクトを入れる
        [SerializeField]
        private GameObject[] _floorObj = new GameObject[(int)GroundType.Num];//地面用の生成オブジェクト
        private List<int> _objectDataList; //オブジェクトデータリスト
        private List<int> _stageDataList;  //ステージデータリスト
    // Start is called before the first frame update
    void Start()
    {
            startPos = this.transform.position;
            _searchWidth = 0;
            _openFilenameExtension = ".csv";
                
            _objFilePath = Application.dataPath + @"\Data\StageData\" + _objFileName + _stageNumber.ToString() + _openFilenameExtension;
            _stageFilePath = Application.dataPath + @"\Data\StageData\" + _stageFileName + _stageNumber.ToString() + _openFilenameExtension;

            _objectDataList = new List<int>(); //データリスト作成  
            _stageDataList = new List<int>();  //データリスト作成
            ReadFile();//ファイルを読み込む
            SearchWidth();
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
            string[] objTexts = File.ReadAllText(_objFilePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in objTexts)
            {
                int tmp = 0;
                Int32.TryParse(text,out tmp);

                _objectDataList.Add(tmp);
            }
            objData = _objectDataList.Count;

            //デバッグ
            for(int i = 0; i < objData; i++)
            {
                Debug.Log(_objectDataList[i]);
            }

            string[] stageTexts = File.ReadAllText(_stageFilePath).Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var text in stageTexts)
            {
                int tmp = 0;
                Int32.TryParse(text, out tmp);

                _stageDataList.Add(tmp);
            }

        }
        //作成関数
        public void BuildStage()
        {
            for(int i = _objectDataList.Count -1; i >= 0;i--)
            {
                if(_objectDataList[i] != -1)
                {
                    GameObject go = Instantiate(_gameObj[_objectDataList[i]]);
                    go.transform.position = this.transform.position;
                }
                if((i) % _searchWidth != 0)
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
            for (int i = _objectDataList.Count - 1; i >= 0; i--)
            {
                if (_objectDataList[i] != -1)
                {
                    GameObject go = Instantiate(_floorObj[_stageDataList[i]]);
                    go.transform.position = this.transform.position;
                }
                if ((i) % _searchWidth != 0)
                {
                    transform.position = new Vector3(this.transform.position.x + _width, this.transform.position.y, this.transform.position.z);
                }
                else
                {
                    transform.position = new Vector3(startPos.x, this.transform.position.y, this.transform.position.z - _width);
                }

            }

            transform.position = new Vector3(startPos.x, this.transform.position.y + 1.0f, startPos.z);
        }

        //widthを探す関数
        public void SearchWidth()
        {
            //widthがオブジェクトデータの総数になるまで回し続ける
            while(true)
            {
                if(_searchWidth * _searchWidth == _objectDataList.Count)
                {
                    //同じだったら抜ける
                    break;
                }

                _searchWidth++;

            }


        }
    }

 

}
