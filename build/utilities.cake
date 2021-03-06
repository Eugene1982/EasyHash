

private void PrintGitVersion(GitVersion version) {
    Information("--------------- GitVersion Info ---------------- ");
    Information("AssemblySemVer is       " + version.AssemblySemVer);
    Information("FullSemVer is           " + version.FullSemVer);
    Information("InformationalVersion is " + version.InformationalVersion);
    Information("LegacySemVer is         " + version.LegacySemVer);
    Information("Major is                " + version.Major);
    Information("MajorMinorPatch is      " + version.MajorMinorPatch);
    Information("Minor is                " + version.Minor);
    Information("Patch is                " + version.Patch);
    Information("SemVer is               " + version.SemVer);
    Information("------------------------------------------------ ");
}

private string GetEnvironmentVariable(string variableName) {
    string val = EnvironmentVariable(variableName);
    if (val == null) {
        throw new Exception("Environment has no " + variableName + " variable");
    }
    return val;
}