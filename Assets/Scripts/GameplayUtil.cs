using System;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.WuZiQi
{
    /// <summary>
    /// 承担一部分计算的职责
    /// </summary>
    public class GameplayUtil
    {
        /// <summary>
        /// 判断鼠标与落棋点之间是否是邻近的
        /// </summary>
        /// <param name="mousePos"></param>
        /// <param name="chessPoint"></param>
        /// <returns></returns>
        public static bool IsNearBy(Vector2 mousePos, Vector2 chessPoint, float threshold)
        {
            //计算鼠标位置与落棋点之间的曼哈顿距离，小于阈值则表示两者相近，返回true
            if (Mathf.Abs(chessPoint.x - mousePos.x) + Mathf.Abs(chessPoint.y - mousePos.y) < threshold)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 八个方向进行递归
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="chessPosStatus"></param>
        /// <param name="snapshotMap"></param>
        /// <returns></returns>
        public static bool IsWin(int row, int col, Gameplay.ChessPosStatus chessPosStatus,
            List<List<Gameplay.ChessPosStatus>> snapshotMap)
        {
            return Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => c < 14, 0, 1) ||
                   Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => c >= 0, 0, -1) ||
                   Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => r >= 0, -1, 0) ||
                   Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => r < 14, 1, 0) ||
                   Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => r >= 0 && c < 14, -1, 1) ||
                   Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => r < 14 && c < 14, 1, 1) ||
                   Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => r >= 0 && c >= 0, -1, -1) ||
                   Cotinious5InLine(row, col, chessPosStatus, snapshotMap, 0, (c, r) => r < 14 && c >= 0, 1, -1);

        }

        /// <summary>
        /// 判断一条线内是否是连续的
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="chessPosStatus"></param>
        /// <param name="snapshotMap"></param>
        /// <param name="count"></param>
        /// <param name="borderCondition"></param>
        /// <param name="rowIncrease"></param>
        /// <param name="colIncrease"></param>
        /// <returns></returns>
        static bool Cotinious5InLine(
            int row, 
            int col, 
            Gameplay.ChessPosStatus chessPosStatus, 
            List<List<Gameplay.ChessPosStatus>> snapshotMap,
            int count,
            Func<int,int,bool> borderCondition,
            int rowIncrease,
            int colIncrease)
        {
            if (borderCondition(row,col) && snapshotMap[row][col] == chessPosStatus)
            {
                count++;

                if (count == 5)
                {
                    return true;
                }
                
                return Cotinious5InLine(row + rowIncrease, col + colIncrease, chessPosStatus, snapshotMap, count,borderCondition,rowIncrease,colIncrease);
            }

            return false;
        }
        

     
    }
}