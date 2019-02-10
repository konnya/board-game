using System;
using System.Collections.Generic;
// using System.Linq.Enumerable;

public class OutOfBoardAreaPositionException : Exception {
  public
   OutOfBoardAreaPositionException() {}

  public
   OutOfBoardAreaPositionException(string message) : base(message) {}

  public
   OutOfBoardAreaPositionException(string message, Exception inner) :
       base(message, inner) {}
}
public class PieceDoesNotExistException : Exception {
  public
   PieceDoesNotExistException() {}

  public
   PieceDoesNotExistException(string message) : base(message) {}

  public
   PieceDoesNotExistException(string message, Exception inner) :
       base(message, inner) {}
}
public class Pos {
  public Pos(int x_, int y_) {
    x = x_;
    y = y_;
  }

  public static Pos operator+ (Pos pos, Pos diff) {
    return new Pos(pos.x + diff.x, pos.y + diff.y);
  }

  public static Pos operator- (Pos pos, Pos diff) {
    return new Pos(pos.x - diff.x, pos.y - diff.y);
  }

  public bool WithIn(Pos min, Pos max) {
    if (x < min.x) { return false;}
    if (y < min.y) { return false;}
    if (x > max.x) { return false;}
    if (y > max.y) { return false;}
    return true;
  }

  public void Set(int x_, int y_) {
    x = x_;
    y = y_;
  }

  public int x;
  public int y;
}

public enum PlayerType {
  Player1 = 0,
  Player2 = 1,
}

public enum Direction {
  Front,
  Back,
  Left,
  Right
}

public class CoordinateTrans {
  static Dictionary<char, int> a2x = new Dictionary<char, int> {
    {'A', 0},
    {'B', 1},
    {'C', 2},
    {'D', 3},
    {'E', 4},
    {'F', 5},
  };

  static Dictionary<char, int> a2y = new Dictionary<char, int> {
    {'6', 5},
    {'5', 4},
    {'4', 3},
    {'3', 2},
    {'2', 1},
    {'1', 0},
  };

  static Dictionary<char, Direction> a2dir = new Dictionary<char, Direction> {
    {'F', Direction.Front},
    {'B', Direction.Back},
    {'L', Direction.Left},
    {'R', Direction.Right},
  };

  public static int AtoX(char x) {
    return a2x[x];
  }

  public static char XtoA(int x) {
    return "ABCDEF"[x];
  }

  public static int AtoY(char y) {
    return a2y[y];
  }

  public static char YtoA(int y) {
    return "123456"[y];
  }

  public static Direction AtoDir(char dir) {
    return a2dir[dir];
  }


}

public class Piece {
  public enum Type {
    Good,
    Bad
  }

  public Piece(Type type) {
    _type = type;
  }

  public void Move(Direction dir) {
    switch (dir) {
      case Direction.Front:
        _pos.x++;
        break;
      case Direction.Back:
        _pos.x--;
        break;
      case Direction.Left:
        _pos.y--;
        break;
      case Direction.Right:
        _pos.y++;
        break;
    }
  }

  public Pos GetPos() {
    return _pos;
  }

  public int X() {
    return _pos.x;
  }

  public int Y() {
    return _pos.y;
  }

  public Type GetPieceType() {
    return _type;
  }

  private Pos _pos = new Pos(0, 0);
  private Type _type;
}

public class Board {
 public static readonly int BoardWidth = 6;
 public static readonly int BoardHeight = 6;
 public static readonly Pos MinPos = new Pos(0, 0);
 public static readonly Pos MaxPos = new Pos(BoardHeight, BoardWidth);

 public class Player {

  public void Assign(Piece piece, Pos pos) {
    gs.Add(piece);
    if (piece.GetPieceType() == Piece.Type.Good) {
      goods.Add(piece);
    } else {
      bads.Add(piece);
    }
    map[pos.x, pos.y] = piece;
  }

  public void MarkAsTaken(Pos pos) {
    var piece = map[pos.x, pos.y];
      Console.WriteLine("taken : x xy");
    if (piece.GetPieceType() == Piece.Type.Good) {
      Console.WriteLine("taken : good");
      goods.Remove(piece);
      taken_goods.Add(piece);
    } else {
      Console.WriteLine("taken : bad");
      bads.Remove(piece);
      taken_bads.Add(piece);
    }
    map[pos.x, pos.y] = null;
  }

  public List<Piece> gs = new List<Piece>();
  public Piece[,] map = new Piece[BoardWidth, BoardHeight];

  public List<Piece> goods = new List<Piece>();
  public List<Piece> bads = new List<Piece>();
  public List<Piece> taken_goods = new List<Piece>();
  public List<Piece> taken_bads = new List<Piece>();
  public List<Piece> taken = new List<Piece>();
 }

