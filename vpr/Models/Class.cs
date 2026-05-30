using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class Class
{
    public int Id { get; set; }

    public string SymbolOfClass { get; set; } = null!;

    public int IdClassLevel { get; set; }

    public virtual ClassLevel ClassLevel { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    public virtual ICollection<TeacherAssignment> TeacherAssignments { get; set; } = new List<TeacherAssignment>();
}
