// -----------------------------------------------------------------------
// <copyright file="PluginLoadContext.cs" company="John Lynch">
//   This file is licensed under the MIT license
//   Copyright (c) 2022 John Lynch
// </copyright>
// -----------------------------------------------------------------------

namespace Narcolepsy.App.Plugins;

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
        if (PluginName is null) throw new ApplicationException("Failed to load plugin");

        AssemblyName Name = new(PluginName);
        return this.LoadFromAssemblyName(Name);
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