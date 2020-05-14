using GeoLearn.Models.Minerals;
using SQLite;
using System;

namespace GeoLearn.Models
{
    [Table("Minerals")]
    public class Mineral
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        private int _priority;

        [Column("Priority")]
        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                if (_priority == 0)
                {
                    _priority = value;
                    return;
                }

                if (value < 1)
                    value = 1;
                if (value > 30)
                    value = 30;
                int delta = _priority - value;
                MineralsList.SumPriority -= delta;
                _priority = value;
            }
        }

        [Column("KeyName")]
        public string KeyName { get; set; }

        /// <summary>
        /// Список всех параметров минерала
        /// </summary>
        public static readonly string[] parametrsList = new string[]
        {
            "name", "chemicalformula", "crystalshape",
            "color", "streak", "luster",
            "transperency", "cleavage", "fracture",
            "hardness", "density", "origin",
            "usage", "diagnosticproperties", "specialproperties"
        };
        public static readonly string[] parametrsListRU = new string[]
        {
            "Название", "Химическая формула", "Форма нахождения",
            "Цвет", "Цвет черты", "Блеск",
            "Прозрачность", "Спайность", "Излом",
            "Твердость", "Плотность", "Происхождение",
            "Использование", "Диагностические черты", "Особые свойста"
        };

        public static readonly int[] examParametrsList = new int[]
                                                { 3,4,5,6,10,13,14 };

        /// <summary>
        /// Костыльный генератор дефолтного минерала
        /// </summary>
        /// <param name="dopText">приписывает этот текст к каждому параметру</param>
        public Mineral(string dopText)
        {
            for (int i = 0; i < parametrsList.Length; i++)
                if (parametrsListRU[i] != "Название")
                    this[parametrsList[i]] = parametrsListRU[i] + " " + dopText;
                else
                    this[parametrsList[i]] = dopText;
        }

        public Mineral() : this("")
        {
        }


        /// <summary>
        /// Объявляет новый минерал
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="chemicalFormula">Химическая формула</param>
        /// <param name="crystalShape">Форма нахождения</param>
        /// <param name="color">Цвет</param>
        /// <param name="streak">Цвет черты</param>
        /// <param name="luster">Блеск</param>
        /// <param name="Transperency">Прозрачность</param>
        /// <param name="cleavage">Спайность</param>
        /// <param name="fracture">Излом</param>
        /// <param name="hardness">Твердость</param>
        /// <param name="density">Плотность</param>
        /// <param name="origin">Происхождение</param>
        /// <param name="usage">Использование</param>
        /// <param name="diagnosticProperties">Диагностические черты</param>
        /// <param name="specialProperties">Особые свойства</param>
        public Mineral(string name, string chemicalFormula, string crystalShape,
            string color, string streak, string luster, string transperency,
            string cleavage, string fracture, string hardness, string density,
            string origin, string usage, string diagnosticProperties, string specialProperties)
        {
            Name = name;
            ChemicalFormula = chemicalFormula;
            CrystalShape = crystalShape;
            Color = color;
            Streak = streak;
            Luster = luster;
            Transperency = transperency;
            Cleavage = cleavage;
            Fracture = fracture;
            Hardness = hardness;
            Density = density;
            Origin = origin;
            Usage = usage;
            DiagnosticProperties = diagnosticProperties;
            SpecialProperties = specialProperties;
        }

        /// <summary>
        /// Индексатор для удобного доступа к параметрам минералов
        /// </summary>
        /// <param name="parametr">название минерала</param>
        /// <returns></returns>
        public string this[string parametr]
        {
            get
            {
                switch (parametr.ToLower())
                {
                    case "name":
                        return Name;
                    case "chemicalformula":
                        return ChemicalFormula;
                    case "crystalshape":
                        return CrystalShape;
                    case "color":
                        return Color;
                    case "streak":
                        return Streak;
                    case "luster":
                        return Luster;
                    case "transperency":
                        return Transperency;
                    case "cleavage":
                        return Cleavage;
                    case "fracture":
                        return Fracture;
                    case "hardness":
                        return Hardness;
                    case "density":
                        return Density;
                    case "origin":
                        return Origin;
                    case "usage":
                        return Usage;
                    case "diagnosticproperties":
                        return DiagnosticProperties;
                    case "specialproperties":
                        return SpecialProperties;
                    default:
                        throw new ArgumentException($"Invalid parametr({parametr})!");
                }
            }

            private set
            {
                switch (parametr.ToLower())
                {
                    case "name":
                        Name = value;
                        break;
                    case "chemicalformula":
                        ChemicalFormula = value;
                        break;
                    case "crystalshape":
                        CrystalShape = value;
                        break;
                    case "color":
                        Color = value;
                        break;
                    case "streak":
                        Streak = value;
                        break;
                    case "luster":
                        Luster = value;
                        break;
                    case "transperency":
                        Transperency = value;
                        break;
                    case "cleavage":
                        Cleavage = value;
                        break;
                    case "fracture":
                        Fracture = value;
                        break;
                    case "hardness":
                        Hardness = value;
                        break;
                    case "density":
                        Density = value;
                        break;
                    case "origin":
                        Origin = value;
                        break;
                    case "usage":
                        Usage = value;
                        break;
                    case "diagnosticproperties":
                        DiagnosticProperties = value;
                        break;
                    case "specialproperties":
                        SpecialProperties = value;
                        break;
                    default:
                        throw new ArgumentException($"Invalid parametr({parametr})!");
                }
            }
        }

        /// <summary>
        /// Название
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

        /// <summary>
        /// Блеск
        /// </summary>
        [Column("ChemicalFormula")]
        public string ChemicalFormula { get; set; }

        /// <summary>
        /// Форма нахождения
        /// </summary>
        [Column("CrystalShape")]
        public string CrystalShape { get; set; }

        /// <summary>
        /// Цвет
        /// </summary>
        [Column("Color")]
        public string Color { get; set; }
        /// <summary>
        /// Цвет черты
        /// </summary>
        [Column("Streak")]
        public string Streak { get; set; }

        /// <summary>
        /// Блеск
        /// </summary>
        [Column("Luster")]
        public string Luster { get; set; }

        /// <summary>
        /// Прозрачность
        /// </summary>
        [Column("Transperency")]
        public string Transperency { get; set; }

        /// <summary>
        /// Спайность
        /// </summary>
        [Column("Cleavage")]
        public string Cleavage { get; set; }

        /// <summary>
        /// Излом
        /// </summary>
        [Column("Fracture")]
        public string Fracture { get; set; }

        /// <summary>
        /// Твердость
        /// </summary>
        [Column("Hardness")]
        public string Hardness { get; set; }

        /// <summary>
        /// Плотность
        /// </summary>
        [Column("Density")]
        public string Density { get; set; }

        /// <summary>
        /// Происхождение
        /// </summary>
        [Column("Origin")]
        public string Origin { get; set; }

        /// <summary>
        /// Использование
        /// </summary>
        [Column("Usage")]
        public string Usage { get; set; }

        /// <summary>
        /// Диагностические черты
        /// </summary>
        [Column("DiagnosticProperties")]
        public string DiagnosticProperties { get; set; }

        /// <summary>
        /// Особые свойства
        /// </summary>
        [Column("SpecialProperties")]
        public string SpecialProperties { get; set; }
    }
}