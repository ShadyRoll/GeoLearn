using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GeoLearn.Models.Minerals
{
    static public class MineralsList
    {
        public static Dictionary<string, Dictionary<string, Mineral>> mineralsList { get; private set; } = new Dictionary<string, Dictionary<string, Mineral>>();

        private static int _sumPriority;

        public static int SumPriority
        {
            get
            {
                return _sumPriority;
            }
            set
            {
                _sumPriority = value;
                if (_sumPriority < 10)
                    foreach (Mineral mineral in mineralsList["ru"].Values)
                    {
                        if (mineral.Priority <= 0)
                        {
                            mineral.Priority += 10;
                            _sumPriority += 10;
                            break;
                        }
                    }
            }
        }

        public static void LoadBaseSql(bool firstLaunch)
        {
            mineralsList["ru"] = App.Database.GetMineralsList("ru");
            mineralsList["eng"] = App.Database.GetMineralsList("eng");

            if (firstLaunch || mineralsList["ru"].Values.First().Priority == 0)
            {
                SumPriority = 0;
                int i;
                using (var en = mineralsList["ru"].Values.GetEnumerator())
                    for (i = 0; i < 4; i++)
                    {
                        en.MoveNext();
                        en.Current.Priority = 10;
                    }
                SumPriority = i * 10;
            }
        }

        public static void LoadBase()
        {
            //using OfficeOpenXml;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(MineralsList)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("GeoLearn.Models.Minerals.MineralsInfo.xlsx");


            using (ExcelPackage xlPackage = new ExcelPackage(stream))
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var myWorksheet = xlPackage.Workbook.Worksheets.FirstOrDefault(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                string name, ChemicalFormula, CrystalShape, Color, Streak, Transparency, Luster,
                    Cleavage, Fracture, Hardness, Density, Origin, mineralName,
                    Usage, DiagnosticProperties, SpecialProperties;

                mineralsList = new Dictionary<string, Dictionary<string, Mineral>>
                {
                    ["ru"] = new Dictionary<string, Mineral>(),
                    ["eng"] = new Dictionary<string, Mineral>()
                };

                var sb = new StringBuilder(); //this is your data
                for (int rowNum = 2; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? string.Empty : c.Value.ToString());
                    var en = row.GetEnumerator(); en.MoveNext(); en.MoveNext();

                    mineralName = en.Current; en.MoveNext();
                    if (mineralName == "")
                        break;

                    name = en.Current.Trim(); en.MoveNext();
                    ChemicalFormula = en.Current.Trim(); en.MoveNext();
                    CrystalShape = en.Current.Trim(); en.MoveNext();
                    Color = en.Current.Trim(); en.MoveNext();
                    Streak = en.Current.Trim(); en.MoveNext();
                    Transparency = en.Current.Trim(); en.MoveNext();
                    Luster = en.Current.Trim(); en.MoveNext();
                    Cleavage = en.Current.Trim(); en.MoveNext();
                    Fracture = en.Current.Trim(); en.MoveNext();
                    Hardness = en.Current.Trim(); en.MoveNext();
                    Density = en.Current.Trim(); en.MoveNext();
                    SpecialProperties = en.Current.Trim(); en.MoveNext();
                    Origin = en.Current.Trim(); en.MoveNext();
                    Usage = en.Current.Trim(); en.MoveNext();
                    DiagnosticProperties = en.Current.Trim(); en.MoveNext();

                    Mineral mineral = new Mineral(name, ChemicalFormula, CrystalShape,
                        Color, Streak, Luster, Transparency, Cleavage,
                        Fracture, Hardness, Density, Origin, Usage,
                        DiagnosticProperties, SpecialProperties)
                    {
                        KeyName = mineralName
                    };

                    mineralsList["ru"][mineralName] = mineral;
                }
            }
        }

        public static Mineral GetByName(string name, string lenguage = "ru") =>
            mineralsList[lenguage.ToLower()][name.ToLower()];
    }
}
