using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using src.Models;

    public class DatabaseContext : IdentityDbContext
    {
        public DatabaseContext (DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }
        public DbSet<Bericht> Berichten {get; set;}
        public DbSet<Moderator> Moderator { get; set; }
        public DbSet<Hulpverlener> Hulpverleners { get; set; }
        public DbSet<Client> Clienten { get; set; }
        public DbSet<Aanmelding> Aanmeldingen {get; set;}

        public DbSet<Chat> Chats {get; set;}
        public DbSet<Voogd> Voogd {get; set;}
        public DbSet<HulpverlenerMelding> HulpverlenerMeldingen {get; set;}
        public DbSet<Account> Accounts {get; set;}
        public DbSet<MisbruikMelding> MisbruikMelding { get; set; }
        public DbSet<ApplicatieGebruiker> ApplicatieGebruikers { get; set; }
        public DbSet<GroepsChat> groepsChats {get; set;}
    }
