using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{

	private static DataManager instance;

	public int Best_Score;
	public int Coin;
	public int Character;


	public static DataManager Instance
	{

		get
		{
			if(null == instance)
			{
				instance = new DataManager();
			}
			return instance;
		}

	}


	public void SetCharacter(int a)
	{
		Character = a;
	}

	public void printData()
	{
		Debug.Log("Coin:" + Coin);
		Debug.Log("Best_Score:" + Best_Score);
		Debug.Log("Character:" + Character);
	}


}
