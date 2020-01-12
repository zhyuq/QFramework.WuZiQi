using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    public Transform top_left;      //左上角空物体，用于标记位置
    public Transform top_right;     //右上角空物体，用于标记位置
    public Transform bottom_left;   //左下角空物体，用于标记位置
    public Transform bottom_right;   //右下角空物体，用于标记位置
    Vector2 pos_TL,pos_TR,pos_BL,pos_BR;   //左上角、右上角、左下角、右下角坐标值
    float gridWith,gridHeight;  //棋盘上一格的宽度和高度
    Vector2 [,] chessPos;   //可以放置棋子的位置
    public enum ChessInfo{Blank = 0, Black = 1, White = -1} //棋盘上每一格的状态，分别为空，黑棋和白旗
    ChessInfo chessInfo;
    ChessInfo[,] snapshotMap;   //期盘状态
    public float threshold = 0.2f;

    Vector2 mousePos;

    public enum Turn{Black,White}
    Turn turn = Turn.Black; //默认黑棋为先
    public GameObject blackChess;   //黑棋对象
    public GameObject whiteChess;   //白旗对象

    public Image winImage;  //用于挂载胜利画面的ImageUI
    public Sprite blackWinSprite;       //黑棋胜利画面
    public Sprite whiteWinSprite;       //白旗胜利画面
    private bool isGameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        InitChessBoard();
    }

    // Update is called once per frame
    void Update()
    {
        PutChess();
    }

    void InitChessBoard(){
        chessPos = new Vector2[15,15];  //分配棋子位置二维数组内存空间
        snapshotMap = new ChessInfo[15,15]; //分配棋盘快照内存空间
        //获得左上角、右上角、左下角、右下角的位置数据
        pos_TL = top_left.position;
        pos_TR = top_right.position;
        pos_BL = bottom_left.position;
        pos_BR = bottom_right.position;
        //计算每格的宽度和高度
        gridWith = (pos_TR.x - pos_TL.x) / 14;
        gridHeight = (pos_TR.y - pos_BR.y) / 14;

        //计算棋盘中心
        // Vector2 center = new Vector2((pos_TR.x - pos_TL.x) * 0.5f, (pos_TL.y - pos_BL.y) * 0.5f);
        // Debug.Log(center);
        // GameObject obj = new GameObject("center");
        // obj.transform.position = new Vector3(center.x,center.y,0);
        //计算每个落棋点的位置，并存入数组中
        for(int row =0; row < 15; row++){
            for(int col = 0; col < 15; col++){
                //把每个落棋点存入二维数组中
                chessPos[row,col] = new Vector2(gridWith * (col - 7), gridHeight * (row-7));

                //测试数据
                GameObject obj = new GameObject(row+" " + col);
                //注意y轴方向上的负号，因为数组索引号与y轴的方向刚好相反
                obj.transform.position = new Vector3(chessPos[row,col].x,-chessPos[row,col].y,0);
                
            }//end col
        }//end row
    }

    void PutChess(){
        if(Input.GetMouseButton(0) && isGameOver == false){
            //把鼠标在屏幕坐标下的位置转换成世界坐标
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.DrawRay(mousePos,Camera.main.transform.forward,Color.red);
            //Debug.Log(Input.mousePosition + " " + mousePos);
            //找到鼠标的位置与棋盘上的哪个落棋点最近
            for(int row = 0; row < 15;row++){
                for(int col = 0; col < 15; col++){
                    //找到最近的落棋点
                    if(IsNearBy(mousePos, chessPos[row,col])){
                        //Debug.Log("mousePos = " + mousePos + " ChessPoint = " + chessPos[row,col]);
                        //在该位置创建棋子
                        CreateChess(row,col,chessPos[row,col]);
                        //判断是否有五子相连
                        //DetectionWiner();
                    }
                }//end col
            }//end row
        }
    }

    bool IsNearBy(Vector2 mousePos, Vector2 chessPoint){
        //计算鼠标位置与落棋点之间的曼哈顿距离，小于阈值则表示两者相近，返回true
        if(Mathf.Abs(chessPoint.x - mousePos.x) + Mathf.Abs(chessPoint.y - mousePos.y) < threshold){
            return true;
        }else{
            return false;
        }
    }

    void CreateChess(int row, int col, Vector2 putPosition){
        //如果该出没有棋子
        if(snapshotMap[row,col] == ChessInfo.Blank){
            //修改快照，把该空位置设置为对应落棋方的状态
            snapshotMap[row,col] = (turn == Turn.Black ? ChessInfo.Black : ChessInfo.White);
            //根据落棋方生成对应的棋子
            switch(turn){
                case Turn.Black:
                    //生成黑棋
                    Instantiate(blackChess,new Vector3(putPosition.x,putPosition.y,0),Quaternion.identity).name = "Black " + row.ToString() + "_" + col.ToString();
                    //判断是否黑方胜
                    DetectionWiner();
                    //交换落棋方
                    turn = Turn.White;
                break;
                case Turn.White:
                    //生成白棋
                    Instantiate(whiteChess,new Vector3(putPosition.x,putPosition.y,0),Quaternion.identity).name ="White " + row.ToString() + "_" + col.ToString();
                    //判断是否白方胜
                    DetectionWiner();
                    //交换落棋方
                    turn = Turn.Black;
                break;
            }
        }
    }

    void DetectionWiner(){
        switch(turn){
            case Turn.Black:
            //→、↑、↗、↘方向的探测,下方黑方和白方的代码类似，是否可以进行重构成函数？
            if(horizontalDetect(ChessInfo.Black) || VerticalDetect(ChessInfo.Black) ||
                SlashDetect(ChessInfo.Black) || BackSlashDetect(ChessInfo.Black)){
                    isGameOver = true;
                    winImage.gameObject.SetActive(true);
                    winImage.sprite = blackWinSprite;
                    Debug.Log("Black Win");
            }
            break;
            case Turn.White:
            //→、↑、↗、↘方向的探测
            if(horizontalDetect(ChessInfo.White) || VerticalDetect(ChessInfo.White) ||
                SlashDetect(ChessInfo.White) || BackSlashDetect(ChessInfo.White)){
                    isGameOver = true;
                    winImage.gameObject.SetActive(true);
                    winImage.sprite = whiteWinSprite;
                    Debug.Log("White Win");
            } 
            break;
        }//end switch
    }

    //→探
    bool horizontalDetect(ChessInfo chessInfo){
        for(int row = 0; row < 15; row++){
            for(int col = 0; col < 11;col++){   //注意索引号
                //从当前棋子开始向右4个落棋点,思考一下代码是否有可以优化的方法
                if(snapshotMap[row,col] == chessInfo && snapshotMap[row,col+1] == chessInfo &&
                    snapshotMap[row,col+2] == chessInfo && snapshotMap[row,col+3] == chessInfo &&
                    snapshotMap[row,col+4] == chessInfo){
                        return true;
                }

            }//end col
        }//end row
        return false;
    }
    //↓探
    bool VerticalDetect(ChessInfo chessInfo){
        for(int row = 0; row < 11; row++){
            for(int col = 0; col < 15; col++){
                if(snapshotMap[row,col] == chessInfo && snapshotMap[row+1,col] == chessInfo &&
                    snapshotMap[row+2,col] == chessInfo && snapshotMap[row+3,col] == chessInfo &&
                    snapshotMap[row+4,col] == chessInfo){
                        return true;
                }
            }//end col
        }//end row
        return false;
    }
    //↙探
    bool SlashDetect(ChessInfo chessInfo){
        for(int row = 0; row < 11; row++){          //注意索引值
            for(int col = 4; col < 15; col++){      //注意索引值
                if(snapshotMap[row,col] == chessInfo && snapshotMap[row+1,col-1] == chessInfo &&
                    snapshotMap[row+2,col-2] == chessInfo && snapshotMap[row+3,col-3] == chessInfo &&
                    snapshotMap[row+4,col-4] == chessInfo){
                    return true;
                }
            }//end col
        }//end row
        return false;
    }
    //↘探
     bool BackSlashDetect(ChessInfo chessInfo){
        for(int row = 0; row < 11;row++){       //注意索引值
            for(int col = 0; col < 11;col++){   //注意索引值
                if(snapshotMap[row,col] == chessInfo && snapshotMap[row+1,col+1] == chessInfo &&
                    snapshotMap[row+2,col+2] == chessInfo && snapshotMap[row+3,col+3] == chessInfo &&
                    snapshotMap[row+4,col+4] == chessInfo){
                    return true;
                }
            }
        }
        return false;        
    }   
}
