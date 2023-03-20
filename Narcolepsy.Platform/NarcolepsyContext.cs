namespace Narcolepsy.Platform;
using System;
using System.Runtime.InteropServices;
using Narcolepsy.Platform.Requests;
using Narcolepsy.Platform.Serialization;

public record NarcolepsyContext(
	Version AppVersion,
	OSPlatform Platform,
	IRequestManager Requests,
	IAssetManager Assets,
	ISerializationManager Serialization);
