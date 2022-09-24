namespace dotnet_hero.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServiceInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = typeof(IStartup).Assembly.ExportedTypes
                .Where(x => typeof(IInstallers).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstallers>().ToList();

            installers.ForEach(installers => installers.InstallServices(services, configuration));
        }
    }
}
