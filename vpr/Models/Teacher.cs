using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
}
