namespace Narcolepsy.App.Collections {
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public record CollectionMetadata(string Id, string Name, DateTime LastModified, int RequestCount, string[] Domains);
}
