// namespace API_Gateway.Database
// {
//     public class EcommerceModule
//     {
//         private readonly Mapper _mapper;

//         public EcommerceModule()
//         {
//             _mapper = new Mapper();
//         }

//         void InsertFirstUser()
//         {
//             string username = "Wazi";
//             string email = "w@gmail.com";
//             string password = "12345678";
//             string mobileNo = "01735152470";
//             int createdBy = 1;
//             DateTime createdDate = DateTime.Now;
//             bool isDeleted = false;

//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     User u = db.Users.First();

//                     if (u == null)
//                     {
//                         db.Users.FromSql($"Insert into Users(Username, Password, Email, MobileNo, CreatedBy, CreatedDate, IsDeleted) values({username},{password},{email},{mobileNo},{createdBy},{createdDate},{isDeleted})\r\n");
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine("Failed to insert user.");
//                 }
//             }
//         }

//         void PopulateProductCategories()
//         {
//             using (var db = new EcommerceContext())
//             {
//                 int pcCount = db.ProductCategories.Count();

//                 if (pcCount > 0)
//                 {
//                     Console.WriteLine("Product Categories already added are already added.");
//                     return;
//                 }
//             }

//             int userId = 1;

//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     List<ProductCategory> productCategories = db.ProductCategories.ToList();

//                     if (productCategories.Count == 0)
//                     {
//                         User u = db.Users.Where(u => u.Id == userId).FirstOrDefault();

//                         Console.WriteLine("No data found");
//                         string categoryDataString = "Electronics, Clothing, Home & Kitchen, Beauty & Personal Care, Health & Wellness, Toys & Games, Sports & Outdoors, Automotive, Office Supplies, Books, Music & Instruments, Pet Supplies, Grocery & Gourmet Food, Baby Products, Jewelry & Accessories, Shoes & Footwear, Garden & Outdoor, Tools & Home Improvement, Fitness & Exercise, Arts & Crafts, Video Games, Furniture, Watches, Industrial & Scientific, Travel Accessories, Smart Home Devices, Party Supplies, Cameras & Photography, Collectibles, Subscription Boxes";
//                         string[] catList = categoryDataString.Split(',');
//                         List<ProductCategory> categories = new List<ProductCategory>();

//                         foreach (string cat in catList)
//                         {
//                             categories.Add(new ProductCategory()
//                             {
//                                 CategoryName = cat,
//                                 IsDeleted = false,
//                                 CreatedBy = userId,
//                                 CreatedDate = DateTime.Now,
//                                 CreatedByUser = null
//                             });
//                         }

//                         try
//                         {
//                             db.ProductCategories.AddRange(categories);
//                             db.SaveChanges();
//                         }
//                         catch (Exception e)
//                         {
//                             Console.WriteLine("Failed to insert product categories");
//                         }
//                     }
//                     else
//                     {
//                         for (int i = 0; i < productCategories.Count; i++)
//                         {
//                             Console.WriteLine($"Cat ID: {i + 1} Product Category: {productCategories[i].CategoryName}");
//                         }
//                     }
//                 }
//                 catch (Exception ex)
//                 {
//                     Console.WriteLine("Failed to fetch product categories");
//                 }
//             }
//         }

//         void PopulateSellers()
//         {
//             using (var db = new EcommerceContext())
//             {
//                 int sellerCount = db.Sellers.Count();

//                 if (sellerCount > 0)
//                 {
//                     Console.WriteLine("Sellers are already added.");
//                     return;
//                 }
//             }

//             string sellerInfo = @"ABC Traders, 123 Main St, 9876543210, abc@example.com, 4.5 :  
//                             XYZ Enterprises, 456 Elm St, 8765432109, xyz@example.com, 4.2 :  
//                             Global Mart, 789 Oak St, 7654321098, global@example.com, 4.8 :  
//                             Sunrise Retail, 101 Maple Ave, 6543210987, sunrise@example.com, 4.3 :  
//                             Elite Supplies, 202 Pine Rd, 5432109876, elite@example.com, 4.7 :  
//                             Best Deals Inc., 303 Cedar Ln, 4321098765, bestdeals@example.com, 4.4 :  
//                             SuperMart, 404 Birch Blvd, 3210987654, supermart@example.com, 4.6 :  
//                             FastShop, 505 Spruce Dr, 2109876543, fastshop@example.com, 4.1 :  
//                             Prime Goods, 606 Redwood Ct, 1098765432, primegoods@example.com, 4.9 :  
//                             QuickBuy, 707 Aspen Way, 1987654321, quickbuy@example.com, 4.0 :  
//                             MegaMart, 808 Willow Rd, 2876543210, megamart@example.com, 4.2 :  
//                             ValueMart, 909 Chestnut Pl, 3765432109, valuemart@example.com, 4.5 :  
//                             Tech Haven, 111 Oakwood St, 4654321098, techhaven@example.com, 4.7 :  
//                             Urban Bazaar, 222 River Ave, 5543210987, urbanbazaar@example.com, 4.3 :  
//                             SmartChoice, 333 Hilltop Blvd, 6432109876, smartchoice@example.com, 4.6 :  
//                             ProDeals, 444 Sunset Dr, 7321098765, prodeals@example.com, 4.4 :  
//                             Trendy Hub, 555 Sunrise Ct, 8210987654, trendyhub@example.com, 4.2 :  
//                             EasyShop, 666 Lakeside Ln, 9109876543, easyshop@example.com, 4.5 :  
//                             FreshMart, 777 Mountain Rd, 1098765432, freshmart@example.com, 4.8 :  
//                             Gadget World, 888 Meadow Ave, 2187654321, gadgetworld@example.com, 4.7 :  
//                             Everyday Needs, 999 Brook St, 3276543210, everyday@example.com, 4.3 :  
//                             Smart Buys, 112 Greenway Ln, 4365432109, smartbuys@example.com, 4.6 :  
//                             Quick Cart, 223 Valley Rd, 5454321098, quickcart@example.com, 4.4 :  
//                             Direct Deals, 334 Summit Ave, 6543210987, directdeals@example.com, 4.2 :  
//                             Global Traders, 445 Highland Ct, 7632109876, globaltraders@example.com, 4.7 :  
//                             BestChoice, 556 Evergreen Dr, 8721098765, bestchoice@example.com, 4.1 :  
//                             Speedy Mart, 667 Maple Grove, 9810987654, speedymart@example.com, 4.5 :  
//                             Elite Emporium, 778 Sycamore St, 1909876543, eliteemporium@example.com, 4.6 :  
//                             Bargain Hub, 889 Redwood Ave, 2998765432, bargainhub@example.com, 4.3 :  
//                             Home Essentials, 990 Walnut Ln, 3087654321, homeessentials@example.com, 4.8  
//                             ";

