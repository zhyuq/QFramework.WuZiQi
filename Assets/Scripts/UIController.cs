using UnityEngine;

namespace QFramework.WuZiQi
{

	public class UIController : MonoBehaviour
	{
		private void Awake()
		{
			ResKit.Init();
		}

		void Start()
		{
			TypeEventSystem.Register<GameOverEvent>(OnGameOver);
		}

		void OnGameOver(GameOverEvent gameOverEvent)
		{
			UIMgr.OpenPanel<UIGameOver>(new UIGameOverData()
			{
				BlackWin = gameOverEvent.IsBlackWin
			});
		}

		private void OnDestroy()
		{
			TypeEventSystem.UnRegister<GameOverEvent>(OnGameOver);
		}
	}
}