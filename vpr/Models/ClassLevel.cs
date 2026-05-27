using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class ClassLevel
{
    public int Id { get; set; }

    public int Number { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<ScoreRatio> ScoreRatios { get; set; } = new List<ScoreRatio>();

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
}
