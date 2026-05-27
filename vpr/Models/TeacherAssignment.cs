using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class TeacherAssignment
{
    public int Id { get; set; }

    public int IdTeacher { get; set; }

    public int IdClass { get; set; }

    public int IdSubject { get; set; }

    public virtual ClassLevel IdClassNavigation { get; set; } = null!;

    public virtual Subject IdSubjectNavigation { get; set; } = null!;

    public virtual Teacher IdTeacherNavigation { get; set; } = null!;

    public virtual ICollection<Protocol> Protocols { get; set; } = new List<Protocol>();
}
