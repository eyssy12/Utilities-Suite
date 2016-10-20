namespace EyssyApps.Organiser.Library
{
    using System;

    [Flags]
    public enum OrganiseType : long
    {
        None = 0,
        File = 1,
        Directory = 2,
        All = 4
    }
}