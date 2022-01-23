
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public enum EthicsType
	{
		Evil = 1,
		Neutral,
		Good,
	}

	public enum ConflictType
	{
		NaGood,
		NaEvil,
		Neutral,
		EvilOrder,
		GoodChaos,
	}
	
	[Serializable]
	public class CharacterData
	{
		public string Name;
		public string Skin;
		public int AgeOfDeath;
		public string DeadReason;
		public int NumberOfChild;
		public int NumberOfKilled;
		public string Crime;
		public EthicsType Ethics;
		public int Complexity = 1;
		public ConflictType Conflict;
		public string Comment;


		public string SummaryDialog;
		public string AgeOfDeathDialog;
		public string DeadReasonDialog;
		public string NumberOfChildDialog;
		public string NumberOfKilledDialog;
		public string CrimeDialog;
		public string RewardDialog;
		public string PenaltyDialog;



//		public Sprite MainSprite;
		// public List<int> MeetRules;
		// public int Score = 1;
	}
}