//             List<Seller> sellers = new List<Seller>();
//             string[] seller_string = sellerInfo.Split(":");

//             foreach (string s1 in seller_string)
//             {
//                 string[] s = s1.Split(",");

//                 Seller seller = new Seller()
//                 {
//                     CompanyName = s[0].Trim(),
//                     Address = s[1].Trim(),
//                     MobileNo = s[2].Trim(),
//                     Email = s[3].Trim(),
//                     Rating = Convert.ToDecimal(s[4].Trim()),
//                     CreatedBy = 1,
//                     CreatedDate = DateTime.Now,
//                     IsDeleted = false
//                 };

//                 sellers.Add(seller);
//             }

//             try
//             {
//                 using (var db = new EcommerceContext())
//                 {
//                     db.Sellers.AddRange(sellers);
//                     db.SaveChanges();
//                 }
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine("Failed to insert sellers");
//             }
//         }

//         void PopulateProducts()
//         {
//             using (var db = new EcommerceContext())
//             {
//                 int productCount = db.Products.Count();

//                 if (productCount > 0)
//                 {
//                     Console.WriteLine("Products already entered");
//                     return;
//                 }
//             }

//             string product_string = @"Smartphone, 1, 4.5, $699, High-performance smartphone with AMOLED display, ABC Traders, Electronics :  
//                             Laptop, 1, 4.7, $1199, Powerful laptop with SSD storage and fast processor, XYZ Enterprises, Electronics :  
//                             Wireless Earbuds, 1, 4.3, $149, Noise-canceling wireless earbuds with long battery life, Global Mart, Electronics :  
//                             Gaming Console, 1, 4.6, $499, Next-gen gaming console with high-resolution graphics, Sunrise Retail, Electronics :  
//                             Smartwatch, 1, 4.4, $199, Fitness tracking smartwatch with heart rate monitor, Elite Supplies, Electronics :  
//                             Men’s T-Shirt, 1, 4.2, $19, Comfortable cotton t-shirt in multiple colors, Best Deals Inc., Clothing :  
//                             Women’s Jeans, 1, 4.5, $49, Stretchable high-waist jeans with a modern fit, SuperMart, Clothing :  
//                             Running Shoes, 1, 4.6, $89, Lightweight running shoes with breathable mesh, FastShop, Clothing :  
//                             Winter Jacket, 1, 4.7, $129, Insulated winter jacket for extreme cold, Prime Goods, Clothing :  
//                             Leather Wallet, 1, 4.3, $39, Genuine leather wallet with RFID protection, QuickBuy, Travel Accessories :  
//                             Microwave Oven, 1, 4.4, $299, High-power microwave with grill function, MegaMart, Home & Kitchen :  
//                             Air Fryer, 1, 4.6, $199, Oil-free cooking air fryer with multiple presets, ValueMart, Home & Kitchen :  
//                             Vacuum Cleaner, 1, 4.3, $159, High-suction vacuum cleaner with HEPA filter, Tech Haven, Home & Kitchen :  
//                             Blender, 1, 4.5, $79, Multi-speed blender with stainless steel blades, Urban Bazaar, Home & Kitchen :  
//                             Hair Dryer, 1, 4.2, $49, Fast-drying hair dryer with ionic technology, SmartChoice, Beauty & Personal Care :  
//                             Shampoo, 1, 4.3, $19, Herbal shampoo with natural extracts, ProDeals, Beauty & Personal Care :  
//                             Electric Toothbrush, 1, 4.5, $99, Rechargeable toothbrush with multiple modes, Trendy Hub, Beauty & Personal Care :  
//                             Face Cream, 1, 4.6, $29, Hydrating face cream with SPF protection, EasyShop, Beauty & Personal Care :  
//                             Dumbbells, 1, 4.7, $79, Adjustable dumbbells for strength training, FreshMart, Fitness & Exercise :  
//                             Yoga Mat, 1, 4.5, $39, Non-slip yoga mat with carrying strap, Gadget World, Fitness & Exercise :  
//                             Basketball, 1, 4.4, $29, Official size and weight basketball, Everyday Needs, Sports & Outdoors :  
//                             Camping Tent, 1, 4.6, $199, Waterproof tent for 4 people, Smart Buys, Sports & Outdoors :  
//                             Car Vacuum Cleaner, 1, 4.3, $59, Portable car vacuum cleaner with attachments, Quick Cart, Automotive :  
//                             LED Headlights, 1, 4.7, $99, Super bright LED headlights for cars, Direct Deals, Automotive :  
//                             Office Chair, 1, 4.5, $149, Ergonomic office chair with lumbar support, Global Traders, Office Supplies :  
//                             Writing Desk, 1, 4.4, $199, Wooden writing desk with storage drawers, BestChoice, Office Supplies :  
//                             Fiction Novel, 1, 4.6, $19, Best-selling fiction novel by top author, Speedy Mart, Books :  
//                             Cookbook, 1, 4.3, $29, Cookbook with 100+ delicious recipes, Elite Emporium, Books :  
//                             Acoustic Guitar, 1, 4.5, $199, Wooden acoustic guitar for beginners, Bargain Hub, Music & Instruments :  
//                             Keyboard Piano, 1, 4.6, $249, 61-key digital piano with built-in speakers, Home Essentials, Music & Instruments :  
//                             Dog Food, 1, 4.4, $39, Nutrient-rich dry dog food for all breeds, ABC Traders, Pet Supplies :  
//                             Cat Litter, 1, 4.5, $19, Odor-control cat litter with clumping formula, XYZ Enterprises, Pet Supplies :  
//                             Organic Coffee, 1, 4.6, $15, Freshly ground organic coffee beans, Global Mart, Grocery & Gourmet Food :  
//                             Protein Powder, 1, 4.7, $49, Whey protein powder for muscle recovery, Sunrise Retail, Grocery & Gourmet Food :  
//                             Baby Stroller, 1, 4.8, $299, Lightweight and foldable baby stroller, Elite Supplies, Baby Products :  
//                             Diapers, 1, 4.5, $39, Ultra-absorbent baby diapers, Best Deals Inc., Baby Products :  
//                             Silver Necklace, 1, 4.3, $99, Sterling silver necklace with pendant, SuperMart, Jewelry & Accessories :  
//                             Luxury Watch, 1, 4.7, $499, Elegant stainless steel luxury watch, FastShop, Watches :  
//                             Artificial Grass Mat, 1, 4.4, $79, Realistic artificial grass mat for outdoors, Prime Goods, Garden & Outdoor :  
//                             Power Drill, 1, 4.6, $129, Cordless power drill with multiple speeds, QuickBuy, Tools & Home Improvement :  
//                             Foam Roller, 1, 4.5, $29, High-density foam roller for muscle relief, MegaMart, Fitness & Exercise :  
//                             Watercolor Paint Set, 1, 4.3, $39, Professional watercolor paint set, ValueMart, Arts & Crafts :  
//                             Gaming Mouse, 1, 4.7, $79, High-precision gaming mouse with RGB lighting, Tech Haven, Video Games :  
//                             Smart Light Bulbs, 1, 4.5, $49, WiFi-enabled smart light bulbs, Urban Bazaar, Smart Home Devices :  
//                             Disposable Party Plates, 1, 4.3, $19, Biodegradable party plates, SmartChoice, Party Supplies :  
//                             DSLR Camera, 1, 4.7, $699, High-resolution DSLR camera, ProDeals, Cameras & Photography :  
//                             Vintage Collectible Coins, 1, 4.8, $199, Rare collectible coin set, Trendy Hub, Collectibles :  
//                             Wine Subscription Box, 1, 4.6, $79, Monthly curated wine selection, EasyShop, Subscription Boxes :  
//                             Electric Kettle, 1, 4.4, $39, Fast-boiling electric kettle, FreshMart, Home & Kitchen :  
//                             Bluetooth Speaker, 1, 4.5, $99, Waterproof portable Bluetooth speaker, Gadget World, Electronics :
//                             ";

