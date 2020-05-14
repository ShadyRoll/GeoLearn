using SQLite;

namespace GeoLearn.Database
{
    [Table("Saves")]
    public class Saves
    {
        public Saves()
        {
        }

        [PrimaryKey, Unique, AutoIncrement, Column("Id")]
        public int Id { get; set; }

        [Column("BestMineScore")]
        public int BestScore { get; set; }

        [Column("FirstLaunch")]
        public int FirstLaunch { get; set; }

        [Column("SuccessPool")]
        public int SuccessPool { get; set; }
    }
}
