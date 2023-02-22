namespace Narcolepsy.Platform {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IAssetManager {
        public void InjectScript(string path, string packageId);

        public void InjectScript(string path);

        public void InjectStylesheet(string path);

        public void InjectStylesheet(string path, string packageId);
    }
}
