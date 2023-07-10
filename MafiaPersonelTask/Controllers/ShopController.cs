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

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            return Ok(context.Products.ToList());
        }

        [HttpGet]
        public IActionResult GetProduct(string ProductName)
        {
            Product? product = null;
            try
            {
                product = context.Products.Where(p => p.ProductName == ProductName).Single();
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
         
            return product == null ? BadRequest() : Ok(product);
        }
 
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

        public IActionResult Index()
        {
            return View();
        }
    }
}
