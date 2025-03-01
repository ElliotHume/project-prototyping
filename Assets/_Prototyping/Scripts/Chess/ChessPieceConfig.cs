using System.Collections.Generic;
using _Prototyping.Chess.Core;
using _Prototyping.Chess.Movement;
using UnityEngine;

namespace _Prototyping.Chess
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Pieces/" + nameof(ChessPieceConfig), fileName = nameof(ChessPieceConfig) + "_Default")]
	public class ChessPieceConfig : ScriptableObject
	{
		public ChessPieceType type;
		public List<ChessMovementType> movementOptions;
	}
}