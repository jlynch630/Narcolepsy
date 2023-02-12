namespace Narcolepsy.Platform;

public record PluginVersion(int Major, int Minor, int Patch) {
	public static bool operator >(PluginVersion a, PluginVersion b) {
		if (a.Major > b.Major) return true;
		if (a.Major < b.Major) return false;

		return a.Minor > b.Minor || (a.Minor == b.Minor && a.Patch > b.Patch);
	}

	public static bool operator <(PluginVersion a, PluginVersion b) {
		if (a.Major < b.Major) return true;
		if (a.Major > b.Major) return false;

		return a.Minor < b.Minor || (a.Minor == b.Minor && a.Patch < b.Patch);
	}

	public override string ToString() => $"{this.Major}.{this.Minor}.{this.Patch}";
}