using System;

namespace MayeXpenses
{
    public class Cost
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public Currencies Currency { get; set; }

        public float Value { get; set; }

        public string ReceiptName { get; set; }

        public Cost()
        {

        }

        public Cost(int id)
        {
            Id = id;
            Date = DateTime.Now;
            System.Diagnostics.Debug.WriteLine(Date.ToString());
            Description = "";
            Currency = SettingsHelper.GetCurrency();
            Value = 0.00f;
        }

        public Cost(long id, string date, string desc, Currencies currency, float val, string receipt = "")
        {
            Id = id;
            Date = DateTime.Parse(date);
            Description = desc;
            Currency = currency;
            Value = val;
            ReceiptName = receipt;
        }

        public string GetDateString()
        {
            return Date == null ? "" : Date.ToString("dd_MM_yyyy_HHmmss").Replace("-", "_").Replace("/", "_").Replace("\\", "_").Replace(":", "-");
        }
    }
}
