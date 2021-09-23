using System;
using SamuraiApp.Data;
using System.Linq;
using System.Collections.Generic;
using SamuraiApp.Domain;
using Microsoft.EntityFrameworkCore;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        private static void Main(string[] args)
        {
            //PrePopulateSamuraisAndBattles();
            //JoinBattleAndSamurai();
            //EnlistSamuraiIntoABattle();
            //EnlistSamuraiIntoABattleUntracked();
            //AddNewSamuraiViaDisconnectedBattleObject();
            //GetSamuraiWithBattles();
            //RemoveJoinBetweenSamuraiAndBattle(3, 1);
            //AddSamuraiWithSecretIdentity();
            //AddSecretIdentityToExistingSamurai();
        }

        private static void AddSecretIdentityToExistingSamurai()
        {
            var samurai = _context.Samurais.Find(2);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julia" };
            _context.Attach(samurai);
            _context.SaveChanges();
        }
        private static void AddSecretIdentityUsingSamuraiId()
        {
            var secretIdentity = new SecretIdentity { RealName = "Isaac", SamuraiId = 1 };
            _context.Add(secretIdentity);
            _context.SaveChanges();
        }
        private static void AddSamuraiWithSecretIdentity()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julie" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void RemoveJoinBetweenSamuraiAndBattle(int samuraiId, int battleId)
        {
            var samurai = _context.Samurais.Include(s => s.SamuraiBattles).SingleOrDefault(s => s.Id == samuraiId);
            var join = samurai.SamuraiBattles.SingleOrDefault(s => s.BattleId == battleId);
            if (join != null)
            {
                _context.Remove(join);
                _context.SaveChanges();
            }
        }
        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle)
                .FirstOrDefault(s => s.Id == 1);
            var battles = samuraiWithBattles.Battles();
        }
        private static void AddNewSamuraiViaDisconnectedBattleObject()
        {
            Battle battle;
            using (var separateOperation = new SamuraiContext())
            {
                battle = separateOperation.Battles.Find(1);
            }
            var newSamurai = new Samurai { Name = "Sampson" };
            battle.SamuraiBattles.Add(new SamuraiBattle { Samurai = newSamurai });
            _context.Attach(battle);
            _context.SaveChanges();
        }

        private static void EnlistSamuraiIntoABattleUntracked()
        {
            Battle battle;
            using (var separateOperation = new SamuraiContext())
            {
                battle = separateOperation.Battles.Find(1);
            }
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 2 });

            _context.Battles.Attach(battle);
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }
        private static void EnlistSamuraiIntoABattle()
        {
            var battle = _context.Battles.Find(1);
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 3 });
            _context.SaveChanges();
        }
        private static void JoinBattleAndSamurai()
        {
            var sbJoin = new SamuraiBattle { SamuraiId = 1, BattleId = 3 };
            _context.Add(sbJoin);
            _context.SaveChanges();
        }
        private static void PrePopulateSamuraisAndBattles()
        {
            _context.AddRange(
             new Samurai { Name = "Kikuchiyo" },
             new Samurai { Name = "Kambei Shimada" },
             new Samurai { Name = "Shichirōji " },
             new Samurai { Name = "Katsushirō Okamoto" },
             new Samurai { Name = "Heihachi Hayashida" },
             new Samurai { Name = "Kyūzō" },
             new Samurai { Name = "Gorōbei Katayama" }
           );

            _context.Battles.AddRange(
             new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) },
             new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },
             new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },
             new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
           );
            _context.SaveChanges();
        }
    }
}
