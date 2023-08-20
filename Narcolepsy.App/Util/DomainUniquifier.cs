//namespace Narcolepsy.App.Util {
//    using Narcolepsy.CodeGen;
//    using System.Globalization;

//    // ReSharper disable once IdentifierTypo
//    internal class DomainUniquifier {
//        private record Domain(string[] User, string Suffix);

//        public static string[] GetUniqueDomains(string[] domains) {
//            // we want to take this list of domains and extract only the unique ones
//            // but, there's a catch. say I provide a.example.com and b.example.com. Then, to prevent clutter
//            // we should consider those the same and return just example.com
//            // but, there's another catch. say we provide a.github.io and b.github.io. Well, github.io is 
//            // really a hosting service, we should consider those separate domains.
//            // Here's what we'll do:
//            //  1. split the domain into its "owner-controlled" part and its suffix (according to the public suffix list, 
//            //         which includes tlds like .com and private endings like github.io)
//            //  2. ?
//            List<Domain> DomainList = new();

//            foreach (string Domain in domains) {
//                foreach (PublicSuffix Suffix in PublicSuffix.Rules) {
//                    if (!Domain.EndsWith(Suffix.Domain) || !Domain.EndsWith($".{Suffix.Domain}")) continue;
//                    // found the ending!

//                    string[] Parts = Domain.Substring(0, Domain.Length - (Suffix.Domain.Length + 1)).Split(".");
//                    string ResolvedSuffix = Suffix.Domain;

//                    if (Suffix.IsWildcard) {
//                        ResolvedSuffix = $"{Parts[^1]}.{ResolvedSuffix}";
//                        Parts = Parts[..^1];
//                    }

//                    DomainList.Add(new DomainUniquifier.Domain(Parts, ResolvedSuffix));
//                    break;
//                }
//            }

//            // now make sure they're unique
//            // todo
//            return DomainList.Select(d => d.User[^1] + "." + d.Suffix).Distinct().ToArray();
//        }
//    }
//}
