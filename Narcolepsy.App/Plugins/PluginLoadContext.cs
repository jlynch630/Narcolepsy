// -----------------------------------------------------------------------
// <copyright file="PluginLoadContext.cs" company="John Lynch">
//   This file is licensed under the MIT license
//   Copyright (c) 2022 John Lynch
// </copyright>
// -----------------------------------------------------------------------

namespace Narcolepsy.App.Plugins;

using Narcolepsy.Platform.Logging;
using System.Reflection;
using System.Runtime.Loader;

internal class PluginLoadContext : AssemblyLoadContext {
    private readonly AssemblyDependencyResolver DependencyResolver;

    private readonly string PluginPath;

    public PluginLoadContext(string pluginPath) {
        this.PluginPath = pluginPath;
        this.DependencyResolver = new AssemblyDependencyResolver(pluginPath);
    }

    public Assembly LoadPluginAssembly() {
        string PluginName = Path.GetFileNameWithoutExtension(this.PluginPath);
        if (PluginName is null) throw new PluginLoadException("Failed to load plugin, plugin path was null");

        AssemblyName AssemblyName = new(PluginName);
        try {
            Assembly Loaded = this.LoadFromAssemblyName(AssemblyName);
            Logger.Verbose("Loaded plugin assembly {PluginName}", PluginName);
            return Loaded;
        } catch (Exception e) {
            throw new PluginLoadException("Failed to load plugin", e);
        }
    }

    protected override Assembly Load(AssemblyName assemblyName) {
        string AssemblyPath = this.DependencyResolver.ResolveAssemblyToPath(assemblyName);
        return AssemblyPath != null ? this.LoadFromAssemblyPath(AssemblyPath) : null;
    }

    protected override nint LoadUnmanagedDll(string unmanagedDllName) {
        string LibraryPath = this.DependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        return LibraryPath != null ? this.LoadUnmanagedDllFromPath(LibraryPath) : IntPtr.Zero;
    }
}