//             string product_string_2 = @"Car Vacuum Cleaner, 1, 4.3, $59, Portable car vacuum cleaner with attachments, Quick Cart, Automotive :  
//                             LED Headlights, 1, 4.7, $99, Super bright LED headlights for cars, Direct Deals, Automotive :  
//                             Office Chair, 1, 4.5, $149, Ergonomic office chair with lumbar support, Global Traders, Office Supplies :  
//                             Writing Desk, 1, 4.4, $199, Wooden writing desk with storage drawers, BestChoice, Office Supplies :  
//                             Fiction Novel, 1, 4.6, $19, Best-selling fiction novel by top author, Speedy Mart, Books :  
//                             Cookbook, 1, 4.3, $29, Cookbook with 100+ delicious recipes, Elite Emporium, Books :  
//                             Acoustic Guitar, 1, 4.5, $199, Wooden acoustic guitar for beginners, Bargain Hub, Music & Instruments :  
//                             Keyboard Piano, 1, 4.6, $249, 61-key digital piano with built-in speakers, Home Essentials, Music & Instruments :  
//                             Dog Food, 1, 4.4, $39, Nutrient-rich dry dog food for all breeds, ABC Traders, Pet Supplies :  
//                             Cat Litter, 1, 4.5, $19, Odor-control cat litter with clumping formula, XYZ Enterprises, Pet Supplies :  
//                             Organic Coffee, 1, 4.6, $15, Freshly ground organic coffee beans, Global Mart, Grocery & Gourmet Food :  
//                             Protein Powder, 1, 4.7, $49, Whey protein powder for muscle recovery, Sunrise Retail, Grocery & Gourmet Food :  
//                             Baby Stroller, 1, 4.8, $299, Lightweight and foldable baby stroller, Elite Supplies, Baby Products :  
//                             Diapers, 1, 4.5, $39, Ultra-absorbent baby diapers, Best Deals Inc., Baby Products :  
//                             Silver Necklace, 1, 4.3, $99, Sterling silver necklace with pendant, SuperMart, Jewelry & Accessories :  
//                             Luxury Watch, 1, 4.7, $499, Elegant stainless steel luxury watch, FastShop, Watches :  
//                             Artificial Grass Mat, 1, 4.4, $79, Realistic artificial grass mat for outdoors, Prime Goods, Garden & Outdoor :  
//                             Power Drill, 1, 4.6, $129, Cordless power drill with multiple speeds, QuickBuy, Tools & Home Improvement :  
//                             Foam Roller, 1, 4.5, $29, High-density foam roller for muscle relief, MegaMart, Fitness & Exercise :  
//                             Watercolor Paint Set, 1, 4.3, $39, Professional watercolor paint set, ValueMart, Arts & Crafts :  
//                             Gaming Mouse, 1, 4.7, $79, High-precision gaming mouse with RGB lighting, Tech Haven, Video Games :  
//                             Smart Light Bulbs, 1, 4.5, $49, WiFi-enabled smart light bulbs, Urban Bazaar, Smart Home Devices :  
//                             Disposable Party Plates, 1, 4.3, $19, Biodegradable party plates, SmartChoice, Party Supplies :  
//                             DSLR Camera, 1, 4.7, $699, High-resolution DSLR camera, ProDeals, Cameras & Photography :  
//                             Vintage Collectible Coins, 1, 4.8, $199, Rare collectible coin set, Trendy Hub, Collectibles :  
//                             Wine Subscription Box, 1, 4.6, $79, Monthly curated wine selection, EasyShop, Subscription Boxes :  
//                             Electric Kettle, 1, 4.4, $39, Fast-boiling electric kettle, FreshMart, Home & Kitchen :  
//                             Bluetooth Speaker, 1, 4.5, $99, Waterproof portable Bluetooth speaker, Gadget World, Electronics :  
//                             Standing Desk, 1, 4.6, $249, Height-adjustable standing desk, Everyday Needs, Office Supplies :  
//                             Camping Stove, 1, 4.4, $89, Portable propane camping stove, Smart Buys, Sports & Outdoors :  
//                             Wireless Keyboard, 1, 4.5, $79, Ergonomic wireless keyboard with backlight, Quick Cart, Office Supplies :  
//                             Action Camera, 1, 4.7, $299, Waterproof action camera with 4K video, Direct Deals, Cameras & Photography :  
//                             Board Game, 1, 4.6, $49, Fun and engaging board game for families, Global Traders, Toys & Games :  
//                             Car Phone Mount, 1, 4.4, $29, Adjustable phone mount for cars, BestChoice, Automotive :  
//                             Resistance Bands, 1, 4.5, $25, Set of resistance bands for workouts, Speedy Mart, Fitness & Exercise :  
//                             Noise-Canceling Headphones, 1, 4.7, $299, Over-ear noise-canceling headphones, Elite Emporium, Electronics :  
//                             Tactical Flashlight, 1, 4.6, $49, High-lumen tactical flashlight, Bargain Hub, Tools & Home Improvement :  
//                             Smart Doorbell, 1, 4.5, $129, Video-enabled smart doorbell, Home Essentials, Smart Home Devices :  
//                             Fishing Rod, 1, 4.4, $89, Lightweight and durable fishing rod, ABC Traders, Sports & Outdoors :  
//                             Projector, 1, 4.6, $399, HD projector with WiFi support, XYZ Enterprises, Electronics :  
//                             Baking Set, 1, 4.5, $79, Complete baking set with utensils, Global Mart, Home & Kitchen :  
//                             Wireless Charging Pad, 1, 4.3, $49, Fast-charging wireless pad, Sunrise Retail, Electronics :  
//                             Robot Vacuum, 1, 4.7, $299, Self-cleaning robot vacuum with mapping, Elite Supplies, Home & Kitchen :  
//                             Smart Thermostat, 1, 4.5, $199, Energy-efficient smart thermostat, Best Deals Inc., Smart Home Devices :  
//                             Luggage Set, 1, 4.6, $249, 3-piece lightweight luggage set, SuperMart, Travel Accessories :  
//                             Garden Hose, 1, 4.4, $39, Expandable and durable garden hose, FastShop, Garden & Outdoor :  
//                             Mechanical Keyboard, 1, 4.7, $129, RGB backlit mechanical keyboard, Prime Goods, Electronics :  
//                             Travel Pillow, 1, 4.5, $29, Memory foam travel pillow, QuickBuy, Travel Accessories :  
//                             Electric Bike, 1, 4.8, $1299, High-speed electric bike with long battery life, MegaMart, Sports & Outdoors  
//                             ";

