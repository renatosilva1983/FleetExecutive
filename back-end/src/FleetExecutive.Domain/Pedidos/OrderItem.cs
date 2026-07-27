using System.Security.Cryptography;
using FleetExecutive.Domain.Common;
using FleetExecutive.Domain.Orcamentos;

namespace FleetExecutive.Domain.Pedidos;

/// <summary>
/// Serviço (trecho) dentro de um Pedido. Chave de acesso de check-in é gerada no servidor, alta
/// entropia, uso único (Estrutura/07-autenticacao-seguranca-rbac.md — anti-tampering item 6).
/// </summary>
public class OrderItem : Entity
{
    public Guid OrderId { get; private set; }
    public Guid? DriverId { get; private set; }
    public Guid? VehicleId { get; private set; }
    public SubtipoServico Tipo { get; private set; }
    public string Origem { get; private set; } = default!;
    public string Destino { get; private set; } = default!;
    public DateTimeOffset DataHoraIda { get; private set; }
    public DateTimeOffset? DataHoraVolta { get; private set; }
    public decimal ValorServico { get; private set; }
    public decimal Acrescimo { get; private set; }
    public decimal Subtotal => ValorServico + Acrescimo;

    public string ChaveAcessoCheckin { get; private set; } = default!;
    public DateTimeOffset? CheckinEm { get; private set; }
    public double? CheckinLat { get; private set; }
    public double? CheckinLng { get; private set; }
    public double? LocalizacaoAtualLat { get; private set; }
    public double? LocalizacaoAtualLng { get; private set; }
    public DateTimeOffset? InicioServicoEm { get; private set; }
    public DateTimeOffset? FimServicoEm { get; private set; }

    private OrderItem() { }

    public static OrderItem Criar(Guid orderId, SubtipoServico tipo, string origem, string destino,
        DateTimeOffset dataHoraIda, decimal valorServico, DateTimeOffset? dataHoraVolta = null,
        Guid? driverId = null, Guid? vehicleId = null)
    {
        return new OrderItem
        {
            OrderId = orderId,
            Tipo = tipo,
            Origem = origem.Trim(),
            Destino = destino.Trim(),
            DataHoraIda = dataHoraIda,
            DataHoraVolta = dataHoraVolta,
            ValorServico = valorServico,
            DriverId = driverId,
            VehicleId = vehicleId,
            ChaveAcessoCheckin = Convert.ToHexString(RandomNumberGenerator.GetBytes(8)),
        };
    }

    public void VincularPrestador(Guid driverId, Guid? vehicleId = null)
    {
        DriverId = driverId;
        VehicleId = vehicleId;
    }

    /// <summary>
    /// Justificativa é obrigatória (Estrutura/03-sistema-atual-analise.md achado #3 — auditoria de
    /// alteração manual de valor). O registro de auditoria em si é responsabilidade do handler da
    /// Application layer (que tem acesso ao autor via ICurrentUser), não deste método de domínio.
    /// </summary>
    public void AdicionarAcrescimo(decimal valor, string justificativa)
    {
        if (string.IsNullOrWhiteSpace(justificativa))
            throw new InvalidOperationException("Justificativa é obrigatória para adicionar um acréscimo.");
        Acrescimo += valor;
    }

    public void RegistrarCheckin(string chaveInformada, double lat, double lng)
    {
        if (chaveInformada != ChaveAcessoCheckin)
            throw new InvalidOperationException("Chave de acesso inválida.");
        if (CheckinEm is not null)
            throw new InvalidOperationException("Check-in já foi registrado para este serviço.");

        CheckinEm = DateTimeOffset.UtcNow;
        CheckinLat = lat;
        CheckinLng = lng;
        LocalizacaoAtualLat = lat;
        LocalizacaoAtualLng = lng;
    }

    public void AtualizarLocalizacao(double lat, double lng)
    {
        LocalizacaoAtualLat = lat;
        LocalizacaoAtualLng = lng;
    }

    public void IniciarServico()
    {
        if (CheckinEm is null)
            throw new InvalidOperationException("Não é possível iniciar o serviço sem check-in registrado.");
        InicioServicoEm = DateTimeOffset.UtcNow;
    }

    public void FinalizarServico()
    {
        if (InicioServicoEm is null)
            throw new InvalidOperationException("Não é possível finalizar um serviço que não foi iniciado.");
        FimServicoEm = DateTimeOffset.UtcNow;
    }
}
