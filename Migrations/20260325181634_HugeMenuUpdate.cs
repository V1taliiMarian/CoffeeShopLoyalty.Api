using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoffeeShopLoyalty.Api.Migrations
{
    /// <inheritdoc />
    public partial class HugeMenuUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoyaltyTransactions_Orders_OrderId",
                table: "LoyaltyTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_Customers_TelegramId",
                table: "Customers");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderType",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "Orders",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalAmount",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusUsed",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAtPurchase",
                table: "OrderItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MenuItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "MenuItems",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MenuItems",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "LoyaltyTransactions",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<decimal>(
                name: "PointsDeducted",
                table: "LoyaltyTransactions",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PointsAdded",
                table: "LoyaltyTransactions",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSpent",
                table: "Customers",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoyaltyLevel",
                table: "Customers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Customers",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusBalance",
                table: "Customers",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Кава та чай" },
                    { 2, null, "Напої б/а" },
                    { 3, null, "Кондитерка" },
                    { 4, null, "Випічка" },
                    { 5, null, "Хліба" },
                    { 6, null, "Круасани та мафіни" },
                    { 7, null, "Піца" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CategoryId", "CostPrice", "Description", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, 15m, "30 мл. Класична міцна кава", true, "Еспресо", 45m },
                    { 2, 1, 16m, "150 мл. Еспресо з додаванням гарячої води", true, "Американо", 50m },
                    { 3, 1, 25m, "250 мл. Еспресо зі збитим гарячим молоком", true, "Капучино", 65m },
                    { 4, 1, 30m, "350 мл. Багато молока та шот еспресо", true, "Лате", 75m },
                    { 5, 1, 35m, "200 мл. Подвійне еспресо та трохи збитого молока", true, "Флет Вайт", 80m },
                    { 6, 1, 45m, "300 мл. Кава на вершках з ванільним цукром", true, "Раф", 90m },
                    { 7, 1, 50m, "300 мл. Японський зелений чай з молоком", true, "Матча Лате", 110m },
                    { 8, 1, 10m, "400 мл. Заварний листовий чай", true, "Чай чорний", 55m },
                    { 9, 1, 10m, "400 мл. Заварний листовий чай з жасмином", true, "Чай зелений", 55m },
                    { 10, 1, 25m, "300 мл. Натуральне какао на молоці", true, "Какао", 70m },
                    { 11, 2, 30m, "400 мл. Лимон, м'ята, цукровий сироп, содова", true, "Лимонад Класичний", 85m },
                    { 12, 2, 35m, "400 мл. Лайм, свіжа м'ята, спрайт, лід", true, "Мохіто б/а", 95m },
                    { 13, 2, 32m, "350 мл. Холодне молоко, еспресо, лід", true, "Айс Лате", 85m },
                    { 14, 2, 40m, "300 мл. Тонік, подвійне еспресо, лід, лимон", true, "Еспресо Тонік", 100m },
                    { 15, 2, 60m, "250 мл. 100% свіжовичавлений сік", true, "Фреш Апельсиновий", 120m },
                    { 16, 2, 65m, "250 мл. 100% свіжовичавлений сік", true, "Фреш Грейпфрутовий", 130m },
                    { 17, 2, 25m, "330 мл. Скляна пляшка", true, "Кока-Кола", 50m },
                    { 18, 2, 18m, "500 мл. Негазована / Газована", true, "Вода Моршинська", 40m },
                    { 19, 2, 50m, "330 мл. Ферментований напій (в асортименті)", true, "Комбуча", 90m },
                    { 20, 2, 45m, "400 мл. Морозиво, молоко, збиті вершки", true, "Мілкшейк Ванільний", 110m },
                    { 21, 3, 45m, "150 г. Класичний листковий торт з заварним кремом", true, "Торт Наполеон", 120m },
                    { 22, 3, 55m, "160 г. Вершковий сир, пісочна основа", true, "Чизкейк Нью-Йорк", 140m },
                    { 23, 3, 60m, "150 г. Савоярді, маскарпоне, кава", true, "Тірамісу", 135m },
                    { 24, 3, 20m, "30 г. Мигдальне тістечко (різні смаки)", true, "Макарон", 55m },
                    { 25, 3, 25m, "80 г. Заварне тісто, крем пломбір", true, "Еклер Ванільний", 80m },
                    { 26, 3, 35m, "120 г. Шоколадний десерт з вологою серединкою", true, "Брауні", 95m },
                    { 27, 3, 18m, "90 г. Бісквітна крихта, какао, ром", true, "Тістечко Картопля", 65m },
                    { 28, 3, 40m, "140 г. Медові коржі зі сметанним кремом", true, "Медовик", 110m },
                    { 29, 3, 35m, "150 г. Вершковий десерт з ягідним кюлі", true, "Панакота", 105m },
                    { 30, 3, 45m, "110 г. Пісочне тісто, заварний крем, сезонні ягоди", true, "Тарталетка Ягідна", 125m },
                    { 31, 4, 30m, "180 г. Булочка з корицею та крем-сиром", true, "Синнабон", 100m },
                    { 32, 4, 15m, "100 г. Печений пиріжок зі свининою та яловичиною", true, "Пиріжок з м'ясом", 45m },
                    { 33, 4, 12m, "100 г. Печений пиріжок з солодкою вишнею", true, "Пиріжок з вишнею", 40m },
                    { 34, 4, 20m, "120 г. Молочна сосиска у листковому тісті", true, "Сосиска в тісті", 55m },
                    { 35, 4, 30m, "150 г. Листкове тісто, сир сулугуні", true, "Хачапурі з сиром", 85m },
                    { 36, 4, 60m, "200 г. Відкритий пиріг з лососем та шпинатом", true, "Кіш з лососем", 150m },
                    { 37, 4, 20m, "120 г. Листкове тісто з яблуком та корицею", true, "Слойка з яблуком", 60m },
                    { 38, 4, 22m, "110 г. Заварний крем та родзинки", true, "Равлик з родзинками", 65m },
                    { 39, 4, 15m, "130 г. Дріжджове тісто, багато маку", true, "Булочка з маком", 50m },
                    { 40, 4, 18m, "120 г. Класична ватрушка з домашнім сиром", true, "Ватрушка з сиром", 55m },
                    { 41, 5, 15m, "300 г. Хрустка скоринка, пористий м'якуш", true, "Багет Французький", 60m },
                    { 42, 5, 18m, "250 г. Класичний італійський хліб на заквасці", true, "Чіабата", 70m },
                    { 43, 5, 20m, "400 г. Темний хліб на житній заквасці", true, "Хліб Житній", 65m },
                    { 44, 5, 22m, "350 г. З кмином та коріандром", true, "Бородінський", 75m },
                    { 45, 5, 25m, "350 г. З додаванням гречаного борошна", true, "Гречаний хліб", 80m },
                    { 46, 5, 15m, "450 г. Ідеально рівний для сендвічів", true, "Тостовий хліб", 55m },
                    { 47, 5, 12m, "400 г. Класичний білий батон", true, "Батон Нарізний", 45m },
                    { 48, 5, 30m, "300 г. Італійський корж з чері та розмарином", true, "Фокача з томатами", 95m },
                    { 49, 5, 8m, "90 г. Бріош з кунжутом", true, "Булочка для бургера", 25m },
                    { 50, 5, 45m, "300 г. На основі рисового та кукурудзяного борошна", true, "Хліб безглютеновий", 120m },
                    { 51, 6, 25m, "80 г. Справжній вершковий круасан на маслі 82%", true, "Круасан Класичний", 70m },
                    { 52, 6, 35m, "110 г. З начинкою з бельгійського шоколаду", true, "Круасан Шоколадний", 95m },
                    { 53, 6, 45m, "130 г. З франжипаном та мигдальними пластівцями", true, "Круасан з мигдалем", 120m },
                    { 54, 6, 55m, "120 г. З фісташковим кремом", true, "Круасан Фісташковий", 135m },
                    { 55, 6, 50m, "160 г. Ситний круасан з сиром чеддер та шинкою", true, "Круасан Шинка/Сир", 140m },
                    { 56, 6, 80m, "170 г. Слабосолений лосось, крем-сир, рукола", true, "Круасан з лососем", 180m },
                    { 57, 6, 25m, "110 г. Вологий кекс з шматочками шоколаду", true, "Мафін Шоколадний", 75m },
                    { 58, 6, 28m, "110 г. Зі свіжою лохиною та чорницею", true, "Мафін Чорничний", 80m },
                    { 59, 6, 20m, "110 г. Класичний кекс з ваніллю", true, "Мафін Ванільний", 65m },
                    { 60, 6, 30m, "120 г. З тягучою солоною карамеллю всередині", true, "Мафін Солона Карамель", 85m },
                    { 61, 7, 60m, "400 г. Томатний соус, моцарела, базилік", true, "Піца Маргарита", 190m },
                    { 62, 7, 85m, "450 г. Моцарела, томатний соус, гостра салямі пепероні", true, "Піца Пепероні", 240m },
                    { 63, 7, 110m, "420 г. Вершковий соус, моцарела, дорблю, пармезан, чеддер", true, "Піца Чотири Сири", 270m },
                    { 64, 7, 120m, "500 г. Бекон, шинка, салямі, куряче філе, моцарела", true, "Піца М'ясна", 290m },
                    { 65, 7, 80m, "460 г. Куряче філе, ананас, кукурудза, вершковий соус", true, "Піца Гавайська", 230m },
                    { 66, 7, 95m, "480 г. Шинка, печериці, артишоки, оливки, моцарела", true, "Піца Капричоза", 260m },
                    { 67, 7, 90m, "440 г. Печериці, білі гриби, трюфельна олія, вершковий соус", true, "Піца Грибна", 250m },
                    { 68, 7, 115m, "430 г. Прошуто крудо, рукола, томати чері, пармезан", true, "Піца Прошуто", 285m },
                    { 69, 7, 90m, "450 г. Гострий соус, салямі, перець халапеньйо", true, "Піца Дьявола", 245m },
                    { 70, 7, 105m, "470 г. Куряче філе, бекон, айсберг, соус цезар, перепелині яйця", true, "Піца Цезар", 280m }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_LoyaltyTransactions_Orders_OrderId",
                table: "LoyaltyTransactions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemId",
                table: "OrderItems",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoyaltyTransactions_Orders_OrderId",
                table: "LoyaltyTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders");

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MenuItems");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmount",
                table: "Orders",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentStatus",
                table: "Orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "PaymentMethod",
                table: "Orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OrderType",
                table: "Orders",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "InvoiceId",
                table: "Orders",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "FinalAmount",
                table: "Orders",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusUsed",
                table: "Orders",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "PriceAtPurchase",
                table: "OrderItems",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "MenuItems",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MenuItems",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "CostPrice",
                table: "MenuItems",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Reason",
                table: "LoyaltyTransactions",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "PointsDeducted",
                table: "LoyaltyTransactions",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "PointsAdded",
                table: "LoyaltyTransactions",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalSpent",
                table: "Customers",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Customers",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LoyaltyLevel",
                table: "Customers",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Customers",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<decimal>(
                name: "BonusBalance",
                table: "Customers",
                type: "numeric(10,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConditionType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_TelegramId",
                table: "Customers",
                column: "TelegramId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LoyaltyTransactions_Orders_OrderId",
                table: "LoyaltyTransactions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_MenuItems_MenuItemId",
                table: "OrderItems",
                column: "MenuItemId",
                principalTable: "MenuItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
