using FleetExecutive.Domain.Common;

namespace FleetExecutive.Domain.Prestadores;

/// <summary>
/// Prestador de serviço — motorista autônomo ou empresa de fretamento terceirizada (Estrutura/02-dominio-e-glossario.md).
/// Cadastro real hoje é simples (nome, status, referência, telefone, comissão %); Idiomas,
/// Capacidades e Documentos são melhorias do protótipo incorporadas aqui desde já (ver
/// Estrutura/11-modulos-cadastro.md). Rotas frequentes e NPS/estatísticas são caches calculados a
/// partir do histórico de pedidos — ficam para quando o módulo Pedidos existir.
/// </summary>
public class Driver : Entity
{
    public string Nome { get; private set; } = default!;
    public TipoPrestador Tipo { get; private set; }
    public bool Ativo { get; private set; } = true;
    public string? Telefone { get; private set; }
    public string? Referencia { get; private set; }
    public decimal ComissaoPercentual { get; private set; }
    public bool Indicacao { get; private set; }
    public string? FonteIndicacao { get; private set; }

    private readonly List<DriverCapability> _capacidades = [];
    public IReadOnlyCollection<DriverCapability> Capacidades => _capacidades.AsReadOnly();

    private readonly List<DriverLanguage> _idiomas = [];
    public IReadOnlyCollection<DriverLanguage> Idiomas => _idiomas.AsReadOnly();

    private readonly List<DriverDocument> _documentos = [];
    public IReadOnlyCollection<DriverDocument> Documentos => _documentos.AsReadOnly();

    private Driver() { }

    public static Driver Criar(string nome, TipoPrestador tipo, decimal comissaoPercentual,
        string? telefone = null, string? referencia = null, bool indicacao = false, string? fonteIndicacao = null)
    {
        if (comissaoPercentual is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(comissaoPercentual), "Comissão precisa estar entre 0 e 100.");

        return new Driver
        {
            Nome = nome.Trim(),
            Tipo = tipo,
            ComissaoPercentual = comissaoPercentual,
            Telefone = telefone?.Trim(),
            Referencia = referencia?.Trim(),
            Indicacao = indicacao,
            FonteIndicacao = fonteIndicacao?.Trim(),
        };
    }

    public void Atualizar(string nome, string? telefone, string? referencia, bool indicacao, string? fonteIndicacao)
    {
        Nome = nome.Trim();
        Telefone = telefone?.Trim();
        Referencia = referencia?.Trim();
        Indicacao = indicacao;
        FonteIndicacao = fonteIndicacao?.Trim();
        MarkUpdated(null);
    }

    public void AtualizarComissao(decimal novaComissaoPercentual)
    {
        if (novaComissaoPercentual is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(novaComissaoPercentual), "Comissão precisa estar entre 0 e 100.");
        ComissaoPercentual = novaComissaoPercentual;
    }

    public void DefinirCapacidades(IEnumerable<DriverCapability> capacidades)
    {
        _capacidades.Clear();
        _capacidades.AddRange(capacidades.Distinct());
    }

    public void AdicionarIdioma(DriverLanguage idioma) => _idiomas.Add(idioma);
    public void AdicionarDocumento(DriverDocument documento) => _documentos.Add(documento);

    public void Desativar() => Ativo = false;
    public void Ativar() => Ativo = true;
}