 PlayerType _first_move;
 int TurnCount = 0;

 Player[] players = new Player[2];

 // List<Piece> _pgs = new List<Piece>();
 // List<Piece> _egs = new List<Piece>();
 // Piece[, ] _pmap = new Piece[BoardHeight, BoardWidth];
 // Piece[, ] _emap = new Piece[BoardHeight, BoardWidth];
 // List<Piece> _ptaken = new List<Piece>();
 // List<Piece> _etaken = new List<Piece>();

 public Board() {
   for (int i = 0; i < players.Length; i++) {
     players[i] = new Player();
   }
 }

 public
  bool WithIn(Pos pos) { return pos.WithIn(MinPos, MaxPos); }

 public
  Pos CalcDiff(PlayerType ptype, Direction dir) {
    switch (ptype) {
      case PlayerType.Player1:
        switch (dir) {
          case Direction.Front:
            return new Pos(-1, 0);
          case Direction.Back:
            return new Pos(+1, 0);
          case Direction.Left:
            return new Pos(0, -1);
          case Direction.Right:
            return new Pos(0, +1);
        }
        break;
      case PlayerType.Player2:
        switch (dir) {
          case Direction.Front:
            return new Pos(+1, 0);
          case Direction.Back:
            return new Pos(-1, 0);
          case Direction.Left:
            return new Pos(0, +1);
          case Direction.Right:
            return new Pos(0, -1);
        }
        break;
    }
    throw new System.Exception();
  }

  private PlayerType CurrentPhase() {
    switch (_first_move) {
      case PlayerType.Player1:
        return TurnCount % 2 == 0 ? PlayerType.Player1 : PlayerType.Player2;
      case PlayerType.Player2:
        return TurnCount % 2 == 0 ? PlayerType.Player2 : PlayerType.Player1;
    }
    // FIXME:
    return PlayerType.Player1;
  }

  public void test() {
    // // grid[1,1] = null;
    // Piece gst = new Piece();
    // _map[2,3] = gst;

    // Console.WriteLine("--- {0}", _map);
    // foreach (Piece gs in _map) {
    //   Console.WriteLine("--- {0}", gs);
    // }
  }

  public void ResetGame(PlayerType first_move) {
    _first_move = first_move;
  }

  private int GetPlayerIndex(PlayerType ptype) {
    return (int)ptype;
  }

  public void LocatePiece(PlayerType ptype, int x, int y, Piece gs) {
    var idx = GetPlayerIndex(ptype);
    players[idx].Assign(gs, new Pos(x, y));
    // players[idx].map[x, y] = gs;
    // players[idx].gs.Add(gs);

    // switch (ptype) {
    //   case PlayerType.Player1:
    //     players[0].map[x, y] = gs;
    //     players[0].gs.Add(gs);
    //     // _pmap[x, y] = gs;
    //     // _pgs.Add(gs);
    //     break;
    //   case PlayerType.Player2:
    //     players[1].map[x, y] = gs;
    //     players[1].gs.Add(gs);
    //     // _emap[x, y] = gs;
    //     // _egs.Add(gs);
    //     break;
    // }
  }

  public Player[] GetPlayers() {
    return players;
  }

  private List<Piece> GetPieces(int pidx) {
    return players[pidx].gs;
  }

  private Piece GetPiece(int pidx, Pos pos) {
    return players[pidx].map[pos.x, pos.y];
  }

  private void MarkAsTaken(int pidx, Pos pos) {
    Console.WriteLine("taken : good-");
    var player = players[pidx];
    player.MarkAsTaken(pos);
    // player.taken.Add(player.map[pos.x, pos.y]);
    // player.map[pos.x, pos.y] = null;
  }

  private void Move(int pidx, Pos from, Pos to) {
    var player = players[pidx];
    player.map[to.x, to.y] = player.map[from.x, from.y];
    player.map[from.x, from.y] = null;
  }

