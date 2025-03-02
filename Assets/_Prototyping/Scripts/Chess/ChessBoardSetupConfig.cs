using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Prototyping.Chess
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Board/" + nameof(ChessBoardSetupConfig), fileName = nameof(ChessBoardSetupConfig) + "_Default")]

	public class ChessBoardSetupConfig : ScriptableObject
	{
		[SerializeField]
		public SerializedDictionary<Vector2Int, ChessPiece> chessPieceMap;
	}
}