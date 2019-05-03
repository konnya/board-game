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