  private void TryMovePlayerPiece(PlayerType ptype, Pos from, Pos to) {
    var idx = GetPlayerIndex(ptype);

    // var gs = GetPiece(_pmap, from);
    var gs = GetPiece(idx, from);
    if (gs == null) {
      var cx = CoordinateTrans.XtoA(from.x);
      var cy = CoordinateTrans.YtoA(from.y);
      throw new PieceDoesNotExistException(
          $"Threre is NOT a PIECE in ({cx},{cy}) or ({from.x},{from.y}).");
    }

    // var friend = GetPiece(_pmap, to);
    var friend = GetPiece(idx, to);
    if (friend != null) {
      var cx = CoordinateTrans.XtoA(from.x);
      var cy = CoordinateTrans.YtoA(from.y);
      throw new PieceDoesNotExistException(
          $"Threre is a FRIEND PIECE in ({cx},{cy}) or ({from.x},{from.y}).");
    }

    // requires System.Linq.Enumerable;
    // var e_indices =
    //     from i in Enumerable.Range(0, players.Length) where i != idx select i;
    // foreach (int i in e_indices) {
    for (int i = 0; i < players.Length; i++) {
      if (i == idx) continue;
      var enemy  = GetPiece(i, to);
      if (enemy != null) {
        MarkAsTaken(i, to);
        break;
      }
    }
    Move(idx, from, to);
    // var enemy  = GetPiece(1, to);
    // if (enemy != null) {
    //   MarkAsTaken(1, to);
    // }
    // Move(0, from ,to);

    return;
  }

  public void TryMovePiece(PlayerType ptype, Pos from, Direction dir) {
    var to = from + CalcDiff(ptype, dir);
    Console.WriteLine($"------> {from.x} {from.y}");
    Console.WriteLine($"------> {CalcDiff(ptype, dir).x} {CalcDiff(ptype, dir).y}");
    if (WithIn(to) == false) {
      var cx = CoordinateTrans.XtoA(to.x);
      var cy = CoordinateTrans.YtoA(to.y);
      throw new OutOfBoardAreaPositionException(
          $"({cx},{cy}) or ({to.x},{to.y}) is out of board area ({BoardHeight},{BoardWidth}).");
    }

    TryMovePlayerPiece(ptype, from, to);
  }

  public void EvalOperation() {
  }

  public void DispCurrentMap() {
    Console.WriteLine("     1   2   3   4   5   6");
    Console.WriteLine("   +---+---+---+---+---+---+");
    for (int x = 0; x < 6; x++) {
      String str = " " + CoordinateTrans.XtoA(x) + " |";
      for (int y = 0; y < 6; y++) {
        if(players[0].map[x, y] != null) {
        // if(_pmap[x, y] != null) {
          if ((x == 0 || x == 5) && (y == 0 || y == 5)) {
            str += "(^)|";
          } else {
            str += " ^ |";
          }
        // } else if(_emap[x, y] != null) {
        } else if(players[1].map[x, y] != null) {
          if ((x == 0 || x == 5) && (y == 0 || y == 5)) {
            str += "(v)|";
          } else {
            str += " v |";
          }
        } else {
          if ((x == 0 || x == 5) && (y == 0 || y == 5)) {
            str += "( )|";
          } else {
            str += "   |";
          }
        }
      }
      Console.WriteLine(str);
      Console.WriteLine("   +---+---+---+---+---+---+");
    }

    Console.WriteLine("");

    Console.WriteLine("Taken Pieces");
    Console.WriteLine(" Player1");
    Console.Write("    Good :");
    foreach (Piece gs in players[0].taken_goods) {
      Console.Write(" v");
    }
    Console.Write(Environment.NewLine);
    Console.Write("    Bad  :");
    foreach (Piece gs in players[0].taken_bads) {
      Console.Write(" v");
    }
    Console.Write(Environment.NewLine);

    Console.WriteLine(" Player2 :");
    Console.Write("    Good :");
    foreach (Piece gs in players[1].taken_goods) {
      Console.Write(" ^");
    }
    Console.Write(Environment.NewLine);
    Console.Write("    Bad  :");
    foreach (Piece gs in players[1].taken_bads) {
      Console.Write(" ^");
    }
    Console.Write(Environment.NewLine);
  }

  public char GetPieceTypeSymbol(Piece gs) {
    switch (gs.GetPieceType()) {
      case Piece.Type.Good:
        return 'G';
      case Piece.Type.Bad:
        return 'B';
    }
    return 'x';
  }

  public void DispPieceInfo() {
    Console.WriteLine("     1   2   3   4   5   6");
    Console.WriteLine("   +---+---+---+---+---+---+");
    for (int x = 0; x < 6; x++) {
      String str = " " + CoordinateTrans.XtoA(x) + " |";
      for (int y = 0; y < 6; y++) {
        if(players[0].map[x, y] != null) {
          var sym = GetPieceTypeSymbol(players[0].map[x, y]);
          if ((x == 0 || x == 5) && (y == 0 || y == 5)) {
            str += "(" + sym + ")|";
          } else {
            str += " " + sym + " |";
          }
        } else if(players[1].map[x, y] != null) {
          var sym = GetPieceTypeSymbol(players[1].map[x, y]);
          if ((x == 0 || x == 5) && (y == 0 || y == 5)) {
            str += "(" + sym + ")|";
          } else {
            str += " " + sym + " |";
          }
        } else {
          if ((x == 0 || x == 5) && (y == 0 || y == 5)) {
            str += "( )|";
          } else {
            str += "   |";
          }
        }
      }
      Console.WriteLine(str);
      Console.WriteLine("   +---+---+---+---+---+---+");
    }
  }
}

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