//             string[] productInfo = product_string.Split(":");
//             string[] productInfo2 = product_string_2.Split(":");

//             string[] sellerNameArray = new string[productInfo.Length];

//             List<Product> products = new List<Product>();
//             List<string> failedProducts = new List<string>();

//             #region Parse the string into Product lists
//             try
//             {
//                 foreach (string p1 in productInfo)
//                 {
//                     string[] p = p1.Split(",");

//                     if (p.Length == 7)
//                     {
//                         try
//                         {
//                             Product product = new Product()
//                             {
//                                 Name = p[0].Trim(),
//                                 DefaultQuantity = Convert.ToInt32(p[1].Trim()),
//                                 Rating = Convert.ToDecimal(p[2].Trim()),
//                                 Price = Convert.ToDecimal(p[3].Trim().Replace("$", "")),
//                                 Description = p[4].Trim(),
//                                 SellerCompanyName = p[5].Trim(),
//                                 ProductCategoryName = p[6].Trim(),
//                                 CreatedBy = 1,
//                                 CreatedDate = DateTime.Now,
//                                 IsDeleted = false
//                             };

//                             products.Add(product);
//                         }
//                         catch (Exception e)
//                         {
//                             Console.WriteLine(e.Message);
//                         }
//                     }
//                     else
//                     {
//                         failedProducts.Add(p1);
//                     }
//                 }
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine("Failed to parse product string #1.");
//             }

//             try
//             {
//                 foreach (string p1 in productInfo2)
//                 {
//                     string[] p = p1.Split(",");

//                     if (p.Length == 7)
//                     {
//                         try
//                         {
//                             Product product = new Product()
//                             {
//                                 Name = p[0].Trim(),
//                                 DefaultQuantity = Convert.ToInt32(p[1].Trim()),
//                                 Rating = Convert.ToDecimal(p[2].Trim()),
//                                 Price = Convert.ToDecimal(p[3].Trim().Replace("$", "")),
//                                 Description = p[4].Trim(),
//                                 SellerCompanyName = p[5].Trim(),
//                                 ProductCategoryName = p[6].Trim(),
//                                 CreatedBy = 1,
//                                 CreatedDate = DateTime.Now,
//                                 IsDeleted = false
//                             };

//                             products.Add(product);
//                         }
//                         catch (Exception e)
//                         {
//                             Console.WriteLine(e.Message);
//                         }
//                     }
//                     else
//                     {
//                         failedProducts.Add(p1);
//                     }
//                 }
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine("Failed to parse product string #2.");
//             }
//             #endregion

//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     foreach (Product p in products)
//                     {
//                         Seller seller = db.Sellers.Where(s => s.CompanyName.Trim() == p.SellerCompanyName.Trim()).Select(x => new Seller() { Id = x.Id }).FirstOrDefault();
//                         ProductCategory category = db.ProductCategories.Where(s => s.CategoryName.Trim() == p.ProductCategoryName.Trim()).Select(x => new ProductCategory() { Id = x.Id }).FirstOrDefault();

//                         p.SellerId = seller != null ? seller.Id : 0;
//                         p.ProductCategoryId = category != null ? category.Id : 0;
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine(e.Message);
//                 }
//             }

//             List<Product> failedProductList = products.Where(p => p.SellerId == 0 || p.ProductCategoryId == 0).ToList();

//             if (failedProductList.Count > 0)
//             {
//                 Console.WriteLine("Failed to Fetch for all products!");
//             }
//             else
//             {
//                 try
//                 {
//                     using (var db = new EcommerceContext())
//                     {
//                         db.Products.AddRange(products);
//                         db.SaveChanges();
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine($"{e.Message}");
//                 }
//             }
//         }

//         void PopulateUserRoles()
//         {
//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     int roleCount = db.Roles.Count();

