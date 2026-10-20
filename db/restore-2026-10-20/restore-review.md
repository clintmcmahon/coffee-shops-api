# Restore review: shops to un-delete

Generated 2026-10-20 from the January snapshot (`data/coffeeshops.mn.json` in the mobile repo) compared with the live API.
Run each group's SQL only after reviewing its list. Every file only touches rows that are currently deleted, and `undo.sql` reverses all of them.

## Rules

- Excluded: chains (harvester list plus regional franchises), not operational, rating below 4.3, fewer than 15 reviews, places that aren't coffee-led (diners, restaurants, bakeries), and a short hand-read list.
- Metro: the January cut was deliberate, so a metro shop only comes back if it answers a question people search for (open late, fireplace, roaster, owner identity, specialty coffee, dogs) at 4.5+ with 50+ reviews.
- Greater Minnesota: 4.6+ with 50+ reviews. Regional hubs get up to 5, every other town gets its single best shop.

## Group 1: Twin Cities metro: shops that answer a searched-for question (33)

| Shop | Town | Rating | Why |
|---|---|---|---|
| Qamaria Yemeni Coffee Co. | Blaine | 4.6 (384) | open late, specialty / cultural coffee |
| Storyteller Cafe | Blaine | 4.9 (81) | live music |
| West Side Perk | Bloomington | 4.6 (237) | opens by 6am, fireplace, women-owned |
| CupBoard Coffee Boutique | Bloomington | 5 (58) | dogs inside |
| Arise Nicolette Coffee Company | Burnsville | 4.5 (239) | fireplace |
| The Buzz Coffee & Cafe | Burnsville | 4.5 (873) | women-owned |
| Dero Coffee Shop | Columbia Heights | 4.8 (127) | open late, specialty / cultural coffee |
| Curiouser Coffee and Elixirs | Eagan | 4.7 (66) | open late, women-owned, Latino-owned |
| Qamaria Yemeni Coffee Co. | Eden Prairie | 4.6 (488) | open late, specialty / cultural coffee |
| Depot Coffee House / The Freight Room | Hopkins | 4.5 (489) | live music |
| Sunshine Coffee Co. | Lake Elmo | 4.9 (149) | specialty / cultural coffee |
| Daily Dose Cafe and Espresso | Maple Grove | 4.6 (564) | opens by 6am |
| Lake and Bryant Cafe | Minneapolis | 4.8 (445) | fireplace, women-owned |
| Sisters' Sludge Coffee Cafe and Wine Bar | Minneapolis | 4.6 (558) | open late, opens by 6am, fireplace |
| Wesley Andrews Coffee & Tea | Minneapolis | 4.6 (543) | roaster, specialty / cultural coffee |
| Corner Coffee Camden | Minneapolis | 4.7 (311) | women-owned |
| Gray Fox Coffee & Wine | Minneapolis | 4.6 (279) | live music, specialty / cultural coffee |
| Café Marguerite | Minneapolis | 4.6 (203) | live music, women-owned |
| Corner Coffee | Minneapolis | 4.6 (379) | live music |
| Muhim’s Café | Minneapolis | 4.6 (116) | open late, specialty / cultural coffee |
| Yummy Bites | Minneapolis | 4.8 (148) | open late |
| Corner Coffee Uptown | Minneapolis | 4.6 (333) | specialty / cultural coffee |
| Rick's Coffee Bar | Minneapolis | 4.7 (151) | opens by 6am |
| Bru House Coffee Shop & Drive Thru | New Brighton | 4.6 (493) | women-owned |
| The Cafe Meow | New Hope | 4.5 (366) | women-owned |
| Mudslingers Drive-Thru Coffee | Roseville | 4.9 (665) | opens by 6am |
| Little Brazil MN Market & Cafe | St Paul | 4.8 (332) | fireplace, Latino-owned |
| Cafe Latte | St Paul | 4.6 (5090) | open late, live music |
| Roundtable Coffee Works | St Paul | 4.9 (180) | roaster |
| The Daily Grind Espresso Cafe | Stillwater | 4.6 (419) | opens by 6am |
| J Tea & Bake (White Bear Lake) | White Bear Lake | 4.8 (71) | Asian-owned, specialty / cultural coffee |
| The Anchor Coffee House | White Bear Lake | 4.6 (258) | live music, women-owned |
| Cajeta Coffee | White Bear Lake | 5 (150) | specialty / cultural coffee |

