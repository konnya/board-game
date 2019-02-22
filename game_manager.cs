using System;
using System.Collections.Generic;
// using System.Linq.Enumerable;

delegate int PlayerTurnOperation(ref Board board);

public class GameManager {
  public GameManager() {
    NumOfPlayers = 2;
    PhaseOrder = new List<int> {0, 1};
    ope = new PlayerTurnOperation[NumOfPlayers];
    ope[0] = delegate (ref Board board) {
      Console.WriteLine("--- Player1's Phase ---");
      var args = Console.ReadLine().Split(' ');
      Console.WriteLine($" --> {args}");
      // _board.TryMovePiece((PlayerType), from, dir);
      return 0;
    };
    ope[1] = delegate (ref Board board) {
      Console.WriteLine("--- Player2's Phase ---");
      var args = Console.ReadLine().Split(' ');
      Console.WriteLine($" --> {args}");
      // _board.TryMovePiece((PlayerType), from, dir);
      return 0;
    };
  }

  public bool IsAllBadPieceTaken(int pidx) {
    var player = _board.GetPlayers()[pidx];
    if (player.bads.Count == 0) {
      return true;
    }
    return false;
  }

  public bool IsAllGoodPieceTaken(int pidx) {
    var player = _board.GetPlayers()[pidx];
    if (player.goods.Count == 0) {
      return true;
    }
    return false;
  }

  public void UpdateLosers() {
    foreach (int pidx in PhaseOrder) {
      if(IsAllGoodPieceTaken(pidx) == true) {
        PhaseOrder.Remove(pidx);
        losers.AddLast(pidx);
      }
    }
  }

  public bool CheckWinner() {
    foreach (int pidx in PhaseOrder) {
      if(IsAllBadPieceTaken(pidx) == true) {
        return true;
      }
    }
    return false;
  }

  public bool CheckEscapablePiece() {
    foreach (int pidx in PhaseOrder) {
      if(IsAllBadPieceTaken(pidx) == true) {
        return true;
      }
    }
    return false;
  }

  public bool CheckGameOver() {
    /* Case1. All Good Pieces are Taken. */
    if (PhaseOrder.Count == 1) {
      return true;
    }
    /* Case2. All Bad Pieces are Taken. */
    if (CheckWinner() == true) {
      return true;
    }
    /* Case3. One of Piece move out of playing area. */
    return CheckEscapablePiece();
  }

  public void AssignBoard(ref Board board) {
    _board = board;
  }

  public void ResetGame(PlayerType first_move) {
    _first_move = first_move;
    OperationCount = 0;
  }

  private PlayerType CurrentPhase() {
    switch (_first_move) {
      case PlayerType.Player1:
        return OperationCount % 2 == 0 ? PlayerType.Player1 : PlayerType.Player2;
      case PlayerType.Player2:
        return OperationCount % 2 == 0 ? PlayerType.Player2 : PlayerType.Player1;
    }
    // FIXME:
    return PlayerType.Player1;
  }

  public void StartGame() {
    while(true) {
      foreach (var pidx in PhaseOrder) {
        Console.WriteLine($" Phase : {pidx}");
        var op = ope[pidx];
        op(ref _board);
        _board.DispCurrentMap();
        OperationCount++;
      }
      TurnCount++;
    }
  }

  private Board _board;
  private int OperationCount = 0;
  private int TurnCount = 0;
  private int NumOfPlayers = 0;
  private List<int> PhaseOrder = new List<int>();
  private PlayerType _first_move = PlayerType.Player1;

  private PlayerTurnOperation[] ope;
  private LinkedList<int> losers = new LinkedList<int>();
}