//                     if (roleCount > 0)
//                     {
//                         Console.WriteLine("Roles have already been added");
//                         return;
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine("Failed to fetch roles");
//                     return;
//                 }

//                 List<Role> rolesToAdd = new List<Role>()
//         {
//             new Role()
//             {
//                 Name = UserRole.ADMIN,
//                 CreatedBy = 1,
//                 CreatedDate = DateTime.Now,
//                 IsDeleted = false,

//             },
//             new Role()
//             {
//                 Name = UserRole.CUSTOMER,
//                 CreatedBy = 1,
//                 CreatedDate = DateTime.Now,
//                 IsDeleted = false,

//             },
//             new Role()
//             {
//                 Name = UserRole.SELLER,
//                 CreatedBy = 1,
//                 CreatedDate = DateTime.Now,
//                 IsDeleted = false,

//             }
//         };

//                 try
//                 {
//                     db.Roles.AddRange(rolesToAdd);
//                     db.SaveChanges();
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine("Failed to add roles");
//                 }
//             }
//         }

//         void PopulateCustomers()
//         {
//             int customerRoleId = 0;

//             using (var db = new EcommerceContext())
//             {
//                 int customerCount = db.Users.Include(u => u.Role).Where(u => u.Role.Name == UserRole.CUSTOMER).Count();

//                 if (customerCount > 0)
//                 {
//                     Console.WriteLine("Customers already added");
//                 }

//                 customerRoleId = db.Roles.Where(r => r.Name == UserRole.CUSTOMER).ToList()[0].Id;
//             }

//             string user_data = @"    JohnDoe,password123,johndoe@example.com,12345678901 ;
//                            JaneSmith,securePass,jane.smith@example.com,12345678902  ;
//                              MikeBrown,passMike,mike.brown@example.com,12345678903  ;
//                              SarahDavis,sarahPass,sarah.davis@example.com,12345678904  ;
//                              ChrisWilson,chrisW123,chris.wilson@example.com,12345678905 ; 
//                              EmilyClark,emilyC456,emily.clark@example.com,12345678906  ;
//                              DavidWhite,daveWpass,david.white@example.com,12345678907 ; 
//                              LauraHall,lauraH789,laura.hall@example.com,12345678908  ;
//                              JamesMoore,jamesM101,james.moore@example.com,12345678909 ; 
//                           OliviaMartin,oliviaM202,olivia.martin@example.com,12345678910  ;
//                         DanielLee,danLee303,daniel.lee@example.com,12345678911  ;
//                        SophiaLopez,sophiaL404,sophia.lopez@example.com,12345678912 ; 
//                      BenjaminHarris,benH505,benjamin.harris@example.com,12345678913 ; 
//                         AvaYoung,avaY606,ava.young@example.com,12345678914  ;
//                           MatthewKing,mattK707,matthew.king@example.com,12345678915  ;
//                             IsabellaWright,isabellaW808,isabella.wright@example.com,12345678916 ; 
//                           HenryScott,henryS909,henry.scott@example.com,12345678917  ;
//                             MiaGreen,miaG101,mia.green@example.com,12345678918  ;
//                             JackAdams,jackA202,jack.adams@example.com,12345678919  ;
//                             EllaBaker,ellaB303,ella.baker@example.com,12345678920  ;
//                              AlexanderNelson,alexN404,alexander.nelson@example.com,12345678921  ;
//                              LilyCarter,lilyC505,lily.carter@example.com,12345678922  ;
//                             SamuelMitchell,samM606,samuel.mitchell@example.com,12345678923  ;
//                             GracePerez,graceP707,grace.perez@example.com,12345678924  ;
//                            LucasRoberts,lucasR808,lucas.roberts@example.com,12345678925  ;
//                            ChloeTurner,chloeT909,chloe.turner@example.com,12345678926  ;
//                          NathanPhillips,nathanP101,nathan.phillips@example.com,12345678927  ;
//                          ZoeCampbell,zoeC202,zoe.campbell@example.com,12345678928  ;
//                          DylanEvans,dylanE303,dylan.evans@example.com,12345678929  ;
//                         ScarlettMurphy,scarlettM404,scarlett.murphy@example.com,12345678930  ;";

//             string[] users_arr = user_data.Split(';');

//             List<User> users = new List<User>();

//             foreach (string s in users_arr)
//             {
//                 string[] user = s.Split(",");

//                 if (user.Length == 4)
//                 {
//                     users.Add(new User()
//                     {
//                         Username = user[0].Trim(),
//                         Password = user[1].Trim(),
//                         Email = user[2].Trim(),
//                         MobileNo = user[3].Trim(),
//                         RoleId = customerRoleId,

//                         CreatedBy = 1,
//                         CreatedDate = DateTime.Now,
//                         IsDeleted = false
//                     });
//                 }
//             }

//             try
//             {
//                 using (var db = new EcommerceContext())
//                 {
//                     db.Users.AddRange(users);
//                     db.SaveChanges();
//                 }
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine(e.Message);
//             }
//         }

//         int getMaxOrderCounterForDate(EcommerceContext db, DateTime currentDate)
//         {
//             int maxOrderCounter = db.Orders.Where(o => o.OrderDate.Date == currentDate.Date).Select(o => o.OrderCounter).Max();

//             return maxOrderCounter == null ? 0 : maxOrderCounter;
//         }

//         bool AddSingleOrder(Order newOrder)
//         {
//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     db.Orders.Add(newOrder);
//                     db.SaveChanges();

//                     return true;
//                 }
//                 catch (Exception e)
//                 {
//                     return false;
//                 }
//             }
//         }

//         bool RemoveSingleOrder(Order order)
//         {
//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     db.Orders.Remove(order);
//                     db.SaveChanges();

//                     return true;
//                 }
//                 catch (Exception e)
//                 {
//                     return false;
//                 }
//             }
//         }

//         bool AddOrderItemsForOrder(Order order, List<OrderItem> orderItems)
//         {
//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     db.OrderItems.AddRange(orderItems);
//                     db.SaveChanges();
//                     return true;
//                 }
//                 catch (Exception e)
//                 {
//                     RemoveSingleOrder(order);
//                     return false;
//                 }
//             }
//         }

