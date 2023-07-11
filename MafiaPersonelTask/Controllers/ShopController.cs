using Domain.Entities;
using Domain.Persistance;
using Mafia.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Xml.Linq;

namespace MafiaPersonelTask.Controllers
{
    public class ShopController : Controller
    {
        private readonly MafiaPersonalContext context = new ();

        private static Dictionary<string, int> AmmunitionTypes = new()
        {
            { "Glock", 1 },
            { "M4", 2 },
            { "AK47", 3 }
        };

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(context.Shops.ToList());
        }

        [HttpGet]
        public IActionResult GetProduct(string ProductName)
        {
            Shop? shop = null;
            try
            {
                shop = context.Shops.Where(p => p.ProductName == ProductName).Single();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
         
            return shop == null ? BadRequest() : Ok(shop);
        }
        /*
        public ActionResult BuyProduct(string ProductName, int MemberId, int CountToBuy)
        {
            Product? product = null;
            int productCount = 0;
            try
            {
                product = context.Products.Where(p => p.ProductName == ProductName).Single();
                productCount = product.Count.Value;
                if (productCount > 0 && productCount >= CountToBuy)
                {
                    FamilyMember? member = context.FamilyMembers.Find(MemberId);
                    if (member != null) 
                    {
                        if (product.PricePerPiece * CountToBuy <= member.Money)
                        {
                            Inventory? inventory = context.Inventories.Find(MemberId);
                            if (inventory != null)
                            {
                                switch (product.ProductName)
                                {
                                    case "BaseballBat":
                                        inventory.BaseballBat += CountToBuy;
                                        break;
                                    case "BrassKnuckles":
                                        inventory.BrassKnuckles += CountToBuy;
                                        break;
                                    case "M4":
                                        inventory.M4 += CountToBuy;
                                        break;
                                    case "AK47":
                                        inventory.Ak47 += CountToBuy;
                                        break;
                                    case "Glock":
                                        inventory.Glock += CountToBuy;
                                        break;
                                    case "5,45x39mm":
                                        inventory._545x39mm += CountToBuy;
                                        break;
                                    case "7,62x39mm":
                                        inventory._762x39mm += CountToBuy;
                                        break;
                                    case "9x19mm":
                                        inventory._9x19mm += CountToBuy;
                                        break;
                                }
                                member.Money -= product.PricePerPiece.Value * CountToBuy;
                                if (member.Money < 0) member.Money = 0;
                                context.SaveChanges();
                                return Ok("Покупка совершенна успешно");

                            }
                            else
                            {
                                return BadRequest("Что-то странное произошло");
                            }
                        }
                        else
                        {
                            return BadRequest("С таким количество денег только с водяным пистолетом ходить");
                        }
                    }
                    else
                    {
                        return BadRequest("Cringe MemberId");
                    }
                }
                else
                {
                    return BadRequest("В магазине недостаточное количество");
                }
                
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
         */ 

        public ActionResult GetWeapons()
        {
            return Ok(context.Weapons.ToList());
        }

        public ActionResult GetFamilyMemberWeapon(int id)
        { 
            FamilyMember member = context.FamilyMembers.Where(p => p.Id == id).Single();
            if (member == null) return NotFound("Такой член мафии не найден");
            Weapon weapon = context.Weapons.Where(p => p.Id == id).Single();
            if (weapon == null) return NotFound("У этого члена мафии нет оружия");
            return Ok(weapon);
        }

        public ActionResult GetMafiaFamilyMoney(int id) 
        {
            MafiaFamily family = context.MafiaFamilies.Where(p => p.Id == id).Single();
            if (family == null) return NotFound();
            return Ok(family.Money);
        }


        public ActionResult BuyWeapon(int FamilyMemberId, string WeaponName)
        {
            FamilyMember member = context.FamilyMembers.Where(p => p.Id == FamilyMemberId).Single();
            if (member == null) return NotFound("Не найден такой член семьи");
            MafiaFamily family = context.MafiaFamilies.Where(p => p.Id == member.MafiaFamilyId).Single();
            //if (weapon == null) return BadRequest("У члена семьи отсуствует ");
            Shop shop = context.Shops.Where(p => p.ProductName == WeaponName).Single();
            if (shop == null) return NotFound("Не существует оружия под таким названием");
            else if (shop.ProductName == "545mm" || shop.ProductName == "762mm" || shop.ProductName == "9mm") return BadRequest("Вы выбрали патрон, а не оружия, для покупки патрон используйте другой запрос");
            if (family.Money < shop.PricePerPiece) return BadRequest("У семьи не хватает денег на ваши хотелки");
            family.Money -= shop.PricePerPiece;

            Weapon weapon = context.Weapons.Where(p => p.FamilyMemberId == FamilyMemberId).Single();
                
            int? AmmunitionTypeId = AmmunitionTypes.ContainsKey(shop.ProductName) ? AmmunitionTypes[shop.ProductName] : null;
            if (weapon == null)
            {
                context.Weapons.Add(new Weapon
                {
                    FamilyMemberId = FamilyMemberId,
                    Name = shop.ProductName,
                    AmmunitionTypeId = AmmunitionTypeId,
                    AmmunitionCount = AmmunitionTypeId == null ? null : 0
                }); 
            }
            else
            {
                weapon.Name = shop.ProductName;
                weapon.AmmunitionTypeId = AmmunitionTypeId;
                weapon.AmmunitionCount = AmmunitionTypeId == null ? null : 0;
            }

            shop.Count -= 1;
            try
            {
                context.SaveChanges();
                return Ok("Запрос успешен");
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        public ActionResult BuyAmmunition(int FamilyMemberId, string AmmunitionName, int Count)
        {
            FamilyMember member = context.FamilyMembers.Where(p => p.Id == FamilyMemberId).Single();
            if (member == null) return NotFound("Не найден такой член семьи");
            MafiaFamily family = context.MafiaFamilies.Where(p => p.Id == member.MafiaFamilyId).Single();
            //if (weapon == null) return BadRequest("У члена семьи отсуствует ");
            Shop? shop = context.Shops.Where(p => p.ProductName == AmmunitionName).Single();
            
            if (shop == null) return NotFound("Не существует патрон под таким названием");
            Weapon weapon = context.Weapons.Where(p => p.FamilyMemberId == FamilyMemberId).Single();
            if (weapon == null) return NotFound("У члена семьи нет оружия");

            else if (weapon.AmmunitionTypeId == null || context.AmmunitionTypes.Where(p => p.Id == weapon.AmmunitionTypeId).Single().Type != AmmunitionName) return BadRequest("Оружие члена семьи не использует такой тип боеприпасов");

            if (family.Money < shop.PricePerPiece * Count) return BadRequest("У семьи не хватает денег на ваши хотелки");
            if (shop.Count < Count) return BadRequest("В магазине нет столько патрон");

            family.Money -= shop.PricePerPiece * Count;
            shop.Count -= Count;
            weapon.AmmunitionCount += Count;

            try
            {
                context.SaveChanges();
                return Ok("Запрос успешен");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
