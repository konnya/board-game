using System;
using System.Collections.Generic;

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
