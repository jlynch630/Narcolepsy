namespace Narcolepsy.Platform.Requests;

using State;

public interface IRequestContext {
	MutableState<string> Name { get; }
}