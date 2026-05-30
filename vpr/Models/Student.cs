using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class Student
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string? ParticipantCode { get; set; }

    public int IdClass { get; set; }

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Protocol> Protocols { get; set; } = new List<Protocol>();
}
