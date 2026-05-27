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

    public virtual Student IdStudentNavigation { get; set; } = null!;

    public virtual TeacherAssignment IdTeacherAssignmentNavigation { get; set; } = null!;
}
