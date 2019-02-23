using System;
using System.Collections.Generic;
// using System.Linq.Enumerable;

public class Pos : IEquatable<Pos> {
  public Pos(int x_, int y_) {
    _x = x_;
    _y = y_;
  }

  public static Pos operator+ (Pos pos, Pos diff) {
    return new Pos(pos._x + diff._x, pos._y + diff._y);
  }

  public static Pos operator- (Pos pos, Pos diff) {
    return new Pos(pos._x - diff._x, pos._y - diff._y);
  }

  public override int GetHashCode() {
    return _x ^ _y;
  }

  public override bool Equals(object obj) {
      if (!(obj is Pos))
          return false;

      return Equals((Pos)obj);
  }

  public bool Equals(Pos other) {
      if (_x != other._x)
          return false;

      return _y == other._y;
  }

  public static bool operator ==(Pos pos1, Pos pos2)
  {
      return pos1.Equals(pos2);
  }

  public static bool operator !=(Pos pos1, Pos pos2)
  {
      return !pos1.Equals(pos2);
  }

 // public static bool operator== (Pos pos1, Pos pos2) {
 //   if (pos1.x == pos2.x && pos1.y == pos2.y) {
 //     return true;
 //   }
 //   return false;
 // }

 // public static bool operator!= (Pos pos1, Pos pos2) {
 //   if (pos1 == pos2) {
 //     return false;
 //   }
 //   return true;
 // }

  public bool WithIn(Pos min, Pos max) {
    if (_x < min._x) { return false;}
    if (_y < min._y) { return false;}
    if (_x > max._x) { return false;}
    if (_y > max._y) { return false;}
    return true;
  }

  public void Set(int x_, int y_) {
    _x = x_;
    _y = y_;
  }

  public int _x;
  public int _y;
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

public interface IMoveSkill {
  List<Pos> GetDsts(Pos from);
}

public class PosMoveSkill : IMoveSkill {
  public PosMoveSkill() {
  }

  public void Add(Pos rel_pos) {
    _rel_pos.Add(rel_pos);
  }

  public List<Pos> GetDsts(Pos from) {
    var poses = new List<Pos>();
    foreach(var rel in _rel_pos) {
      var pos = from + rel;
      poses.Add(pos);
    }
    return poses;
  }

  private List<Pos> _rel_pos = new List<Pos>();
}

public class Piece {
  public enum Type {
    Good,
    Bad,
  }

  public enum MoveType {
    Dir,  // DirMoveSkill
    Pos,  // PosMoveSkill
    Dist, // DistMoveSkill
  }

  public Piece(Type type, Direction dir) {
    _type = type;
    _dir  = dir;
  }

  public Type GetPieceType() {
    return _type;
  }

  // FIXME : use move skill
  public Pos CalcDiff(Direction dir) {
    if (_dir == Direction.Front) {
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
    } else if (_dir == Direction.Back) {
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
    }
    throw new System.Exception();
  }

  public void AddMoveSkill(IMoveSkill skill) {
    _move_skills.Add(skill);
  }

  public bool CheckMovablePositions(Pos from, Pos to) {
    var res = new List<Pos>();
    foreach (var skill in _move_skills) {
      foreach (var dst in skill.GetDsts(from)) {
        Console.WriteLine($"x y = {dst._x} {dst._y}");
        if (dst == to) {
          return true;
        }
      }
    }
    return false;
  }

  public List<Pos> GetMovablePositions(Pos from) {
    var res = new List<Pos>();
    foreach (var skill in _move_skills) {
      res.AddRange(skill.GetDsts(from));
    }
    return res;
  }

  public bool[,] GetMovableMap(Pos from, int max_x, int max_y) {
    var map = new bool[max_x, max_y];

    foreach (var pos in GetMovablePositions(from)) {
      map[pos._x, pos._y] = true;
    }

    return map;
  }

  private Type _type;
  private Direction _dir;
  private List<IMoveSkill> _move_skills = new List<IMoveSkill>();
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
    map[pos._x, pos._y] = piece;
  }

