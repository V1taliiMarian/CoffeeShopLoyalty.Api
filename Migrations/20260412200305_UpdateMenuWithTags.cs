using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeShopLoyalty.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMenuWithTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ContainsGluten",
                table: "MenuItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ContainsLactose",
                table: "MenuItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "MenuItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpicy",
                table: "MenuItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "WeightOrVolume",
                table: "MenuItems",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Класична міцна кава", false, false, "30 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Еспресо з додаванням гарячої води", false, false, "150 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Еспресо зі збитим гарячим молоком", false, false, "250 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Багато молока та шот еспресо", false, false, "350 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Подвійне еспресо та трохи збитого молока", false, false, "200 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Кава на вершках з ванільним цукром", false, false, "300 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Японський зелений чай з молоком", true, false, "300 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Заварний листовий чай", false, false, "400 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Заварний листовий чай з жасмином", false, false, "400 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Натуральне какао на молоці", false, false, "300 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Лимон, м'ята, цукровий сироп, содова", false, false, "400 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Лайм, свіжа м'ята, спрайт, лід", false, false, "400 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Холодне молоко, еспресо, лід", false, false, "350 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Тонік, подвійне еспресо, лід, лимон", false, false, "300 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "100% свіжовичавлений сік", false, false, "250 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "100% свіжовичавлений сік", false, false, "250 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Скляна пляшка", false, false, "330 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Негазована / Газована", false, false, "500 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "Ферментований напій (в асортименті)", true, false, "330 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Морозиво, молоко, збиті вершки", false, false, "400 мл" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Класичний листковий торт з заварним кремом", false, false, "150 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Вершковий сир, пісочна основа", false, false, "160 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Савоярді, маскарпоне, кава", false, false, "150 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Мигдальне тістечко (різні смаки)", false, false, "30 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Заварне тісто, крем пломбір", false, false, "80 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Шоколадний десерт з вологою серединкою", false, false, "120 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Бісквітна крихта, какао, ром", false, false, "90 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Медові коржі зі сметанним кремом", false, false, "140 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, true, "Вершковий десерт з ягідним кюлі", true, false, "150 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Пісочне тісто, заварний крем, сезонні ягоди", false, false, "110 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Булочка з корицею та крем-сиром", false, false, "180 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Печений пиріжок зі свининою та яловичиною", false, false, "100 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Печений пиріжок з солодкою вишнею", false, false, "100 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Молочна сосиска у листковому тісті", false, false, "120 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Листкове тісто, сир сулугуні", false, false, "150 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Відкритий пиріг з лососем та шпинатом", true, false, "200 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Листкове тісто з яблуком та корицею", false, false, "120 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Заварний крем та родзинки", false, false, "110 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Дріжджове тісто, багато маку", false, false, "130 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 40,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Класична ватрушка з домашнім сиром", false, false, "120 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Хрустка скоринка, пористий м'якуш", false, false, "300 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Класичний італійський хліб на заквасці", false, false, "250 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Темний хліб на житній заквасці", false, false, "400 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "З кмином та коріандром", false, false, "350 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "З додаванням гречаного борошна", false, false, "350 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Ідеально рівний для сендвічів", false, false, "450 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Класичний білий батон", false, false, "400 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, false, "Італійський корж з чері та розмарином", false, false, "300 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Бріош з кунжутом", false, false, "90 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { false, false, "На основі рисового та кукурудзяного борошна", true, false, "300 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Справжній вершковий круасан на маслі 82%", false, false, "80 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "З начинкою з бельгійського шоколаду", false, false, "110 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "З франжипаном та мигдальними пластівцями", false, false, "130 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "З фісташковим кремом", false, false, "120 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Ситний круасан з сиром чеддер та шинкою", false, false, "160 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Слабосолений лосось, крем-сир, рукола", true, false, "170 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Вологий кекс з шматочками шоколаду", false, false, "110 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Зі свіжою лохиною та чорницею", false, false, "110 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Класичний кекс з ваніллю", false, false, "110 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "З тягучою солоною карамеллю всередині", true, false, "120 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Томатний соус, моцарела, базилік", false, false, "400 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Моцарела, томатний соус, гостра салямі пепероні", false, true, "450 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Вершковий соус, моцарела, дорблю, пармезан, чеддер", false, false, "420 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Бекон, шинка, салямі, куряче філе, моцарела", false, false, "500 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Куряче філе, ананас, кукурудза, вершковий соус", false, false, "460 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Шинка, печериці, артишоки, оливки, моцарела", false, false, "480 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Печериці, білі гриби, трюфельна олія, вершковий соус", false, false, "440 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Прошуто крудо, рукола, томати чері, пармезан", true, false, "430 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Гострий соус, салямі, перець халапеньйо", false, true, "450 г" });

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "ContainsGluten", "ContainsLactose", "Description", "IsNew", "IsSpicy", "WeightOrVolume" },
                values: new object[] { true, true, "Куряче філе, бекон, айсберг, соус цезар, перепелині яйця", true, false, "470 г" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainsGluten",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "ContainsLactose",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "IsSpicy",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "WeightOrVolume",
                table: "MenuItems");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "30 мл. Класична міцна кава");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "150 мл. Еспресо з додаванням гарячої води");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "250 мл. Еспресо зі збитим гарячим молоком");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 4,
                column: "Description",
                value: "350 мл. Багато молока та шот еспресо");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 5,
                column: "Description",
                value: "200 мл. Подвійне еспресо та трохи збитого молока");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 6,
                column: "Description",
                value: "300 мл. Кава на вершках з ванільним цукром");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 7,
                column: "Description",
                value: "300 мл. Японський зелений чай з молоком");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 8,
                column: "Description",
                value: "400 мл. Заварний листовий чай");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 9,
                column: "Description",
                value: "400 мл. Заварний листовий чай з жасмином");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 10,
                column: "Description",
                value: "300 мл. Натуральне какао на молоці");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 11,
                column: "Description",
                value: "400 мл. Лимон, м'ята, цукровий сироп, содова");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 12,
                column: "Description",
                value: "400 мл. Лайм, свіжа м'ята, спрайт, лід");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 13,
                column: "Description",
                value: "350 мл. Холодне молоко, еспресо, лід");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 14,
                column: "Description",
                value: "300 мл. Тонік, подвійне еспресо, лід, лимон");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 15,
                column: "Description",
                value: "250 мл. 100% свіжовичавлений сік");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 16,
                column: "Description",
                value: "250 мл. 100% свіжовичавлений сік");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 17,
                column: "Description",
                value: "330 мл. Скляна пляшка");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 18,
                column: "Description",
                value: "500 мл. Негазована / Газована");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 19,
                column: "Description",
                value: "330 мл. Ферментований напій (в асортименті)");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 20,
                column: "Description",
                value: "400 мл. Морозиво, молоко, збиті вершки");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 21,
                column: "Description",
                value: "150 г. Класичний листковий торт з заварним кремом");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 22,
                column: "Description",
                value: "160 г. Вершковий сир, пісочна основа");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 23,
                column: "Description",
                value: "150 г. Савоярді, маскарпоне, кава");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 24,
                column: "Description",
                value: "30 г. Мигдальне тістечко (різні смаки)");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 25,
                column: "Description",
                value: "80 г. Заварне тісто, крем пломбір");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 26,
                column: "Description",
                value: "120 г. Шоколадний десерт з вологою серединкою");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 27,
                column: "Description",
                value: "90 г. Бісквітна крихта, какао, ром");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 28,
                column: "Description",
                value: "140 г. Медові коржі зі сметанним кремом");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 29,
                column: "Description",
                value: "150 г. Вершковий десерт з ягідним кюлі");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 30,
                column: "Description",
                value: "110 г. Пісочне тісто, заварний крем, сезонні ягоди");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 31,
                column: "Description",
                value: "180 г. Булочка з корицею та крем-сиром");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 32,
                column: "Description",
                value: "100 г. Печений пиріжок зі свининою та яловичиною");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 33,
                column: "Description",
                value: "100 г. Печений пиріжок з солодкою вишнею");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 34,
                column: "Description",
                value: "120 г. Молочна сосиска у листковому тісті");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 35,
                column: "Description",
                value: "150 г. Листкове тісто, сир сулугуні");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 36,
                column: "Description",
                value: "200 г. Відкритий пиріг з лососем та шпинатом");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 37,
                column: "Description",
                value: "120 г. Листкове тісто з яблуком та корицею");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 38,
                column: "Description",
                value: "110 г. Заварний крем та родзинки");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 39,
                column: "Description",
                value: "130 г. Дріжджове тісто, багато маку");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 40,
                column: "Description",
                value: "120 г. Класична ватрушка з домашнім сиром");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 41,
                column: "Description",
                value: "300 г. Хрустка скоринка, пористий м'якуш");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 42,
                column: "Description",
                value: "250 г. Класичний італійський хліб на заквасці");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 43,
                column: "Description",
                value: "400 г. Темний хліб на житній заквасці");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 44,
                column: "Description",
                value: "350 г. З кмином та коріандром");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 45,
                column: "Description",
                value: "350 г. З додаванням гречаного борошна");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 46,
                column: "Description",
                value: "450 г. Ідеально рівний для сендвічів");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 47,
                column: "Description",
                value: "400 г. Класичний білий батон");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 48,
                column: "Description",
                value: "300 г. Італійський корж з чері та розмарином");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 49,
                column: "Description",
                value: "90 г. Бріош з кунжутом");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 50,
                column: "Description",
                value: "300 г. На основі рисового та кукурудзяного борошна");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 51,
                column: "Description",
                value: "80 г. Справжній вершковий круасан на маслі 82%");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 52,
                column: "Description",
                value: "110 г. З начинкою з бельгійського шоколаду");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 53,
                column: "Description",
                value: "130 г. З франжипаном та мигдальними пластівцями");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 54,
                column: "Description",
                value: "120 г. З фісташковим кремом");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 55,
                column: "Description",
                value: "160 г. Ситний круасан з сиром чеддер та шинкою");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 56,
                column: "Description",
                value: "170 г. Слабосолений лосось, крем-сир, рукола");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 57,
                column: "Description",
                value: "110 г. Вологий кекс з шматочками шоколаду");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 58,
                column: "Description",
                value: "110 г. Зі свіжою лохиною та чорницею");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 59,
                column: "Description",
                value: "110 г. Класичний кекс з ваніллю");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 60,
                column: "Description",
                value: "120 г. З тягучою солоною карамеллю всередині");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 61,
                column: "Description",
                value: "400 г. Томатний соус, моцарела, базилік");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 62,
                column: "Description",
                value: "450 г. Моцарела, томатний соус, гостра салямі пепероні");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 63,
                column: "Description",
                value: "420 г. Вершковий соус, моцарела, дорблю, пармезан, чеддер");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 64,
                column: "Description",
                value: "500 г. Бекон, шинка, салямі, куряче філе, моцарела");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 65,
                column: "Description",
                value: "460 г. Куряче філе, ананас, кукурудза, вершковий соус");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 66,
                column: "Description",
                value: "480 г. Шинка, печериці, артишоки, оливки, моцарела");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 67,
                column: "Description",
                value: "440 г. Печериці, білі гриби, трюфельна олія, вершковий соус");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 68,
                column: "Description",
                value: "430 г. Прошуто крудо, рукола, томати чері, пармезан");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 69,
                column: "Description",
                value: "450 г. Гострий соус, салямі, перець халапеньйо");

            migrationBuilder.UpdateData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 70,
                column: "Description",
                value: "470 г. Куряче філе, бекон, айсберг, соус цезар, перепелині яйця");
        }
    }
}
