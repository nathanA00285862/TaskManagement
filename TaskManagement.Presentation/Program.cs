using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using MediatR;
using TaskManagement.Application.Commands;
using TaskManagement.Application.Queries;
using TaskManagement.Infrastructure;
using Exporter.Abstractions;
using System.Runtime.Loader;
using System.Reflection;
using TaskManagement.Domain;
using System.Collections.Generic;
using System.IO;

namespace TaskManagement.Presentation;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        // Set up dependency injection
        var services = new ServiceCollection();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly));
        services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
        services.AddFeatureManagement(configuration.GetSection("FeatureManagement"));
        var provider = services.BuildServiceProvider();

        var mediator = provider.GetRequiredService<IMediator>();
        var featureManager = provider.GetRequiredService<IFeatureManager>();
        var exporters = LoadPlugins();

        // Menu loop
        while (true)
        {
            Console.WriteLine("\nTask Management System");
            Console.WriteLine("1. Create Task");
            Console.WriteLine("2. View All Tasks");
            Console.WriteLine("3. View Task Details");
            Console.WriteLine("4. Mark Task Complete");
            Console.WriteLine("5. Delete Task");
            Console.WriteLine("6. Export Tasks");
            Console.WriteLine("7. Exit");
            Console.Write("Select an option: ");

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Write("Enter task description: ");
                    var description = Console.ReadLine();
                    string? priority = null;
                    if (await featureManager.IsEnabledAsync("TaskPrioritization"))
                    {
                        Console.Write("Enter priority (Low/Medium/High): ");
                        priority = Console.ReadLine();
                    }
                    var createCommand = new CreateTaskCommand(description!, priority);
                    var newTask = await mediator.Send(createCommand);
                    Console.WriteLine($"Task created with ID: {newTask.Id}");
                    break;

                case "2":
                    var tasks = await mediator.Send(new GetAllTasksQuery());
                    Console.WriteLine("\nTasks:");
                    bool showPriority = await featureManager.IsEnabledAsync("TaskPrioritization");
                    foreach (var task in tasks)
                    {
                        var status = task.IsCompleted ? "Completed" : "Pending";
                        var priorityDisplay = showPriority && task.Priority != null ? $" | Priority: {task.Priority}" : "";
                        Console.WriteLine($"ID: {task.Id} | {task.Description} | Status: {status}{priorityDisplay}");
                    }
                    break;

                case "3":
                    Console.Write("Enter task ID: ");
                    if (int.TryParse(Console.ReadLine(), out int id))
                    {
                        var task = await mediator.Send(new GetTaskByIdQuery(id));
                        if (task != null)
                        {
                            var status = task.IsCompleted ? "Completed" : "Pending";
                            var priorityDisplay = (await featureManager.IsEnabledAsync("TaskPrioritization")) && task.Priority != null ? $" | Priority: {task.Priority}" : "";
                            Console.WriteLine($"ID: {task.Id} | {task.Description} | Status: {status}{priorityDisplay}");
                        }
                        else
                        {
                            Console.WriteLine("Task not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }
                    break;

                   
                case "4":
                    Console.Write("Enter task ID to mark complete: ");
                    if (int.TryParse(Console.ReadLine(), out int completeId))
                    {
                        var task = await mediator.Send(new GetTaskByIdQuery(completeId));
                        if (task != null)
                        {
                            await mediator.Send(new UpdateTaskCommand(completeId, true));
                            Console.WriteLine("Task marked as complete.");
                        }
                        else
                        {
                            Console.WriteLine("Task not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }
                    break;

                case "5":
                    Console.Write("Enter task ID to delete: ");
                    if (int.TryParse(Console.ReadLine(), out int deleteId))
                    {
                        var task = await mediator.Send(new GetTaskByIdQuery(deleteId));
                        if (task != null)
                        {
                            await mediator.Send(new DeleteTaskCommand(deleteId));
                            Console.WriteLine("Task deleted.");
                        }
                        else
                        {
                            Console.WriteLine("Task not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid ID.");
                    }
                    break;

                case "6":
                    Console.WriteLine("Available formats: " + string.Join(", ", exporters.Select(e => e.GetFormatName())));
                    Console.Write("Enter format: ");
                    var format = Console.ReadLine();
                    var exporter = exporters.FirstOrDefault(e => e.GetFormatName().Equals(format, StringComparison.OrdinalIgnoreCase));
                    if (exporter != null)
                    {
                        var allTasks = await mediator.Send(new GetAllTasksQuery());
                        var data = exporter.Export(allTasks);
                        File.WriteAllBytes($"tasks.{format}", data);
                        Console.WriteLine($"Tasks exported to tasks.{format}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid format.");
                    }
                    break;

                case "7":
                    return;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    static List<IExporter> LoadPlugins()
    {
        var pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
        if (!Directory.Exists(pluginsPath)) return new List<IExporter>();

        var exporters = new List<IExporter>();
        foreach (var dll in Directory.GetFiles(pluginsPath, "*.dll"))
        {
            var context = new PluginLoadContext(dll);
            var assembly = context.LoadFromAssemblyPath(dll);
            foreach (var type in assembly.GetTypes().Where(t => typeof(IExporter).IsAssignableFrom(t) && !t.IsInterface))
            {
                var exporter = (IExporter)Activator.CreateInstance(type)!;
                exporters.Add(exporter);
            }
        }
        return exporters;
    }

    class PluginLoadContext : AssemblyLoadContext
    {
        private readonly AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            string? assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            return null;
        }
    }
}