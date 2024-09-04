using Microsoft.IdentityModel.Protocols.Configuration;

namespace OracaoApp_API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetRequiredValue(this IConfiguration configuration, string key)
        {
            var value = configuration[key] ?? throw new InvalidConfigurationException($"Configuração '{key}' inválida ou não encontrada.");
            return value;
        }
    }
}
