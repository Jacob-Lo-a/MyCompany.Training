using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Training.Core
{
    //舊寫法
    public class Order1
    {
        private int orderId;
        private int amount;

        public Order1(int orderId, int amount)
        {
            this.orderId = orderId;
            this.amount = amount;
        }


        public void CollectionExpressions1()
        {
            int[] a = { 1, 2, 3 };
            int[] b = { 4, 5, 6 };
            int[] combined = a.Concat(b).ToArray();
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


    }

    //新寫法(c#12)
    public class Order2(int orderId, int amount)
    {
        public int OrderId { get; set; } = orderId;
        public int Amount { get; set; } = amount;

        public void CollectionExpressions2()
        {
            int[] a = [1, 2, 3];
            int[] b = [4, 5, 6];
            int[] combined = [.. a, .. b]; //[1, 2, 3, 4, 5, 6]
        }

        public double Discount2(string status) => status switch
        {
            "VIP" => 0.8,
            "Normal" => 0.9,
            _ => throw new ArgumentException("please input VIP or Normal")
        };



    }


}
