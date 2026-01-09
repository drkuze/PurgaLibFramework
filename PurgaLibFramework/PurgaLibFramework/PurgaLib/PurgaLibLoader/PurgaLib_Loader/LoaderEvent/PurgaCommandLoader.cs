using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandSystem;
using LabApi.Features.Console;
using RemoteAdmin;

namespace PurgaLibFramework.PurgaLibFramework.PurgaLib.PurgaLibLoader.PurgaLib_Loader.LoaderEvent
{
    public static class CommandLoader
    {
        private const string LoggerPrefix = "[PURGALIB_COMMAND_LOADER]";

        private class RegisteredEntry
        {
            public ICommand Command { get; set; }
            public Type HandlerType { get; set; }
        }

        private static readonly Dictionary<object, List<RegisteredEntry>> RegisteredCommands = new();

        public static void RegisterCommands(object plugin)
        {
            if (plugin == null) return;

            Assembly asm = plugin.GetType().Assembly;
            string pluginName = plugin.GetType().Name;

            var registered = new List<RegisteredEntry>();

            Type[] types;
            try
            {
                types = asm.GetTypes();
            }
            catch (ReflectionTypeLoadException rtle)
            {
                Logger.Error($"{LoggerPrefix} Error reading assembly types: {rtle}");
                types = rtle.Types?.Where(t => t != null).ToArray() ?? Array.Empty<Type>();
            }
            catch (Exception ex)
            {
                Logger.Error($"{LoggerPrefix} Error reading assembly types: {ex}");
                return;
            }

            foreach (Type type in types)
            {
                if (type == null || !typeof(ICommand).IsAssignableFrom(type) || type.IsAbstract)
                    continue;

                foreach (var attrData in type.GetCustomAttributesData())
                {
                    if (attrData.AttributeType != typeof(CommandHandlerAttribute))
                        continue;

                    if (attrData.ConstructorArguments.Count == 0) continue;

                    var arg = attrData.ConstructorArguments[0].Value;
                    if (arg is not Type handlerType)
                    {
                        Logger.Error($"{LoggerPrefix} CommandHandlerAttribute argument is not a Type for {type.FullName}");
                        continue;
                    }

                    if (TryRegister(type, handlerType, out ICommand created, pluginName, out string err))
                    {
                        registered.Add(new RegisteredEntry { Command = created, HandlerType = handlerType });
                        Logger.Raw($"{LoggerPrefix} Registered command '{created.Command}' for plugin '{pluginName}'", ConsoleColor.Green);
                    }
                    else
                    {
                        Logger.Error($"{LoggerPrefix} Failed to register command {type.Name} : {err}");
                    }
                }
            }

            RegisteredCommands[plugin] = registered;
        }

        public static void UnregisterCommands(object plugin)
        {
            if (!RegisteredCommands.TryGetValue(plugin, out var list)) return;

            foreach (var entry in list)
            {
                CommandHandler handler = ResolveHandlerSafely(entry.HandlerType);
                if (handler == null)
                {
                    Logger.Error($"{LoggerPrefix} Could not find handler {entry.HandlerType?.Name} to unregister.");
                    continue;
                }

                try
                {
                    foreach (var c in handler.AllCommands.ToArray())
                    {
                        if (ReferenceEquals(c, entry.Command) || string.Equals(c.Command, entry.Command.Command, StringComparison.OrdinalIgnoreCase))
                        {
                            handler.UnregisterCommand(entry.Command);
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"{LoggerPrefix} Error unregistering command {entry.Command.Command}: {ex}");
                }
            }

            RegisteredCommands.Remove(plugin);
        }

        private static bool TryRegister(Type commandType, Type handlerType, out ICommand command, string pluginName, out string error)
        {
            command = null;
            error = null;

            CommandHandler handler = ResolveHandlerSafely(handlerType);
            if (handler == null)
            {
                error = $"Handler '{handlerType.Name}' not found/resolvable at runtime.";
                return false;
            }

            object inst;
            try
            {
                inst = Activator.CreateInstance(commandType);
            }
            catch (Exception ex)
            {
                error = $"Error creating command instance: {ex.Message}";
                return false;
            }

            if (!(inst is ICommand cmd))
            {
                error = "Instance does not implement ICommand.";
                return false;
            }

            command = cmd;

            try
            {
                foreach (var existing in handler.AllCommands)
                {
                    if (string.Equals(existing.Command, cmd.Command, StringComparison.OrdinalIgnoreCase))
                        return true;

                    var aliasesProp = existing.GetType().GetProperty("Aliases", BindingFlags.Public | BindingFlags.Instance);
                    if (aliasesProp != null)
                    {
                        var aliasesObj = aliasesProp.GetValue(existing) as IEnumerable<string>;
                        if (aliasesObj != null && aliasesObj.Any(a => string.Equals(a, cmd.Command, StringComparison.OrdinalIgnoreCase)))
                            return true;
                    }
                }
            }
            catch { }

            try
            {
                handler.RegisterCommand(command);
            }
            catch (Exception ex)
            {
                error = $"Exception registering command: {ex.Message}";
                return false;
            }

            return true;
        }

        private static CommandHandler ResolveHandlerSafely(Type handlerType)
        {
            if (handlerType == null) return null;

            try
            {
                if (handlerType == typeof(RemoteAdminCommandHandler))
                    return CommandProcessor.RemoteAdminCommandHandler;

                if (handlerType == typeof(ClientCommandHandler))
                    return QueryProcessor.DotCommandHandler;

                if (handlerType == typeof(GameConsoleCommandHandler))
                {
                    Logger.Warn($"{LoggerPrefix} GameConsoleCommandHandler not available, skipping.");
                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logger.Error($"{LoggerPrefix} Error resolving handler {handlerType.Name}: {ex}");
                return null;
            }
        }
    }
}