//         Product GetProductById(int productId)
//         {
//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     return db.Products.Where(p => p.Id == productId).First();
//                 }
//                 catch (Exception e)
//                 {
//                     return null;
//                 }
//             }
//         }

//         void AddOrders(DateTime date)
//         {
//             int maxCustomerId = 0; int maxProductId = 0; int maxOrderId = 0; int maxOrderCounter = 0;
//             List<Order> orders = new List<Order>(); User u;

//             using (var db = new EcommerceContext())
//             {
//                 maxCustomerId = db.Users.Include(u => u.Role).Where(u => u.Role.Name == UserRole.CUSTOMER).Select(c => c.Id).Max();
//                 maxProductId = db.Products.Select(p => p.Id).Max();
//                 maxOrderId = db.Orders.Count();

//                 orders = db.Orders.ToList();

//                 try
//                 {
//                     maxOrderCounter = orders.Count() == 0 ? 1 : getMaxOrderCounterForDate(db, date);
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine("Failed to fetch Order Count");
//                     Console.WriteLine(e.Message);
//                     return;
//                 }

//                 u = new User();

//                 while (u.Id == 0 || u == null)
//                 {
//                     try
//                     {
//                         int userId = new Random().Next(2, maxCustomerId);
//                         User u1 = db.Users.Where(u => u.Id == userId).First();

//                         if (u1 != null)
//                         {
//                             u = u1;
//                         }
//                     }
//                     catch
//                     {
//                         Console.WriteLine("Failed to add a user");
//                     }
//                 }

//                 maxOrderCounter++;
//             }

//             Order newOrder = new Order()
//             {
//                 OrderDate = date,
//                 OrderCounter = maxOrderCounter,
//                 UserId = u.Id,
//                 Status = OrderStatus.COMPLETED,
//                 NetAmount = 0,
//                 CreatedBy = 1,
//                 CreatedDate = date,
//                 IsDeleted = false
//             };

//             bool orderAdded;

//             try
//             {
//                 orderAdded = AddSingleOrder(newOrder);
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine("Failed to add Order");
//                 Console.WriteLine(e.ToString());
//                 return;
//             }

//             if (orderAdded)
//             {
//                 List<OrderItem> orderItems = new List<OrderItem>();

//                 int maxProductQuantity = new Random().Next(1, 10);

//                 try
//                 {
//                     for (int i = 0; i < maxProductQuantity; i++)
//                     {
//                         Product p = new Product();

//                         while (p == null || p.Id == 0)
//                         {
//                             int productId = new Random().Next(1, maxProductId);

//                             try
//                             {
//                                 p = GetProductById(productId);
//                             }
//                             catch (Exception e)
//                             {
//                                 p = new Product();
//                                 Console.WriteLine("No product found with ID: " + productId);
//                             }
//                         }

//                         int quantity = new Random().Next(1, 50);

//                         OrderItem item = new OrderItem()
//                         {
//                             OrderId = newOrder.Id,
//                             ProductId = p.Id,
//                             Quantity = quantity,
//                             GrossAmount = p.Price * quantity,
//                             Status = OrderItemStatus.AVAILABLE,
//                             CreatedBy = 1,
//                             CreatedDate = date,
//                             IsDeleted = false
//                         };

//                         orderItems.Add(item);
//                     }

//                     try
//                     {
//                         AddOrderItemsForOrder(newOrder, orderItems);
//                     }
//                     catch (Exception e)
//                     {
//                         Console.WriteLine(e.ToString());
//                     }
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine("Failed to add a product");
//                 }
//             }
//         }

//         void UpdateNetAmountOfOrders()
//         {
//             List<Order> orders = new List<Order>();
//             using (var db = new EcommerceContext())
//             {
//                 orders = db.Orders.ToList();
//             }

//             if (orders.Count() > 0)
//             {
//                 using (var db = new EcommerceContext())
//                 {
//                     foreach (Order order in orders)
//                     {
//                         order.Items.Clear();
//                         order.Items = db.OrderItems.Include(i => i.Product).Where(i => i.OrderId == order.Id).ToList();

//                         foreach (OrderItem i in order.Items)
//                         {
//                             order.NetAmount += i.Quantity * i.Product.Price;
//                         }
//                     }
//                 }
//             }

//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     db.Orders.UpdateRange(orders);
//                     db.SaveChanges();
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine("Failed to update order amount");
//                 }
//             }
//         }

//         Order GetOrderInformation()
//         {
//             int orderId = 11;
//             List<OrderItem> allItems = new List<OrderItem>();
//             List<OrderItemDto> dtoItems = new List<OrderItemDto>();
//             OrderInformationDto orderInfo = new OrderInformationDto();

//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     /*This option has a chance of having cyclic errors*/
//                     ////Full Query
//                     //allItems = db.OrderItems
//                     //    .Where(i => i.OrderId == orderId)
//                     //    .Include(i => i.Product).ThenInclude(p => p.ProductCategory)
//                     //    .Include(i => i.Order).ThenInclude(o => o.User)
//                     //    .ToList();

//                     /*This does not*/
//                     //Using DTOs
//                     dtoItems = db.OrderItems.Where(i => i.OrderId == orderId).Include(i => i.Product).ThenInclude(p => p.ProductCategory).OrderBy(i => i.Product.Name)
//                         .Select(i => _mapper.MapToOrderItemDto(i))
//                         .AsNoTracking().ToList();

//                     orderInfo = db.Orders.Where(o => o.Id == orderId).Include(o => o.User)
//                         .Select(o => _mapper.MapToOrderInformationDto(dtoItems, o))
//                         .AsNoTracking().First();

//                     var json = JsonSerializer.Serialize(orderInfo);
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine($"{e.Message}");
//                 }
//             }

//             return new Order();
//         }

//         List<Order> GetAllPaginatedOrders(int pageNo, float pageSize, int lastEntryId = 3158)
//         {
//             float totalOrders = 0; float totalPages = 0; List<OrderInformationDto> orders = new List<OrderInformationDto>();

//             using (var db = new EcommerceContext())
//             {
//                 totalOrders = db.Orders.Count();
//                 totalPages = totalOrders / pageSize;

//                 totalPages = totalPages % 1 == 0 ? totalPages : (int)totalPages + 1;

//                 ////Method #1 Fetch/Offset or Skip/Take
//                 //DateTime startTime = DateTime.Now;

