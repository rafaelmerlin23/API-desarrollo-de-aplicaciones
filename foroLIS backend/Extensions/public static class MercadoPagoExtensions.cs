namespace foroLIS_backend.Extensions;

using MercadoPago.Config;

public static class MercadoPagoExtensions
{
    public static void AddMercadoPago(this IServiceCollection services, IConfiguration config)
    {
        var token = config["MercadoPago:AccessToken"];
        MercadoPagoConfig.AccessToken = token;
    }
}