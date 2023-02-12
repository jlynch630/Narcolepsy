namespace Narcolepsy.Platform
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using Narcolepsy.Platform.Requests;

    public record NarcolepsyContext(Version AppVersion, OSPlatform Platform, IRequestManager Requests) {
	}
}
