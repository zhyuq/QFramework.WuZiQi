using UnityEngine;
using QFramework;

namespace QFramework.WuZiQi
{
	public partial class ChessBoard : ViewController
	{
		private Vector2 mTLPos;
		private Vector2 mTRPos;
		private Vector2 mBLPos;
		private Vector2 mBRPos;

		private float mGridWidth = 0;
		private float mGridHeight = 0;
		void Awake()
		{
			mTLPos = TL.localPosition;
			mTRPos = TR.localPosition;
			mBLPos = BL.localPosition;
			mBRPos = BR.localPosition;

			mGridWidth = (mTRPos.x - mTLPos.x) / 14;
			mGridHeight = (mTLPos.y - mBLPos.y) / 14;
			
		}

		public float GridWidth
		{
			get => mGridWidth;
		}

		public float GridHeight => mGridHeight;
	}
}
