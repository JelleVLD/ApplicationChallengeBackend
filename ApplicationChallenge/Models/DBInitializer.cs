﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class DBInitializer
    {
        public static void Initialize(ApplicationContext context)
        {

            string HashPassword(string password)
            {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 2000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                string PasswordHash = Convert.ToBase64String(hashBytes);

                return PasswordHash;
            }

            context.Database.EnsureCreated();
            if (context.Bedrijven.Any())
            {
                return;
            }
            else
            {
                context.Bedrijven.AddRange(
         new Bedrijf { Naam = "GrassHopper", Postcode="2550",Straat="Veldkant", Nr="33b", Stad="Kontich", Biografie = "Grasshopper Academy", Foto = "Grasshopper.jpg" }
                                );
            }
            if (context.UserTypes.Any())
            {
                return;
            }
            else
            {
                context.UserTypes.AddRange(
                                new UserType {Soort="Admin" },
                                new UserType {Soort = "Maker" },
                                new UserType {Soort = "Bedrijf" }
                                );
            }
            if (context.Admins.Any())
            {
                return;
            }
            else
            {
                context.Admins.AddRange(
                                new Admin{Voornaam="De",Achternaam="Admin"}
                                );
            }
            if (context.Skills.Any())
            {
                return;
            }
            else
            {
                context.Skills.AddRange(
                                new Skill { Naam="Javascript" },
                                 new Skill { Naam = ".Net" },
                                  new Skill { Naam = "HTML5" },
                                   new Skill { Naam = "Java" }
                                );
            }
            if (context.Makers.Any())
            {
                return;
            }
            else
            {
                context.Makers.AddRange(
                                new Maker { Nickname = "Student123", Voornaam = "Jelle", Achternaam = "Van Langendonck", LinkedInLink = "https://www.linkedin.com/in/jelle-van-langendonck/", Ervaring = 3, GeboorteDatum = DateTime.Parse("13/11/1998"), Nr = "51", Straat = "Bochtstraat", Postcode = 2550, Stad = "Kontich", Biografie="Ik ben Jelle",Foto="jelle.jpg",CV="cv.pdf"}
                                );
            }
            context.SaveChanges();
            if (context.Opdrachten.Any())
            {
                return;
            }
            else
            {
                context.Opdrachten.AddRange(
                                new Opdracht {BedrijfId=1,Titel="Application Challenge",Omschrijving="Dit is een Challenge voor de studenten van Thomas More.",Postcode="2440",WoonPlaats="Geel",Straat="KleinhoefStraat",StraatNr="4" }
                                );
            }
            if (context.Tags.Any())
            {
                return;
            }
            else
            {
                context.Tags.AddRange(
                                new Tag {Naam=".NET"},
                                new Tag { Naam = "JavaScript" },
                                new Tag { Naam = "Hackathon" },
                                new Tag { Naam = "Angular" }
                                );
            }
            context.SaveChanges();
            if (context.UserLogins.Any())
            {
                return;
            }
            else
            {
                context.UserLogins.AddRange(
                                new UserLogin { Username = "Admin", Password = HashPassword("Admin1"), UserTypeId = 1, AdminId = 1, Email = "r0705177@student.thomasmore.be" },
                                new UserLogin { Username = "Student", Password = HashPassword("Student1"), UserTypeId = 2, MakerId = 1, Email = "r0697191@student.thomasmore.be", },
                                new UserLogin { Username = "Bedrijf", Password = HashPassword("Bedrijf1"), UserTypeId = 3, BedrijfId = 1, Email = "r0123456@student.thomasmore.be", }
                                );
            }
            if (context.SkillMakers.Any())
            {
                return;
            }
            else
            {
                context.SkillMakers.AddRange(
                                new SkillMaker { MakerId=1,SkillId=1,ExpertisePercentage=90,Interesse=0},
                                new SkillMaker { MakerId=1,SkillId=2,ExpertisePercentage=65,Interesse=0}
                                );
            }
            if (context.Reviews.Any())
            {
                return;
            }
            else
            {
                context.Reviews.AddRange(
                                new Review { MakerId = 1, ReviewTekst = "Goede Samenwerking, Vlotte ondersteuning", NaarBedrijf = true, BedrijfId = 1,Score=4 },
                                new Review { MakerId = 1, ReviewTekst = "Minimalistisch Design, Tijdige Upload", NaarBedrijf = false, BedrijfId = 1, Score =5 }
                                ) ;
            }
            if (context.OpdrachtTags.Any())
            {
                return;
            }
            else
            {
                context.OpdrachtTags.AddRange(
                                new OpdrachtTag { OpdrachtId=1,TagId=1}
                                );
            }
            context.SaveChanges();
            if (context.BedrijfTags.Any())
            {
                return;
            }
            else
            {
                context.BedrijfTags.AddRange(
                                new BedrijfTag { BedrijfId = 1, TagId =4 }
                                );
            }
            if (context.Permissions.Any())
            {
                return;
            }
            else
            {
                context.Permissions.AddRange(
                                new Permission { Title = "Bedrijf.OnGetID", UserTypeId = 3 },
                                new Permission { Title = "Bedrijf.OnDeleteID", UserTypeId = 3 },
                                new Permission { Title = "Opdracht.OnPutID", UserTypeId = 3 },
                                new Permission { Title = "Opdracht.OnDeleteID", UserTypeId = 3 },
                                new Permission { Title = "OpdrachtTag.OnGetBedrijfID", UserTypeId = 3 },
                                new Permission { Title = "Tag.OnCreate", UserTypeId = 3 },
                                new Permission { Title = "OpdrachtTag.OnDelete", UserTypeId = 3 },
                                new Permission { Title = "OpdrachtTag.OnCreate", UserTypeId = 3 },
                                new Permission { Title = "BedrijfTag.OnDelete", UserTypeId = 3 },
                                new Permission { Title = "BedrijfTag.OnCreate", UserTypeId = 3 }
                                );
            }
            context.SaveChanges();
        }

    }
}
