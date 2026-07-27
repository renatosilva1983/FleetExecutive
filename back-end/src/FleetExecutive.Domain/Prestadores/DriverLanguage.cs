using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Prestadores;

public class DriverLanguage : Entity
{
    public Guid DriverId { get; private set; }
    public string Idioma { get; private set; } = default!;
    public NivelIdioma Nivel { get; private set; }

    private DriverLanguage() { }

    public static DriverLanguage Criar(Guid driverId, string idioma, NivelIdioma nivel)
    {
        return new DriverLanguage { DriverId = driverId, Idioma = idioma.Trim(), Nivel = nivel };
    }
}