//                 //orders = db.Orders.Include(o => o.User)
//                 //    .OrderBy(o => o.OrderDate)
//                 //    .OrderBy(o => o.OrderCounter)
//                 //    .Skip(((int)pageNo - 1) * (int)pageSize)
//                 //    .Take((int)pageSize)
//                 //    .Select(o => _mapper.MapToOrderInformationDto(null, o))
//                 //    .ToList();

//                 //DateTime endTime = DateTime.Now;
//                 //TimeSpan ts = endTime - startTime;

//                 //Method #2 KeySet
//                 DateTime startTime = DateTime.Now;

//                 orders = db.Orders.Include(o => o.User)
//                     .OrderByDescending(o => o.OrderDate)
//                     .OrderByDescending(o => o.OrderCounter)
//                     .Where(o => o.Id > lastEntryId)
//                     .Take((int)pageSize)
//                     .Select(o => _mapper.MapToOrderInformationDto(null, o))
//                     .ToList();

//                 DateTime endTime = DateTime.Now;
//                 TimeSpan ts = endTime - startTime;

//                 Console.WriteLine($"Max ID of this page is {orders[orders.Count - 1].OrderId}");
//                 Console.WriteLine($"Method #1 takes {ts.TotalMilliseconds} seconds to complete.");
//             }

//             return new List<Order>();
//         }

//         async Task UpdateOrderDatesAndCounters()
//         {
//             List<DateTime> distinctDates = new List<DateTime>();

//             //Fetch the list of orders
//             List<Order> orderList = new List<Order>();

//             using (var db = new EcommerceContext())
//             {
//                 orderList = await db.Orders.ToListAsync();

//                 if (orderList.Count() == 0)
//                 {
//                     Console.WriteLine("No orders were found.");
//                     return;
//                 }

//                 distinctDates = orderList.Select(o => o.OrderDate.Date).Distinct().ToList();

//                 if (distinctDates.Count != 1)
//                 {
//                     Console.WriteLine("Orders have been divided into dates");
//                     return;
//                 }

//                 //Generate a list of random dates
//                 List<DateTime> dates = new List<DateTime>();

//                 DateTime startDate = Convert.ToDateTime("2025-02-01");
//                 dates = Enumerable.Range(1, 28).Select(o => startDate.AddDays(o)).ToList();

//                 Random r = new Random(); DateTime currDate; int orderCounter;

//                 foreach (Order o in orderList)
//                 {
//                     currDate = dates[r.Next(0, dates.Count - 1)];
//                     orderCounter = orderList.Where(o => o.OrderDate.Date == currDate.Date).Count() + 1;

//                     o.OrderDate = currDate;
//                     o.OrderCounter = orderCounter;
//                 }

//                 using (var trx = await db.Database.BeginTransactionAsync())
//                 {
//                     try
//                     {
//                         db.Orders.UpdateRange(orderList);

//                         await db.SaveChangesAsync();

//                         await trx.CommitAsync();

//                         Console.WriteLine("Updated all orders");
//                     }
//                     catch (Exception e)
//                     {
//                         await trx.RollbackAsync();

//                         Console.WriteLine("Failed to update Order dates.");
//                     }
//                 }
//             }
//         }

//         void LoadCustomerOrderExpenseReport()
//         {
//             using (var db = new EcommerceContext())
//             {
//                 DateTime startDate = Convert.ToDateTime("2025-02-01");
//                 DateTime endDate = Convert.ToDateTime("2025-02-28");

//                 List<CustomerTotalExpenseReport> result = db.Orders.Include(o => o.User)
//                     .Where(o => o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date)
//                     .GroupBy(x => new { x.User.Id, x.User.Username, x.User.MobileNo, x.User.Email })
//                     .Select(y => new CustomerTotalExpenseReport()
//                     {
//                         UserId = y.Key.Id,
//                         Username = y.Key.Username,
//                         MobileNo = y.Key.MobileNo,
//                         Email = y.Key.Email,
//                         TotalExpense = y.Sum(y => y.NetAmount)
//                     })
//                     .ToList();

//                 foreach (var record in result)
//                 {
//                     Console.WriteLine($"Username: {record.Username}, Total Expense: ${Math.Round(record.TotalExpense, 2).ToString("#,##0.00")}");
//                 }
//             }
//         }

//         void GroupJoiningLinqQuery()
//         {
//             //string demoSql = @" select 
//             //                     o.UserId,u.Username, u.MobileNo, u.Email, oi.TotalItemsPurchased, sum(o.NetAmount) TotalExpense
//             //                    from Orders o
//             //                     inner join Users u on u.Id = o.UserId
//             //                     inner join (
//             //                      select
//             //                       o.UserId, count(*) TotalItemsPurchased
//             //                      from OrderItems oi
//             //                       inner join Orders o on o.Id = oi.OrderId
//             //                      group by o.UserId
//             //                     ) oi on oi.UserId = o.UserId
//             //                    where o.OrderDate between '20250201' and '20250301'
//             //                    group by o.UserId,u.Username, u.MobileNo, u.Email, oi.TotalItemsPurchased
//             //                    order by o.UserId";

//             using (var db = new EcommerceContext())
//             {
//                 DateTime startDate = Convert.ToDateTime("2025-02-01");
//                 DateTime endDate = Convert.ToDateTime("2025-02-28");

//                 //Own
//                 List<CustomerTotalExpenseReport> result = db.Orders.AsNoTracking().Include(o => o.User)
//                     .Where(o => o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date)
//                     .GroupBy(x => new { x.User.Id, x.User.Username, x.User.MobileNo, x.User.Email })
//                     .Select(y => new
//                     {
//                         y.Key.Id,
//                         y.Key.Username,
//                         y.Key.MobileNo,
//                         y.Key.Email,
//                         TotalExpense = y.Sum(y => y.NetAmount)
//                     })
//                     .GroupJoin(
//                         db.OrderItems.AsNoTracking().Include(oi => oi.Order).GroupBy(oi => oi.Order.UserId)
//                         .Select(x => new { x.Key, TotalItems = x.Count() }),
//                         o => o.Id,
//                         x => x.Key,
//                         (o, x) => new { o, x = x.First() }
//                     )
//                     .Select(a => new CustomerTotalExpenseReport()
//                     {
//                         UserId = a.o.Id,
//                         Username = a.o.Username,
//                         MobileNo = a.o.MobileNo,
//                         Email = a.o.Email,
//                         TotalExpense = a.o.TotalExpense,
//                         TotalItemsPurchased = a.x.TotalItems
//                     })
//                     .OrderBy(a => a.UserId)
//                     .ToList();

