using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationChallenge.Models
{
    public class DBInitializer
    {
        public static void Initialize(ApplicationContext context)
        {

            context.Database.EnsureCreated();
            if (context.Bedrijven.Any())
            {
                return;
            }
            else
            {
                context.Bedrijven.AddRange(
                                new Bedrijf { Naam = "GrassHopper", Adres = "Veldkant 33b, 2550 Kontich", Biografie = "Grasshopper Academy", Foto = "Grasshopper.jpg" }
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
                                new Maker { Nickname = "Student123", Voornaam = "Jelle", Achternaam = "Van Langendonck", LinkedInLink = "https://www.linkedin.com/in/jelle-van-langendonck/", Email = "r0705177@student.thomasmore.be", Ervaring = 3, GeboorteDatum = DateTime.Parse("13/11/1998"),GsmNr="+32494692400",Biografie="Ik ben Jelle",Foto="jelle.jpg",CV="cv.pdf"}
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
                                new Opdracht {BedrijfId=1,Titel="Application Challenge",Omschrijving="Dit is een Challenge voor de studenten van Thomas More.",Locatie= "Kleinhoefstraat 4, 2440 Geel" }
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
                                new UserLogin { Username = "Admin", Password = "Admin1", UserTypeId = 1, AdminId = 1 },
                                new UserLogin { Username = "Student", Password = "Student1", UserTypeId = 2, MakerId = 1 },
                                new UserLogin { Username = "Bedrijf", Password = "Bedrijf1", UserTypeId = 3, BedrijfId = 1 }
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
                                new Permission {Title="Bedrijf.OnGetID" },
                                new Permission { Title = "Bedrijf.OnDeleteID" },
                                new Permission { Title = "Opdracht.OnPutID" },
                                new Permission { Title = "Opdracht.OnDeleteID" },
                                 new Permission { Title = "OpdrachtTag.OnGetBedrijfID" },
                                 new Permission { Title = "Tag.OnCreate" },
                                 new Permission { Title = "OpdrachtTag.OnDelete" },
                                 new Permission { Title = "OpdrachtTag.OnCreate" }
                                );
            }
            context.SaveChanges();
        }
    }
}