## Group 2: Greater Minnesota regional hubs (Duluth, Rochester, St Cloud and others) (44)

| Shop | Town | Rating | Why |
|---|---|---|---|
| Common Ground Coffee House | Alexandria | 4.7 (239) | Greater MN gap (0 live in Alexandria), opens by 6am, live music |
| Deja Bleu House Of Coffee | Alexandria | 4.6 (105) | Greater MN gap (0 live in Alexandria) |
| Hot Off the Press Comics and Coffee | Austin | 4.9 (67) | Greater MN gap (0 live in Austin), open late |
| Coffee House On Main | Austin | 4.7 (190) | Greater MN gap (0 live in Austin) |
| Wild Hare Bistro and Coffee House | Bemidji | 4.7 (209) | Greater MN gap (1 live in Bemidji) |
| The Coffee District | Bemidji | 4.6 (153) | Greater MN gap (1 live in Bemidji), fireplace |
| Coco Moon Coffee Bar | Brainerd | 4.8 (432) | Greater MN gap (0 live in Brainerd), live music |
| Loco Espress Coffee House & Boutique | Brainerd | 4.8 (420) | Greater MN gap (0 live in Brainerd) |
| Wussow's Concert Cafe | Duluth | 4.7 (804) | Greater MN, open late, live music |
| Studio Café | Duluth | 4.9 (149) | Greater MN, open late, live music, dogs inside, specialty / cultural coffee |
| Yellow Bike Coffee Fitgers | Duluth | 4.6 (76) | Greater MN |
| Northern Grounds | Ely | 4.6 (475) | Greater MN gap (0 live in Ely), roaster, live music, women-owned |
| Good Day Coffee | Faribault | 4.8 (74) | Greater MN gap (2 live in Faribault) |
| Perks Coffee & Tea (Downtown Washington) | Fergus Falls | 4.7 (137) | Greater MN gap (1 live in Fergus Falls), opens by 6am |
| Perks Coffee & Tea (Fir Location) | Fergus Falls | 4.9 (69) | Greater MN gap (1 live in Fergus Falls) |
| Cally's Cafe & Coffee | Fergus Falls | 4.7 (177) | Greater MN gap (1 live in Fergus Falls) |
| Java Moose Espresso Cafe | Grand Marais | 4.7 (1147) | Greater MN gap (0 live in Grand Marais) |
| Paddle On Coffee & Maker Emporium | Lanesboro | 4.6 (59) | Greater MN gap (0 live in Lanesboro), fireplace, live music, veteran-owned, women-owned |
| Coffee Hag | Mankato | 4.8 (940) | Greater MN gap (0 live in Mankato), live music |
| River Rock Coffee & Tea | Mankato | 4.6 (182) | Greater MN gap (0 live in Mankato) |
| Twenty Below Coffee Co. Moorhead | Moorhead | 4.7 (259) | Greater MN gap (0 live in Moorhead), opens by 6am |
| The Coffee Keller | New Ulm | 4.8 (62) | Greater MN gap (0 live in New Ulm) |
| Sippet Coffee & Bagels | New Ulm | 4.6 (121) | Greater MN gap (0 live in New Ulm) |
| James Gang Coffeehouse & Eatery | Northfield | 4.6 (340) | Greater MN gap (2 live in Northfield), opens by 6am, fireplace, veteran-owned, women-owned |
| The HideAway Vault | Northfield | 4.6 (57) | Greater MN gap (2 live in Northfield), fireplace, live music, women-owned |
| Goodbye Blue Monday Coffeehouse | Northfield | 4.6 (375) | Greater MN gap (2 live in Northfield) |
| Cottage Tearoom & Fare | Northfield | 4.7 (80) | Greater MN gap (2 live in Northfield) |
| The HideAway Coffeehouse and Wine Bar | Northfield | 4.6 (373) | Greater MN gap (2 live in Northfield) |
| Chill-aqui | Owatonna | 4.9 (110) | Greater MN gap (0 live in Owatonna), specialty / cultural coffee |
| Central Park Coffee Co. | Owatonna | 4.7 (204) | Greater MN gap (0 live in Owatonna) |
| Espresso 34 | Park Rapids | 4.6 (223) | Greater MN gap (0 live in Park Rapids) |
| Emmaville Crossing (Inn, Store, Cafe' and Bar) | Park Rapids | 4.6 (285) | Greater MN gap (0 live in Park Rapids) |
| Mandy's Coffee & Cafe | Red Wing | 4.8 (321) | Greater MN gap (0 live in Red Wing), women-owned |
| Café Steam | Rochester | 4.6 (1151) | Greater MN, open late, live music, women-owned |
| Café Aquí | Rochester | 4.7 (270) | Greater MN, live music, women-owned |
| St. James Coffee | Rochester | 4.8 (174) | Greater MN, fireplace, live music |
| Bravo Espresso & Gourmet | Rochester | 4.8 (100) | Greater MN, opens by 6am |
| Lumen Coffee | Rochester | 4.7 (193) | Greater MN |
| Korppi Coffee + Bakeshop | St Cloud | 4.8 (314) | Greater MN, women-owned |
| District Coffee | St Cloud | 4.9 (165) | Greater MN |
| Lu Lu Beans | Willmar | 4.8 (654) | Greater MN gap (0 live in Willmar) |
| Blooming Grounds Coffee House | Winona | 4.6 (829) | Greater MN, open late, fireplace, live music |
| Blue Heron Coffeehouse | Winona | 4.7 (479) | Greater MN |
| Mugby Coffee Co. | Winona | 4.7 (179) | Greater MN, opens by 6am |

## Group 3: Greater Minnesota: the best shop in each smaller town (84)

| Shop | Town | Rating | Why |
|---|---|---|---|
| Gregorys Coffee | Albertville | 4.7 (71) | Greater MN gap (0 live in Albertville) |
| In Hot Water Coffee Shop | Annandale | 4.8 (201) | Greater MN gap (0 live in Annandale), women-owned |
| The Hive Coffee And Bakeshop | Aurora | 4.7 (216) | Greater MN gap (0 live in Aurora) |
| Gathering Grounds Coffee Shop | Avon | 4.8 (98) | Greater MN gap (1 live in Avon) |
| Rother's Just Desserts/Prelude Coffee | Bagley | 4.7 (76) | Greater MN gap (2 live in Bagley), women-owned |
| The Chickadee Coffeehouse & Deli | Barnum | 4.7 (392) | Greater MN gap (0 live in Barnum), women-owned |
| New Moon Coffeehouse and Eatery | Baudette | 4.9 (84) | Greater MN gap (0 live in Baudette), women-owned |
| Dueling Brews | Becker | 4.6 (313) | Greater MN gap (0 live in Becker), open late, opens by 6am, live music |
| The Wired Rooster Coffee Shop | Caledonia | 4.8 (223) | Greater MN gap (0 live in Caledonia), opens by 6am |
| CannonBelles Coffee & Ice Cream | Cannon Falls | 4.7 (383) | Greater MN gap (0 live in Cannon Falls), opens by 6am, fireplace, veteran-owned, women-owned |
| Getaway Motor Cafe | Carver | 4.8 (251) | Greater MN gap (0 live in Carver), fireplace |
| Jones Coffee - Chaska | Chaska | 4.8 (101) | Greater MN gap (0 live in Chaska), specialty / cultural coffee |
| Gordy's Warming House | Cloquet | 4.7 (878) | Greater MN gap (0 live in Cloquet), fireplace, live music |
| Cold Press Cafe | Cold Spring | 4.8 (175) | Greater MN gap (0 live in Cold Spring), opens by 6am |
| Logan Pass Coffeehouse | Cologne | 5 (130) | Greater MN gap (0 live in Cologne), opens by 6am |
| Crosslake Coffee | Crosslake | 4.7 (180) | Greater MN gap (0 live in Crosslake), women-owned |
| Roasted Rail Coffee House LLC | Dilworth | 4.6 (146) | Greater MN gap (0 live in Dilworth), opens by 6am |
| Tally Ho Koffie | Edgerton | 4.7 (96) | Greater MN gap (0 live in Edgerton) |
| Jazz Shepherd Coffee | Elgin | 4.9 (51) | Greater MN gap (0 live in Elgin) |
| Beans and Boots Coffee | Farmington | 4.8 (79) | Greater MN gap (1 live in Farmington), opens by 6am |
| Morning Glory Florist, LLC | Fertile | 5 (50) | Greater MN gap (0 live in Fertile), veteran-owned, women-owned |
| The Monarch Coffee House | Frazee | 4.7 (52) | Greater MN gap (0 live in Frazee) |
| Rhema Coffee | Glencoe | 4.9 (84) | Greater MN gap (0 live in Glencoe), dogs inside, women-owned |
| The Goodhue Depot | Goodhue | 4.9 (51) | Greater MN gap (0 live in Goodhue) |
| Fill Thy Cup | Hawley | 4.6 (107) | Greater MN gap (0 live in Hawley), opens by 6am, fireplace |
| Casa Latte LLC | Hermantown | 4.7 (209) | Greater MN gap (0 live in Hermantown) |
| Cozy Cove | Hill City | 4.6 (58) | Greater MN gap (0 live in Hill City) |
| Barista's Coffee House | Houston | 4.7 (188) | Greater MN gap (1 live in Houston), opens by 6am |
| Coffee Landing Cafe | International Falls | 4.7 (446) | Greater MN gap (0 live in International Falls), opens by 6am |
| Rendezvous Coffee | Isanti | 4.7 (397) | Greater MN gap (0 live in Isanti), opens by 6am |
| City Hall Creamery & Coffee | Jordan | 4.9 (135) | Greater MN gap (0 live in Jordan), open late |
| Due North Coffee Co. | Kasson | 4.6 (65) | Greater MN gap (2 live in Kasson), opens by 6am |
| Rise & Grind | Kimball | 4.8 (70) | Greater MN gap (0 live in Kimball) |
| Heidi’s Huggamug Cafe | Lake City | 4.9 (275) | Greater MN gap (1 live in Lake City), opens by 6am |
| SOJO Coffee Company | Lakefield | 5 (65) | Greater MN gap (0 live in Lakefield) |
| Lucy's | Little Falls | 4.8 (87) | Greater MN gap (0 live in Little Falls) |
| 110 Coffee Co. | Long Prairie | 4.7 (95) | Greater MN gap (1 live in Long Prairie), opens by 6am |
| Common Grounds 218 | Longville | 4.7 (115) | Greater MN gap (0 live in Longville), women-owned |
| WildFlowers Coffee Boutique | Luverne | 4.7 (175) | Greater MN gap (0 live in Luverne) |
| County Seat Coffeehouse | Mantorville | 4.7 (207) | Greater MN gap (0 live in Mantorville), fireplace, women-owned |
| Spooky Brew Coffeehouse | Maple Lake | 4.8 (358) | Greater MN gap (0 live in Maple Lake) |
| Bagels & Brew | Marshall | 4.8 (197) | Greater MN gap (0 live in Marshall) |
| School House Cafe | McGregor | 4.6 (418) | Greater MN gap (0 live in McGregor), opens by 6am, women-owned |
| The Cotty- formerly Cottage House Cafe | Menahga | 4.7 (448) | Greater MN gap (1 live in Menahga), opens by 6am |
| The Bee Caffè | Milaca | 4.7 (110) | Greater MN gap (1 live in Milaca), women-owned |
| Java River | Montevideo | 4.8 (186) | Greater MN gap (0 live in Montevideo), fireplace, live music |
| Joe Jitters Coffee House | Moose Lake | 4.6 (311) | Greater MN gap (0 live in Moose Lake) |
| Muskie Waters Co. | Nevis | 4.7 (247) | Greater MN gap (0 live in Nevis), women-owned |
| Lake Affect Coffee | New London | 4.8 (86) | Greater MN gap (0 live in New London) |
| Cedar Press Coffee | New Prague | 4.8 (84) | Greater MN gap (0 live in New Prague) |
| Alli B's Coffee Shop and Boutique | Norwood Young America | 4.9 (180) | Greater MN gap (0 live in Norwood Young America), opens by 6am, women-owned |
| Abundant Life Coffee LLC | Osakis | 4.7 (69) | Greater MN gap (0 live in Osakis), live music |
| Firehouse Coffee Company & Salon 227 | Paynesville | 4.7 (99) | Greater MN gap (0 live in Paynesville) |
| Lakes Latte | Pequot Lakes | 4.7 (335) | Greater MN gap (1 live in Pequot Lakes), fireplace |
| Gathering Grounds Coffee Cafe | Perham | 4.8 (216) | Greater MN gap (0 live in Perham), live music, women-owned |
| The Shante | Pillager | 4.7 (210) | Greater MN gap (0 live in Pillager) |
| Cabina Coffee | Pine City | 4.6 (262) | Greater MN gap (0 live in Pine City) |
| Lifehouse Coffee | Pine River | 4.9 (129) | Greater MN gap (0 live in Pine River), fireplace |
| Norsland Lefse | Rushford | 4.6 (134) | Greater MN gap (0 live in Rushford) |
| Jitters Java Cafe | Sauk Centre | 4.6 (454) | Greater MN gap (0 live in Sauk Centre), opens by 6am, fireplace, women-owned |
| CTC Coffee Ta Cream | Shakopee | 4.8 (252) | Greater MN gap (0 live in Shakopee) |
| Timber Coffee Company | Silver Bay | 4.9 (270) | Greater MN gap (0 live in Silver Bay) |
| The Plaid Moose Coffeehouse and Cafe | Slayton | 4.9 (198) | Greater MN gap (0 live in Slayton), women-owned |
| Sleepy Eye Coffee Co. | Sleepy Eye | 4.7 (102) | Greater MN gap (0 live in Sleepy Eye), live music, women-owned |
| Some Like It Hot | Spring Valley | 4.9 (51) | Greater MN gap (0 live in Spring Valley) |
| Central Grind Coffeehouse | St Augusta | 4.9 (54) | Greater MN gap (0 live in St Augusta), opens by 6am |
| Mocha Monkey | St Bonifacius | 4.7 (283) | Greater MN gap (0 live in St Bonifacius), opens by 6am |
| Encore Coffee Cafe | St James | 4.8 (93) | Greater MN gap (0 live in St James), women-owned |
| The Local Blend | St Joseph | 4.8 (589) | Greater MN gap (1 live in St Joseph), fireplace |
| River Rock Coffee & Tea | St Peter | 4.7 (551) | Greater MN gap (0 live in St Peter), opens by 6am, women-owned |
| Stomping Grounds | Staples | 4.7 (177) | Greater MN gap (0 live in Staples) |
| The Neon Loon | Starbuck | 4.8 (61) | Greater MN gap (1 live in Starbuck) |
| Coffee Talk | Taylors Falls | 4.8 (455) | Greater MN gap (0 live in Taylors Falls), dogs inside |
| Sulu's Espresso Café | Tower | 4.9 (82) | Greater MN gap (0 live in Tower) |
| Deja Brew | Tyler | 4.9 (59) | Greater MN gap (0 live in Tyler) |
| Otter Coffee and Ice Cream | Vergas | 4.7 (84) | Greater MN gap (0 live in Vergas) |
| Ruby's Roost Bakery & Coffee | Victoria | 4.8 (341) | Greater MN gap (0 live in Victoria), women-owned |
| Mocha Monkey | Waconia | 4.7 (504) | Greater MN, fireplace |
| Owly Coffee Co. | Wadena | 4.8 (198) | Greater MN gap (1 live in Wadena) |
| Walker Bay Coffee Co | Walker | 4.7 (242) | Greater MN gap (0 live in Walker) |
| Lake of the Woods Coffee Company | Warroad | 4.6 (109) | Greater MN gap (0 live in Warroad) |
| Cool Wave Coffee | Waterville | 5 (121) | Greater MN gap (0 live in Waterville), opens by 6am, women-owned |
| Phileo's Coffee and Eatery | Worthington | 4.7 (127) | Greater MN gap (0 live in Worthington) |
| Natures Grind Coffee Co | Zumbrota | 4.9 (77) | Greater MN gap (0 live in Zumbrota) |