//                 //GPT Optimized
//                 var orderItemsGroupedByUserId = db.OrderItems.AsNoTracking()
//                     .Include(oi => oi.Order)
//                     .GroupBy(oi => oi.Order.UserId)
//                     .Select(x => new { x.Key, TotalItems = x.Count() });

//                 List<CustomerTotalExpenseReport> result2 = db.Orders.AsNoTracking().Include(o => o.User)
//                     .Where(o => o.OrderDate.Date >= startDate.Date && o.OrderDate.Date <= endDate.Date)
//                     .GroupBy(x => new { x.User.Id, x.User.Username, x.User.MobileNo, x.User.Email })
//                     .Select(y => new
//                     {
//                         y.Key.Id,
//                         y.Key.Username,
//                         y.Key.MobileNo,
//                         y.Key.Email,
//                         TotalExpense = y.Sum(y => y.NetAmount)
//                     })
//                     .GroupJoin(
//                         orderItemsGroupedByUserId,
//                         o => o.Id,
//                         x => x.Key,
//                         (o, x) => new { o, x = x.First() }
//                     )
//                     .Select(a => new CustomerTotalExpenseReport()
//                     {
//                         UserId = a.o.Id,
//                         Username = a.o.Username,
//                         MobileNo = a.o.MobileNo,
//                         Email = a.o.Email,
//                         TotalExpense = a.o.TotalExpense,
//                         TotalItemsPurchased = a.x.TotalItems
//                     })
//                     .OrderBy(a => a.UserId)
//                     .ToList();

//                 foreach (var record in result2)
//                 {
//                     Console.WriteLine($"Username: {record.Username}, Total Items Purchased: {record.TotalItemsPurchased}, Total Expense: {Math.Round(record.TotalExpense, 2).ToString("#,##0.00")}");
//                 }
//             }
//         }

//         void AddCustomerTransaction(Random r)
//         {
//             List<CustomerTransaction> transactions = new List<CustomerTransaction>();

//             using (var db = new EcommerceContext())
//             {
//                 if (db.CustomerTransactions.Any())
//                 {
//                     Console.WriteLine("Transactions have already been added.");
//                     return;
//                 }

//                 var orders = db.Orders.GroupBy(o => new { o.OrderDate.Date, o.UserId }).Select(x => new { OrderDate = x.Key.Date, x.Key.UserId, TotalExpense = x.Sum(x => x.NetAmount) }).ToList();

//                 ////Own version
//                 //foreach (var order in orders)
//                 //{
//                 //    CustomerTransaction t = new CustomerTransaction()
//                 //    {
//                 //        UserId = order.UserId,
//                 //        TransactionDate = order.OrderDate,
//                 //        TransactionType = TransactionType.DEPOSIT,
//                 //        Amount = (decimal)order.TotalExpense * (decimal)(1 + r.NextDouble()),
//                 //        CreatedBy = 1,
//                 //        CreatedDate = order.OrderDate,
//                 //        IsDeleted = false,
//                 //    };

//                 //    CustomerTransaction p = new CustomerTransaction()
//                 //    {
//                 //        UserId = order.UserId,
//                 //        TransactionDate = order.OrderDate,
//                 //        TransactionType = TransactionType.PURCHASE,
//                 //        Amount = (decimal)order.TotalExpense,
//                 //        CreatedBy = 1,
//                 //        CreatedDate = order.OrderDate,
//                 //        IsDeleted = false,
//                 //    };

//                 //    transactions.Add(t);
//                 //    transactions.Add(p);
//                 //}

//                 //GPT Optimized
//                 transactions = orders.SelectMany(order => new[]
//                 {
//             new CustomerTransaction()
//             {
//                 UserId = order.UserId,
//                 TransactionDate = order.OrderDate,
//                 TransactionType = TransactionType.DEPOSIT,
//                 Amount = order.TotalExpense * (decimal)(1 + r.NextDouble()),
//                 CreatedBy = 1,
//                 CreatedDate = order.OrderDate,
//                 IsDeleted = false,
//             },

//             new CustomerTransaction()
//             {
//                 UserId = order.UserId,
//                 TransactionDate = order.OrderDate,
//                 TransactionType = TransactionType.PURCHASE,
//                 Amount = order.TotalExpense,
//                 CreatedBy = 1,
//                 CreatedDate = order.OrderDate,
//                 IsDeleted = false,
//             }
//         }).ToList();

//                 try
//                 {
//                     db.CustomerTransactions.AddRange(transactions);
//                     db.SaveChanges();
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine($"Failed to add transactions");
//                     Console.WriteLine($"{e.Message}");
//                 }
//             }
//         }

//         public async Task<int> ProcessDailyTransactions()
//         {
//             using (var db = new EcommerceContext())
//             {
//                 try
//                 {
//                     await db.Database.ExecuteSqlAsync($@"
//                 truncate table AccountSummaryHistories

//                 Insert into AccountSummaryHistories(TransactionDate, UserId, CashAmount, TotalDepositAmount, TotalWithdrawAmount, TotalPurchaseAmount, CreatedBy, CreatedDate, IsDeleted)
//                 select 
// 		                Convert(date,ct.TransactionDate) TransactionDate, ct.UserId,
// 		                sum(case when ct.TransactionType = 'deposit' then ct.Amount else 0 end) - sum(case when ct.TransactionType = 'purchase' then ct.Amount else 0 end) CashAmount,
// 		                sum(case when ct.TransactionType = 'deposit' then ct.Amount else 0 end) TotalDeposit,
// 		                sum(case when ct.TransactionType = 'purchase' then ct.Amount else 0 end) TotalPurchase,
// 		                0 TotalWithdrawAmount,
// 		                1, GETDATE(), 0
//                 from CustomerTransactions ct
//                 group by TransactionDate, UserId");

//                     return 1;
//                 }
//                 catch (Exception e)
//                 {
//                     Console.WriteLine($"{e.Message}");
//                     return -1;
//                 }
//             }
//         }
//     }
// }