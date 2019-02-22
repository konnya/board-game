using System;
// using System.Collections.Generic;
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
