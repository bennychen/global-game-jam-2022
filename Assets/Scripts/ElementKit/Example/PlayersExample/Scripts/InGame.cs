using UnityEngine;
using System.Collections.Generic;

namespace PlayersExample
{
	public enum ColorType
	{
		Red,
		Green,
		Blue
	}

	public class InGame : Codeplay.SceneElement<InGame>
	{
		[Codeplay.InjectElement("LocalPlayer")]
		public Player LocalPlayer { get; private set; }

		public List<Player> AIPlayers { get { return _aiPlayers; } }

		protected override void OnInit(object data)
		{
			_aiPlayers = new List<Player>();

			Debug.Log("init InGame, local player= " + LocalPlayer);
			PlayerViews localPlayerViews = CreatePlayerViews(LocalPlayer, _localPlayerType, _localVehicleType);
			localPlayerViews.name = "LocalPlayer";
		}

		private void OnGUI()
		{
			if (GUILayout.Button("Spawn AI Player"))
			{
				CreateAIPlayer();
			}
		}

		private void CreateAIPlayer()
		{
			Player aiPlayer = (GameObject.Instantiate(
					_playerElementPrefab.gameObject) as GameObject).GetComponent<Player>();
			_aiPlayers.Add(aiPlayer);
			aiPlayer.transform.parent = transform;
			aiPlayer.name = "AI Player " + _aiPlayers.Count;
			aiPlayer.Vehicle.name = "AI Vehicle " + _aiPlayers.Count;

			PlayerViews aiPlayerViews = CreatePlayerViews(
					aiPlayer, (ColorType)Random.Range(0, 3), (ColorType)Random.Range(0, 3));
			aiPlayerViews.transform.position = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
			aiPlayerViews.name = "AI Player Views " + _aiPlayers.Count;

			Codeplay.SceneElementManager.Instance.AddElement(aiPlayer);
			Codeplay.SceneElementManager.Instance.AddElement(aiPlayer.Vehicle);
		}

		private PlayerViews CreatePlayerViews(Player player, ColorType playerType, ColorType vehicleType)
		{
			PlayerViews playerViews = player.CreateViewsNodeFromPrefab<PlayerViews>(GetPlayer(playerType));
			playerViews.transform.position = Vector3.up;
			VehicleViews vehicleViews = player.Vehicle.CreateViewsNodeFromPrefab<VehicleViews>(GetVehicle(vehicleType));
			vehicleViews.transform.parent = playerViews.transform;
			return playerViews;
		}

		private PlayerViews GetPlayer(ColorType type)
		{
			switch (type)
			{
				case ColorType.Red:
					return _redPlayerPrefab;
				case ColorType.Green:
					return _greenPlayerPrefab;
				case ColorType.Blue:
					return _bluePlayerPrefab;
				default:
					return _redPlayerPrefab;
			}
		}

		private VehicleViews GetVehicle(ColorType type)
		{
			switch (type)
			{
				case ColorType.Red:
					return _redVehiclePrefab;
				case ColorType.Green:
					return _greenVehiclePrefab;
				case ColorType.Blue:
					return _blueVehiclePrefab;
				default:
					return _redVehiclePrefab;
			}
		}

		[SerializeField]
		private ColorType _localPlayerType;
		[SerializeField]
		private ColorType _localVehicleType;
		[SerializeField]
		private PlayerViews _redPlayerPrefab;
		[SerializeField]
		private PlayerViews _greenPlayerPrefab;
		[SerializeField]
		private PlayerViews _bluePlayerPrefab;
		[SerializeField]
		private VehicleViews _redVehiclePrefab;
		[SerializeField]
		private VehicleViews _greenVehiclePrefab;
		[SerializeField]
		private VehicleViews _blueVehiclePrefab;
		[SerializeField]
		private Player _playerElementPrefab;

		private List<Player> _aiPlayers;
	}
}
