using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    public class Order1
    {
        private int orderId;
        private int amount;

        public Order1(int orderId, int amount)
        {
            this.orderId = orderId;
            this.amount = amount;
        }

        public int OrderId { get; set; }
        public int Amount { get; set; }

        public void CollectionExpressions()
        {
            int[] a = [1, 2, 3];
            int[] b = [4, 5, 6];
            int[] combined = [.. a, .. b];
        }

        public double Discount1(string status)
        {
            switch (status)
            {
                case "VIP":
                    return 0.8;
                case "Normal":
                    return 0.9;
                default:
                    throw new ArgumentException("請輸入VIP 或 Normal");
            }
            
        }

        public double Discount2(string status)
        {
            double coupong = status switch
            {
                "VIP" => 0.8,
                "Normal" => 0.9,
                _ => throw new ArgumentException("please input VIP or Normal")
            };
            return coupong;
        }
    }
    public class Order2(int orderId, int amount)
    {
        public int OrderId { get; set; } = orderId;
        public int Amount { get; set; } = amount;
    }

}
