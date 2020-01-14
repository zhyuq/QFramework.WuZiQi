using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace QFramework
{
	
	public class NewBehaviourScript : MonoBehaviour
	{
		void Start()
		{
			TypeEventSystem.Register<IGameStateEvent>(OnGameStateEvent);
			
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				TypeEventSystem.Send<IGameStateEvent>(new GameStartEvent());
			}
			
			if (Input.GetMouseButtonDown(1))
			{
				TypeEventSystem.Send<IGameStateEvent>(new GamePauseEvent());
			}
		}

		void OnGameStateEvent(IGameStateEvent gameStaEvent)
		{
			if (gameStaEvent is GameStartEvent)
			{
				Debug.Log("Is GameStart");
			}

			if (gameStaEvent is GamePauseEvent)
			{
				Debug.Log("Is Game Pause");
			}
		}

		private void OnDestroy()
		{
			TypeEventSystem.UnRegister<IGameStateEvent>(OnGameStateEvent);
		}
	}

	public interface IGameStateEvent
	{
		
	}

	public class GameStartEvent : IGameStateEvent
	{
		
	}

	public class GamePauseEvent : IGameStateEvent
	{
		
	}
	
}