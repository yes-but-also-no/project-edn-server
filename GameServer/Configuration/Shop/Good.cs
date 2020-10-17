using System;
using System.Linq;

namespace GameServer.Configuration.Shop
{
    /// <summary>
    /// Represents a single item contract and price
    /// WIP
    /// </summary>
    public class Good
    {
        /// <summary>
        /// Not sure if we need this but here it is
        /// </summary>
        public uint Id { get; set; }
        
        /// <summary>
        /// Price in credits
        /// </summary>
        public uint CreditPrice { get; set; }
        
        /// <summary>
        /// Price in coins
        /// Not sure what happens when you set both
        /// </summary>
        public uint CoinPrice { get; set; }
        
        /// <summary>
        /// Only used for unit sets afaik
        /// </summary>
        public string ItemNameCode { get; set; }
        
        /// <summary>
        /// Only used for unit sets afaik
        /// </summary>
        public string ItemDescCode { get; set; }
        
        /// <summary>
        /// The contract type for this item
        /// </summary>
        public ContractType ContractType { get; set; }
        
        /// <summary>
        /// The product type this item is
        /// </summary>
        public ProductType ProductType { get; set; }
        
        /// <summary>
        /// The contract value for this item
        /// Qty is qty
        /// Durability is durability
        /// Time is days
        /// </summary>
        public uint ContractValue { get; set; }
        
        /// <summary>
        /// The templates for this item, colon seperated
        /// </summary>
        public string TemplateString { get; set; }

        /// <summary>
        /// The computed templates in this item
        /// Always one, unless it is a unit set
        /// </summary>
        public uint[] Templates => TemplateString.Split(":").Select(t => Convert.ToUInt32(t)).ToArray();
    }

    /// <summary>
    /// The contract type for this item
    /// </summary>
    public enum ContractType : int
    {
        FixedQty = 0,
        Durability = 1,
        Time = 2
    }

    public enum ProductType
    {
        Part = 0,
        Code = 1,
        Etc = 2,
        UnitSet = 3,
        Operator = 4
    }
}