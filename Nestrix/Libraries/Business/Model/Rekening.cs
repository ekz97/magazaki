﻿using LogicLayer.Exceptions;

namespace LogicLayer.Model;

public class Rekening
{
    public Guid Rekeningnummer { get; set; }
    public RekeningType RekeningType { get; set; }
    public decimal KredietLimiet { get; set; }
    public decimal Saldo { get; set; }
    public List<Transactie> Transacties { get; set; }
    public Gebruiker Gebruiker { get; set; }
    
    // Nieuwe rekening aanmaken
    public Rekening(RekeningType rekeningType, decimal kredietLimiet, Gebruiker gebruiker)
    {
        Rekeningnummer = Guid.NewGuid();
        ZetRekeningType(rekeningType);
        ZetKredietLimiet(kredietLimiet);
        ZetGebruiker(gebruiker);
        Saldo = 0;
        Transacties = new List<Transactie>();
    }
    
    // Rekening wijzigen
    public Rekening(Guid id, decimal kredietLimiet)
    {
        Rekeningnummer = id;
        // RekeningType wijzigen toelaten?
        // ZetRekeningType(rekeningType);
        ZetKredietLimiet(kredietLimiet);
        // Gebruiker wijzigen toelaten?
    }

    // Rekening ophalen uit database// Controles zijn niet nodig
    public Rekening(Guid rekeningnummer, RekeningType rekeningType, decimal kredietLimiet, decimal saldo,
        List<Transactie> transacties, Gebruiker gebruiker)
    {
        Rekeningnummer = rekeningnummer;
        RekeningType = rekeningType;
        KredietLimiet = kredietLimiet;
        Saldo = saldo;
        Transacties = transacties;
        Gebruiker = gebruiker;
    }
    
    // Test constructor
    public Rekening()
    {
    }
    
    public void ZetRekeningType(RekeningType rekeningType)
    {
        RekeningType = rekeningType;
    }
    
    public void ZetKredietLimiet(decimal kredietLimiet)
    {
        if (kredietLimiet < 0)
        {
            throw new RekeningException("KredietLimiet mag niet negatief zijn.");
        }
        
        KredietLimiet = kredietLimiet;
    }
    
    public void ZetGebruiker(Gebruiker gebruiker)
    {
        if (gebruiker == null)
        {
            throw new RekeningException("Gebruiker mag niet leeg zijn.");
        }
        
        Gebruiker = gebruiker;
    }
}