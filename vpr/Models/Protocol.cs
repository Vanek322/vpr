using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class Protocol
{
    public int Id { get; set; }

    public int IdStudent { get; set; }

    public int IdTeacherAssignment { get; set; }

    public int Variant { get; set; }

    public int? PreviousGrade { get; set; }

    public int TotalScore { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual TeacherAssignment TeacherAssignment { get; set; } = null!;
}