  public void MarkAsTaken(Pos pos) {
    var piece = map[pos._x, pos._y];
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
    map[pos._x, pos._y] = null;
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
  Pos CalcDiff(int player_id, Pos from, Direction dir) {
    var piece = players[player_id].map[from._x, from._y];
    return piece.CalcDiff(dir);
    // TODO:
    // switch (ptype) {
    //   case PlayerType.Player1:
    //     switch (dir) {
    //       case Direction.Front:
    //         return new Pos(-1, 0);
    //       case Direction.Back:
    //         return new Pos(+1, 0);
    //       case Direction.Left:
    //         return new Pos(0, -1);
    //       case Direction.Right:
    //         return new Pos(0, +1);
    //     }
    //     break;
    //   case PlayerType.Player2:
    //     switch (dir) {
    //       case Direction.Front:
    //         return new Pos(+1, 0);
    //       case Direction.Back:
    //         return new Pos(-1, 0);
    //       case Direction.Left:
    //         return new Pos(0, +1);
    //       case Direction.Right:
    //         return new Pos(0, -1);
    //     }
    //     break;
    // }
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

  private Piece GetPiece(int player_id, Pos pos) {
    return players[player_id].map[pos._x, pos._y];
  }

  private void MarkAsTaken(int pidx, Pos pos) {
    Console.WriteLine("taken : good-");
    var player = players[pidx];
    player.MarkAsTaken(pos);
    // player.taken.Add(player.map[pos._x, pos._y]);
    // player.map[pos._x, pos._y] = null;
  }

  private void Move(int player_id, Pos from, Pos to) {
    var player = players[player_id];
    player.map[to._x, to._y] = player.map[from._x, from._y];
    player.map[from._x, from._y] = null;
  }

  private bool TryMovePlayerPiece(int player_id, Pos from, Pos to) {
    var gs = GetPiece(player_id, from);

    if (gs.CheckMovablePositions(from, to) == false) {
      var cfx = CoordinateTrans.XtoA(from._x);
      var cfy = CoordinateTrans.YtoA(from._y);
      var ctx = CoordinateTrans.XtoA(to._x);
      var cty = CoordinateTrans.YtoA(to._y);
      Console.WriteLine(
          $"This PIECE can not move from {cfx} {cfy} to {ctx} {cty}");
      Console.WriteLine(
          $"                            ({from._x} {from._y} to {to._x} {to._y})");
      return false;
    }

    if (gs == null) {
      var cx = CoordinateTrans.XtoA(from._x);
      var cy = CoordinateTrans.YtoA(from._y);
      throw new PieceDoesNotExistException(
          $"Threre is NOT a PIECE in ({cx},{cy}) or ({from._x},{from._y}).");
    }

    var friend = GetPiece(player_id, to);
    if (friend != null) {
      var cx = CoordinateTrans.XtoA(to._x);
      var cy = CoordinateTrans.YtoA(to._y);
      throw new PieceDoesNotExistException(
          $"Threre is a FRIEND PIECE in ({cx},{cy}) or ({to._x},{to._y}).");
    }

    // FIXME: use foreach for players
    // requires System.Linq.Enumerable;
    // var e_indices =
    //     from i in Enumerable.Range(0, players.Length) where i != player_id select i;
    // foreach (int i in e_indices) {
    for (int i = 0; i < players.Length; i++) {
      if (i == player_id) continue;
      var enemy  = GetPiece(i, to);
      if (enemy != null) {
        MarkAsTaken(i, to);
        break;
      }
    }
    Move(player_id, from, to);
    // var enemy  = GetPiece(1, to);
    // if (enemy != null) {
    //   MarkAsTaken(1, to);
    // }
    // Move(0, from ,to);

    return true;
  }

  public void TryMovePiece(int player_id, Pos from, Direction dir) {
    var to = from + CalcDiff(player_id, from, dir);
    Console.WriteLine($"------> from : {from._x} {from._y}");
    Console.WriteLine($"        to   : {to._x} {to._y}");
    Console.WriteLine($"             : {CalcDiff(player_id, from, dir)._x} {CalcDiff(player_id, from, dir)._y}");
    if (WithIn(to) == false) {
      var cx = CoordinateTrans.XtoA(to._x);
      var cy = CoordinateTrans.YtoA(to._y);
      throw new OutOfBoardAreaPositionException(
          $"({cx},{cy}) or ({to._x},{to._y}) is out of board area ({BoardHeight},{BoardWidth}).");
    }

    TryMovePlayerPiece(player_id, from, to);
  }

  public void TryMovePiece(int player_id, Pos from, Pos to) {
    if (WithIn(to) == false) {
      var cx = CoordinateTrans.XtoA(to._x);
      var cy = CoordinateTrans.YtoA(to._y);
      throw new OutOfBoardAreaPositionException(
          $"({cx},{cy}) or ({to._x},{to._y}) is out of board area ({BoardHeight},{BoardWidth}).");
    }

    TryMovePlayerPiece(player_id, from, to);
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

  public void DispMovablePos(int player_id, Pos cur) {
    var piece = players[player_id].map[cur._x, cur._y];
    if (piece == null) {
      return;
    }
    var map = piece.GetMovableMap(cur, 6, 6);
    Console.WriteLine("     1   2   3   4   5   6");
    Console.WriteLine("   +---+---+---+---+---+---+");
    for (int x = 0; x < 6; x++) {
      String str = " " + CoordinateTrans.XtoA(x) + " |";
      for (int y = 0; y < 6; y++) {
        if (map[x, y] == true) {
          str += " * |";
        } else {
          if(players[0].map[x, y] != null) {
            var sym = GetPieceTypeSymbol(players[0].map[x, y]);
            str += " " + sym + " |";
          } else if(players[1].map[x, y] != null) {
            var sym = GetPieceTypeSymbol(players[1].map[x, y]);
            str += " " + sym + " |";
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
