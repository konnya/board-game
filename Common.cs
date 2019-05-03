using System;
using System.Collections.Generic;

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
