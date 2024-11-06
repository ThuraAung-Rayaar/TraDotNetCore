namespace DotNetTraining.MinimalAPI.Models
{
    public class DrugDataModel
    {
        public class Drug_Sample
        {
            public required Drug_Idendity[] Drug_Tbl { get; set; }
        }

        public class Drug_Idendity
        {
            public int Drug_id { get; set; }
            public string Brand_name { get; set; }
            public string Generic_name { get; set; }
            public Category Category { get; set; }
            public string Dosage { get; set; }
            public string Side_Effect { get; set; }
            public string Drug_interaction { get; set; }
            public string Production_Date { get; set; }
            public string Expire_Date { get; set; }
        }

        public class Category
        {
            public string Availability { get; set; }
            public string Source { get; set; }
            public string Administration { get; set; }
            public string Therapeutic { get; set; }
        }

    }
}
