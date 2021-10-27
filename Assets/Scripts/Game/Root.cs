using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using QFramework;

namespace QFramework.WuZiQi
{
	public partial class Root : ViewController
	{
		private Vector2[,] mChessPos;
		
		public enum ChessPosStatus
		{
			Blank = 0,
			Black = 1,
			White = 2
			
		}


		private float Threshold = 0.2f;
		
		private List<List<ChessPosStatus>> mChessMap;

		private ChessPosStatus mTurn = ChessPosStatus.Black;

		private GameObject mBlackChessPrefab;
		private GameObject mWhiteChessPrefab;
		
		private ResLoader mResLoader = null;

		private bool isGameOver = false;
		
		void Start()
		{
			Log.I("root start");
			mResLoader = ResLoader.Allocate();
			mBlackChessPrefab = mResLoader.LoadSync<GameObject>("WhiteChess");
			mWhiteChessPrefab = mResLoader.LoadSync<GameObject>("WhiteChess");
			
			InitBoard();

			var worldPos = Vector2.zero;
			this.Repeat().Until(() => Input.GetMouseButton(0) && !isGameOver).Event((() =>
			{
				Log.I("mouse down");
				worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				for (int row = 0; row < 15; row++)
				{
					for (int col = 0; col < 15; col++)
					{
						var chessPos = mChessPos[row, col];
						if (RootGameUtil.IsNearBy(worldPos, chessPos, Threshold))
						{
							CreateChess(row, col, chessPos);
						}
					}
					
				}
			})).Begin();
		}

		void InitBoard()
		{
			mChessPos = new Vector2[15, 15];
			mChessMap = Enumerable.Range(0, 15).Select(i =>
			{
				return Enumerable.Range(0, 15).Select(i1 => new ChessPosStatus()).ToList();
			}).ToList();

			for (int row = 0; row < 15; row++)
			{
				for (int col = 0; col < 15; col++)
				{
					mChessPos[row, col] = new Vector2((col-7)*Chessboard.GridWidth, (row-7)*Chessboard.GridHeight);
				}
			}
		}

		void CreateChess(int row, int col, Vector2 putPos)
		{
			if (mChessMap[row][col] == ChessPosStatus.Blank)
			{
				mChessMap[row][col] = mTurn;
				if (mTurn == ChessPosStatus.Black)
				{
					mBlackChessPrefab.Instantiate()
						.transform
						.Parent(Chess)
						.LocalIdentity()
						.LocalPosition(putPos.x, putPos.y, 0)
						.Name("chess" + "-" + row + "-" + col);
				}
				else
				{
					mWhiteChessPrefab.Instantiate()
						.transform
						.Parent(Chess)
						.LocalIdentity()
						.LocalPosition(putPos.x, putPos.y, 0)
						.Name("chess" + "-" + row + "-" + col);
				}
			}
		}
	}
}
