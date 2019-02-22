using System;
using System.Collections.Generic;
// using System.Linq.Enumerable;

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
    // Console.WriteLine("--- Piece Test");
    // Piece gst = new Piece(Piece.Type.Good);
    // Console.WriteLine("--- Pos : x:{0} y:{1}", gst.X(), gst.Y());
    // Console.WriteLine("--- Move Front");
    // gst.Move(Direction.Front);
    // Console.WriteLine("--- Pos : x:{0} y:{1}", gst.X(), gst.Y());
    // Console.WriteLine("--- Move Right");
    // gst.Move(Direction.Right);
    // Console.WriteLine("--- Pos : x:{0} y:{1}", gst.X(), gst.Y());

    Board brd = new Board();
    for (int i = 0; i < 4; i++) {
      Piece gst1 = new Piece(Piece.Type.Good, Direction.Back);
      brd.LocatePiece(PlayerType.Player1, 4, i+1, gst1);
      Piece gst2 = new Piece(Piece.Type.Bad, Direction.Back);
      brd.LocatePiece(PlayerType.Player1, 5, i+1, gst2);
    }
    for (int i = 0; i < 4; i++) {
      Piece gst1 = new Piece(Piece.Type.Good, Direction.Front);
      brd.LocatePiece(PlayerType.Player2, 0, i+1, gst1);
      Piece gst2 = new Piece(Piece.Type.Bad, Direction.Front);
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

      // FIXME: 1st argument must be player-id
      // board.TryMovePiece(PlayerType.Player1, from, dir);
      board.TryMovePiece((int)PlayerType.Player1, from, dir);

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

      // FIXME: 1st argument must be player-id
      // board.TryMovePiece(PlayerType.Player2, from, dir);
      board.TryMovePiece((int)PlayerType.Player2, from, dir);

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