delegate int ConsoleCommand(string[] args, Board brd);

public class Sample {
  public static string[] GetUserInput() {
    string[] args;
    try {
      args = Console.ReadLine().Split(' ');
    } catch (System.NullReferenceException e) {
      Console.Write("\n");
      args = new string[] {""};
    }
    return args;
  }

  public static void Main() {
    Console.WriteLine("--- Piece Test");
    Piece gst = new Piece(Piece.Type.Good);
    Console.WriteLine("--- Pos : x:{0} y:{1}", gst.X(), gst.Y());
    Console.WriteLine("--- Move Front");
    gst.Move(Direction.Front);
    Console.WriteLine("--- Pos : x:{0} y:{1}", gst.X(), gst.Y());
    Console.WriteLine("--- Move Right");
    gst.Move(Direction.Right);
    Console.WriteLine("--- Pos : x:{0} y:{1}", gst.X(), gst.Y());

    Board brd = new Board();
    for (int i = 0; i < 4; i++) {
      Piece gst1 = new Piece(Piece.Type.Good);
      brd.LocatePiece(PlayerType.Player1, 4, i+1, gst1);
      Piece gst2 = new Piece(Piece.Type.Bad);
      brd.LocatePiece(PlayerType.Player1, 5, i+1, gst2);
    }
    for (int i = 0; i < 4; i++) {
      Piece gst1 = new Piece(Piece.Type.Good);
      brd.LocatePiece(PlayerType.Player2, 0, i+1, gst1);
      Piece gst2 = new Piece(Piece.Type.Bad);
      brd.LocatePiece(PlayerType.Player2, 1, i+1, gst2);
    }
    brd.test();
    brd.DispCurrentMap();

    var commands = new Dictionary<string, ConsoleCommand>();
    commands.Add("args",  delegate (string[] args, Board board) {
      Console.WriteLine(" Command : {0}", args[0]);
      for (int i = 0; i < args.Length; i++) {
        Console.WriteLine(" {0} : {1}", i, args[i]);
      }
      return 0;
    });
    commands.Add("info", delegate(string[] args, Board board) {
      board.DispPieceInfo();
      return 0;
    });
    commands.Add("start",  delegate (string[] args, Board board) {
      var mgr = new GameManager();
      mgr.AssignBoard(ref board);
      mgr.StartGame();

      return 0;
    });
    commands.Add("movep",  delegate (string[] args, Board board) {
      if (args.Length < 4) {
        Console.WriteLine(" {0} fromA-F from1-6 <Front,Back,Left,Right> ", args[0]);
        return 22;
      }

      var x = CoordinateTrans.AtoX(args[1][0]);
      var y = CoordinateTrans.AtoY(args[2][0]);
      var from = new Pos(x, y);
      var dir = CoordinateTrans.AtoDir(args[3][0]);

      board.TryMovePiece(PlayerType.Player1, from, dir);

      return 0;
    });
    commands.Add("movee",  delegate (string[] args, Board board) {
      if (args.Length < 4) {
        Console.WriteLine(" {0} fromA-F from1-6 <Front,Back,Left,Right> ", args[0]);
        return 22;
      }

      var x = CoordinateTrans.AtoX(args[1][0]);
      var y = CoordinateTrans.AtoY(args[2][0]);
      var from = new Pos(x, y);
      var dir = CoordinateTrans.AtoDir(args[3][0]);

      board.TryMovePiece(PlayerType.Player2, from, dir);

      return 0;
    });

    do {
      Console.Write("$ ");
      var args = GetUserInput();
      Console.WriteLine(" --> <{0}> ", args[0]);

      if (args[0] == "exit") {
        Console.WriteLine("good bye!");
        break;
      }
      if (args[0] == "help") {
          Console.WriteLine(" command name : discription");
        foreach (var name in commands.Keys) {
          Console.WriteLine(" {0} : {1}", name, "n/a");
        }
        continue;
      }

      try {
        var cmd = commands[args[0]];
        try {
          var eno = cmd(args, brd);
          Console.WriteLine(" Command {0} --> {1}", args, eno);
        } catch (System.Exception e) {
          Console.WriteLine(" Command {0} throw exception", args);
          Console.WriteLine(e);
        }
      } catch (System.Collections.Generic.KeyNotFoundException) {
        Console.WriteLine(" unknown command : {0}", args[0]);
      }

      brd.DispCurrentMap();
    }
    while(true);
  }
}
