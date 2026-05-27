using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class Subject
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ScoreRatio> ScoreRatios { get; set; } = new List<ScoreRatio>();

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
}
