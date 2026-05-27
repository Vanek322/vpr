using System;
using System.Collections.Generic;

namespace vpr.Models;

public partial class ScoreRatio
{
    public int Id { get; set; }

    public int IdClassLevel { get; set; }

    public int MinScore { get; set; }

    public int MaxScore { get; set; }

    public int Grade { get; set; }

    public int IdSubject { get; set; }

    public virtual ClassLevel IdClassLevelNavigation { get; set; } = null!;

    public virtual Subject IdSubjectNavigation { get; set; } = null!;
}
