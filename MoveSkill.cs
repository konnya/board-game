using System;
using System.Collections.Generic;

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
