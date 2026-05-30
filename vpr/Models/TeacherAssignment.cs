using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class TeacherAssignment
{
    public int Id { get; set; }

    public int IdTeacher { get; set; }

    public int IdClass { get; set; }

    public int IdSubject { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Teacher Teacher { get; set; } = null!;

    public virtual ICollection<Protocol> Protocols { get; set; } = new List<Protocol>();
}
