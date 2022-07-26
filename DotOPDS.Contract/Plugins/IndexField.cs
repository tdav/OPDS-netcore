﻿namespace DotOPDS.Contract.Plugins;

/// <summary>
/// Additional searchable field.
/// </summary>
public class IndexField
{
    /// <summary>
    /// Field name as specified in <see cref="Models.MetaField"/>.
    /// </summary>
    public string Field { get; set; } = null!;

    /// <summary>
    /// Will be displayed in OPDS catalog.
    /// </summary>
    public string DisplayName { get; set; } = null!;
}
