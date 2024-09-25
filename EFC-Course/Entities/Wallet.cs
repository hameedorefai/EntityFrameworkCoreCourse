using System.ComponentModel.DataAnnotations.Schema;

namespace EFC_Course.Entities
{


    // This annotation maps the Wallet class to the "Wallets" table in the database.
    [Table("Wallets")]
    public class Wallet
    {
        // This annotation maps the WalletID property to the "Id" column in the "Wallets" table.
        [Column("Id")]

        public int WalletID { get; set; }
        public string? Holder { get; set; }
        public decimal? Balance { get; set; }

        //public Wallet(int? id, string holder, decimal balance)
        //{
        //    if(id == null)
        //    Id = 0;
        //    Holder = holder;
        //    Balance = balance;
        //}

        public override string ToString()
        {
            return $"[{WalletID}] {Holder} ({Balance:C})";
        }
    }
}
