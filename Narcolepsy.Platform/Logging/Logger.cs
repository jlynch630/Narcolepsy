namespace Narcolepsy.Platform.Logging;

using Serilog;
using Serilog.Events;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Serilog.Core;

public static class Logger {
    private static Serilog.Core.Logger Log;

    static Logger() =>
        Logger.Log = new LoggerConfiguration()
                     .MinimumLevel.Verbose()
                     .WriteTo.Console()
                     .WriteTo.Debug()
                     .CreateLogger();

    public static void AddSink(ILogEventSink sink) {
        Logger.Log = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.Debug()
            .WriteTo.Sink(sink)
            .CreateLogger();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Debug(string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Debug, Assembly.GetCallingAssembly(), messageTemplate, propertyValues);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Debug(Exception e, string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Debug, Assembly.GetCallingAssembly(), messageTemplate, propertyValues, e);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Verbose(string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Verbose, Assembly.GetCallingAssembly(), messageTemplate, propertyValues);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Verbose(Exception e, string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Verbose, Assembly.GetCallingAssembly(), messageTemplate, propertyValues, e);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Information(string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Information, Assembly.GetCallingAssembly(), messageTemplate, propertyValues);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Information(Exception e, string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Information, Assembly.GetCallingAssembly(), messageTemplate, propertyValues, e);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Warning(Exception? e, string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Warning, Assembly.GetCallingAssembly(), messageTemplate, propertyValues, e);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Error(Exception? e, string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Error, Assembly.GetCallingAssembly(), messageTemplate, propertyValues, e);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Warning(string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Warning, Assembly.GetCallingAssembly(), messageTemplate, propertyValues);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [MessageTemplateFormatMethod("messageTemplate")]
    public static void Error(string messageTemplate, params object?[]? propertyValues) =>
        Logger.Write(LogEventLevel.Error, Assembly.GetCallingAssembly(), messageTemplate, propertyValues);

    private static void Write(LogEventLevel level, Assembly calling, string messageTemplate, object?[]? propertyValues, Exception? e = null) {
        string? CallingAssemblyName = calling.GetName().Name;
        object?[] AllValues = Enumerable.Repeat(CallingAssemblyName, 1).Concat(propertyValues ?? Array.Empty<object?>()).ToArray();
        Logger.Log.Write(level, e, $"[{{Assembly}}] {messageTemplate}", AllValues);
    }
}

