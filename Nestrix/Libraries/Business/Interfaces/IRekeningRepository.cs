using LogicLayer.Model;

namespace LogicLayer.Interfaces;

public interface IRekeningRepository
{
    Task RekeningToevoegenAsync(Rekening rekening);
    Task RekeningVerwijderenAsync(Guid id);
    Task RekeningWijzigenAsync(Guid id, Rekening rekening);
    Task<Rekening?> RekeningOphalenAsync(Guid id, int depth);
    Task<Rekening?> RekeningOphalenViaEmailAsync(string email, int depth);
    Task<Rekening?> RekeningOphalenViaIBANAsync(string iban, int depth);
}